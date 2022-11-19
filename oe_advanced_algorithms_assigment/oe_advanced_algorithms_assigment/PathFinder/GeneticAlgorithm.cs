using System;
using System.IO;
using oe_advanced_algorithms_assigment.Util;

namespace oe_advanced_algorithms_assigment.PathFinder
{
    public class GeneticAlgorithm
    { 
        const int MAX_INSTRUCTION_CODE = 59;
        const int TIME_LIMIT = 1000;

        private int[,] map;
        private int direction = 1;
        private int positionX;
        private int positionY;
        private int startXPosition;
        private int startYPosition;
        private int exitXPosition;
        private int exitYPosition;

        private int populationSize;
        private int codeLength;
        private int[][] machineCodes;

        public GeneticAlgorithm(int populationSize, int codeLength)
        {
            this.populationSize = populationSize;
            this.codeLength = codeLength;
            machineCodes = new int[populationSize][];
            for (int i = 0; i < machineCodes.Length; i++)
            {
                machineCodes[i] = new int[codeLength];
            }
        }

        public void Execute(int iterationCount)
        {
            InitializePopulation();
            for (int i = 0; i < iterationCount; i++)
            {
                SortMachineCodes();
                Mate(10, 50);
                Mutate(50, 5);
                string best = "";
                foreach (int c in machineCodes[0])
                {
                    best += c;
                }
                Logger.Info($"#{i + 1} Code: {best}");
            }
        }

        public void LoadInputData(string path)
        {
            try
            {
                string[] lines = File.ReadAllLines(path);
                map = new int[lines.Length, codeLength];
                int x = 0;
                int y = 0;
                foreach (string line in lines)
                {
                    x = 0;
                    foreach (char c in line)
                    {
                        if (c == '*')
                        {
                            map[y, x] = 1;
                        }
                        else
                        {
                            map[y, x] = 0;
                            if (c == 'S')
                            {
                                startXPosition = x;
                                startYPosition = y;
                            }
                            else if (c == 'E')
                            {
                                exitXPosition = x;
                                exitYPosition = y;
                            }
                        }
                        x++;
                    }
                    y++;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Error reading input", e);
            }
        }

        private void Mutate(int from, int mutationRate)
        {
            int random = 0;
            for (int i = from; i < machineCodes.Length; i++)
            {
                for (int j = 0; j < mutationRate; j++)
                {
                    random = Utilities.Random.Next(0, machineCodes[i].Length);
                    machineCodes[i][random] = Utilities.Random.Next(0, MAX_INSTRUCTION_CODE + 1);
                }
            }
        }

        private void Mate(int from, int max)
        {
            int random1 = 0;
            int random2 = 0;
            int k = 0;
            for (int i = from; i < machineCodes.Length / 2; i++)
            {
                random1 = Utilities.Random.Next(0, machineCodes.Length - max);
                random2 = Utilities.Random.Next(0, max);
                for (int j = random1; j < machineCodes[i].Length; j++)
                {
                    machineCodes[i][j] = machineCodes[random2][j];
                }
                random2 = Utilities.Random.Next(0, max);
                k = 0;
                for (int j = 0; j < machineCodes[i].Length; j++)
                {
                    if (!ContainsGene(machineCodes[i], machineCodes[random2][j]))
                    {
                        machineCodes[i][k] = machineCodes[random2][j];
                        k++;
                    }
                }
            }
        }

        private bool ContainsGene(int[] codeHeads, int gene)
        {
            int i = 0;
            while (i < codeHeads.Length && codeHeads[i] != gene)
            {
                i++;
            }
            return i < codeHeads.Length;
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < machineCodes.Length; i++)
            {
                for (int j = 0; j < machineCodes[i].Length; j++)
                {
                    machineCodes[i][j] = Utilities.Random.Next(1, MAX_INSTRUCTION_CODE + 1);
                }
            }
        }

        private void SortMachineCodes()
        {
            int[] temp = new int[machineCodes.Length];
            for (int i = 0; i < machineCodes.Length; i++)
            {
                for (int j = i; j < machineCodes.Length; j++)
                {
                    if (Objective(machineCodes[i]) > Objective(machineCodes[j]))
                    {
                        temp = machineCodes[i];
                        machineCodes[i] = machineCodes[j];
                        machineCodes[j] = temp;
                    }
                }
            }
        }

        private double Objective(int[] code)
        {
            Execute(code);
            return DistanceToExit();
        }

        private double DistanceToExit()
        {
            if (IsExit())
            {
                return 0;
            }
            return Math.Sqrt(Math.Pow(positionX - exitXPosition, 2) + Math.Pow(positionY - exitYPosition, 2));
        }

        private bool IsExit()
        {
            return positionX == exitXPosition && positionY == exitYPosition;
        }

        private void Execute(int[] code)
        {
            Start();
            int t = 0;
            uint ip = 0;
            while (!IsExit() && t < TIME_LIMIT && ip < code.Length)
            {
                int cmd = code[ip];
                switch (cmd / 10)
                {
                    case 0:
                        ip -= (uint)(cmd % 10 + 1);
                        break;
                    case 1:
                        ip += (uint)(cmd % 10 - 1);
                        break;
                    case 2:
                        MoveForward();
                        break;
                    case 3:
                        TurnLeft();
                        break;
                    case 4:
                        TurnRight();
                        break;
                    case 5:
                        if (IsWallForward())
                            ip++;
                        break;
                    default:
                        break;
                }
                ip++;
                t++;
            }
        }

        private void Start()
        {
            positionX = startXPosition;
            positionY = startYPosition;
        }

        private void TurnLeft()
        {
            direction = (direction - 1 + 4) % 4;
        }

        private void TurnRight()
        {
            direction = (direction + 1) % 4;
        }

        private void MoveForward()
        {
            if (!IsWallForward())
            {
                ForwardPosisiton(ref positionX, ref positionY);
            }
        }

        private bool IsWallForward()
        {
            int nextX = 0;
            int nextY = 0;
            ForwardPosisiton(ref nextX, ref nextY);
            return map[nextY, nextX] == 1;
        }

        private void ForwardPosisiton(ref int x, ref int y)
        {
            x = positionX + ((direction == 1 || direction == 3) ? -(direction - 2) : 0);
            y = positionY + ((direction == 0 || direction == 2) ? (direction - 1) : 0);
        }
    }
}
