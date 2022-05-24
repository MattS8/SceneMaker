using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.Model.Routine
{
    internal class PinData
    {
        public static readonly int D2 = 0;
        public static readonly int D3 = 1;
        public static readonly int D4 = 2;
        public static readonly int D5 = 3;
        public static readonly int D6 = 4;
        public static readonly int D7 = 5;
        public static readonly int D8 = 6;
        public static readonly int D9 = 7;
        public static readonly int D10 = 8;
        public static readonly int D11 = 9;
        public static readonly int D12 = 10;
        public static readonly int D13 = 11;

        public static readonly int A0 = 12;
        public static readonly int A1 = 13;
        public static readonly int A2 = 14;
        public static readonly int A3 = 15;
        public static readonly int A4 = 16;
        public static readonly int A5 = 17;   
        public static readonly int A6 = 18;
        public static readonly int A7 = 19;

        public string PinName;
        public bool IsPinEnabled;
        public bool IsTrainPin;
        public PinType Type;

        public PinData(string pinName = "", bool isPinEnabled = false, bool isTrainPin = false, PinType type = PinType.Light)
        {
            PinName = pinName;
            IsPinEnabled = isPinEnabled;
            IsTrainPin = isTrainPin;
            Type = type;
        }
     }

    public enum PinType
    {
        Light,
        Train,
        PowerOrSkip,
        Volume,
        MotionSensor
    }
}
