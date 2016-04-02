using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace ArduinoDataMonitoring
{
    class SerialManager
    {
        private SerialPort mySerialPort = new SerialPort();
        private string priorValue;

        public SerialManager()
        {
            mySerialPort.BaudRate = 9600;
            mySerialPort.PortName = "COM3"; //may have to change this depending on the computer, could be an input
            mySerialPort.Open();
        }

        public void ChangePort(string port)
        {
            mySerialPort.PortName = port;
        }

        public void OpenPort()
        {
            if (!mySerialPort.IsOpen)
            {
                mySerialPort.Open();
            }
        }

        public void ClosePort()
        {
            mySerialPort.Close();
        }

        public bool IsOpen()
        {
            return mySerialPort.IsOpen;
        }
        
        public string ReadData()
        {
            if(mySerialPort.IsOpen)
            {
                priorValue = mySerialPort.ReadLine();
                return priorValue;
            }
            else
            {
                return priorValue;
            }
        }
    }
}
