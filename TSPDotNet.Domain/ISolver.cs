namespace TSPDotNet.Domain
{
    public interface ISolver
    {
        SolutionRoute Solve(Problem inputProblem);
    }
}
