using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace ArduinoDataMonitoring
{
    class SerialManager
    {
        private SerialPort mySerialPort = new SerialPort();
        private string priorValue;
        private FormMain form;

        public SerialManager()
        {
            mySerialPort.BaudRate = 9600;
            mySerialPort.PortName = "COM3"; //default port
        }

        public SerialManager(FormMain form): base()
        {
            this.form = form;
        }

        public void ChangePort(string port)
        {
            mySerialPort.Close();
            mySerialPort.PortName = port;
        }

        public void OpenPort()
        {
            if (!mySerialPort.IsOpen)
            {
                try
                {
                    mySerialPort.Open();
                }
                catch(System.IO.IOException e)
                {
                    form.ShowMessage("Could not open port, please make sure the correct port is selected and that the Arduino is plugged in.");
                }  
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
