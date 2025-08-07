using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPDotNet.Domain.Calculators;
public interface IDistanceCalculator
{
    double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
}
