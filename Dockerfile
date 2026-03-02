FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Mandelbrot.sln .
COPY Mandelbrot/Mandelbrot.fsproj       Mandelbrot/
COPY Repository/Repository.csproj       Repository/
COPY MandelbrotWeb/MandelbrotWeb.csproj  MandelbrotWeb/
COPY MandelbrotConsole/MandelbrotConsole.fsproj MandelbrotConsole/

COPY Mandelbrot/    Mandelbrot/
COPY Repository/    Repository/
COPY MandelbrotWeb/ MandelbrotWeb/

RUN dotnet publish MandelbrotWeb/MandelbrotWeb.csproj \
    -c Release -o /app/publish \
    /p:ErrorOnDuplicatePublishOutputFiles=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "MandelbrotWeb.dll"]
