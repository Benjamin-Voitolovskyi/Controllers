namespace Lab1
{
    class PID_Regulator : Regulator
    {
        private double x = 0.0;
        private double prevX = 0.0;

        private double u = 0.0;
        private double prevU = 0.0;

        private double error = 0.0;
        private double prevError = 0.0;
        private double integral = 0.0;
        private double derivative = 0.0;

        private double proportionalCoef = 0.5;
        private double integralCoef = 0.3;
        private double diffCoef = 0.2;

        protected override void SetU(double time)
        {
            error = Y(time) - x;

            integral += error * deltaTime;
            derivative = (error - prevError) / deltaTime;

            prevU = u;
            u = proportionalCoef * error + integralCoef * integral + diffCoef * derivative;
        }

        public override double X(double time)
        {
            SetU(time);

            double temp = x;
            double deltaTimeSquare = deltaTime * deltaTime;

            x = (x * (2 * d[0] + deltaTime * d[1] - deltaTimeSquare * d[2]) - d[0] * prevX
                 + deltaTime * c[0] * u + prevU * (deltaTimeSquare * c[1] - deltaTime * c[0]))
                 / (d[0] + deltaTime * d[1]);

            prevX = temp;

            return x;
        }
    }
}
