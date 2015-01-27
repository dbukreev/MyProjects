﻿using System;

namespace WinFormsGraphApplication.Extensions
{
	public static class DoubleEqual
	{
		public static double Eps = 0.00000001;

		public static bool IsEqual(this double a, double b)
		{
			return Math.Abs(a - b) < Eps;
		}
	}
}