using TSPDotNet.Domain;
using TSPDotNet.Domain.Calculators;

namespace TSPDotNet.AntColonyOptimisation.Service;
public class AntDirector
{
    public double[,] PheromoneMatrix;
    public double[,] DistanceMatrix;


    public SolutionRoute SolveProblem(Problem problem, int numAnts, int numIterations, int? startIndex, int? seed)
    {
        var ants = new Ant[numAnts];
        var randomProvider = seed == null ? new Random() : new Random(seed.Value);
        var startCity = startIndex ?? randomProvider.Next(problem.Locations.Count - 1);
        PheromoneMatrix = InitialisePheromoneMatrix(problem.Locations.Count, 1);
        DistanceMatrix = InitialiseDistanceMatrix(problem);
        var bestSolution = new SolutionRoute();
        bestSolution.totalDistance = double.MaxValue;


        for(int i = 0; i < numIterations; i++)
        {
            var solution = Iterate(problem, numAnts, startIndex, ants, randomProvider, startCity);

            if(solution.totalDistance <  bestSolution.totalDistance)
            {
                bestSolution = solution;
            }
        }

        return bestSolution;
    }

    private SolutionRoute Iterate(Problem problem, int numAnts, int? startIndex, Ant[] ants, Random randomProvider, int startCity)
    {
        for (int j = 0; j < numAnts; j++)
        {
            ants[j] = new Ant(problem, DistanceMatrix, PheromoneMatrix, randomProvider);
        }

        foreach (var ant in ants)
        {
            ant.Solve(startIndex ?? randomProvider.Next(startCity));
        }

        var bestAnt = GetBestAnt(ants);

        DepositPheromone(bestAnt.Solution, problem.Locations.Count);
        EvaporatePheromone(problem.Locations.Count);

        return bestAnt.Solution;
    }

    private void DepositPheromone(SolutionRoute solution, int numLocations)
    {
        var pheromoneAmount = 1 / solution.totalDistance;
        for(int i = 0; i < numLocations-1; i++)
        {
            var currentLocation = solution.route[i];
            var nextLocation = solution.route[i+1];

            PheromoneMatrix[currentLocation, nextLocation] += pheromoneAmount;
        }
        
        var finalLocation = solution.route[numLocations-1];
        var firstLocation = solution.route[0];
        PheromoneMatrix[finalLocation, firstLocation] += pheromoneAmount;
    }

    private void EvaporatePheromone(int numLocations)
    {
        for (int i = 0; i < numLocations; i++)
        {
            for (int j = 0; j < numLocations; j++)
            {
                PheromoneMatrix[i, j] *= 0.95;
            }
        }
    }

    private Ant GetBestAnt(Ant[] ants)
    {
        var shortest = double.MaxValue;
        var shortestIndex = -1;

        for (int i = 0; i < ants.Length; i++)
        {
            if (ants[i].Solution.totalDistance < shortest)
            {
                shortestIndex = i;
            }
        }

        return ants[shortestIndex];
    }

    private double[,] InitialiseDistanceMatrix(Problem problem)
    {
        var numLocations = problem.Locations.Count;
        var locations = problem.Locations;
        var matrix = new double[problem.Locations.Count, problem.Locations.Count];
        var distanceCalculator = GetCalculator(problem.DistanceMetric);

        for (int i = 0; i < problem.Locations.Count; i++)
        {
            for (int j = 0; j < problem.Locations.Count; j++)
            {
                matrix[i, j] = distanceCalculator.CalculateDistance(locations[i].latitude, locations[i].longitude, locations[j].latitude, locations[j].longitude);
            }
        }

        return matrix;
    }

    private static double[,] InitialisePheromoneMatrix(int numCities, double initialValue)
    {
        var matrix = new double[numCities, numCities];

        for (int i = 0; i < numCities; i++)
        {
            for (int j = 0; j < numCities; j++)
            {
                matrix[i, j] = initialValue;
            }
        }

        return matrix;
    }


    private IDistanceCalculator? GetCalculator(DistanceMetric distanceMetric)
    {
        return distanceMetric switch
        {
            DistanceMetric.Haversine => new HaversineCalculator(),
            _ => throw new NotImplementedException()
        };
    }
}
