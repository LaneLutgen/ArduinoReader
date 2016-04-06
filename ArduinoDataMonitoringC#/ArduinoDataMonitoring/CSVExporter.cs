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
        private List<string> dataList;
        private List<TimeSpan> times;
        private readonly string directoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DeionizerLogFiles\\";

        public CSVExporter()
        {
            filecount = 0;
            dataList = new List<string>();
            times = new List<TimeSpan>();
        }

        public void CreateCsvFile()
        {
            Directory.CreateDirectory(directoryPath);

            string filePath = directoryPath + "Log" + filecount +".csv";
            File.Create(filePath).Close();

            string delimiter = ",";
            string[][] output = new string[][]{
                dataList.ToArray()
            };
            int length = output.GetLength(0);
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));
            File.AppendAllText(filePath, sb.ToString());
        }

        public void AddData(string data)
        {
            dataList.Add(data);
        }

        public void AddTimestamp(TimeSpan currentTime)
        {
            times.Add(currentTime);
        }

        public void ClearData()
        {
            dataList.Clear();
        }
    }
}



