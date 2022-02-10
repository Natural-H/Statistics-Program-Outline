using System;
using System.Collections.Generic;
using System.Linq;

namespace Stadistics_Program_Outline
{
    static class Program
    {
        enum Types { Int, Decimal1, Decimal2 }

        static void Main()
        {
            Console.Clear();
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Testing  a  fancy  title"));
            Console.WriteLine("Do you need integer numbers or decimal numbers?\n" +
                "Press the key to select option:" +
                "\n 1 -> Integers" +
                "\n 2 -> Decimals" +
                "\n 3 -> Decimal with 0.01 of presition" +
                "\n\n Esc -> Close program");

            Action action = null!;     // For some reason, I need to assign de value two times.
            action = () =>
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.KeyChar)
                {
                    case '1':
                        CollectData(Types.Int);
                        Main();
                        break;
                    case '2':
                        Console.WriteLine("Working with decimals (0.1 of presition)");
                        CollectData(Types.Decimal1);
                        Main();
                        break;
                    case '3':
                        Console.WriteLine("Working with decimals (0.01 of presition)");
                        CollectData(Types.Decimal2);
                        Main();
                        break;
                    case (char)27:
                        return;
                    default:
                        action();
                        break;
                }
            };
            action();
        }

        static void CollectData(Types type)
        {
            Console.Clear();
            Console.WriteLine("How many data do you have? ");

            switch (type)
            {
                case Types.Int:
                    int[] IntArray = new int[Convert.ToInt32(Console.ReadLine())];

                    Console.WriteLine();
                    for (int i = 0; i < IntArray.Length; i++)
                    {
                        Console.Write($"Give me data {i + 1}: ");
                        IntArray[i] = Convert.ToInt32(Console.ReadLine());
                    }

                    StatTable statTable = new(IntArray);
                    statTable.ShowInfo();
                    break;
                case Types.Decimal1:
                    decimal[] DecArray = new decimal[Convert.ToInt32(Console.ReadLine())];

                    Console.WriteLine();
                    for (int i = 0; i < DecArray.Length; i++)
                    {
                        Console.Write($"Give me data {i + 1}: ");
                        DecArray[i] = Convert.ToDecimal(Console.ReadLine());
                    }

                    DecStatTable decStatTable = new(DecArray);
                    decStatTable.ShowInfo();
                    break;
                case Types.Decimal2:
                    decimal[] DecArray1 = new decimal[Convert.ToInt32(Console.ReadLine())];

                    Console.WriteLine();
                    for (int i = 0; i < DecArray1.Length; i++)
                    {
                        Console.Write($"Give me data {i + 1}: ");
                        DecArray1[i] = Convert.ToDecimal(Console.ReadLine());
                    }

                    DecStatTable decStatTable1 = new(DecArray1, 2);
                    decStatTable1.ShowInfo();
                    break;
                default:
                    break;
            };
        }
    }
}