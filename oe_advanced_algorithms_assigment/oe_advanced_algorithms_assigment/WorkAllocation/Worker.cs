using System;
namespace oe_advanced_algorithms_assigment.WorkAllocation
{
    public class Worker
    {
        public int Salary { get; private set; }
        public double Quality { get; private set; }

        public Worker(int salary, double quality)
        {
            this.Salary = salary;
            this.Quality = quality;
        }
    }
    
}
