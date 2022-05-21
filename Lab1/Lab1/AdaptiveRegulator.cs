using System;

namespace Lab1
{
    class AdaptiveRegulator : Regulator
    {
        private int n = 4;
        private int m = 1;
        private int r = 100;

        private double[] xs;
        private double[] us;

        private double[] a;
        private double[] b;

        public AdaptiveRegulator()
        {
            xs = new double[r + n];
            us = new double[r + m];

            a = new double[n];
            b = new double[m];
            for (int i = 0; i < n; ++i)
                a[i] = 1.0;
            for (int j = 0; j < m; ++j)
                b[j] = 1.0;
        }

        private double HypotheticalX(int iteration)
        {
            double x = 0.0;

            for (int i = 0; i < n; ++i)
                x += a[i] * xs[iteration + i + 1];

            for (int i = 0; i < m; ++i)
                x += b[i] * us[iteration + i];

            return x;
        }

        private void SelfTuning()
        {
            double[] prevA = new double[n];
            double[] prevB = new double[m];

            Array.Copy(a, prevA, n);
            Array.Copy(b, prevB, m);

            const double gamma = 0.1;
            const double precision = 0.01;
            double diff;

            do
            {
                diff = 0.0;

                for (int j = 0; j < n; ++j)
                {
                    a[j] = prevA[j];

                    for (int i = 1; i <= r; ++i)
                    {
                        double sum = xs[i - 1] - HypotheticalX(i - 1);
                        sum /= i;
                        a[j] += 2 * gamma * sum * xs[i];
                    }

                    if (Math.Abs(a[j] - prevA[j]) > diff)
                        diff = Math.Abs(a[j] - prevA[j]);
                }

                for (int j = 0; j < m; ++j)
                {
                    b[j] = prevB[j];

                    for (int i = 1; i <= r; ++i)
                    {
                        double sum = xs[i - 1] - HypotheticalX(i - 1);
                        sum /= i;
                        b[j] += 2 * gamma * sum * us[i];
                    }

                    if (Math.Abs(b[j] - prevB[j]) > diff)
                        diff = Math.Abs(b[j] - prevB[j]);
                }

                Array.Copy(a, prevA, n);
                Array.Copy(b, prevB, m);
            } while (diff > precision);
        }

        public override double X(double time)
        {
            SelfTuning();
            SetU(time);

            double deltaTimeSquare = deltaTime * deltaTime;

            double x = (xs[0] * (2 * d[0] + deltaTime * d[1] - deltaTimeSquare * d[2]) - d[0] * xs[1]
                 + deltaTime * c[0] * us[0] + us[1] * (deltaTimeSquare * c[1] - deltaTime * c[0]))
                 / (d[0] + deltaTime * d[1]);

            for (int i = r + n - 1; i > 0; --i)
                xs[i] = xs[i - 1];
            xs[0] = x;

            return x;
        }

        protected override void SetU(double time)
        {
            for (int i = r + m - 1; i > 0; --i)
                us[i] = us[i - 1];

            us[0] = Y(time);

            for (int i = 0; i < n; ++i)
                us[0] -= a[i] * xs[i];
            for (int i = 1; i < m; ++i)
                us[0] -= b[i] * us[i];

            us[0] /= b[0];
        }
    }
}
