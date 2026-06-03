using System;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Ipopt;

[TestFixture]
public class IpoptSolverTest
{
    // Simple unconstrained differentiable problem: minimise x² + (y-3)²
    // Optimum at (0, 3), f* = 0.
    private sealed class QuadraticProblem : ManagedProblemBase
    {
        private readonly DoubleVector _lb = new(new[] { -5.0, -5.0 });
        private readonly DoubleVector _ub = new(new[] {  5.0,  5.0 });

        public override string get_name() => "QuadraticProblem";
        public override PairOfDoubleVectors get_bounds() => new(_lb, _ub);
        public override ThreadSafety get_thread_safety() => ThreadSafety.Constant;

        public override DoubleVector fitness(DoubleVector x)
            => new(new[] { x[0] * x[0] + (x[1] - 3.0) * (x[1] - 3.0) });

        public override bool has_gradient() => true;
        public override DoubleVector gradient(DoubleVector x)
            => new(new[] { 2.0 * x[0], 2.0 * (x[1] - 3.0) });

        // Explicit dense sparsity: IPOPT requires this to correctly set up the NLP.
        public override bool has_gradient_sparsity() => true;
        public override SparsityPattern gradient_sparsity() => Sparsity((0u, 0u), (0u, 1u));
    }

    [Test]
    public void IpoptIsAvailable()
    {
        Assert.That(OptionalSolverAvailability.IsIpoptAvailable, Is.True,
            "Pagmo.NET.Ipopt must be built against a native library that includes IPOPT.");
    }

    [Test]
    public void IpoptCanBeInstantiated()
    {
        using var algo = new ipopt();
        Assert.That(algo, Is.Not.Null);
        Assert.That(algo.get_name(), Is.Not.Empty);
    }

    [Test]
    public void IpoptOptionsAccepted()
    {
        using var algo = new ipopt();
        Assert.DoesNotThrow(() => algo.set_integer_option("print_level", 0));
        Assert.DoesNotThrow(() => algo.set_string_option("linear_solver", "mumps"));
        Assert.DoesNotThrow(() => algo.set_numeric_option("tol", 1e-8));
    }

    [Test]
    public void IpoptEvolvesAndImproves()
    {
        using var prob = new QuadraticProblem();
        using var algo = new ipopt();

        using var pop = new population(prob, 1u, 42u);
        double fInitial = pop.champion_f()[0];

        using var evolved = algo.evolve(pop);

        Assert.That(evolved, Is.Not.Null, "evolve() must return a non-null population");

        int resultCode = algo.GetLastOptimizationResultCode();
        double fBest = evolved.champion_f()[0];

        Assert.That(resultCode, Is.EqualTo(0).Or.EqualTo(1),
            $"IPOPT must report Solve_Succeeded (0) or Solved_To_Acceptable_Level (1); got {resultCode}");
        Assert.That(fBest, Is.LessThan(1e-6),
            $"IPOPT must converge near f*=0 (initial f={fInitial:F4}, final f={fBest:F6}, code={resultCode})");
    }

    [Test]
    public void IpoptLogLinesAvailableAfterSolve()
    {
        using var prob = new QuadraticProblem();
        using var algo = new ipopt();
        // print_level=0 suppresses console output; pagmo still captures internal log entries.
        algo.set_integer_option("print_level", 0);

        using var pop = new population(prob, 1u, 42u);
        using var _ = algo.evolve(pop);

        // GetTypedLogLines() must not throw and must return a non-null list.
        var log = algo.GetTypedLogLines();
        Assert.That(log, Is.Not.Null);
    }

    [Test]
    public void IpoptToAlgorithmProducesNamedAlgorithm()
    {
        using var algo = new ipopt();
        using var erased = algo.to_algorithm();
        Assert.That(erased.get_name(), Is.Not.Empty);
    }

    [Test]
    public void IpoptGetSeedThrowsNotSupported()
    {
        using var algo = new ipopt();
        Assert.Throws<NotSupportedException>(() => algo.get_seed());
    }

    [Test]
    public void IpoptSetSeedThrowsNotSupported()
    {
        using var algo = new ipopt();
        Assert.Throws<NotSupportedException>(() => algo.set_seed(42u));
    }
}
