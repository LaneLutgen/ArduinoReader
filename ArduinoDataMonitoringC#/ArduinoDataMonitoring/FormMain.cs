using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoDataMonitoring
{
    public partial class FormMain : Form
    {
        private SerialManager serialManager;
        private CSVExporter csvExporter;
        private volatile bool logging;
        private TimeSpan timeSpan;
        private DateTime start;
        private DateTime finish;
        private volatile bool isPressure;

        private int salinityZeroValueCount = 0; // used to differentiate an actual 0 value from a digital low on the PWM

        public FormMain()
        {
            InitializeComponent();
            serialManager = new SerialManager(this);
            csvExporter = new CSVExporter();
            checkBox3.Checked = true;
            label6.Visible = false;
            label7.Visible = false;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            logging = true;
            start = DateTime.Now;
            label7.Visible = true;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            logging = false;
            label7.Visible = false;
            csvExporter.CreateCsvFile();
        }

        private void Begin_Click(object sender, EventArgs e)
        {
            serialManager.OpenPort();
            label6.Visible = true;
            Thread t = new Thread(new ThreadStart(ReadDataAsync));
            t.Start();
        }

        private void ReadDataAsync()
        {
            while (serialManager.IsOpen())
            {
                ReadData();
            }
        }

        private void ReadData()
        {
            bool isDigitalLow = false;

            try
            {
                if (InvokeRequired)
                {
                    MethodInvoker method = new MethodInvoker(ReadData);
                    Invoke(method);
                    return;
                }
                string data = serialManager.ReadData();
                if (data.Contains("p"))
                {
                    data = data.Substring(0, data.Length - 2);
                    textBox1.Text = data;
                    isPressure = true;
                }
                else if (data.Contains("s"))
                {
                    //Data needs to be overridden now that the salinity probe is read through a multimeter
                    //data = data.Substring(0, data.Length - 2);
                    data = textBox2.Text;
                    //if(!data.Equals("0.00") || salinityZeroValueCount > 20)
                    //{
                    //    textBox2.Text = data;
                    //    isDigitalLow = true;
                    //    salinityZeroValueCount = 0;
                    //}
                    //else
                    //{
                    //    isDigitalLow = false;
                    //    salinityZeroValueCount++;
                    //}
                    isPressure = false;
                }

                if (logging && !isDigitalLow)
                {
                    csvExporter.AddData(data, isPressure);
                    finish = DateTime.Now;
                    timeSpan = finish - start;
                    csvExporter.AddTimestamp(timeSpan);
                    
                }
            }
            catch(ObjectDisposedException ex)
            {
                Console.WriteLine("Form was closed prematurely");
                Process.GetCurrentProcess().Kill();
            }  
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            serialManager.ClosePort();
            label6.Visible = false;
        }

        private void COM1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                serialManager.ChangePort("COM1");
            }
        }

        private void COM2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                serialManager.ChangePort("COM2");
            }
        }

        private void COM3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox2.Checked = false;
                checkBox1.Checked = false;
                checkBox4.Checked = false;
                serialManager.ChangePort("COM3");
            }
        }

        private void COM4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox1.Checked = false;
                serialManager.ChangePort("COM4");
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string currentText = textBox2.Text;
            double amps;
            try
            {
                amps = Double.Parse(currentText);
            }
            catch(FormatException ex)
            {
                amps = 0;
                ShowMessage("Invalid input");
            }

            int percentage = (int)(2.5027946838902 * amps - 14.215935908583);

            if(percentage < 0)
            {
                percentage = 0;
            }

            textBox2.Text = percentage.ToString();
        }
    }
}
