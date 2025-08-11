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
        
        for(int i = 0; i < numIterations; i++)
        {
            for (int j = 0; j < numAnts; j++)
            {
                ants[j] = new Ant(problem, DistanceMatrix, PheromoneMatrix);
            }

            foreach (var ant in ants)
            {
                ant.Solve(startIndex ?? randomProvider.Next(startCity));
            }
        }

        var bestAnt = GetBestAnt(ants);
        return bestAnt.Solution;
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
