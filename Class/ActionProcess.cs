using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class ActionProcess
    {
        private Frequency frequency;
        private ExpectedMath expectedMath;
        private Dispersion dispersion;
        public SetPoint setPoint;

        public ActionProcess()
        {
            frequency = new Frequency(60);
            expectedMath = new ExpectedMath(frequency);
            dispersion = new Dispersion(frequency, expectedMath);
            setPoint = ActionCalculation();
            PrintSetPoint();
        }

        public SetPoint ActionCalculation()
        {
            for (int i = 0; i <= 3; i++)
            {
                PrintArrayFrequency();

                if (expectedMath.GetChecking() && dispersion.CheckingRatioValue)
                {
                    break;
                }                  
                else if (i == 0)
                {                    
                    frequency.ResizeFrequency(90);
                    expectedMath.ResizeExpectedMath();
                    dispersion.ResizeDispersion();
                }
                else if (i == 1)
                {                   
                    frequency.ResizeFrequency(60);
                    frequency.ResizeFrequency();
                    expectedMath.ResizeExpectedMath();
                    dispersion.ResizeDispersion();
                }
                else if (i == 2)
                {                    
                    frequency.ResizeFrequency(90);
                    expectedMath.ResizeExpectedMath();
                    dispersion.ResizeDispersion();
                }
            }


            return new SetPoint(frequency, expectedMath, dispersion);
        }


        public void PrintArrayFrequency()
        {
            int rows = frequency.IntervalCount;
            int column = frequency.BinsArray.Length / rows;

            Console.WriteLine($"Массив распределения:");
            Console.WriteLine($"Индекс\tИнтервал\tРаспределение");

            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"{i} - \t{frequency.BinsArray[i, 0]} - \t{frequency.BinsArray[i, 1]}");
            }

            Console.WriteLine();
            Console.WriteLine($"Мин: {frequency.MinValue}\tМакс: {frequency.MaxValue}\tИнтервал: {frequency.IntervalValue}\tПлотность: {frequency.densityVertex}");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine($"Индекс вершины холостого хода: {expectedMath.IndexIdling} - Индекс вершины проката:  {expectedMath.IndexRollingMill}");
            Console.WriteLine($"Соотношение точек вершины ХХ = {expectedMath.DensityIdling} - Соотношение точек вершины проката = {expectedMath.DensityRolling}");
            Console.WriteLine($"Плотность точек вершины ХХ = {expectedMath.SumDensityIdling} - Плотность точек вершины проката = {expectedMath.SumDensityRolling}");
            Console.WriteLine($"Сумма точек = {expectedMath.SumDensityArray}");
            Console.WriteLine();

            Console.WriteLine($"Сигма холостого хода: {dispersion.SigmaIdling} - Сигма проката: {dispersion.SigmaRollingMill}");

            float mo = frequency.BinsArray[expectedMath.IndexRollingMill, 0] - frequency.BinsArray[expectedMath.IndexIdling, 0];
            float g = (dispersion.SigmaRollingMill + dispersion.SigmaIdling) * 4;

            Console.WriteLine($"Проверка (МОхх - МОпр) >= (Gпр + Gхх) * 4 = " +
            $"({frequency.BinsArray[expectedMath.IndexRollingMill, 0]} - {frequency.BinsArray[expectedMath.IndexIdling, 0]}) >= " +
            $"({dispersion.SigmaRollingMill} + {dispersion.SigmaIdling}) * 4 = {dispersion.CheckingRatioValue}");

            Console.WriteLine($"----------------------------------------------------------------");
        }

        public void PrintSetPoint()
        {
            Console.WriteLine();
            Console.WriteLine($"Уставка холостого хода: {setPoint.SetPointIdling} - Уставка проката: {setPoint.SetPointRollingMill}");
            Console.WriteLine();

            Console.WriteLine("\nКонец");
            Console.ReadKey();
        }

    }
}
