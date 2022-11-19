using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using oe_advanced_algorithms_assigment.Util;

namespace oe_advanced_algorithms_assigment.SmallestBoundaryPolygon
{
    public class StochasticHillClimbing
    {
        private List<Point> points;
        private List<Point> climbers;

        public StochasticHillClimbing()
        {
            points = new List<Point>();
            climbers = new List<Point>();
        }

        public void Solve(int numberOfClimbers, int iterationCount)
        {
            CreateClimbers(numberOfClimbers);

            int oldX;
            int oldY;
            double length;
            int i = 1;
            while (i <= iterationCount)
            {
                for (int j = 0; j < climbers.Count; j++)
                {
                    length = LengthOfBoundary(climbers);
                    oldX = climbers[j].X;
                    oldY = climbers[j].Y;
                    int newX = climbers[j].X + Utilities.Random.Next(-20, 20);
                    int newY = climbers[j].Y + Utilities.Random.Next(-20, 20);
                    if (length < LengthOfBoundary(climbers) || OuterDistanceToBoundary(climbers) > 0)
                    {
                        newX = oldX;
                        newY = oldY;
                    }
                    climbers[j].X = newX;
                    climbers[j].Y = newY;
                    Logger.Info($"Iteration #{i} Climber: #{j + 1} x = {climbers[j].X} y = {climbers[j].Y} Length of boundary = {length}");
                }

                i++;
            }
        }

        public void LoadInput(string path)
        {
            try
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    int[] coordinates = line.Split('\t').Select(x => int.Parse(x)).ToArray();
                    points.Add(new Point(coordinates[0], coordinates[1]));
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error reading input", e);
            }
        }

        private void CreateClimbers(int count)
        {
            int middleX = points.Sum(p => p.X) / points.Count;
            int middleY = points.Sum(p => p.Y) / points.Count;

            double maxDistance = 0;
            foreach (Point point in points)
            {
                double distance = Math.Sqrt(Math.Pow((middleX - point.X), 2) + Math.Pow((middleY - point.Y), 2));
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                }
            }

            double rad = maxDistance + 500;

            for (int i = 0; i < count; i++)
            {
                int newX = Convert.ToInt32(middleX + rad * Math.Sin((360 / count * i) * (Math.PI / 180)));
                int newY = Convert.ToInt32(middleY + rad * Math.Cos((360 / count * i) * (Math.PI / 180)));
                climbers.Add(new Point(newX, newY));
            }
        }

        private double LengthOfBoundary(List<Point> solution)
        {
            double sumLength = 0;

            for (int i = 0; i < solution.Count - 1; i++)
            {
                Point p1 = solution[i];
                Point p2 = solution[(i + 1) % solution.Count];
                sumLength += Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            }
            return sumLength;
        }

        private double OuterDistanceToBoundary(List<Point> solution)
        {
            double sumMinDistances = 0;

            for (int i = 0; i < points.Count; i++)
            {
                double minDistance = Double.MaxValue;
                for (int j = 0; j < solution.Count; j++)
                {
                    double actualDistance = DistanceFromLine(solution[j], solution[(j + 1) % solution.Count], points[i]);
                    if (j == 0 || actualDistance < minDistance)
                    {
                        minDistance = actualDistance;
                    }
                }
                if (minDistance < 0)
                {
                    sumMinDistances += -minDistance;
                }
            }

            return sumMinDistances;
        }

        private double DistanceFromLine(Point lp1, Point lp2, Point p)
        {
            return ((lp2.Y - lp1.Y) * p.X - (lp2.X - lp1.X) * p.Y + lp2.X * lp1.Y - lp2.Y * lp1.X)
                / Math.Sqrt(Math.Pow(lp2.Y - lp1.Y, 2) + Math.Pow(lp2.X - lp1.X, 2));
        }
    }
}
