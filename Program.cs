using System;
using System.Collections.Generic;
using System.Linq;

namespace Stadistics_Program_Outline
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many data do you have? ");
            int[] JustData = new int[Convert.ToInt32(Console.ReadLine())];

            Console.WriteLine();
            for (int i = 0; i < JustData.Length; i++)
            {
                Console.Write($"Give me data {i + 1}: ");
                JustData[i] = Convert.ToInt32(Console.ReadLine());
            }

            StadisticTable stadisticTable = new(JustData);
            stadisticTable.PrintInfo();
        }
    }
}
