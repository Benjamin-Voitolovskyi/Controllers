namespace Lab1
{
    abstract class Regulator
    {
        protected double[] d = { 1.0, 2.0, 1.0 };
        protected double[] c = { 0.1, 1.0 };

        protected double deltaTime = 0.01;
        public double DeltaTime { get => deltaTime; set => deltaTime = value; }

        public double Y(double time)
        {
            return 5.0;
        }

        protected abstract void SetU(double time);
        public abstract double X(double time);
    }
}
