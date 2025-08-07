using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Service;

public class AntColonyOptimisationSolver : ISolver
{
    public SolutionRoute Solve(Problem inputProblem)
    {
        return new SolutionRoute(new List<int> { 0, 1, 2}, 1.23m);
    }
}
