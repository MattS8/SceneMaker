using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.Model.Routine
{
    
    internal class RoutineData
    {
        public static readonly int NUM_PINS = 19;

        public string RoutineName;
        public PinData[] Pin = new PinData[NUM_PINS];
        public List<Tuple<double, double>>[] OnOffTimes = new List<Tuple<double, double>>[NUM_PINS];
        public int TrainInitializationDuration = 0;
        public int RoutineRandomizerSeed = 0;
        public int InitialAllLightsOnDuration = 0;

        public RoutineData(string routineName = "", PinData[] pinData = null, List<Tuple<double, double>>[] onOffTimes = null,
            int trainInitializationDuraiton = 0, int routineRandomizerSeed = 0, int initialAllLightsOnDuration = 0)
        {
            RoutineName = routineName;
            TrainInitializationDuration = trainInitializationDuraiton;
            RoutineRandomizerSeed = routineRandomizerSeed;
            InitialAllLightsOnDuration = initialAllLightsOnDuration;

            for (int i = 0; i < NUM_PINS; i++)
            {
                Pin[i] = (pinData != null && pinData.Length > i) ? pinData[i] : new PinData();
                OnOffTimes[i] = (onOffTimes != null && onOffTimes.Length > i) ? onOffTimes[i] : new List<Tuple<double, double>>(); 
            }
        }

    }
}
