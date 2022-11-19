using System;
namespace oe_advanced_algorithms_assigment.Util
{
    public class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public static void Error(string message)
        {
            Console.WriteLine($"ERROR: {message}");
        }

        public static void Error(string message, Exception e)
        {
            Console.WriteLine($"ERROR: {message} \n{e.Message}");
        }
    }
}
