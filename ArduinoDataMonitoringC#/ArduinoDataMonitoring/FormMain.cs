using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public FormMain()
        {
            InitializeComponent();
            serialManager = new SerialManager(this);
            csvExporter = new CSVExporter();
            checkBox3.Checked = true;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            logging = true;
            start = DateTime.Now;
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            logging = false;
            csvExporter.CreateCsvFile();
        }

        private void Begin_Click(object sender, EventArgs e)
        {
            serialManager.OpenPort();
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
                }
                else if (data.Contains("s"))
                {
                    data = data.Substring(0, data.Length - 2);
                    textBox2.Text = data;
                }

                if (logging)
                {
                    csvExporter.AddData(data);
                    finish = DateTime.Now;
                    timeSpan = finish - start;
                    csvExporter.AddTimestamp(timeSpan);
                }
            }
            catch(ObjectDisposedException ex)
            {
                Console.WriteLine("Form was closed prematurely");
                Application.Exit();
            }  
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            serialManager.ClosePort();
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
    }
}
