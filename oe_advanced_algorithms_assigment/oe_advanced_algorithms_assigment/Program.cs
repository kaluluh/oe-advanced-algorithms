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
            pathFinder.LoadInputData("Map.txt");
            pathFinder.Execute(50);

            StochasticHillClimbing polygonSolver = new StochasticHillClimbing();
            polygonSolver.LoadInputData("Points.txt");
            polygonSolver.Execute(3, 300);

            SimulatedAnnealing workAllocation = new SimulatedAnnealing();
            workAllocation.LoadInputData("Salary.txt");
            workAllocation.Execute(1000);

            Console.ReadKey();
        }
    }
}
