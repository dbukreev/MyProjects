using System;
using System.IO;

namespace GenerateXYConsole
{
	public class Generator
	{
		public Generator(double LeftBoundary = -10.0, double RightBoundary = 10.0, double Step = 0.1)
		{
			_leftBoundary = LeftBoundary;
			_rightBoundary = RightBoundary;
			_step = Step;
			_numberOfPoints = (int) ((_rightBoundary - _leftBoundary)/_step);
		}

		private readonly int _numberOfPoints;

		private readonly double _leftBoundary;

		private double _rightBoundary;

		private readonly double _step;

		private double Function(double x)
		{
			return Math.Sqrt(x);
		}

		public void Generate()
		{
			var file = new StreamWriter(@"C:\Users\bars\Desktop\MyProjects\WpfGraphApplication\WpfGraphApplication\Graphics\test5.txt");
			for (int i = 0; i <= _numberOfPoints; i++)
			{
				file.WriteLine(string.Format("{0} {1}", _leftBoundary + i * _step, Function(_leftBoundary + i * _step)));
			}
			file.Close();
		}
	}
}