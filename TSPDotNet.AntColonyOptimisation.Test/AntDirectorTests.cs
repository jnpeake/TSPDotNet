using TSPDotNet.AntColonyOptimisation.Service;
using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Test;
public class AntDirectorTests
{
    Problem exampleProblem = new(
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
    public void GivenSolveMethodCalled_SolutionObjectReturned()
    {
        var director = new AntDirector();
        var result = director.SolveProblem(exampleProblem, 12, 10, null, null);

        Assert.IsType<SolutionRoute>(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(5)]
    public void GivenStartCityProvided_SolutionStartsAtSpecifiedCity(int startCity)
    {
        var director = new AntDirector();
        var result = director.SolveProblem(exampleProblem, 12, 10, startCity, null);

        Assert.Equal(startCity, result.route.First());
    }

    [Fact]
    public void GivenSolveMethodCalled_DistanceMatrixIsCreated()
    {
        var director = new AntDirector();
        director.SolveProblem(exampleProblem, 12, 10, null, null);


        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (i == j)
                {
                    Assert.True(director.DistanceMatrix[i, j] == 0);
                }
                else
                {
                    Assert.True(director.DistanceMatrix[i, j] > 0);
                }
            }
        }
    }

    [Fact]
    public void GivenSolveMethodCalled_PheromoneMatrixIsCreated()
    {
        var director = new AntDirector();
        director.SolveProblem(exampleProblem, 12, 10, null, null);

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Assert.True(director.PheromoneMatrix[i, j] == 1d);
            }
        }
    }
}
