using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPDotNet.Domain.Calculators;
public class HaversineCalculator : IDistanceCalculator
{
    public double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        const double R = 6371.0; // Earth radius in kilometers. Use 3958.8 for miles.

        double dLat = ToRadians(x2 - x1);
        double dLon = ToRadians(y2 - y1);

        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(ToRadians(x1)) * Math.Cos(ToRadians(x2)) *
                   Math.Pow(Math.Sin(dLon / 2), 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return Double.Round(R * c, 3);
    }

    private static double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180.0);
    }
}
