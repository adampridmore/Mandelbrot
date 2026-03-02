# Performance

## 2026-03-02

```time dotnet run -- 5```
user    2m43.854s

Baseline

## 2026-03-02 — Avoid sqrt in escape check

Replace `Complex.Abs(z) > 2.0` with `z.Real * z.Real + z.Imaginary * z.Imaginary > 4.0`

```time dotnet run -- 5```
real    1m4.268s
user    2m17.917s
sys     0m26.838s

## 2026-03-02 — Cardioid / period-2 bulb pre-check

Skip iteration loop entirely for points inside the main cardioid or period-2 bulb.
Benefit is greatest for tiles covering the interior of the set (low zoom levels).
At zoom 0–5, most tiles are on or near the boundary so the gain is modest here.

```time dotnet run -- 5```
real    1m7.196s
user    2m16.160s
sys     0m44.410s
