# Pagmo.NET.Ipopt

Optional IPOPT (Interior Point OPTimizer) add-on for [Pagmo.NET](https://github.com/samthegliderpilot/pagmo.NET).

IPOPT is a gradient-based interior-point solver for large-scale nonlinear constrained optimization. It requires the problem to supply gradients (`has_gradient() = true`).

## Requirements

- .NET 8+
- `Pagmo.NET` 1.0.0-beta.6 (pulled in automatically as a dependency)
- No separate IPOPT installation required — the solver is statically linked into the `Pagmo.NET` native library

## Installation

Once published to NuGet.org:
```
dotnet add package Pagmo.NET.Ipopt --version 1.0.0-beta.6
```

## Usage

```csharp
using pagmo;

// IPOPT requires gradients — implement has_gradient() and gradient() on your problem.
using var algo = new ipopt();
algo.set_integer_option("print_level", 0);   // suppress console output
algo.set_numeric_option("tol", 1e-8);        // convergence tolerance

using var island = Island.Create(algo, myGradientProblem, popSize: 1, seed: 42);
island.Evolve(1);
island.WaitCheck();
```

### Useful IPOPT options

| Option | Type | Description |
|---|---|---|
| `tol` | numeric | Convergence tolerance (default `1e-8`) |
| `max_iter` | integer | Maximum iterations (default `3000`) |
| `print_level` | integer | Console verbosity 0–12 (default `5`) |
| `linear_solver` | string | `mumps` (default when available), `ma27`, `ma57`, `ma86`, `ma97`, `pardiso` |
| `hessian_approximation` | string | `exact` or `limited-memory` (L-BFGS) |

### Log extraction

```csharp
using var evolved = algo.evolve(pop);
foreach (var line in algo.GetTypedLogLines())
    Console.WriteLine($"iter obj={line.Objective:F6} feasible={line.Feasible}");

int code = algo.GetLastOptimizationResultCode();
// 0 = Solve_Succeeded, 1 = Solved_To_Acceptable_Level
```

### Known limitations

- SPRAL linear solver is not included in this build.

## License

Wrapper code: LGPL-2.1-or-later. See [LICENSE](LICENSE).
IPOPT itself: EPL-2.0. See [NOTICE](NOTICE).

## Related

- [pagmo.NET](https://github.com/samthegliderpilot/pagmo.NET) — base C# bindings
- [pagmoNet](https://github.com/samthegliderpilot/pagmoNet) — shared SWIG + native bridge
- [PagmoNet4j.ipopt](https://github.com/samthegliderpilot/PagmoNet4j.ipopt) — Java/Kotlin equivalent
