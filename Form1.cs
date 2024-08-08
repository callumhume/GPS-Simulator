using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace GPSSimulator
{
    public partial class Form1 : Form
    {

        string selectedCOMPort = "COM1";
        int selectedBaudRate = 38400;
        int selectedFixRate = 5;
        string newline = "\r\n";
        Timer gpsFixTimer;
        double startLatitude = 42.255637;
        double startLongitude = -85.661945;
        double targetSpeed = 2.0; //  km/h // TODO: start/stop button that changes this target in the backend but leaves the original value visible to the user.  Selected target vs current target??
        double acceleration = 0.2; // km/sec
        double speed = 0.0;
        double bearing = 45.0;

        public Form1()
        {
            InitializeComponent();
            gpsFixTimer = new Timer();
            gpsFixTimer.Interval = 1000 / selectedFixRate;
            gpsFixTimer.Tick += new EventHandler(handleGPSCalculations);

            populateComboBoxLists();

            setupMapChart();

            setMapPosition(startLatitude, startLongitude);

            // TODO: Select previous settings

            // TODO: Create GPS logic

            // TODO: Create output logic (autoconnect at beginning)

            gpsFixTimer.Start();

            // TODO: Create position control logic
        }

        private void handleGPSCalculations(object sender, EventArgs e)
        {
            outputGPSFix();
            calculateNextGPSFix();
        }

        private void outputGPSFix()
        {
            // TODO: Create an actual GGA sentence
            serialPrintLine(DateTime.Now.TimeOfDay.ToString());
        }

        private void calculateNextGPSFix()
        {

        }

        private void populateComboBoxLists()
        {
            string[] COMPorts = SerialPort.GetPortNames();

            for (int i = 0; i < COMPorts.Length; i++)
            {
                Console.WriteLine(COMPorts[i]); // TODO: Only print out with some debug mode?
                comboBox_COMSelector.Items.Add(COMPorts[i]);
            }

            comboBox_COMSelector.SelectedIndex = 0;
            comboBox_COMSelector.Refresh();


            int[] baudRates = { //110,
                                //300,
                                //600,
                                //1200,
                                //2400,
                                //4800,
                                9600,
                                //14400,
                                19200,
                                38400,
                                //57600,
                                115200,
                                //230400,
                                //460800,
                                //921600
                                };

            for (int i = 0; i < baudRates.Length; i++)
            {
                Console.WriteLine(baudRates[i].ToString());
                comboBox_BaudSelector.Items.Add(baudRates[i]);
            }

            comboBox_BaudSelector.SelectedIndex = 3;
            comboBox_BaudSelector.Refresh();


            int[] fixRates = { 1, 2, 4, 5, 10, 20 };

            for (int i = 0; i < fixRates.Length; i++)
            {
                Console.WriteLine(fixRates[i].ToString());
                comboBox_FixRateSelector.Items.Add(fixRates[i]);
            }

            comboBox_FixRateSelector.SelectedIndex = 3;
            comboBox_FixRateSelector.Refresh();


            string[] newlineOptions = { "CR", "LF", "CRLF" };

            for (int i = 0; i < newlineOptions.Length; i++)
            {
                Console.WriteLine(newlineOptions[i].ToString());
                comboBox_NewlineSelector.Items.Add(newlineOptions[i]);
            }

            comboBox_NewlineSelector.SelectedIndex = 2;
            comboBox_NewlineSelector.Refresh();
        }

        private void setupMapChart()
        {
            chart1.Series.Add("Position");
            chart1.Series.FindByName("Position").ChartType = SeriesChartType.Point;
            chart1.Series.FindByName("Position").ChartArea = "ChartArea1";
            chart1.Series.Add("Trail");
            chart1.Series.FindByName("Trail").ChartType = SeriesChartType.Line;
            chart1.Series.FindByName("Trail").ChartArea = "ChartArea1";
        }

        private void setMapPosition(double latitude, double longitude)
        {
            // Set position
            chart1.Series.FindByName("Position").Points.Clear();
            chart1.Series.FindByName("Position").Points.AddXY(longitude, latitude);
            // Add to trail
            chart1.Series.FindByName("Trail").Points.AddXY(longitude, latitude);
            // Update GUI labels
            label_latitude.Text = latitude.ToString();
            label_longitude.Text = longitude.ToString();
            // Move map bounds with fixed zoom
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Minimum = longitude - 0.01;
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Maximum = longitude + 0.01;
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Minimum = latitude - 0.01;
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Maximum = latitude + 0.01;
            // TODO: Implement user-selectable zoom
        }

        private void serialPrint(string output)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(output);
            }
            else
            {
                throw new Exception("Serial port must be opened to write to it!");
            }
        }

        private void serialPrintLine(string output)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(output);
                serialPort1.Write(newline);
            }
            else
            {
                throw new Exception("Serial port must be opened to write to it!");
            }
        }

        private void printSettings()
        {
            Console.WriteLine("Selected COM Port:  " + selectedCOMPort);
            Console.WriteLine("Selected baud rate: " + selectedBaudRate);
            Console.WriteLine("Selected fix rate:  " + selectedFixRate);
            string newlinerepresentation = "";
            if (newline.Contains("\r")) newlinerepresentation += "CR";
            if (newline.Contains("\n")) newlinerepresentation += "LF";
            Console.WriteLine("Selected newline:   " + newlinerepresentation);

            serialPrintLine("Test serial output on " + selectedCOMPort);
        }

        private void comboBox_COMSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCOMPort = comboBox_COMSelector.SelectedItem.ToString();
            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.PortName = selectedCOMPort;
            serialPort1.Open();
            printSettings();
        }

        private void comboBox_COMSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedCOMPort = comboBox_COMSelector.Text;
            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.PortName = selectedCOMPort;
            serialPort1.Open();
            printSettings();
        }

        private void comboBox_BaudSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBaudRate = int.Parse(comboBox_BaudSelector.SelectedItem.ToString());
            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.BaudRate = selectedBaudRate;
            serialPort1.Open();
            printSettings();
        }

        private void comboBox_BaudSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedBaudRate = int.Parse(comboBox_BaudSelector.SelectedItem.ToString());
            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.BaudRate = selectedBaudRate;
            serialPort1.Open();
            printSettings();
        }

        private void comboBox_FixRateSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFixRate = int.Parse(comboBox_FixRateSelector.SelectedItem.ToString());
            gpsFixTimer.Interval = 1000 / selectedFixRate;
            printSettings();
        }

        private void comboBox_FixRateSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedFixRate = int.Parse(comboBox_FixRateSelector.SelectedItem.ToString());
            gpsFixTimer.Interval = 1000 / selectedFixRate;
            printSettings();
        }

        private void comboBox_NewlineSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox_NewlineSelector.SelectedIndex)
            {
                case 0: // CR
                    newline = "\r";
                    break;
                case 1: // LF
                    newline = "\n";
                    break; 
                case 2: // CRLF
                default:
                    newline = "\r\n";
                    break;
            }

            printSettings();
        }

        private void comboBox_NewlineSelector_TextUpdate(object sender, EventArgs e)
        {
            switch (comboBox_NewlineSelector.SelectedIndex)
            {
                case 0: // CR
                    newline = "\r";
                    break;
                case 1: // LF
                    newline = "\n";
                    break;
                case 2: // CRLF
                default:
                    newline = "\r\n";
                    break;
            }

            printSettings();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up after ourselves!
            gpsFixTimer.Stop();
            serialPrintLine("Goodbye!");
            serialPort1.Close();
        }
    }
}
