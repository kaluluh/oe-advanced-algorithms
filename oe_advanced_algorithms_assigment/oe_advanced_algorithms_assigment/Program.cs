using System;
using oe_advanced_algorithms_assigment.PathFinder;
using oe_advanced_algorithms_assigment.SmallestBoundaryPolygon;
using oe_advanced_algorithms_assigment.WorkAllocation;

namespace oe_advanced_algorithms_assigment
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm pathFinder = new GeneticAlgorithm(100, 10);
            pathFinder.LoadInput("Map.txt");
            pathFinder.Solve(50);

            StochasticHillClimbing polygonSolver = new StochasticHillClimbing();
            polygonSolver.LoadInput("Points.txt");
            polygonSolver.Solve(3, 300);

            SimulatedAnnealing workAllocation = new SimulatedAnnealing();
            workAllocation.LoadInput("Salary.txt");
            workAllocation.Solve(1000);

            Console.ReadKey();
        }
    }
}
