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
    private List<int> _route = new();
    public double[,] PheromoneMatrix;
    public double[,] DistanceMatrix;
    private Problem _problem;
    private IDistanceCalculator _distanceCalculator;

    public Ant(Problem problem)
    {
        _problem = problem;
        _distanceCalculator = GetCalculator(problem.DistanceMetric);
        PheromoneMatrix = InitialisePheromoneMatrix(problem.Locations.Count, 1);
        DistanceMatrix = InitialiseDistanceMatrix(problem);
    }

    private IDistanceCalculator? GetCalculator(DistanceMetric distanceMetric)
    {
        return _problem.DistanceMetric switch
        {
            DistanceMetric.Haversine => new HaversineCalculator(),
            _ => throw new NotImplementedException()
        };
    }

    private double[,] InitialiseDistanceMatrix(Problem problem)
    {
        var numLocations = problem.Locations.Count;
        var locations = problem.Locations;
        var matrix = new double[problem.Locations.Count, problem.Locations.Count];

        for (int i = 0; i < problem.Locations.Count; i++)
        {
            for(int j = 0; j < problem.Locations.Count; j++)
            {
                matrix[i, j] = _distanceCalculator.CalculateDistance(locations[i].latitude, locations[i].longitude, locations[j].latitude, locations[j].longitude);
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
                if (i == j)
                {
                    matrix[i, j] = double.MaxValue;
                }

                else
                {
                    matrix[i, j] = initialValue;
                }
            }
        }

        return matrix;
    }

    public SolutionRoute Solve()
    {
        var numLocations = _problem.Locations.Count;
        var currentLocation = _problem.StartIndex;
        var visited = new HashSet<int>();
        var solution = new SolutionRoute();
        solution.route.Add(currentLocation);
        visited.Add(currentLocation);

        for(int i = 0; i < numLocations-1; i++)
        {
            (int, double) selectedLocation = SelectLocation(currentLocation, numLocations, visited);
            currentLocation = selectedLocation.Item1;
            solution.route.Add(selectedLocation.Item1);
            visited.Add(selectedLocation.Item1);

            solution.totalDistance += selectedLocation.Item2;
        }

        return solution;

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

            var distanceTo = DistanceMatrix[currentLocation, i] * PheromoneMatrix[currentLocation, i];
            if (distanceTo < shortestDistance)
            {
                shortestDistance = distanceTo;
                selectedLocation = i;
            }
        }

        return (selectedLocation, shortestDistance);
    }
}
