using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TSPDotNet.AntColonyOptimisation.Service;
using TSPDotNet.Domain;

namespace TSPDotNet.AntColonyOptimisation.Test;
public class ExperimentRunner
{
    [Fact]
    public void RunExperiments()
    {
        var file = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),"Resources", "berlin52.json"));
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        options.Converters.Add(new JsonStringEnumConverter());
        var problem = JsonSerializer.Deserialize<Problem>(file, options);

        var director = new AntDirector();


        var alphas = new double[] {0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6 };
        var betas = new double[] { 0, 0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6 };
        var bestAlpha = 10d;
        var bestBeta = 10d;
        var bestSolutionDistance = double.MaxValue;
        var bestSolution = new SolutionRoute();

        foreach(var alpha in alphas)
        {
            foreach(var beta in betas)
            {
                for (int i = 0; i < 1; i++)
                {
                    var solution = director.SolveProblem(problem, 16, 500, null, null, alpha, beta, 0.8);
                    Console.WriteLine($"Alpha: {alpha} | Beta: {beta} | Distance: {solution.totalDistance}");
                    if (solution.totalDistance < bestSolutionDistance)
                    {
                        bestSolutionDistance = solution.totalDistance;
                        bestSolution = solution;
                        bestAlpha = alpha;
                        bestBeta = beta;
                    }
                }
            }
        }

        Console.WriteLine($"Best Alpha: {bestAlpha} | Best Beta: {bestBeta} | Best Solution Distance: {bestSolutionDistance} | Best Solution Sequence: {String.Join(",", bestSolution.route)}");
        Assert.NotNull(1);
    }
}
