
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
            this.comboBox_COMSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_BaudSelector = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_FixRateSelector = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
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
            this.label2.Location = new System.Drawing.Point(162, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud Rate";
            // 
            // comboBox_BaudSelector
            // 
            this.comboBox_BaudSelector.FormattingEnabled = true;
            this.comboBox_BaudSelector.Location = new System.Drawing.Point(227, 417);
            this.comboBox_BaudSelector.Name = "comboBox_BaudSelector";
            this.comboBox_BaudSelector.Size = new System.Drawing.Size(82, 21);
            this.comboBox_BaudSelector.TabIndex = 3;
            this.comboBox_BaudSelector.SelectedIndexChanged += new System.EventHandler(this.comboBox_BaudSelector_SelectedIndexChanged);
            this.comboBox_BaudSelector.TextUpdate += new System.EventHandler(this.comboBox_BaudSelector_TextUpdate);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(332, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fix Rate (Hz)";
            // 
            // comboBox_FixRateSelector
            // 
            this.comboBox_FixRateSelector.FormattingEnabled = true;
            this.comboBox_FixRateSelector.Location = new System.Drawing.Point(406, 417);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBox_FixRateSelector);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_BaudSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_COMSelector);
            this.Name = "Form1";
            this.Text = "GPS Simulator";
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
    }
}

