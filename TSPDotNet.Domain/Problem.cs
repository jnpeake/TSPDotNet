
namespace TSPDotNet.Domain;
public record Problem(List<Location> Locations, int StartIndex, int EndIndex, DistanceMetric DistanceMetric);
