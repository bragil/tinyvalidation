
using BenchmarkDotNet.Running;
using TinyValidation.BenchmarkApp.Benchmarks;

var resultado = BenchmarkRunner.Run<ValidationBenchmark>();
