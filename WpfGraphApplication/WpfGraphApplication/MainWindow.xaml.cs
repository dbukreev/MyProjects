using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
			ClearButton_Click(null, null);
		}

		private double _xMin;
		private double _xMax;
		private double _yMin;
		private double _yMax;
		private List<List<Point>> _graphList;
		private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			Point mousePosition = e.GetPosition(MyCanvas);
			double xPosition = _xMin + mousePosition.X*(_xMax - _xMin)/MyCanvas.Width;
			double yPosition = _yMin + (MyCanvas.Height - mousePosition.Y)*(_yMax - _yMin)/MyCanvas.Height;
			TextBlockPoint.Text = string.Format("X: {0}\tY: {1}", xPosition, yPosition);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			_xMin = 0;
			_yMin = 0;
			_xMax = 10;
			_yMax = 10;
			MyCanvas.Children.Clear();
			for (int i = 0; i <= 9; i++)
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
				AddTextToCanvas(i.ToString(), i*50, 0);
				AddTextToCanvas(i.ToString(), 0, i*50);
			}
			TextBlockStatus.Text = "Очищено";
		}

		private void AddTextToCanvas(string text, double x, double y)
		{
			var textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.Foreground = System.Windows.Media.Brushes.Black;
			Canvas.SetLeft(textBlock, x);
			Canvas.SetBottom(textBlock, y);
			MyCanvas.Children.Add(textBlock);
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
	
			_graphList = new List<List<Point>>();
			int counter = 1;
			foreach (var fileName in dialog.FileNames)
			{

				var pointList = new List<Point>();
				var file = new StreamReader(fileName);
				string line;
				while ((line = file.ReadLine()) != null)
				{
					double x = double.Parse(line.Split()[0]);
					double y = double.Parse(line.Split()[1]);
					pointList.Add(new Point(x, y));
				}
				file.Close();
				_graphList.Add(pointList);
				MyListBox.Items.Add(new States{ Code = counter, Name = string.Format("График {0}", counter)});
				counter++;
			}
		}

		private void DrawButton_Click(object sender, RoutedEventArgs e)
		{
			if (_graphList != null && _graphList.Any())
			{
				var selectedGraphs =  MyListBox.SelectedItems;
				foreach (var graph in selectedGraphs)
				{
					int i = graph.Code;
					var points = _graphList[i];
					var polyline = new Polyline();
					polyline.Points = new PointCollection(points);
				}
			}
		}
	}

	public class States
	{
		public int Code
		{ get; set; }
		public String Name
		{ get; set; }
	}
}
