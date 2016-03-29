using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace ArduinoDataMonitoring
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());

            ReadSerial();
        }

        static void ReadSerial()
        {
            SerialPort mySerialPort = new SerialPort();
            mySerialPort.BaudRate = 9600;
            mySerialPort.PortName = "COM3"; //may have to change this depending on the computer, could be an input
            mySerialPort.Open();

            string data;
            while(true)
            {
                data = mySerialPort.ReadLine();
                Console.WriteLine(data);
            }
        }

    }
}
