# Pagmo.NET.Ipopt

Optional IPOPT (Interior Point OPTimizer) add-on for [Pagmo.NET](https://github.com/samthegliderpilot/pagmo.NET).

IPOPT is a gradient-based interior-point solver for large-scale nonlinear constrained optimization. It requires the problem to supply gradients (`has_gradient() = true`).

## Installation

```
dotnet add package Pagmo.NET.Ipopt --version 1.0.0-beta.6
```

IPOPT is statically linked into the `Pagmo.NET` native library — no separate installation required.

## Usage

```csharp
using pagmo;

// ipopt requires gradients — implement has_gradient() and gradient() on your problem.
using var algo = new ipopt();
algo.set_string_option("linear_solver", "mumps");
algo.set_integer_option("print_level", 0);

using var island = Island.Create(algo, myProblem, popSize: 1, seed: 42);
island.Evolve(1);
island.WaitCheck();
```

## License

Wrapper code: LGPL-2.1-or-later. See [LICENSE](LICENSE).  
IPOPT itself: EPL-2.0. See [NOTICE](NOTICE).

## Related

- [pagmo.NET](https://github.com/samthegliderpilot/pagmo.NET) — base C# bindings
- [pagmoNet](https://github.com/samthegliderpilot/pagmoNet) — shared SWIG + native bridge
- [PagmoNet4j.ipopt](https://github.com/samthegliderpilot/PagmoNet4j.ipopt) — Java/Kotlin equivalent
