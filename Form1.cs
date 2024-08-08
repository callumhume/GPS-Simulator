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
        double latitude = 42.255637;
        double longitude = -85.661945;
        double targetSpeed = 10.0; //  km/h // TODO: start/stop button that changes this target in the backend but leaves the original value visible to the user.  Selected target vs current target??
        double acceleration = 0.5; // km/h/sec
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

            setMapPosition(latitude, longitude);

            // TODO: Select previous settings

            // TODO: Create GPS logic

            // TODO: Create output logic (autoconnect at beginning)

            gpsFixTimer.Start();

            // TODO: Create position control logic
        }

        int updatesInThisDirection = 0;

        private void handleGPSCalculations(object sender, EventArgs e)
        {
            outputGPSFix();
            setMapPosition(latitude, longitude);
            calculateNextGPSFix();

            if (++updatesInThisDirection >= 30 * selectedFixRate)
            { // For testing purposes, turn 90 degrees to the right every 10 seconds
                bearing = (bearing + 90) % 360;
                updatesInThisDirection = 0;
            }
        }

        private void outputGPSFix()
        {
            // TODO: Create an actual GGA sentence
            serialPrintLine(DateTime.Now.TimeOfDay.ToString());
        }

        // TODO: Proper geoid compensation
        // TODO: User-selectable geoid model/datum (WGS-84, etc)

        // At 45 degrees north, one degree latitude is approximately equal to 111,131.745 meters
        double kmToLatFactor = 111131.745;
        // At 45 degrees north, one degree longitude is approximately equal to 78,846.805 meters
        double kmToLonFactor = 78846.805;

        private void calculateNextGPSFix()
        {
            double deltaTime = 1000.0 / selectedFixRate; // Ideal time, not looking at clock drift forward/backward or taking actual uptime if this task gets fired slightly earlier/later
            serialPrintLine(deltaTime.ToString());
            double accelerationPerUpdate = acceleration / selectedFixRate;
            // Update speed as needed.  We're not trying to make a physics simulator here so we only care about acceleration steps.  No jerk or rice krispies here...
            if (speed < targetSpeed)
            { // Case need to speed up
                if (speed > targetSpeed - accelerationPerUpdate)
                { // Case within one acceleration step of target.  Do not overshoot.
                    speed = targetSpeed;
                }
                else
                { // Case outside one acceleration step, so make that step
                    speed += accelerationPerUpdate;
                }
            }
            else if (speed > targetSpeed)
            { // Case need to slow down
                if (speed < targetSpeed + accelerationPerUpdate)
                { // Case within one acceleration step of target.  Do not overshoot.
                    speed = targetSpeed;
                }
                else
                { // Case outside one acceleration step, so make that step.
                    speed -= accelerationPerUpdate;
                }
            }
            // Final case, speed is already at target.  No need for further changes until target changes.
            // Calculate how far we're moving this update
            // speed (km/hr)
            // / 60  (km/min)
            // * 1000 (m/min)
            // / 60  (m/sec)
            // / 1000 (m/msec)
            // ^ deltaTime (m/update)
            double deltaPosition = speed * 1000.0 / 60.0 / 60.0 / selectedFixRate; // Convert km/hr to m/update
            serialPrintLine(accelerationPerUpdate.ToString());
            serialPrintLine(speed.ToString());
            serialPrintLine(deltaPosition.ToString());
            // Methodology:  Delta position is currently in km.  We need to calculate the x/y components with our current bearing in mind.
            // TODO: Are there considerations to make for spheroid/geoid math or can we assume a local plane??
            // When we add these components to our current coordinates, we need to compensate for the difference in absolute distance vs longitude, right?
            // At more polar latitudes, the same unit of longitude becomes less physical distance as the lines converge

            // remember sin(90) = 1
            // cos (0) = 1
            // cos gives x component, sin gives y component
            // TODO: Degrees or radians?
            double bearingRadians = Math.PI * bearing / 180.0;
            double longitudeFactor = Math.Cos(bearingRadians);
            double latitudeFactor = Math.Sin(bearingRadians);
            double longitudeOffset = deltaPosition * longitudeFactor;
            double latitudeOffset = deltaPosition * latitudeFactor;
            // TODO: Latitude compensation.  Currently completely flat-earthing
            longitudeOffset /= kmToLonFactor;
            latitudeOffset /= kmToLatFactor;
            longitude += longitudeOffset;
            latitude += latitudeOffset;
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
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.LabelStyle.Enabled = false;
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.LabelStyle.Enabled = false;
        }

        // TODO: Rename to something like updateUI since this is updating map and labels - all the visual feedback to the user
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
            label_currentSpeed.Text = speed.ToString();
            label_Bearing.Text = bearing.ToString();
            // Move map bounds with fixed zoom
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Minimum = longitude - 0.001;
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Maximum = longitude + 0.001;
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Minimum = latitude - 0.001;
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Maximum = latitude + 0.001;
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
