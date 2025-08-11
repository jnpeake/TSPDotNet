using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Service;

public class AntColonyOptimisationSolver : ISolver
{
    public SolutionRoute Solve(Problem inputProblem)
    {
        return new SolutionRoute { 
            route = new List<int> { 0, 1, 2 }, 
            totalDistance = 1.23d };
    }
}
