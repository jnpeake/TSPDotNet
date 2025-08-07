using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPDotNet.Domain;

public class SolutionRoute
{
    public List<int> route { get; set; } = new List<int>();
    public double totalDistance { get; set; }
}

