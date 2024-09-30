using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class ExpectedMath
    {
        /// <summary>
        /// Индекс вершины холостого хода в массиве значений интервалов и распределений BinsArray экземпляра Frequency
        /// </summary>
        public int IndexIdling { get; set; }

        /// <summary>
        /// Индекс вершины проката в массиве значений интервалов и распределений BinsArray экземпляра Frequency
        /// </summary>
        public int IndexRollingMill { get; set; }

        /// <summary>
        /// Минимальное количество точек на отрезке для расчета
        /// </summary>
        public int StepPoint { get; set; }

        private readonly Frequency frequency;

        public ExpectedMath(Frequency frequency)
        {
            StepPoint = 3;
            this.frequency = frequency;
            //IndexVertex = SetVertex();
            SetVertex();
            GetDensity();
        }

        /// <summary>
        /// Метод выполняет проверку выполнения определения вершины холостого хода и вершины проката
        /// </summary>
        /// <returns>Возвращает True - вершины определены, False - вершины не определены</returns>
        public bool GetChecking()
        {
            return (IndexIdling != IndexRollingMill && CheckDensity(IndexIdling) && CheckDensity(IndexRollingMill));
        }

        /// <summary>
        /// Метод пересчитывает объект ExpectedMath по новым данным объекта Frequency
        /// </summary>
        public void ResizeExpectedMath()
        {
            SetVertex();
            GetDensity();
        }

        /// <summary>
        /// Метод получения точек холостого хода и проката
        /// </summary>
        /// <returns>Массив точек</returns>
        private void SetVertex()
        {
            int rowsBins = frequency.BinsArray.GetUpperBound(0) + 1;
            //int columnsBins = frequency.BinsArray.GetUpperBound(1) + 1;

            int resultTemp = -1;

            int[] tempArray = Array.Empty<int>();

            for (int i = 1; i < rowsBins - 1; i++)
            {
                if (frequency.BinsArray[i, 1] <= StepPoint)
                {
                    if (resultTemp == -1)
                        continue;
                }
                else
                {
                    if (resultTemp == -1)
                    {
                        resultTemp = i;
                    }
                    else if (frequency.BinsArray[i, 1] >= frequency.BinsArray[resultTemp, 1])
                    {
                        resultTemp = i;
                    }

                    if (i + 1 < rowsBins - 1)
                        continue;
                }

                if (resultTemp != -1)
                {
                    tempArray = tempArray.Append(resultTemp).ToArray();
                    resultTemp = -1;
                }
            }

            if (tempArray.Length < 2)
            {
                IndexIdling = tempArray[0];
                IndexRollingMill = tempArray[0];
            }
            else if (tempArray.Length > 2)
            {
                int[] tempArrayNew = new int[] { tempArray[0], tempArray[tempArray.Length - 1] };

                for (int i = 1; i < tempArray.Length; i++)
                {
                    if (frequency.BinsArray[tempArrayNew[0], 1] < frequency.BinsArray[tempArray[i], 1])
                    {
                        tempArrayNew[0] = tempArray[i];
                    }
                }

                for (int i = tempArray.Length - 1; i >= 0; i--)
                {
                    if (tempArrayNew[0] != tempArray[i])
                    {
                        if (frequency.BinsArray[tempArrayNew[1], 1] < frequency.BinsArray[tempArray[i], 1]
                        && frequency.BinsArray[tempArray[i], 1] != frequency.BinsArray[tempArrayNew[0], 1])
                        {
                            tempArrayNew[1] = tempArray[i];
                        }
                    }
                }

                if (tempArrayNew[0] < tempArrayNew[1])
                {
                    IndexIdling = tempArrayNew[0];
                    IndexRollingMill = tempArrayNew[1];
                }
                else
                {
                    IndexIdling = tempArrayNew[1];
                    IndexRollingMill = tempArrayNew[0];
                }
            }
            else
            {
                if (tempArray[0] < tempArray[1])
                {
                    IndexIdling = tempArray[0];
                    IndexRollingMill = tempArray[1];
                }
                else
                {
                    IndexIdling = tempArray[1];
                    IndexRollingMill = tempArray[0];
                }
            }
        }

        public float DensityIdling { get; set; }
        public float DensityRolling { get; set; }

        public float SumDensityIdling { get; set; }

        public float SumDensityRolling { get; set; }

        public float SumDensityArray { get; set; }

        private void GetDensity()
        {
            float sum = SumPointArray();

            SumDensityArray = sum;
            SumDensityIdling = SumDensity(IndexIdling);
            SumDensityRolling = SumDensity(IndexRollingMill);
            DensityIdling = SumDensity(IndexIdling) / sum;
            DensityRolling = SumDensity(IndexRollingMill) / sum;
        }

        private bool CheckDensity(int point)
        {
            float sumVertex = SumDensity(point);
            float sumPoint = SumPointArray();

            float coefficient = 0.1f;

            float result = sumVertex / sumPoint;

            return result >= coefficient;
        }

        private float SumPointArray()
        {
            float result = 0.0f;
            int rows = frequency.BinsArray.GetUpperBound(0) + 1;

            for (int i = 0; i < rows; i++)
            {
                result += frequency.BinsArray[i, 1];
            }

            return result;
        }

        private float SumDensity(int index)
        {
            int rows = frequency.BinsArray.GetUpperBound(0) + 1;
            float result = frequency.BinsArray[index, 1];

            for (int i = index + 1; i < rows; i++)
            {
                if (frequency.BinsArray[i, 1] <= StepPoint)
                {
                    result += frequency.BinsArray[i, 1];
                    break;
                }

                result += frequency.BinsArray[i,1];
            }

            for (int i = index - 1; i >= 0; i--)
            {
                if (frequency.BinsArray[i, 1] <= StepPoint)
                {
                    result += frequency.BinsArray[i, 1];
                    break;
                }

                result += frequency.BinsArray[i, 1];
            }

            return result;
        }

    }
}
