﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using WpfGraphApplication.Extensions;

namespace WpfGraphApplication
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			MyInizializeComponent();
			
		}

		private void MyInizializeComponent()
		{
			_graphList = new List<List<Point>>();
			_numberCurrentsGraphs = new List<int>();
			_colorsList = new Color[20];
			_zoom = 1.01;
			ClearCanvas();
		}

		private double _xMin;
		private double _xMax;
		private double _yMin;
		private double _yMax;
		private List<List<Point>> _graphList;
		private Color [] _colorsList; 
		private List<int> _numberCurrentsGraphs;
		private double _zoom;

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				DefaultExt = ".txt",
				Filter = "Файлы txt (*.txt)|*.txt",
				Multiselect = true
			};
			bool? result = dialog.ShowDialog();
			if (result != true)
				return;

			int counter = 1;
			int currentCountInList = _graphList.Count;
			foreach (var fileName in dialog.FileNames)
			{
				var pointList = new List<Point>();
				var file = new StreamReader(fileName);
				string line;
				while ((line = file.ReadLine()) != null)
				{
					double x = double.Parse(line.Split()[0].Replace(".", ","));
					double y = double.Parse(line.Split()[1].Replace(".", ","));
					pointList.Add(new Point(x, y));
				}
				file.Close();
				_graphList.Add(pointList);
				MyListBox.Items.Add(new GraphicInfo { Code = counter + currentCountInList, Name = string.Format("F{0}(x)", counter + currentCountInList) });
				counter++;
			}
			TextBlockStatus.Text = string.Format("Загружено функций: {0}", counter - 1);
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			_graphList.Clear();
			MyListBox.Items.Clear();
			TextBlockStatus.Text = "Все файлы графиков удалены";
		}

		private void DrawButton_Click(object sender, RoutedEventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					var funchNumbers = GetNumbersSelectedFunctions();
					ClearCanvas();
					GetMaxAndMin(funchNumbers);
					DrawXY();
					var rnd = new Random();
					foreach (var number in funchNumbers)
					{
						var graphPoints = _graphList[number];
						Color color = Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));
						_colorsList[number] = color;
						DrawFunction(graphPoints, color);
					}

					_numberCurrentsGraphs.Clear();
					_numberCurrentsGraphs = funchNumbers.ToList();
				}
				else
				{
					MessageBox.Show("Выберите график!", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				if (MessageBox.Show("Список графиков пуст!\nДобавить из файла?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					AddButton_Click(null, null);
				}
			}

		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			_xMin = 0;
			_yMin = 0;
			_xMax = 0;
			_yMax = 0;
			_numberCurrentsGraphs.Clear();
			ClearCanvas();
		}

		private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			Point mousePosition = e.GetPosition(MyCanvas);
			double xPosition = _xMin + mousePosition.X*(_xMax - _xMin)/MyCanvas.Width;
			double yPosition = _yMin + (MyCanvas.Height - mousePosition.Y)*(_yMax - _yMin)/MyCanvas.Height;
			TextBlockPoint.Text = string.Format("X: {0}\tY: {1}", Math.Round(xPosition, 4), Math.Round(yPosition, 4));
		}

		private void MyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!_numberCurrentsGraphs.Any())
				return;
			ClearCanvas();

			double lenX = _xMax - _xMin;
			double lenY = _yMax - _yMin;
			double deltaX = 0;
			double deltaY = 0;
			if (e.Delta > 0)
			{
				deltaX = -(lenX - lenX / _zoom) / 2.0;
				deltaY = -(lenY - lenY / _zoom) / 2.0;
			}

			if (e.Delta < 0)
			{
				deltaX = (lenX * _zoom - lenX) / 2.0;
				deltaY = (lenY * _zoom - lenY) / 2.0;
			}

			_xMin = _xMin - deltaX;
			_xMax = _xMax + deltaX;
			_yMin = _yMin - deltaY;
			_yMax = _yMax + deltaY;

			foreach (var numbers in _numberCurrentsGraphs)
			{
				DrawFunction(_graphList[numbers], _colorsList[numbers]);
			}
			DrawXY();
		}

		private void AddTextToCanvasX(string text, double x, double y)
		{
			var textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Foreground = System.Windows.Media.Brushes.White;
			Canvas.SetLeft(textBlock, x);
			Canvas.SetBottom(textBlock, y);
			MyCanvasX.Children.Add(textBlock);
		}

		private void AddTextToCanvasY(string text, double x, double y)
		{
			var textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Foreground = System.Windows.Media.Brushes.White;
			Canvas.SetLeft(textBlock, x);
			Canvas.SetBottom(textBlock, y);
			MyCanvasY.Children.Add(textBlock);
		}

		private void ClearCanvas()
		{
			MyCanvas.Children.Clear();
			MyCanvasX.Children.Clear();
			MyCanvasY.Children.Clear();
			for (int i = 0; i <= 10; i++)
			{
				var myLineVertical = new Line
				{
					X1 = 50 * i,
					X2 = 50 * i,
					Y1 = 0,
					Y2 = MyCanvas.Height,
					Stroke = System.Windows.Media.Brushes.Gray
				};

				var myLineHorizontal = new Line
				{
					X1 = 0,
					X2 = MyCanvas.Width,
					Y1 = 50 * i,
					Y2 = 50 * i,
					Stroke = System.Windows.Media.Brushes.Gray
				};
				MyCanvas.Children.Add(myLineVertical);
				MyCanvas.Children.Add(myLineHorizontal);
			}
			TextBlockStatus.Text = "Полотно очищено";
		}

		private void GetMaxAndMin(IEnumerable<int> numGraph)
		{
			_xMax = double.MinValue;
			_xMin = double.MaxValue;
			_yMax = double.MinValue;
			_yMin = double.MaxValue;

			foreach (var number in numGraph)
			{
				var graph = _graphList[number];
				double localXMax = graph.Max(_ => _.X);
				double localXMin = graph.Min(_ => _.X);
				double localYMax = graph.Max(_ => _.Y);
				double localYMin = graph.Min(_ => _.Y);

				double addLenX = (localXMax - localXMin)/4;
				double addLenY = (localYMax - localYMin)/4;
				localXMax += addLenX;
				localXMin -= addLenX;
				localYMax += addLenY;
				localYMin -= addLenY;

				_xMax = localXMax > _xMax ? localXMax : _xMax;
				_xMin = localXMin < _xMin ? localXMin : _xMin;
				_yMax = localYMax > _yMax ? localYMax : _yMax;
				_yMin = localYMin < _yMin ? localYMin : _yMin;
			}
		}

		private void DrawXY()
		{
			double hx = (_xMax - _xMin) / 10.0;
			double hy = (_yMax - _yMin) / 10.0;
			for (int i = 0; i < 10; i++)
			{
				AddTextToCanvasX(string.Format("{0}", Math.Round(_xMin + i * hx, 2)), i * 50, 5);
				AddTextToCanvasY(string.Format("{0}", Math.Round(_yMin + i * hy, 2)), 5, i * 50 + 20);
			}
		}

		private void DrawFunction(IEnumerable<Point> pointsList, Color color)
		{
			var line = new Polyline();
			foreach (var point in pointsList)
			{
				Point p = GetNewCoordinates(point);
				line.Points.Add(p);
			}
	
			line.Stroke = new SolidColorBrush(color);
			line.StrokeThickness = 3;
			MyCanvas.Children.Add(line);
		}

		private void RevertButton_Click(object sender, RoutedEventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					var funchNumbers = GetNumbersSelectedFunctions();
					funchNumbers = RevertGraph(funchNumbers);
					ClearCanvas();
					GetMaxAndMin(funchNumbers);
					DrawXY();
					var rnd = new Random();
					foreach (var number in funchNumbers)
					{
						var graphPoints = _graphList[number];
						Color color = Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));
						_colorsList[number] = color;
						DrawFunction(graphPoints, color);
					}

					_numberCurrentsGraphs.Clear();
					_numberCurrentsGraphs = funchNumbers.ToList();
				}
				else
				{
					MessageBox.Show("Выберите график!", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				if (MessageBox.Show("Список графиков пуст!\nДобавить из файла?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					AddButton_Click(null, null);
				}
			}
		}

		private IEnumerable<int> RevertGraph(IEnumerable<int> numGraph)
		{
			var revertNumberList = new List<int>();
			int currentCountInList = _graphList.Count;
			foreach (var number in numGraph)
			{
				currentCountInList++;
				var newPointsList = _graphList[number].Select(_ => new Point(_.X,-_.Y));
				_graphList.Add(newPointsList.ToList());
				revertNumberList.Add(currentCountInList - 1);
				MyListBox.Items.Add(new GraphicInfo { Code =  currentCountInList, Name = string.Format("-F{0}(x)", number + 1) });
			}
			return revertNumberList;
		}

		private void UnionButton_Click(object sender, RoutedEventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					var funchNumbers = GetNumbersSelectedFunctions();
					funchNumbers = UnionGraph(funchNumbers);
					ClearCanvas();
					GetMaxAndMin(funchNumbers);
					DrawXY();
					var rnd = new Random();
					foreach (var number in funchNumbers)
					{
						var graphPoints = _graphList[number];
						Color color = Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));
						_colorsList[number] = color;
						DrawFunction(graphPoints, color);
					}
					_numberCurrentsGraphs.Clear();
					_numberCurrentsGraphs = funchNumbers.ToList();
				}
				else
				{
					MessageBox.Show("Выберите график!", "", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			else
			{
				if (MessageBox.Show("Список графиков пуст!\nДобавить из файла?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					AddButton_Click(null, null);
				}
			}
		}

		private IEnumerable<int> UnionGraph(IEnumerable<int> numGraph)
		{
			var unionNumberList = new List<int>();
			int currentCountInList = _graphList.Count;
			var unionGraph = new List<Point>();
			foreach (var number in numGraph)
			{
				var newPointsList = _graphList[number];
				foreach (var point in newPointsList)
				{
					var findResult = unionGraph.Find(_ => _.X.IsEqual(point.X));
					if (findResult != default(Point))
					{
						unionGraph.Remove(findResult);
						findResult.X = (findResult.X + point.X)/2.0;
						findResult.Y = (findResult.Y + point.Y)/2.0;
						unionGraph.Add(findResult);
					}
					else
					{
						unionGraph.Add(point);
					}
				}
			}
			currentCountInList++;
			unionGraph = unionGraph.OrderBy(_ => _.X).ToList();
			_graphList.Add(unionGraph);
			MyListBox.Items.Add(new GraphicInfo { Code = currentCountInList, Name = string.Format("Union(Fi(x))") });
			unionNumberList.Add(currentCountInList - 1);
			return unionNumberList;
		}

		private IEnumerable<int> GetNumbersSelectedFunctions()
		{
			return 
				from object funch in MyListBox.SelectedItems
				select Int32.Parse(funch.ToString())
				into k
				select k - 1;
		}

		private Point GetNewCoordinates(Point point)
		{
			double x = MyCanvas.Width * (point.X - _xMin) / (_xMax - _xMin);
			double y = MyCanvas.Height * (1 - (point.Y - _yMin) / (_yMax - _yMin));
			return new Point(x, y);
		}

	}

	public class GraphicInfo
	{
		public int Code { get; set; }

		public String Name { get; set; }

		public override string ToString()
		{
			return Code.ToString();
		}
	}
}
