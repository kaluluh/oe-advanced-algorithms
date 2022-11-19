using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using oe_advanced_algorithms_assigment.Util;

namespace oe_advanced_algorithms_assigment.WorkAllocation
{
    public class SimulatedAnnealing
    {
        private const double K = 1.0 / 100000;
        private const double EPSILON = 0.001;
        private const double T_EPSILON = 0.001;

        private double temperature = 10000;
        private int requestedTime;

        List<Worker> workers;
        List<double> currentSolutions;

        public SimulatedAnnealing()
        {
            workers = new List<Worker>();
        }

        public void Execute(int iterationCount)
        {
            for (int i = 0; i < iterationCount; i++)
            {
                foreach (int rate in currentSolutions)
                {
                    List<double> nextSolutions = this.GetNextSolutions(rate);
                    double currentFitness = CalculateFitness(currentSolutions);
                    double nextFitness = CalculateFitness(nextSolutions);
                    double diff = nextFitness - currentFitness;
                    if (diff < 0)
                    {
                        if (nextFitness < currentFitness)
                        {
                            currentSolutions = nextSolutions;
                        }
                        break;
                    }
                    else if (diff > 1)
                    {
                        double temperature = GenerateTemperature();
                        if (Utilities.Random.NextDouble() < this.GenerateProbability(diff))
                        {
                            currentSolutions = nextSolutions;
                            break;
                        }
                    }
                }
                Logger.Info($"#{i + 1} Salary: {SumSalary(currentSolutions)} Quality: {AvarageQuality(currentSolutions)}");
            }
        }

        public void LoadInputData(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                requestedTime = int.Parse(lines[0]);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split('\t');
                    workers.Add(new Worker(int.Parse(parts[0]), double.Parse(parts[1])));
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error reading input", e);
            }

            currentSolutions = GenerateRandomSolutions();
        }

        private double GenerateTemperature()
        {
            return temperature * (1 - T_EPSILON);
        }

        private double GenerateProbability(double difference)
        {
            return Math.Pow(Math.E, -difference / (K * temperature));
        }

        private double SumSalary(List<double> solution)
        {
            double sum = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                sum += solution[i] * workers[i].Salary;
            }

            return sum;
        }

        private double AvarageQuality(List<double> solution)
        {
            double sum = 0;
            for (int i = 0; i < solution.Count; i++)
            {
                sum += solution[i] * workers[i].Quality;
            }

            return sum / requestedTime;
        }

        private double CalculateFitness(List<double> solution)
        {
            return SumSalary(solution) / AvarageQuality(solution);
        }

        private List<double> GenerateRandomSolutions()
        {
            List<double> result = new List<double>();
            double sum = requestedTime;
            for (int i = 0; i < workers.Count; i++)
            {
                if (i + 1 == workers.Count)
                {
                    result.Add(sum);
                }
                else
                {
                    double value = Utilities.Random.NextDouble() * sum / 2;
                    sum -= value;
                    result.Add(value);
                }
            }

            return result;
        }
        private int GetNextRate(int s)
        {
            return (int)(Utilities.Random.NextDouble() >= 0.5 ?
            (s + EPSILON <= requestedTime ? s + EPSILON : requestedTime)
            : ((s - EPSILON) > 0 ? s - EPSILON : 0));
        }

        private List<double> GetNextSolutions(int rate)
        {
            int newRate = this.GetNextRate(rate);
            double diff = rate - newRate + requestedTime - currentSolutions.Sum();
            double sum = requestedTime - rate;

            List<double> newSolutions = new List<double>(currentSolutions);
            for (int i = 0; i < newSolutions.Count; i++)
            {
                if (newSolutions[i] == rate)
                {
                    newSolutions[i] = newRate;
                }
            }

            for (int i = 0; i < workers.Count; i++)
            {
                double element = newSolutions[i];
                if (element != newRate)
                {
                    int value = (int)(element + (diff * element / sum));
                    for (int j = 0; j < newSolutions.Count; j++)
                    {
                        if (newSolutions[i] == element)
                        {
                            newSolutions[i] = value;
                        }
                    }
                }
            }

            return newSolutions;
        }
    }
}
