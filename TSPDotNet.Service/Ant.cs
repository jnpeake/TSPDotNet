using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Service;
public class Ant
{
    public SolutionRoute Solution;
    private Problem _problem;
    private Random _random;
    private readonly double[,] _distanceMatrix;
    private readonly double[,] _pheromoneMatrix;
    private readonly double _alpha;
    private readonly double _beta;

    public Ant(Problem problem, double[,] distanceMatrix, double[,] pheromoneMatrix, Random random, double alpha, double beta)
    {
        _problem = problem;
        _pheromoneMatrix = pheromoneMatrix;
        _distanceMatrix = distanceMatrix;
        _random = random;
        _alpha = alpha;
        _beta = beta;
    }

    public SolutionRoute Solve(int startIndex)
    {
        var numLocations = _problem.Locations.Count;
        var currentLocation = startIndex;
        var visited = new HashSet<int>();

        Solution = new SolutionRoute();
        Solution.route.Add(currentLocation);
        visited.Add(currentLocation);

        for(int i = 0; i < numLocations-1; i++)
        {
            (int, double) selectedLocation = SelectLocation(currentLocation, numLocations, visited);
            currentLocation = selectedLocation.Item1;
            Solution.route.Add(selectedLocation.Item1);
            visited.Add(selectedLocation.Item1);

            Solution.totalDistance += selectedLocation.Item2;
        }

        Solution.totalDistance += _distanceMatrix[Solution.route[numLocations - 1], Solution.route[0]];

        return Solution;

    }

    private (int, double) SelectLocation(int currentLocation, int numLocations, HashSet<int> visited)
    {
        var shortestDistance = double.MaxValue;
        var selectedLocation = -1;
        for (int i = 0; i < numLocations; i++)
        {
            if(visited.Contains(i))
            {
                continue;
            }

            var distanceTo = Math.Pow(_distanceMatrix[currentLocation, i], _beta) * Math.Pow(_pheromoneMatrix[currentLocation, i], _alpha) * _random.NextDouble();
            if (distanceTo < shortestDistance)
            {
                shortestDistance = distanceTo;
                selectedLocation = i;
            }
        }

        return (selectedLocation, _distanceMatrix[currentLocation, selectedLocation]);
    }
}
