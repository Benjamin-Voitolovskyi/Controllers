using System;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        private PID_Regulator pidRegulator = new PID_Regulator();
        private AdaptiveRegulator adaptiveRegulator = new AdaptiveRegulator();
        private double time = 0.0;

        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < 3; ++i)
                chart1.Series[i].Points.Clear();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            pidRegulator.DeltaTime = timer1.Interval / 1000.0;
            timer1.Start();

            chart1.Series[0].Points.AddXY(time, pidRegulator.Y(time));
            chart1.Series[1].Points.AddXY(time, 0.0);
            chart1.Series[2].Points.AddXY(time, 0.0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time += pidRegulator.DeltaTime;
            chart1.Series[0].Points.AddXY(time, pidRegulator.Y(time));
            chart1.Series[1].Points.AddXY(time, pidRegulator.X(time));
            chart1.Series[2].Points.AddXY(time, adaptiveRegulator.X(time));
        }
    }
}
