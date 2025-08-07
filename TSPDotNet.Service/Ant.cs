using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPDotNet.Domain;
using TSPDotNet.Domain.Calculators;

namespace TSPDotNet.AntColonyOptimisation.Service;
public class Ant
{
    private List<int> Route = new();
    private double[,] pheromoneMatrix;
    public double[,] DistanceMatrix;
    public Problem _problem;

    public Ant(Problem problem)
    {
        _problem = problem;
        pheromoneMatrix = InitialisePheromoneMatrix(problem.Locations.Count, 1);
        DistanceMatrix = InitialiseDistanceMatrix(problem);
    }

    private static double[,] InitialiseDistanceMatrix(Problem problem)
    {
        var numLocations = problem.Locations.Count;
        var locations = problem.Locations;
        var matrix = new double[problem.Locations.Count, problem.Locations.Count];
        var calculator = problem.DistanceMetric switch
        {
            DistanceMetric.Haversine => new HaversineCalculator()
        };

        for (int i = 0; i < problem.Locations.Count; i++)
        {
            for(int j = 0; j < problem.Locations.Count; j++)
            {
                matrix[i, j] = calculator.CalculateDistance(locations[i].latitude, locations[i].longitude, locations[j].latitude, locations[j].longitude);
            }
        }

        return matrix;
    }

    private static double[,] InitialisePheromoneMatrix(int numCities, double initialValue)
    {
        var matrix = new double[numCities, numCities];

        for(int i = 0; i < numCities; i++)
        {
            for(int j = 0; j < numCities; j++)
            {
                matrix[i, j] = initialValue;
            }
        }

        return matrix;
    }

}
