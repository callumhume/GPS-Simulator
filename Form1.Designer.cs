
namespace GPSSimulator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox_COMSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_BaudSelector = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_FixRateSelector = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_NewlineSelector = new System.Windows.Forms.ComboBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_latitude = new System.Windows.Forms.Label();
            this.label_longitude = new System.Windows.Forms.Label();
            this.textBox_TargetSpeed = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_Accel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label_currentSpeed = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label_Bearing = new System.Windows.Forms.Label();
            this.checkBox_drawTrail = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox_ProjectionSelector = new System.Windows.Forms.ComboBox();
            this.textBox_TargetBearing = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.button_ZoomOut = new System.Windows.Forms.Button();
            this.label_Zoom = new System.Windows.Forms.Label();
            this.button_ZoomIn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_COMSelector
            // 
            this.comboBox_COMSelector.FormattingEnabled = true;
            this.comboBox_COMSelector.Location = new System.Drawing.Point(71, 417);
            this.comboBox_COMSelector.Name = "comboBox_COMSelector";
            this.comboBox_COMSelector.Size = new System.Drawing.Size(67, 21);
            this.comboBox_COMSelector.TabIndex = 0;
            this.comboBox_COMSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_COMSelector_SelectedIndexChanged);
            this.comboBox_COMSelector.TextUpdate += new System.EventHandler(this.comboBox_COMSelector_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 420);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "COM Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate";
            // 
            // comboBox_BaudSelector
            // 
            this.comboBox_BaudSelector.FormattingEnabled = true;
            this.comboBox_BaudSelector.Location = new System.Drawing.Point(208, 417);
            this.comboBox_BaudSelector.Name = "comboBox_BaudSelector";
            this.comboBox_BaudSelector.Size = new System.Drawing.Size(82, 21);
            this.comboBox_BaudSelector.TabIndex = 3;
            this.comboBox_BaudSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_BaudSelector_SelectedIndexChanged);
            this.comboBox_BaudSelector.TextUpdate += new System.EventHandler(this.comboBox_BaudSelector_TextUpdate);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fix Rate (Hz)";
            // 
            // comboBox_FixRateSelector
            // 
            this.comboBox_FixRateSelector.FormattingEnabled = true;
            this.comboBox_FixRateSelector.Location = new System.Drawing.Point(370, 418);
            this.comboBox_FixRateSelector.Name = "comboBox_FixRateSelector";
            this.comboBox_FixRateSelector.Size = new System.Drawing.Size(64, 21);
            this.comboBox_FixRateSelector.TabIndex = 5;
            this.comboBox_FixRateSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_FixRateSelector_SelectedIndexChanged);
            this.comboBox_FixRateSelector.TextUpdate += new System.EventHandler(this.comboBox_FixRateSelector_TextUpdate);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(440, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Newline";
            // 
            // comboBox_NewlineSelector
            // 
            this.comboBox_NewlineSelector.FormattingEnabled = true;
            this.comboBox_NewlineSelector.Location = new System.Drawing.Point(491, 418);
            this.comboBox_NewlineSelector.Name = "comboBox_NewlineSelector";
            this.comboBox_NewlineSelector.Size = new System.Drawing.Size(65, 21);
            this.comboBox_NewlineSelector.TabIndex = 7;
            this.comboBox_NewlineSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_NewlineSelector_SelectedIndexChanged);
            this.comboBox_NewlineSelector.TextUpdate += new System.EventHandler(this.comboBox_NewlineSelector_TextUpdate);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(13, 13);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(417, 398);
            this.chart1.TabIndex = 8;
            this.chart1.Text = "chart1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(486, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Latitude:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(477, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Longitude:";
            // 
            // label_latitude
            // 
            this.label_latitude.AutoSize = true;
            this.label_latitude.Location = new System.Drawing.Point(541, 13);
            this.label_latitude.Name = "label_latitude";
            this.label_latitude.Size = new System.Drawing.Size(35, 13);
            this.label_latitude.TabIndex = 11;
            this.label_latitude.Text = "label7";
            // 
            // label_longitude
            // 
            this.label_longitude.AutoSize = true;
            this.label_longitude.Location = new System.Drawing.Point(541, 30);
            this.label_longitude.Name = "label_longitude";
            this.label_longitude.Size = new System.Drawing.Size(35, 13);
            this.label_longitude.TabIndex = 12;
            this.label_longitude.Text = "label7";
            // 
            // textBox_TargetSpeed
            // 
            this.textBox_TargetSpeed.Location = new System.Drawing.Point(544, 47);
            this.textBox_TargetSpeed.Name = "textBox_TargetSpeed";
            this.textBox_TargetSpeed.Size = new System.Drawing.Size(49, 20);
            this.textBox_TargetSpeed.TabIndex = 13;
            this.textBox_TargetSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_TargetSpeed_KeyPress);
            this.textBox_TargetSpeed.Leave += new System.EventHandler(this.textBox_TargetSpeed_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(461, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Target speed:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(600, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "TODO: Unit systems";
            // 
            // textBox_Accel
            // 
            this.textBox_Accel.Location = new System.Drawing.Point(544, 74);
            this.textBox_Accel.Name = "textBox_Accel";
            this.textBox_Accel.Size = new System.Drawing.Size(49, 20);
            this.textBox_Accel.TabIndex = 16;
            this.textBox_Accel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_Accel_KeyPress);
            this.textBox_Accel.Leave += new System.EventHandler(this.textBox_Accel_Leave);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(465, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Acceleration:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(490, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Speed:";
            // 
            // label_currentSpeed
            // 
            this.label_currentSpeed.AutoSize = true;
            this.label_currentSpeed.Location = new System.Drawing.Point(541, 130);
            this.label_currentSpeed.Name = "label_currentSpeed";
            this.label_currentSpeed.Size = new System.Drawing.Size(41, 13);
            this.label_currentSpeed.TabIndex = 19;
            this.label_currentSpeed.Text = "label11";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(485, 152);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Bearing:";
            // 
            // label_Bearing
            // 
            this.label_Bearing.AutoSize = true;
            this.label_Bearing.Location = new System.Drawing.Point(541, 152);
            this.label_Bearing.Name = "label_Bearing";
            this.label_Bearing.Size = new System.Drawing.Size(41, 13);
            this.label_Bearing.TabIndex = 21;
            this.label_Bearing.Text = "label12";
            // 
            // checkBox_drawTrail
            // 
            this.checkBox_drawTrail.AutoSize = true;
            this.checkBox_drawTrail.Checked = true;
            this.checkBox_drawTrail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_drawTrail.Location = new System.Drawing.Point(709, 420);
            this.checkBox_drawTrail.Name = "checkBox_drawTrail";
            this.checkBox_drawTrail.Size = new System.Drawing.Size(74, 17);
            this.checkBox_drawTrail.TabIndex = 22;
            this.checkBox_drawTrail.Text = "Draw Trail";
            this.checkBox_drawTrail.UseVisualStyleBackColor = true;
            this.checkBox_drawTrail.CheckedChanged += new System.EventHandler(this.checkBox_drawTrail_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(562, 421);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Projection";
            // 
            // comboBox_ProjectionSelector
            // 
            this.comboBox_ProjectionSelector.FormattingEnabled = true;
            this.comboBox_ProjectionSelector.Location = new System.Drawing.Point(622, 418);
            this.comboBox_ProjectionSelector.Name = "comboBox_ProjectionSelector";
            this.comboBox_ProjectionSelector.Size = new System.Drawing.Size(81, 21);
            this.comboBox_ProjectionSelector.TabIndex = 24;
            this.comboBox_ProjectionSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_ProjectionSelector_SelectedIndexChanged);
            this.comboBox_ProjectionSelector.TextUpdate += new System.EventHandler(this.comboBox_ProjectionSelector_TextUpdate);
            // 
            // textBox_TargetBearing
            // 
            this.textBox_TargetBearing.Location = new System.Drawing.Point(544, 101);
            this.textBox_TargetBearing.Name = "textBox_TargetBearing";
            this.textBox_TargetBearing.Size = new System.Drawing.Size(49, 20);
            this.textBox_TargetBearing.TabIndex = 25;
            this.textBox_TargetBearing.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_TargetBearing_KeyPress);
            this.textBox_TargetBearing.Leave += new System.EventHandler(this.textBox_TargetBearing_Leave);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(452, 104);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(82, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "Target heading:";
            // 
            // button_ZoomOut
            // 
            this.button_ZoomOut.Location = new System.Drawing.Point(692, 72);
            this.button_ZoomOut.Name = "button_ZoomOut";
            this.button_ZoomOut.Size = new System.Drawing.Size(75, 23);
            this.button_ZoomOut.TabIndex = 27;
            this.button_ZoomOut.Text = "Zoom out";
            this.button_ZoomOut.UseVisualStyleBackColor = true;
            this.button_ZoomOut.Click += new System.EventHandler(this.button_ZoomOut_Click);
            // 
            // label_Zoom
            // 
            this.label_Zoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Zoom.AutoSize = true;
            this.label_Zoom.Location = new System.Drawing.Point(706, 104);
            this.label_Zoom.Name = "label_Zoom";
            this.label_Zoom.Size = new System.Drawing.Size(41, 13);
            this.label_Zoom.TabIndex = 28;
            this.label_Zoom.Text = "label14";
            // 
            // button_ZoomIn
            // 
            this.button_ZoomIn.Location = new System.Drawing.Point(692, 125);
            this.button_ZoomIn.Name = "button_ZoomIn";
            this.button_ZoomIn.Size = new System.Drawing.Size(75, 23);
            this.button_ZoomIn.TabIndex = 29;
            this.button_ZoomIn.Text = "Zoom in";
            this.button_ZoomIn.UseVisualStyleBackColor = true;
            this.button_ZoomIn.Click += new System.EventHandler(this.button_ZoomIn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_ZoomIn);
            this.Controls.Add(this.label_Zoom);
            this.Controls.Add(this.button_ZoomOut);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_TargetBearing);
            this.Controls.Add(this.comboBox_ProjectionSelector);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.checkBox_drawTrail);
            this.Controls.Add(this.label_Bearing);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label_currentSpeed);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_Accel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_TargetSpeed);
            this.Controls.Add(this.label_longitude);
            this.Controls.Add(this.label_latitude);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.comboBox_NewlineSelector);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_FixRateSelector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_BaudSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_COMSelector);
            this.Name = "Form1";
            this.Text = "GPS Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_COMSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_BaudSelector;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_FixRateSelector;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_NewlineSelector;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_latitude;
        private System.Windows.Forms.Label label_longitude;
        private System.Windows.Forms.TextBox textBox_TargetSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Accel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label_currentSpeed;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label_Bearing;
        private System.Windows.Forms.CheckBox checkBox_drawTrail;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_ProjectionSelector;
        private System.Windows.Forms.TextBox textBox_TargetBearing;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button button_ZoomOut;
        private System.Windows.Forms.Label label_Zoom;
        private System.Windows.Forms.Button button_ZoomIn;
    }
}

