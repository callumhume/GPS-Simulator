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

namespace GPSSimulator
{
    public partial class Form1 : Form
    {

        string selectedCOMPort = "COM1";
        int selectedBaudRate = 38400;
        int selectedFixRate = 5;

        public Form1()
        {
            InitializeComponent();

            populateComboBoxLists();

            // TODO: Select previous settings

            // TODO: Create GPS logic

            // TODO: Create output logic (autoconnect at beginning)

            // TODO: Create position control logic
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

            comboBox_BaudSelector.SelectedIndex = 0;
            comboBox_BaudSelector.Refresh();


            int[] fixRates = { 1, 2, 4, 5, 10, 20 };

            for (int i = 0; i < fixRates.Length; i++)
            {
                Console.WriteLine(fixRates[i].ToString());
                comboBox_FixRateSelector.Items.Add(fixRates[i]);
            }

            comboBox_FixRateSelector.SelectedIndex = 0;
            comboBox_FixRateSelector.Refresh();
        }

        private void printSettings()
        {
            Console.WriteLine("Selected COM Port:  " + selectedCOMPort);
            Console.WriteLine("Selected baud rate: " + selectedBaudRate);
            Console.WriteLine("Selected fix rate:  " + selectedFixRate);
        }

        private void comboBox_COMSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCOMPort = comboBox_COMSelector.SelectedItem.ToString();
            printSettings();
        }

        private void comboBox_COMSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedCOMPort = comboBox_COMSelector.Text;
            printSettings();
        }

        private void comboBox_BaudSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBaudRate = int.Parse(comboBox_BaudSelector.SelectedItem.ToString());
            printSettings();
        }

        private void comboBox_BaudSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedBaudRate = int.Parse(comboBox_BaudSelector.SelectedItem.ToString());
            printSettings();
        }

        private void comboBox_FixRateSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFixRate = int.Parse(comboBox_FixRateSelector.SelectedItem.ToString());
            printSettings();
        }

        private void comboBox_FixRateSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedFixRate = int.Parse(comboBox_FixRateSelector.SelectedItem.ToString());
            printSettings();
        }
    }
}
