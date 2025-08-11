﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPDotNet.Domain.Calculators;
public class EuclideanCalculator : IDistanceCalculator
{
    public double CalculateDistance(double x1, double y1, double x2, double y2)
    {
        double deltaX = x2 - x1;
        double deltaY = y2 - y1;
        return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
