using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stadistics_Program_Outline
{
    internal class StadisticTable
    {
        public int[] DataArray { get; }
        public int Range { get; }
        public int ClassCount { get; }
        public int ClassLenght { get; }

        private List<int>[] ClassLimits = new List<int>[2];
        private List<float>[] RealClassLimits = new List<float>[2];

        private List<int> ClassMark = new();

        private List<int> Frequency = new();
        private List<float> Relative_Frequency = new();

        public StadisticTable(int[] DataArray)
        {
            this.DataArray = DataArray;

            Range = DataArray.Max() - DataArray.Min();
            ClassCount = (int)(1 + 3.3 * Math.Log(DataArray.Length, 10));
            ClassLenght = (int)Math.Round((double)Range / ClassCount, 0, MidpointRounding.ToPositiveInfinity);

            ClassLimits[0] = new List<int>(new int[ClassCount]);
            ClassLimits[1] = new List<int>(new int[ClassCount]);
            RealClassLimits[0] = new List<float>(new float[ClassCount]);
            RealClassLimits[1] = new List<float>(new float[ClassCount]);

            ClassMark = new List<int>(new int[ClassCount]);

            Frequency = new List<int>(new int[ClassCount]);
            Relative_Frequency = new List<float>(new float[ClassCount]);

            AssignLimits();

            for (int i = 0; i < Frequency.Count; i++)
            {
                Frequency[i] = (int)Count(DataArray, (uint)ClassLimits[0][i], (uint)ClassLimits[1][i]);
                Relative_Frequency[i] = Frequency[i] * 100.0f / DataArray.Length;
                ClassMark[i] = (ClassLimits[0][i] + ClassLimits[1][i]) / 2;
            }
        }

        private void AssignLimits()
        {
            ClassLimits[0][0] = DataArray.Min();

            for (int i = 1; i < ClassCount; i++)
                ClassLimits[0][i] = ClassLimits[0][i - 1] + ClassLenght;

            for (int i = 0; i < ClassCount; i++)
                ClassLimits[1][i] = ClassLimits[0][i] + ClassLenght - 1;

            for (int i = 1; i < ClassCount; i++)
            {
                RealClassLimits[0][i] = (ClassLimits[1][i - 1] + ClassLimits[0][i]) / 2.0f;
                RealClassLimits[1][i - 1] = RealClassLimits[0][i];
            }

            RealClassLimits[0][0] = RealClassLimits[1][0] - ClassLenght;
            RealClassLimits[1][ClassCount - 1] = RealClassLimits[0][ClassCount - 1] + ClassLenght;
        }

        private uint Count(int[] array, uint MinRange, uint MaxRange)
        {
            uint count = 0;

            foreach (var item in array)
                if (item >= MinRange && item <= MaxRange)
                    count++;

            return count;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"\n\nClass\t Limits\t\t Real Limits\t Class Mark\t Frequency\t Relative Frequency");

            for (int i = 0; i < ClassCount; i++)
                Console.WriteLine($"  {i + 1}\t {ClassLimits[0][i]} - {ClassLimits[1][i]}\t {RealClassLimits[0][i]} - {RealClassLimits[1][i]}\t      {ClassMark[i]}    \t     {Frequency[i]}\t\t {Math.Round(Relative_Frequency[i], 2)}%");
        }
    }
}
