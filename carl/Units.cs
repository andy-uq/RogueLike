using System;

namespace carl
{
	public static class Units
	{
		public const double Tolerance = 0.0001;
		public const double Epsilon = 0.00001;

		public static bool IsZero(double value)
		{
			return Math.Abs(value) < Epsilon;
		}

		public static bool Equal(double a, double b)
		{
			return Math.Abs(a - b) < Epsilon;
		}
	}
}