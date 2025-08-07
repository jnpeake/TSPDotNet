using TSPDotNet.Domain;
using AutoFixture;

namespace TSPDotNet.AntColonyOptimisation.Service.Test;
public class AntColonyOptimisationSolverTests
{
    [Fact]
    public void GivenSolverIsProvidedWithProblem_WhenSolveIsCalled_ThenSolutionRouteIsReturned()
    {
        var fixture = new Fixture();
        var solver = new AntColonyOptimisationSolver();
        var problem = fixture.Create<Problem>();

        var result = solver.Solve(problem);

        Assert.NotNull(result);
        Assert.IsType<SolutionRoute>(result);
    }
}
