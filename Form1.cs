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
        // User-settable, should be saved
        string selectedCOMPort = "COM1";
        int selectedBaudRate = 38400; // TODO: Enum?
        int selectedFixRate = 5; // TODO: Enum?
        int selectedQuality = 1; // TODO: Enum?
        string newline = "\r\n";
        int selectedProjection = 0; // TODO: Enum?
        bool drawTrail = true;
        int zoomLevel = 2; // Potential list: 10, 25, 50, 100, 250, 500, 1000
        // TODO: Start lat/long
        int demoDriveType = 3; // 0 = spiral CW
                               // 1 = spiral CCW
                               // 2 = rows
                               // 3 = octagon CW
                               // 4 = octagon CCW
                               // TODO: enum
                               // TODO: UI selector
        int turnMode = 0; // 0 = auto (nearest)
                          // 1 = CW / right turn
                          // 2 = CCW / left turn
                          // TODO: enum
                          // TODO: UI selector

        int numSatellites = 4;

        Timer gpsFixTimer;
        // User-settable
        double targetSpeed = 10.0; //  km/h // TODO: start/stop button that changes this target in the backend but leaves the original value visible to the user.  Selected target vs current target??
        double targetBearing = 45.0;
        double acceleration = 0.5; // km/h/sec
        // TODO: How can we get this from turning radius?  This does not seem to turn at the same radius if the turn angle is obtuse (wider turn) or acute (sharper turn)
        // Maybe it is, just looks weird because of  map projectiosn.  TODO: test without coordinate smushing
        // Turning seems to work fine on planar projection.  Even has a wider turning radius at a higher speed, which makes sense
        // TODO: Once auto-turn is implemented, also implement speed-up/slow-down for turn-arounds
        double turningRate = 5.0; // Degrees per second
        // Internal
        double latitude = 42.255637;
        double longitude = -85.661945;

        // Meters per degree
        double[] latitudeOffsetFactor = {
            111131.745,     // Plane - equatorial flat earth scale
            111131.745      // WGS-84
        };
        // Meters per degree
        double[] longitudeOffsetFactor = {
            111131.745,     // Plane - equatorial flat earth scale
            78846.805       // WGS-84 @ 45deg north // TODO: Compensate long based on lat
            };
        double speed = 0.0;
        double bearing = 45.0;
        double distanceTraveled = 0.0;

        int maxTrailPoints = 10000;

        public Form1()
        {
            InitializeComponent();
            gpsFixTimer = new Timer();
            gpsFixTimer.Interval = 1000 / selectedFixRate;
            gpsFixTimer.Tick += new EventHandler(handleGPSCalculations);

            populateUIElements();

            setupMapChart();

            setMapPosition(latitude, longitude);

            // TODO: Select previous settings

            // TODO: Create GPS logic

            // TODO: Create output logic (autoconnect at beginning)

            gpsFixTimer.Start();

            // TODO: Create position control logic
        }

        int updatesInThisDirection = 0;
        int maxSecondsThisDirection = 25;

        private void handleGPSCalculations(object sender, EventArgs e)
        {
            outputGPSFix();
            setMapPosition(latitude, longitude);
            calculateNextGPSFix();

            if (checkBox_DemoDrive.Checked)
            {
                switch (demoDriveType)
                {
                    case 0: // Clockwise spiral
                        if (++updatesInThisDirection >= maxSecondsThisDirection * selectedFixRate)
                        { // For testing purposes, turn 90 degrees to the left every x seconds
                            targetBearing = (targetBearing + 45 + 360) % 360;
                            textBox_TargetBearing.Text = targetBearing.ToString();
                            updatesInThisDirection = 0;
                            maxSecondsThisDirection += selectedFixRate;
                        }
                        break;
                    case 1: // Counter-clockwise spiral
                        if (++updatesInThisDirection >= maxSecondsThisDirection * selectedFixRate)
                        { // For testing purposes, turn 90 degrees to the right every x seconds
                            targetBearing = (targetBearing - 45 + 360) % 360;
                            textBox_TargetBearing.Text = targetBearing.ToString();
                            updatesInThisDirection = 0;
                            maxSecondsThisDirection += selectedFixRate;
                        }
                        break;
                    case 3: // Clockwise octagon
                        maxSecondsThisDirection = 30;
                        if (++updatesInThisDirection >= maxSecondsThisDirection * selectedFixRate)
                        { // For testing purposes, turn 90 degrees to the left every x seconds
                            targetBearing = (targetBearing + 45 + 360) % 360;
                            textBox_TargetBearing.Text = targetBearing.ToString();
                            updatesInThisDirection = 0;
                        }
                        break;
                    case 4: // Counter-clockwise octagon
                        maxSecondsThisDirection = 30;
                        if (++updatesInThisDirection >= maxSecondsThisDirection * selectedFixRate)
                        { // For testing purposes, turn 90 degrees to the right every x seconds
                            targetBearing = (targetBearing - 45 + 360) % 360;
                            textBox_TargetBearing.Text = targetBearing.ToString();
                            updatesInThisDirection = 0;
                        }
                        break;
                    case 2: // Rows
                    default:
                        // TODO: Fixed number of updates?
                        if (++updatesInThisDirection >= maxSecondsThisDirection * selectedFixRate)
                        { // For testing purposes, turn 180 degrees every x seconds
                            targetBearing = (targetBearing + 180 + 360) % 360;
                            textBox_TargetBearing.Text = targetBearing.ToString();
                            updatesInThisDirection = 0;
                            maxSecondsThisDirection += selectedFixRate;
                        }
                        break;
                }

            }
        }

        private void outputGPSFix()
        {
            TimeSpan fixTime = DateTime.Now.TimeOfDay;
            // GGA sentence structure:
            // $GPGGA,hhmmss.ss,ddmm.mm,a,ddmm.mm,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx*hh
            // see https://logiqx.github.io/gps-wizard/nmea/messages/gga.html
            // TODO: the rest of the GGA string
            //       num sats
            //       HDOP
            //       Elevation (start user-configurable, use to compensate for surface speed vs coordinate speed!)
            //       Elevation units
            //       Geoid separation
            //       Geoid separation units
            //       Differential GPS age
            //       Differential GPS station ID
            //       Checksum
            string formattedGGA = string.Format("$GPGGA,{0:D2}{1:D2}{2:D2}.{3:D2},{4:N0}{5:N7},{6},{7:N0}{8:N7},{9},{10:D1},{11:D2},{12:N1},{13:N3},{14},{15:N3},{16},{17},{18}",
                fixTime.Hours, fixTime.Minutes, fixTime.Seconds, fixTime.Milliseconds / 10,
                Math.Floor(Math.Abs(latitude)), (Math.Abs(latitude) - Math.Floor(Math.Abs(latitude))) * 60, latitude > 0 ? "N" : "S",
                Math.Floor(Math.Abs(longitude)), (Math.Abs(longitude) - Math.Floor(Math.Abs(longitude))) * 60, longitude > 0 ? "E" : "W",
                selectedQuality, // Quality indicator (type of GPS fix)
                numSatellites,   // satellites
                1.0,   // TODO: HDOP (may need to change the formatting placeholder??
                250.0, // TODO: Altitude
                "M",   // Altitude units
                15.0,  // TODO: undulation (difference between WGS-84 ellipsoid and "the geoid"
                "M",   // Undulation units
                null,  // differential GPS correction data age (empty when DGPS not present)
                null   // DGPS base station ID (empty when DGPS not present)
                );

            formattedGGA = addChecksum(formattedGGA);


            serialPrintLine(formattedGGA);
            // TODO: VTG

        }

        private string addChecksum(string input)
        {
            char checksum = (char)0;
            for (int i = 0; i < input.Length; i++)
            {
                checksum += input[i];
            }
            return string.Format("{0}*{1:X2}", input, (byte)checksum);
        }

        // TODO: Proper geoid compensation

        private void calculateNextGPSFix()
        {
            double deltaTime = 1000.0 / selectedFixRate; // Ideal time, not looking at clock drift forward/backward or taking actual uptime if this task gets fired slightly earlier/later
            //serialPrintLine(deltaTime.ToString());
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
            distanceTraveled += deltaPosition / 1000; // Track distance in km
            label_distanceTraveled.Text = distanceTraveled.ToString("N3");
            //serialPrintLine(accelerationPerUpdate.ToString());
            //serialPrintLine(speed.ToString());
            //serialPrintLine(deltaPosition.ToString());
            // Methodology:  Delta position is currently in km.  We need to calculate the x/y components with our current bearing in mind.
            // TODO: Are there considerations to make for spheroid/geoid math or can we assume a local plane??
            // When we add these components to our current coordinates, we need to compensate for the difference in absolute distance vs longitude, right?
            // At more polar latitudes, the same unit of longitude becomes less physical distance as the lines converge

            // TODO: When implementing right/left turn options, this might get taken care of.  But a graceful/predictable implementation for "turn 180 degrees"

            double bearingChangePerUpdate = turningRate / selectedFixRate;
            // If bearing is between 270 and 90, switch systems to -180 to +180 to account for rollover.  Then output results in 0-360
            // Update bearing as needed.  Same logic as speed updates but we'll need a wraparound at 0/360

            switch (turnMode)
            {
                case 1:
                    // Clockwise (right) turns only
                    // TODO: May still need to account for w0-degree rollover like auto turns...
                    if (bearing < 90 || bearing >= 270 || targetBearing < 90 || targetBearing >= 270)
                    {
                        if ((bearing > 180 ? bearing - 360 : bearing) != (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                        {
                            if ((bearing > 180 ? bearing - 360 : bearing) < (targetBearing > 180 ? targetBearing - 360 : targetBearing) && (bearing > 180 ? bearing - 360 : bearing) + bearingChangePerUpdate > (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                            { // Need to step right, but one step will overshoot.  Set to target.
                                setBearing(targetBearing);
                            }
                            else
                            { // Normal update step to the right
                                setBearing(bearing + bearingChangePerUpdate);
                            }
                        }
                    }
                    else
                    {
                        if (bearing != targetBearing)
                        {
                            if (bearing < targetBearing && bearing + bearingChangePerUpdate > targetBearing)
                            { // Need to step right, but one step will overshoot.  Set to target.
                                setBearing(targetBearing);
                            }
                            else
                            { // Normal update step to the right
                                setBearing(bearing + bearingChangePerUpdate);
                            }
                        }
                    }
                    break;
                case 2:
                    // Counter-clockwise (left) turns only
                    // TODO: May still need to account for w0-degree rollover like auto turns...
                    if (bearing < 90 || bearing >= 270 || targetBearing < 90 || targetBearing >= 270)
                    {
                        if ((bearing > 180 ? bearing - 360 : bearing) != (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                        {
                            if ((bearing > 180 ? bearing - 360 : bearing) > (targetBearing > 180 ? targetBearing - 360 : targetBearing) && (bearing > 180 ? bearing - 360 : bearing) - bearingChangePerUpdate < (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                            { // Need to step left, but one step will overshoot.  Set to target.
                                setBearing(targetBearing);
                            }
                            else
                            { // Normal update step to the left
                                setBearing(bearing - bearingChangePerUpdate);
                            }
                        }
                    }
                    else
                    {
                        if (bearing != targetBearing)
                        {
                            if (bearing > targetBearing && bearing - bearingChangePerUpdate < targetBearing)
                            { // Need to step left, but one step will overshoot.  Set to target.
                                setBearing(targetBearing);
                            }
                            else
                            { // Normal update step to the left
                                setBearing(bearing - bearingChangePerUpdate);
                            }
                        }
                    }
                    break;
                case 0:
                default:
                    // Auto (nearest) turns
                    if (bearing < 90 || bearing >= 270 || targetBearing < 90 || targetBearing >= 270)
                    { // Current bearing is between 270 and 90 (north-facing)
                      //Console.WriteLine("Adjusted bearing is " + (bearing > 180 ? bearing - 360 : bearing) + " and adjusted target is " + (targetBearing > 180 ? targetBearing - 360 : targetBearing));
                      // Do all conversions absed solely on bearing.  If bearing gets converted, so does targetBearing. Nevermind...
                        if ((bearing > 180 ? bearing - 360 : bearing) < (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                        { // Case need to turn right TODO: Verify C# functionality of % with negative numbers
                            if ((bearing > 180 ? bearing - 360 : bearing) > (targetBearing > 180 ? targetBearing - 360 : targetBearing) - bearingChangePerUpdate)
                            { // Case within one turning step of target.  Do not overshoot.
                                setBearing(targetBearing);
                            }
                            else
                            { // Case outside one turning step, so make that step
                                setBearing(bearing + bearingChangePerUpdate);
                            }
                        }
                        else if ((bearing > 180 ? bearing - 360 : bearing) > (targetBearing > 180 ? targetBearing - 360 : targetBearing))
                        { // Case need to turn left
                            if ((bearing > 180 ? bearing - 360 : bearing) < (targetBearing > 180 ? targetBearing - 360 : targetBearing) + bearingChangePerUpdate)
                            { // Case within one turning step of target.  Do not overshoot.
                                setBearing(targetBearing);
                            }
                            else
                            { // Case outside one turning step, so make that step.
                                setBearing(bearing - bearingChangePerUpdate);
                            }
                        }
                    }
                    else
                    { // Current bearing is between 90 and 270 (south-facing)
                      //Console.WriteLine("Normal bearing is " + bearing + " and normal target is " + targetBearing);
                        if (bearing < targetBearing)
                        { // Case need to turn right
                            if (bearing > targetBearing - bearingChangePerUpdate)
                            { // Case within one turning step of target.  Do not overshoot.
                                setBearing(targetBearing);
                            }
                            else
                            { // Case outside one turning step, so make that step
                                setBearing(bearing + bearingChangePerUpdate);
                            }
                        }
                        else if (bearing > targetBearing)
                        { // Case need to turn left
                            if (bearing < targetBearing + bearingChangePerUpdate)
                            { // Case within one turning step of target.  Do not overshoot.
                                setBearing(targetBearing);
                            }
                            else
                            { // Case outside one turning step, so make that step.
                                setBearing(bearing - bearingChangePerUpdate);
                            }
                        }
                    }
                    break;
            }


            // TODO: Account for rollover at 360/0

            // remember sin(90) = 1
            // cos (0) = 1
            // cos gives x component, sin gives y component
            // TODO: Degrees or radians?
            double bearingRadians = Math.PI * bearing / 180.0;
            //double longitudeFactor = Math.Cos(bearingRadians);
            //double latitudeFactor = Math.Sin(bearingRadians);
            // With needing to mirror bearing around the line x = y, why don't I just switch which component is used where?
            double latitudeFactor = Math.Cos(bearingRadians);
            double longitudeFactor = Math.Sin(bearingRadians);
            double longitudeOffset = deltaPosition * longitudeFactor;
            double latitudeOffset = deltaPosition * latitudeFactor;
            // TODO: Latitude compensation.  Currently completely flat-earthing
            longitudeOffset /= longitudeOffsetFactor[selectedProjection];
            latitudeOffset /= latitudeOffsetFactor[selectedProjection];
            longitude += longitudeOffset;
            latitude += latitudeOffset;
        }

        private void setBearing(double newBearing)
        { //  Ensure bearing is always in the range [0, 360)
            bearing = (newBearing + 360) % 360;
            if (bearing == 360) bearing = 0;
        }

        private void populateUIElements()
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


            string[] projectionOptions = { "Plane", "WGS-84" };

            for (int i = 0; i < projectionOptions.Length; i++)
            {
                Console.WriteLine(projectionOptions[i].ToString());
                comboBox_ProjectionSelector.Items.Add(projectionOptions[i]);
            }

            comboBox_ProjectionSelector.SelectedIndex = 1;
            comboBox_ProjectionSelector.Refresh();

            string[] qualityOptions = { "Invalid",
                                        "GPS",
                                        "DGPS",
                                        "Invalid",
                                        "RTK-Fixed",
                                        "RTK-Float",
                                        "Dead-reckoning",
                                        "Manual/fixed",
                                        "Simulator",
                                        "WAAS/SBAS"};

            for (int i = 0; i < qualityOptions.Length; i++)
            {
                Console.WriteLine(qualityOptions[i].ToString());
                comboBox_GPSQualitySelector.Items.Add(qualityOptions[i]);
            }

            comboBox_GPSQualitySelector.SelectedIndex = 1;
            comboBox_GPSQualitySelector.Refresh();

            string[] turnOptions = { "Nearest",
                                     "Right only",
                                     "Left only"};

            for (int i = 0; i < turnOptions.Length; i++)
            {
                Console.WriteLine(turnOptions[i].ToString());
                comboBox_TurnModeSelector.Items.Add(turnOptions[i]);
            }

            comboBox_TurnModeSelector.SelectedIndex = 0;
            comboBox_TurnModeSelector.Refresh();

            textBox_TargetSpeed.Text = targetSpeed.ToString();
            textBox_Accel.Text = acceleration.ToString();
            textBox_TargetBearing.Text = targetBearing.ToString();
            textBox_NumSatellites.Text = numSatellites.ToString();

            label_Zoom.Text = getZoomLevelMeters(zoomLevel).ToString();
        }

        private double getZoomLevelMeters(int zoomIndex)
        {
            switch (zoomIndex)
            { // Yeah, this really should be an enum... Whatever
                case 1:
                    return 10;
                case 2:
                    return 25;
                case 3:
                    return 50;
                case 4:
                    return 100;
                case 5:
                    return 250;
                case 6:
                    return 500;
                case 7:
                    return 1000;
                case 0:
                default:
                    return 5;
            }
        }

        private void setupMapChart()
        {
            chart1.Series.Add("Position");
            chart1.Series.FindByName("Position").ChartType = SeriesChartType.Point;
            chart1.Series.FindByName("Position").MarkerSize *= 3;
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
            if (chart1.Series.FindByName("Trail").Points.Count >= maxTrailPoints) chart1.Series.FindByName("Trail").Points.RemoveAt(0); // Oldest point at lowest index
            if (drawTrail) chart1.Series.FindByName("Trail").Points.AddXY(longitude, latitude);
            // Update GUI labels
            label_latitude.Text = latitude.ToString("N8");
            label_longitude.Text = longitude.ToString("N8");
            label_currentSpeed.Text = speed.ToString("N4");
            label_Bearing.Text = bearing.ToString("N4");
            // Move map bounds with fixed zoom
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Minimum = longitude - (getZoomLevelMeters(zoomLevel) / 2 / longitudeOffsetFactor[selectedProjection]);
            chart1.ChartAreas.FindByName("ChartArea1").AxisX.Maximum = longitude + (getZoomLevelMeters(zoomLevel) / 2 / longitudeOffsetFactor[selectedProjection]);
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Minimum = latitude - (getZoomLevelMeters(zoomLevel) / 2 / latitudeOffsetFactor[selectedProjection]);
            chart1.ChartAreas.FindByName("ChartArea1").AxisY.Maximum = latitude + (getZoomLevelMeters(zoomLevel) / 2 / latitudeOffsetFactor[selectedProjection]);
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
            Console.WriteLine("Selected COM Port:   " + selectedCOMPort);
            Console.WriteLine("Selected baud rate:  " + selectedBaudRate);
            Console.WriteLine("Selected fix rate:   " + selectedFixRate);
            string newlinerepresentation = "";
            if (newline.Contains("\r")) newlinerepresentation += "CR";
            if (newline.Contains("\n")) newlinerepresentation += "LF";
            Console.WriteLine("Selected newline:    " + newlinerepresentation);
            string projection = "";
            switch (selectedProjection)
            {
                case 0:
                    projection += "Plane";
                    break;
                case 1:
                    projection += "WGS-84";
                    break;
                default:
                    projection = "Unknown";
                    break;
            }
            Console.WriteLine("Selected projection: " + projection);
            string quality = "";
            switch (selectedQuality)
            {
                case 0:
                    quality += "Invalid";
                    break;
                case 1:
                    quality += "GPS";
                    break;
                case 2:
                    quality += "DGPS";
                    break;
                case 3:
                    quality += "Invalid";
                    break;
                case 4:
                    quality += "RTK-Fixed";
                    break;
                case 5:
                    quality += "RTK-Float";
                    break;
                case 6:
                    quality += "Dead-reckoning";
                    break;
                case 7:
                    quality += "Manual/fixed";
                    break;
                case 8:
                    quality += "Simulator";
                    break;
                case 9:
                    quality += "WAAS/SBAS";
                    break;
                default:
                    quality = "Unknown";
                    break;
            }
            Console.WriteLine("Selected quality:    " + quality);
            string turnModeString = "";
            switch (turnMode)
            {
                case 0:
                    turnModeString += "Nearest";
                    break;
                case 1:
                    turnModeString += "Right";
                    break;
                case 2:
                    turnModeString += "Left";
                    break;
                default:
                    turnModeString = "Unknown";
                    break;
            }
            Console.WriteLine("Selected turn mode:  " + turnModeString);
        }

        private void comboBox_COMSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                selectedCOMPort = comboBox_COMSelector.SelectedItem.ToString();
                if (serialPort1.IsOpen) serialPort1.Close();
                serialPort1.PortName = selectedCOMPort;
                serialPort1.Open();
                printSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void comboBox_COMSelector_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                selectedCOMPort = comboBox_COMSelector.Text;
                if (serialPort1.IsOpen) serialPort1.Close();
                serialPort1.PortName = selectedCOMPort;
                serialPort1.Open();
                printSettings();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        private void comboBox_ProjectionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProjection = comboBox_ProjectionSelector.SelectedIndex;

            printSettings();
        }

        private void comboBox_ProjectionSelector_TextUpdate(object sender, EventArgs e)
        {
            selectedProjection = comboBox_ProjectionSelector.SelectedIndex;

            printSettings();
        }

        private void comboBox_GPSQualitySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedQuality = comboBox_GPSQualitySelector.SelectedIndex;

            printSettings();
        }

        private void comboBox_GPSQualitySelector_TextUpdate(object sender, EventArgs e)
        {
            selectedQuality = comboBox_GPSQualitySelector.SelectedIndex;

            printSettings();
        }

        private void comboBox_TurnModeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            turnMode = comboBox_TurnModeSelector.SelectedIndex;

            printSettings();
        }

        private void comboBox_TurnModeSelector_TextUpdate(object sender, EventArgs e)
        {
            turnMode = comboBox_TurnModeSelector.SelectedIndex;

            printSettings();
        }

        private void checkBox_drawTrail_CheckedChanged(object sender, EventArgs e)
        {
            drawTrail = checkBox_drawTrail.Checked;
        }

        private void textBox_TargetSpeed_Leave(object sender, EventArgs e)
        {
            targetSpeed = double.Parse(textBox_TargetSpeed.Text);
        }

        private void textBox_TargetSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Contains("\r") || e.KeyChar.ToString().Contains("\n"))
            {
                targetSpeed = double.Parse(textBox_TargetSpeed.Text);
            }
        }

        private void textBox_Accel_Leave(object sender, EventArgs e)
        {
            acceleration = double.Parse(textBox_Accel.Text);
        }

        private void textBox_Accel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Contains("\r") || e.KeyChar.ToString().Contains("\n"))
            {
                acceleration = double.Parse(textBox_Accel.Text);
            }
        }

        private void textBox_TargetBearing_Leave(object sender, EventArgs e)
        {
            targetBearing = double.Parse(textBox_TargetBearing.Text);
        }

        private void textBox_TargetBearing_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Contains("\r") || e.KeyChar.ToString().Contains("\n"))
            {
                targetBearing = double.Parse(textBox_TargetBearing.Text);
            }
        }

        private void button_ZoomIn_Click(object sender, EventArgs e)
        {
            if (zoomLevel > 0)
            {
                zoomLevel--;
                label_Zoom.Text = getZoomLevelMeters(zoomLevel).ToString();
            }
        }

        private void button_ZoomOut_Click(object sender, EventArgs e)
        {
            if (zoomLevel < 7)
            { // TODO: Defined zoom limit
                zoomLevel++;
                label_Zoom.Text = getZoomLevelMeters(zoomLevel).ToString();
            }
        }

        private void button_North_Click(object sender, EventArgs e)
        {
            targetBearing = 0;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_NorthEast_Click(object sender, EventArgs e)
        {
            targetBearing = 45;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_East_Click(object sender, EventArgs e)
        {
            targetBearing = 90;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_SouthEast_Click(object sender, EventArgs e)
        {
            targetBearing = 135;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_South_Click(object sender, EventArgs e)
        {
            targetBearing = 180;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_SouthWest_Click(object sender, EventArgs e)
        {
            targetBearing = 225;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_West_Click(object sender, EventArgs e)
        {
            targetBearing = 270;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_NorthWest_Click(object sender, EventArgs e)
        {
            targetBearing = 315;
            textBox_TargetBearing.Text = targetBearing.ToString();
        }

        private void button_ResetDistance_Click(object sender, EventArgs e)
        {
            distanceTraveled = 0;
            label_distanceTraveled.Text = distanceTraveled.ToString("N3");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up after ourselves!
            gpsFixTimer.Stop();
            serialPrintLine("Goodbye!");
            serialPort1.Close();
        }


        private void textBox_NumSatellites_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString().Contains("\r") || e.KeyChar.ToString().Contains("\n"))
            {
                numSatellites = int.Parse(textBox_NumSatellites.Text);
            }
        }

        private void textBox_NumSatellites_Leave(object sender, EventArgs e)
        { // TODO: parse check on all textboxes?  What if I enter letters?
            numSatellites = int.Parse(textBox_NumSatellites.Text);
        }

        private void contributingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("https://github.com/callumhume/GPS-Simulator");
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Better changelog stuff?  Formatting???
            MessageBox.Show("0.1.0: Inital testing/demo release", "GPS Simulator Changelog");
        }

        private void licensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Better licensing stuff?  Formatting???
            MessageBox.Show("GPS Simulator is tentatively written under the GPLv3 (https://www.gnu.org/licenses/gpl-3.0.en.html).\n" +
                "\n" +
                "At some point before full release, the project will likely be moved to one of the Fair Source licenses (https://fair.io/licenses/)", "GPS Simulator Licenses");
        }
    }
}
