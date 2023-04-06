using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace PacmanC_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isOpen = true;
            Console.CursorVisible = false;
            int maxScore = 149;
            char[,] map = ReadMap("map.txt");
            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            int pacmanX = 1, pacmanY = 1;
            int score = 0, enemyScore = 0;
            int[] enemyPosition = { 25, 4 };

            while (isOpen)
            {
                if (maxScore <= 0)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.White;
                    if (score > enemyScore) Console.WriteLine("Winner is you");
                    else if (score < enemyScore) Console.WriteLine("Winner is bot");
                    else Console.WriteLine("Tie.");
                    break;
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                DrawMap(map);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.Write('@');

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(enemyPosition[0], enemyPosition[1]);
                Console.Write('@');



                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(32, 0);
                Console.Write($"Score: {score}");
                Console.SetCursorPosition(32, 1);
                Console.Write($"Enemy score: {enemyScore}");
                pressedKey = Console.ReadKey();
                EnemyWalk(ref enemyPosition, map, ref maxScore, ref enemyScore);
                HandleInput(pressedKey, ref pacmanX, ref pacmanY, map, ref score, ref maxScore);
            }

        }

        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);

            char[,] map = new char[GetMaxLengthOfLine(file), file.Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = file[j][i];

                }
            }
            return map;
        }
        private static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (string line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }
            return maxLength;
        }
        private static void DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {

                    Console.Write(map[x, y]);
                }
                Console.WriteLine();
            }

        }
        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int pacmanX,
            ref int pacmanY, char[,] map, ref int score, ref int maxScore)
        {
            int[] direction = GetDirection(pressedKey);
            int nextPacmanPositionX = pacmanX + direction[0];
            int nextPacmanPositionY = pacmanY + direction[1];


            char nextCell = map[nextPacmanPositionX, nextPacmanPositionY];

            if (map[nextPacmanPositionX, nextPacmanPositionY] != '#')
            {
                pacmanX = nextPacmanPositionX;
                pacmanY = nextPacmanPositionY;

                if (map[nextPacmanPositionX, nextPacmanPositionY] == '.')
                {
                    score++;
                    maxScore--;
                    map[nextPacmanPositionX, nextPacmanPositionY] = ' ';
                }
            }
        }
        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                direction[1] = -1;
            }
            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                direction[1] = 1;
            }
            else if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                direction[0] = -1;
            }
            else if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                direction[0] = 1;
            }

            return direction;
        }
        private static int[] EnemyGetDirection()
        {
            Random rand = new Random();
            int[] direction = { 0, 0 };
            direction[0] = rand.Next(-1, 2);
            if (rand.Next(0, 2) == 0)
            {
                if (rand.Next(-1, 2) == -1)
                {
                    direction[0] = -1;
                }
                else if (rand.Next(-1, 2) == 1)
                {
                    direction[0] = 1;
                }
            }
            else if (rand.Next(0, 2) == 1)
            {
                if (rand.Next(-1, 2) == -1)
                {
                    direction[1] = -1;
                }
                else if (rand.Next(-1, 2) == 1)
                {
                    direction[1] = 1;
                }
            }

            return direction;
        }

        private static void EnemyWalk(ref int[] enemyPosition, char[,] map, ref int maxScore, ref int enemyScore)
        {
            int[] enemyDirection = EnemyGetDirection();
            int nextEnemyPositionX = enemyPosition[0] + enemyDirection[0];
            int nextEnemyPositionY = enemyPosition[1] + enemyDirection[1];

            if (map[nextEnemyPositionX, nextEnemyPositionY] != '#')
            {
                enemyPosition[0] = nextEnemyPositionX;
                enemyPosition[1] = nextEnemyPositionY;

                if (map[nextEnemyPositionX, nextEnemyPositionY] == '.')
                {
                    enemyScore++;
                    maxScore--;
                    map[nextEnemyPositionX, nextEnemyPositionY] = ' ';
                }
            }
        }
    }
}
