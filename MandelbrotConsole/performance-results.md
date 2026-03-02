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
