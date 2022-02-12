using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stadistics_Program_Outline
{
    public interface IStadisticTable
    {
        protected struct Class<T> where T : struct
        {
            public T ILimit { get; set; }
            public T SLimit { get; set; }
            public decimal RILimit { get; set; }
            public decimal RSLimit { get; set; }
            public decimal Mark { get; set; }
            public uint Frequency { get; set; }
            public decimal RFrequency { get; set; }
        }

        void AssignLimits();
        void ShowInfo();
    }
}
