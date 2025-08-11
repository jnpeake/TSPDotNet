using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPDotNet.AntColonyOptimisation.Service;
using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Test;
public class AntTests
{
    Problem exampleProblem = new Problem(
            Locations: new List<Location>
            {
                new Location(51.5074, -0.1278),   // London
                new Location(52.4862, -1.8904),   // Birmingham
                new Location(53.4808, -2.2426),   // Manchester
                new Location(55.9533, -3.1883),   // Edinburgh
                new Location(51.4545, -2.5879),   // Bristol
                new Location(53.8008, -1.5491),   // Leeds
                new Location(50.7184, -3.5339),   // Exeter
                new Location(52.6297, 1.2974),    // Norwich
                new Location(51.7520, -1.2577),   // Oxford
                new Location(54.9784, -1.6174)    // Newcastle
            },
            StartIndex: 0,
            EndIndex: 0,
            DistanceMetric: DistanceMetric.Haversine
        );

   

    [Fact]
    public void GivenSolveIsCalled_ValidSolutionIsReturned()
    {
        var numLocations = exampleProblem.Locations.Count;
        var distanceMatrix = new double[numLocations,numLocations];
        for (int i = 0; i < numLocations; i++)
        {
            for (int j = 0; j < numLocations; j++)
            {
                distanceMatrix[i, j] = 1;
            }
        }
        var pheromoneMatrix = new double[numLocations, numLocations];

        var ant = new Ant(exampleProblem, distanceMatrix, pheromoneMatrix, new Random());
        var solution = ant.Solve(0);

        Assert.True(solution.totalDistance > 0);
        Assert.True(solution.route.Count == 10);
        Assert.True(solution.route.Distinct().Count() == 10);
    }
}
