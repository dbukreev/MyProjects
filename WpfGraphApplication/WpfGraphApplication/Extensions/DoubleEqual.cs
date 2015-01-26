using System;

namespace WpfGraphApplication.Extensions
{
	public static class DoubleEqual
	{
		public static double Eps = 0.000001;

		public static bool IsEqual(this double a, double b)
		{
			return Math.Abs(a - b) < Eps;
		}
	}
}