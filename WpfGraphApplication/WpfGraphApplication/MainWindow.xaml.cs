using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

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
			ClearButton_Click(null, null);
			_graphList = new List<List<Point>>();
		}

		private double _xMin;
		private double _xMax;
		private double _yMin;
		private double _yMax;
		private List<List<Point>> _graphList;
		//ScaleTransform st = new ScaleTransform();

		private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			Point mousePosition = e.GetPosition(MyCanvas);
			double xPosition = _xMin + mousePosition.X*(_xMax - _xMin)/MyCanvas.Width;
			double yPosition = _yMin + (MyCanvas.Height - mousePosition.Y)*(_yMax - _yMin)/MyCanvas.Height;
			TextBlockPoint.Text = string.Format("X: {0}\tY: {1}", Math.Round(xPosition, 4), Math.Round(yPosition, 4));
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			_xMin = 0;
			_yMin = 0;
			_xMax = 10;
			_yMax = 10;
			MyCanvas.Children.Clear();
			MyCanvasX.Children.Clear();
			MyCanvasY.Children.Clear();
			for (int i = 0; i <= 10; i++)
			{
				var myLineVertical = new Line
				{
					X1 = 50*i,
					X2 = 50*i,
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
			TextBlockStatus.Text = "Очищено";
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
					double x = double.Parse(line.Split()[0].Replace(".",","));
					double y = double.Parse(line.Split()[1].Replace(".", ","));
					pointList.Add(new Point(x, y));
				}
				file.Close();
				_graphList.Add(pointList);
				MyListBox.Items.Add(new States { Code = counter + currentCountInList, Name = string.Format("График {0}", counter + currentCountInList) });
				counter++;
			}
			TextBlockStatus.Text = string.Format("Загружено графиков: {0}", counter - 1);
		}

		private void DrawButton_Click(object sender, RoutedEventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					ClearButton_Click(null, null);
					var selectedGraphs = MyListBox.SelectedItems;
					var numGraph = (from object graph in selectedGraphs
									select Int32.Parse(graph.ToString())
										into k
										select k - 1).ToList();
					GetMaxAndMin(numGraph);
					DrawGraph(numGraph);
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

		private Point GetNewCoordinates(Point point)
		{
			double x = MyCanvas.Width*(point.X - _xMin)/(_xMax - _xMin);
			double y = MyCanvas.Height*(1 -(point.Y - _yMin)/(_yMax - _yMin));
			return new Point(x, y);
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
				_xMax = localXMax > _xMax ? localXMax : _xMax;
				_xMin = localXMin < _xMin ? localXMin : _xMin;
				_yMax = localYMax > _yMax ? localYMax : _yMax;
				_yMin = localYMin < _yMin ? localYMin : _yMin;
			}
		}

		private void DrawGraph(IEnumerable<int> numGraph)
		{
			var rnd = new Random();
			foreach (var number in numGraph)
			{
				var graphPoints = _graphList[number];
				var line = new Polyline();
				foreach (var point in graphPoints)
				{
					line.Points.Add(GetNewCoordinates(point));
				}
				Color color = Color.FromRgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255));
				line.Stroke = new SolidColorBrush(color);
				line.StrokeThickness = 3;
				MyCanvas.Children.Add(line);
			}

			double hx = (_xMax - _xMin)/10.0;
			double hy = (_yMax - _yMin)/10.0;
			for (int i = 0; i < 10; i++)
			{
				AddTextToCanvasX(string.Format("{0}", Math.Round(_xMin + i * hx, 2)), i*50, 5);
				AddTextToCanvasY(string.Format("{0}", Math.Round(_yMin + i * hy, 2)), 5, i*50 + 20);
			}

			TextBlockStatus.Text = "График построен";
		}

		private void MyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			//const double ScaleRate = 2;
			//Canvas c = sender as Canvas;
			//c.RenderTransform = st;
			//if (e.Delta > 0)
			//{
			//	st.ScaleX *= ScaleRate;
			//	st.ScaleY *= ScaleRate;
			//}
			//else
			//{
			//	st.ScaleX /= ScaleRate;
			//	st.ScaleY /= ScaleRate;
			//}
		}
	}

	public class States
	{
		public int Code { get; set; }

		public String Name { get; set; }

		public override string ToString()
		{
			return Code.ToString();
		}
	}
}
