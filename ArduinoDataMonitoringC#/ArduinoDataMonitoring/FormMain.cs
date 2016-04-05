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

        public FormMain()
        {
            InitializeComponent();
            serialManager = new SerialManager(this);
            checkBox3.Checked = true;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            csvExporter = new CSVExporter();
        }

        private void Stop_Click(object sender, EventArgs e)
        {

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
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(ReadData);
                Invoke(method);
                return;
            }
            Console.WriteLine(serialManager.ReadData());
            textBox1.Text = serialManager.ReadData();
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
