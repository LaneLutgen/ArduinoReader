using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArduinoDataMonitoring
{
    class CSVExporter
    {
        private int filecount;
        private List<string> pressureData;
        private List<string> salinityData;
        private List<TimeSpan> times;
        private readonly string directoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DeionizerLogFiles\\";

        public CSVExporter()
        {
            filecount = 0;
            pressureData = new List<string>();
            salinityData = new List<string>();
            times = new List<TimeSpan>();
        }

        public void CreateCsvFile()
        {
            Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "Log" + filecount +".csv";
            File.Create(filePath).Close();

            string delimiter = ",";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(delimiter, "Time", "Pressure (psi)", "Salinity (%)"));

            int index = 0;
            foreach(TimeSpan t in times)
            {
                try
                {
                    sb.AppendLine(string.Join(delimiter, times.ElementAt(index) ,pressureData.ElementAt(index), salinityData.ElementAt(index)));
                }
                catch(ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine("End of list");
                }
                ++index;
            }     
            File.AppendAllText(filePath, sb.ToString());
        }

        public void AddData(string data, bool isPressure)
        {
            if(isPressure)
            {
                pressureData.Add(data);
            }
            else
            {
                salinityData.Add(data);
            }
        }

        public void AddTimestamp(TimeSpan currentTime)
        {
            times.Add(currentTime);
        }

        public void ClearData()
        {
            pressureData.Clear();
        }
    }
}



