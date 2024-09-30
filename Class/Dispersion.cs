using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class Dispersion
    {
        /// <summary>
        /// Значение одной сигмы холостого хода 
        /// </summary>
        public float SigmaIdling {  get; set; }

        /// <summary>
        /// Значение одной сигмы проката 
        /// </summary>
        public float SigmaRollingMill { get; set; }

        /// <summary>
        /// Результат проверки выполнения сравнения True - результат устраивает, False - результат не устраивает (необходимо изменить условия расчета)
        /// </summary>
        public bool CheckingRatioValue {  get; set; }

        private readonly Frequency frequency;

        private readonly ExpectedMath expectedMath;

        public Dispersion(Frequency frequency, ExpectedMath expectedMath) 
        { 
            this.frequency = frequency;
            this.expectedMath = expectedMath;
            SigmaIdling = SetSigmaIdling();
            SigmaRollingMill = SetSigmaRollingMill();
            CheckingRatioValue = CheckingRatio();
        }

        /// <summary>
        /// Метод перерасчета объекта Dispersion по новым данным
        /// </summary>
        public void ResizeDispersion()
        {
            SigmaIdling = SetSigmaIdling();
            SigmaRollingMill = SetSigmaRollingMill();
            CheckingRatioValue = CheckingRatio();
        }

        /// <summary>
        /// Метод расчета одной сигма холостого хода
        /// </summary>
        /// <returns>Значение одной сигма холостого хода/returns>
        private float SetSigmaIdling()
        {
            int indexEnd = -1;

            for (int i = expectedMath.IndexIdling; i >= 0; i--)
            {
                if (frequency.BinsArray[i,1] <= expectedMath.StepPoint || i == 0)
                {
                    indexEnd = i; 
                    break;
                }
            }

            float sigma = (frequency.BinsArray[expectedMath.IndexIdling, 0] - frequency.BinsArray[indexEnd, 0]) / 3;

            return sigma;
        }

        /// <summary>
        /// Метод расчета одной сигма проката
        /// </summary>
        /// <returns>Значение одной сигма проката</returns>
        private float SetSigmaRollingMill()
        {
            if (!expectedMath.GetChecking())
            {
                return 0.0f;
            }

            int indexEnd = -1;
            int rowLength = frequency.BinsArray.GetUpperBound(0) + 1; 

            for (int i = expectedMath.IndexRollingMill; i < rowLength; i++)
            {
                if (frequency.BinsArray[i, 1] <= expectedMath.StepPoint || i == rowLength - 1)
                {
                    indexEnd = i;
                    break;
                }
            }

            //Console.WriteLine($"indexEnd = {indexEnd}");
            float sigma = (frequency.BinsArray[indexEnd, 0] - frequency.BinsArray[expectedMath.IndexRollingMill, 0]) / 3;

            return sigma;
        }

        /// <summary>
        /// Метод выполняет проверку выполнения сравнения 
        /// </summary>
        /// <returns>Возвращает True - результат устраивает, False - результат не устраивает (необходимо изменить условия расчета)</returns>
        private bool CheckingRatio()
        {
            if (!expectedMath.GetChecking())
            {
                return false;
            }

            float mo = frequency.BinsArray[expectedMath.IndexRollingMill, 0] - frequency.BinsArray[expectedMath.IndexIdling, 0];
            float g = (SigmaRollingMill + SigmaIdling) * 4;

            return mo >= g;
        }

    }
}
