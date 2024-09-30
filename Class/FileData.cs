using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinitionSetPoint.Class
{
    internal class FileData
    {
        readonly string path;
        readonly string directory = "Resources";
        //readonly string nameFile = "Data_6.txt";
        readonly string nameFile = "Клеть N2.txt";
        readonly string nameRead = "DataReadFile_Клеть N2.txt";

        public FileData()
        {
            path = Directory.GetCurrentDirectory();
        }

        public float[] ReadDadaFile()
        {
            string pathFull = @$"{path}\{directory}\{nameFile}";

            float[] objects = Array.Empty<float>();

            using (StreamReader streamReader = new StreamReader(pathFull))
            {
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    objects = objects.Append(float.Parse(line)).ToArray();
                }
            }

            return objects;
        }

        public List<float> ReadDataReadFile()
        {
            string pathFull = @$"{path}\{directory}\{nameRead}";

            List<float> objects = new List<float>(); 

            using (StreamReader streamReader = new StreamReader(pathFull))
            {
                string? line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    objects.Add(float.Parse(line));
                }
            }

            return objects;
        }


        public async Task<object[]> ReadDadaFileAsync()
        {
            string pathFull = @$"{path}\{directory}\{nameFile}";

            object[] objects = Array.Empty<object>();

            using (StreamReader streamReader = new StreamReader(pathFull))
            {
                object? line;

                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    objects = objects.Append(line).ToArray();
                }
            }

            return objects;           
        }

       
    }
}
