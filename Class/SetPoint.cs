using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class SetPoint
    {
        /// <summary>
        /// Уставка холостого хода
        /// </summary>
        public float SetPointIdling {  get; set; }

        /// <summary>
        /// Уставка проката
        /// </summary>
        public float SetPointRollingMill { get; set;}

        /// <summary>
        /// Коэфициент сигма
        /// </summary>
        private readonly int coefficientK;

        private readonly Frequency frequency;

        private readonly ExpectedMath expectedMath;

        private readonly Dispersion dispersion;

        public SetPoint(Frequency frequency, ExpectedMath expectedMath, Dispersion dispersion)
        {
            this.frequency = frequency;
            this.expectedMath = expectedMath;
            this.dispersion = dispersion;
            coefficientK = 3;
            SetPointIdling = SetSetPointIdling();
            SetPointRollingMill = SetSetPointRollingMill();
            NewSetPoint();
        }

        private float SetSetPointRollingMill()
        {
            if (expectedMath.GetChecking() && dispersion.CheckingRatioValue)
            {
                return frequency.BinsArray[expectedMath.IndexRollingMill, 0] - (coefficientK * dispersion.SigmaRollingMill);
            }

            return 0.0f;          
        }

        private float SetSetPointIdling()
        {
            if (expectedMath.GetChecking() && dispersion.CheckingRatioValue)
            {
                return frequency.BinsArray[expectedMath.IndexIdling, 0] + (coefficientK * dispersion.SigmaIdling);
            }

            return 0.0f;
        }

        private void NewSetPoint()
        {
            if (SetPointRollingMill > 0.0f && SetPointIdling > 0.0f)
            {
                float pointXX = SetPointRollingMill - SetPointIdling;

                SetPointIdling += (pointXX * 0.6f);

                SetPointRollingMill -= (pointXX * 0.2f);
            }
        }
    }
}
