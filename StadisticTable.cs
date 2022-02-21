using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stadistics_Program_Outline
{
    class StatTable : IStadisticTable
    {
        readonly uint Decimals;
        private decimal[] DataArray { get; }
        public decimal Range { get; }
        public uint ClassCount { get; }
        public decimal ClassLenght { get; }
        private readonly IStadisticTable.Class<decimal>[] Classes;

        public decimal Average { get; }
        public decimal CentralData { get; }
        private IStadisticTable.Class<decimal> central;
        private IStadisticTable.Class<decimal> modal;
        public decimal Mediana { get; }
        public decimal Moda { get; }
        public decimal MediaArmonica { get; }

        public StatTable(decimal[] DataArray, uint decimals = 0)
        {
            this.DataArray = DataArray;
            this.Decimals = decimals;

            Range = DataArray.Max() - DataArray.Min();
            ClassCount = (uint)(1 + 3.3 * Math.Log(DataArray.Length, 10));
            ClassLenght = Math.Round(Range / ClassCount, (int)Decimals, MidpointRounding.ToPositiveInfinity);

            Classes = new IStadisticTable.Class<decimal>[ClassCount];
            AssignLimits();

            for (int i = 0; i < ClassCount; i++)
            {
                Classes[i].Frequency = Count(DataArray, Classes[i].ILimit, Classes[i].SLimit);
                Classes[i].RFrequency = Classes[i].Frequency * 100.0m / DataArray.Length;
                Classes[i].Mark = (Classes[i].ILimit + Classes[i].SLimit) / 2.0m;
                Average += Classes[i].Frequency * Classes[i].Mark;
            }

            Average /= DataArray.Length;
            CentralData = DataArray.Length / 2.0m;
            central = Classes[AcummIndexOf(CentralData)];
            Mediana = central.RILimit + ((DataArray.Length / 2.0m) - SumFrequencyUppon(AcummIndexOf(CentralData) - 1)) / central.Frequency * ClassLenght;

            modal = Classes[GetMaxFreqIndex()];
            decimal A1 = modal.Frequency - Classes[GetMaxFreqIndex() - 1].Frequency;
            decimal A2 = modal.Frequency - Classes[GetMaxFreqIndex() + 1].Frequency;
            Moda = modal.RILimit + A1 / (A1 + A2) * ClassLenght;
            MediaArmonica = GetMediaArmonica((int)ClassCount - 1);
        }

        decimal SumFrequencyUppon(int count, decimal buffer = 0)
        {
            return count < 0 ? buffer : SumFrequencyUppon(count - 1, Classes[count].Frequency + buffer);
        }

        decimal GetMediaArmonica(int count, decimal buffer = 0)
        {
            return count < 0 ? DataArray.Length / buffer : GetMediaArmonica(count - 1, (Classes[count].Frequency / Classes[count].Mark) + buffer);
        }

        public void AssignLimits()
        {
            Classes[0].ILimit = DataArray.Min();

            for (int i = 1; i < ClassCount; i++)
                Classes[i].ILimit = Classes[i - 1].ILimit + ClassLenght;

            for (int i = 0; i < ClassCount; i++)
                Classes[i].SLimit = Classes[i].ILimit + ClassLenght - (1.0m / (decimal)Math.Pow(10, Decimals));

            for (int i = 1; i < ClassCount; i++)
            {
                Classes[i].RILimit = (Classes[i - 1].SLimit + Classes[i].ILimit) / 2.0m;
                Classes[i - 1].RSLimit = Classes[i].RILimit;
            }

            Classes[0].RILimit = Classes[0].RSLimit - ClassLenght;
            Classes[ClassCount - 1].RSLimit = Classes[ClassCount - 1].RILimit + ClassLenght;

            
        }

        static uint Count(decimal[] array, decimal MinRange, decimal MaxRange)
        {
            uint count = 0;

            foreach (var item in array)
                if (item >= MinRange && item <= MaxRange)
                    count++;

            return count;
        }

        int AcummIndexOf(decimal value)
        {
            decimal temp = 0;

            for (int i = 0; i < Classes.Length; i++)
            {
                temp += Classes[i].Frequency;
                if (temp >= value)
                    return i;
            }

            return 0;
        }

        uint IndexOf(uint value)
        {
            for (uint i = 0; i < Classes.Length; i++)
                if (Classes[i].Frequency == value)
                    return i;

            return 0;
        }

        uint GetMaxFreqIndex()
        {
            uint temp = 0;

            foreach (var item in Classes)
                if (item.Frequency >= temp)
                    temp = item.Frequency;

            return IndexOf(temp);
        }

        bool CheckLimits()
        {
            foreach (var item in Classes)
                if ((item.SLimit - item.ILimit + (1.0m / (decimal)Math.Pow(10, Decimals))) != ClassLenght)
                    return false;

            return true;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"\n\nMax Value: {DataArray.Max()}\nMin Value: {DataArray.Min()}\nRange: {Range}\nClass Lenght: {ClassLenght}");
            Console.WriteLine($"\nClass" +
                $"\tLimits" +
                $"\t\t\tReal Limits" +
                $"\t\tClass Mark" +
                $"\tFrequency" +
                $"\tRelative Frequency");

            for (int i = 0; i < ClassCount; i++)
                Console.WriteLine($"  {i + 1}" +
                    $"\t{Classes[i].ILimit} - {Classes[i].SLimit}" +
                    $"\t\t{Classes[i].RILimit} - {Classes[i].RSLimit}" +
                    $"\t\t{Classes[i].Mark}" +
                    $"\t\t{Classes[i].Frequency}" +
                    $"\t\t{Math.Round(Classes[i].RFrequency, 2)}%");

            Console.WriteLine(CheckLimits() ? "\nLimits Check Passed" : "\nSomething is wrong with the limits... Did you select the correct mode?");

            Console.WriteLine($"\n\nCentralization measures:" +
                $"\nCentral Class: {AcummIndexOf(CentralData) + 1}" +
                $"\nModal Class: {GetMaxFreqIndex() + 1}" +
                $"\nCentral Data: {CentralData}" +
                $"\n\nAverage: {Math.Round(Average, 2)}" +
                $"\nMediana: {Mediana}" +
                $"\nModa: {Moda}" +
                $"\nGeometric Mid: Pending..." +
                $"\nArmonic Mid: {Math.Round(MediaArmonica, 2)}");

            Console.WriteLine("\nPress 'e' to continue...");
            Action action = null!;
            action = () =>
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.KeyChar != 'e')
                    action();
            };
            action();
        }
    }
}
