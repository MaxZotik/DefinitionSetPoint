using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class Crate
    {
        public float SetPointCrateIdling { get; private set; }

        public float SetPointCrateRolling { get; private set; }

        public ModeCrates SetModeCrate { get; private set; }



        public List<float> ValuesList { get; set; }


        public Crate()
        {
            ValuesList = new FileData().ReadDataReadFile();
            SetPointCrateIdling = 0.0f;
            SetPointCrateRolling = 0.0f;
            SetModeCrate = ModeCrates.Stop;

            DefineModeCrate();
        }

        public void DefineModeCrate()
        {
            ModeCrates modeCratesTemp = ModeCrates.Idling;
            ModeCrates modeCratesOldTemp = ModeCrates.Idling;

            for (int i = 0; i < ValuesList.Count; i++)
            {
                float value = ValuesList[i];

                if (SetModeCrate == ModeCrates.Stop)
                {
                    SetPointCrateIdling = 0.0f;
                    SetPointCrateRolling = 0.0f;
                    SetModeCrate = ModeCrates.NoData;
                }
                else if (SetPointCrateIdling == 0.0f && SetPointCrateRolling == 0.0f)
                {
                    SetModeCrate = ModeCrates.NoData;
                }
                else if ((SetPointCrateRolling * 3) < value)
                {
                    SetModeCrate = ModeCrates.Rolling;
                }
                else if(SetPointCrateIdling < value && SetPointCrateRolling > value)
                {
                    modeCratesTemp = ModeCrates.NoMode;
                }
                else if(SetPointCrateIdling > value)
                {
                    modeCratesTemp = ModeCrates.Idling;
                }
                else if (SetPointCrateRolling < value)
                {
                    modeCratesTemp = ModeCrates.Rolling;
                }



                if (SetModeCrate != modeCratesTemp && SetModeCrate != modeCratesOldTemp)
                {
                    SetModeCrate = modeCratesOldTemp;
                }

                modeCratesOldTemp = modeCratesTemp;

                Console.WriteLine($"{value}\t - {SetModeCrate}");
            }

            Console.ReadKey();
        }

    }
}
