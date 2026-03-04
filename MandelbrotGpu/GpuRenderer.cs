using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;

namespace MandelbrotGpu;

public static class GpuRenderer
{
    private sealed record GpuState(
        Context Context,
        Accelerator Accelerator,
        Action<Index1D, ArrayView1D<int, Stride1D.Dense>, int, int, double, double, double, double, int> Kernel);

    private static readonly Lazy<GpuState?> _state =
        new(InitialiseGpu, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly SemaphoreSlim _lock = new(1, 1);

    public static bool IsAvailable => _state.Value is not null;

    private static GpuState? InitialiseGpu()
    {
        try
        {
            var context = Context.Create(b => b.Cuda());
            var accelerator = context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context);
            Console.WriteLine($"[GpuRenderer] Using CUDA: {accelerator.Name}");
            var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView1D<int, Stride1D.Dense>, int, int, double, double, double, double, int>(MandelbrotKernel);
            return new GpuState(context, accelerator, kernel);
        }
        catch
        {
            // CUDA not available, try OpenCL
        }

        try
        {
            var context = Context.Create(b => b.OpenCL());
            var accelerator = context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context);
            Console.WriteLine($"[GpuRenderer] Using OpenCL: {accelerator.Name}");
            var kernel = accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView1D<int, Stride1D.Dense>, int, int, double, double, double, double, int>(MandelbrotKernel);
            return new GpuState(context, accelerator, kernel);
        }
        catch
        {
            // OpenCL not available either
        }

        Console.WriteLine("[GpuRenderer] No GPU backend available, falling back to CPU renderer.");
        return null;
    }

    /// <summary>
    /// Renders a Mandelbrot tile on the GPU.
    /// Returns a flat int[] of length width*height.
    /// -1 means the point is in the set (render black). Other values are the escape iteration count.
    /// </summary>
    public static int[] RenderTile(
        int width, int height,
        double xMin, double xMax, double yMin, double yMax,
        int maxIterations)
    {
        var state = _state.Value ?? throw new InvalidOperationException("GPU is not available.");
        int pixelCount = width * height;

        _lock.Wait();
        try
        {
            using var outputBuffer = state.Accelerator.Allocate1D<int>(pixelCount);
            state.Kernel(pixelCount, outputBuffer.View, width, height, xMin, xMax, yMin, yMax, maxIterations);
            state.Accelerator.Synchronize();
            return outputBuffer.GetAsArray1D();
        }
        finally
        {
            _lock.Release();
        }
    }

    private static void MandelbrotKernel(
        Index1D index,
        ArrayView1D<int, Stride1D.Dense> output,
        int width, int height,
        double xMin, double xMax,
        double yMin, double yMax,
        int maxIterations)
    {
        int px = (int)index % width;
        int py = (int)index / width;

        double cx = xMin + (xMax - xMin) * px / width;
        double cy = yMin + (yMax - yMin) * py / height;

        double zx = 0.0;
        double zy = 0.0;
        int iter = 0;

        while (iter < maxIterations && zx * zx + zy * zy < 4.0)
        {
            double temp = zx * zx - zy * zy + cx;
            zy = 2.0 * zx * zy + cy;
            zx = temp;
            iter++;
        }

        output[index] = iter >= maxIterations ? -1 : iter;
    }
}
