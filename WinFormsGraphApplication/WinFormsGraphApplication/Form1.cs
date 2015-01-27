using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Point = System.Windows.Point;

namespace WinFormsGraphApplication
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			MyInizializeComponent();
		}

	
		private double _xMin;
		private double _xMax;
		private double _yMin;
		private double _yMax;
		private List<List<Point>> _graphList;
		private Pen[] _colorsList;
		private List<int> _numberCurrentsGraphs;
		private double _zoom;

		private void AddButton_Click(object sender, EventArgs e)
		{
			var dialog = new OpenFileDialog
			{
				DefaultExt = ".txt",
				Filter = "Файлы txt (*.txt)|*.txt",
				Multiselect = true
			};
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.Cancel || result == DialogResult.Ignore)
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
				MyListBox.DisplayMember = "Name";
				counter++;
			}
			TextBlockStatus.Text = string.Format("Загружено функций: {0}", counter - 1);
		}

		private void MyInizializeComponent()
		{
			_graphList = new List<List<Point>>();
			_numberCurrentsGraphs = new List<int>();
			_colorsList = new Pen[20];
			_zoom = 1.1;
			ClearCanvas();
		}

		private void ClearCanvas()
		{
			//MyCanvas.Invalidate();
			//MyCanvasX.Invalidate();
			//MyCanvasY.Invalidate();
			//for (int i = 0; i <= 10; i++)
			//{
			//	var myLineVertical = new Line
			//	{
			//		X1 = 50 * i,
			//		X2 = 50 * i,
			//		Y1 = 0,
			//		Y2 = MyCanvas.Height,
			//		Stroke = System.Windows.Media.Brushes.Gray
			//	};

			//	var myLineHorizontal = new Line
			//	{
			//		X1 = 0,
			//		X2 = MyCanvas.Width,
			//		Y1 = 50 * i,
			//		Y2 = 50 * i,
			//		Stroke = System.Windows.Media.Brushes.Gray
			//	};
			//	MyCanvas.Children.Add(myLineVertical);
			//	MyCanvas.Children.Add(myLineHorizontal);
			//}
			TextBlockStatus.Text = "Полотно очищено";
		}

		private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			var mousePosition = new Point(e.X, e.Y);
			double xPosition = _xMin + mousePosition.X * (_xMax - _xMin) / MyCanvas.Width;
			double yPosition = _yMin + (MyCanvas.Height - mousePosition.Y) * (_yMax - _yMin) / MyCanvas.Height;
			TextBlockPoint.Text = string.Format("X: {0}\tY: {1}", Math.Round(xPosition, 4), Math.Round(yPosition, 4));
		}

		private void MyCanvas_Paint(object sender, PaintEventArgs e)
		{
			//MyCanvasX.Invalidate();
			//MyCanvasY.Invalidate();

			Graphics panelGraphics = this.MyCanvas.CreateGraphics();

			for (int i = 0; i <= 10; i++)
			{
				panelGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.LightGray, 2), 50*i, 0, 50*i, MyCanvas.Height);
				panelGraphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.LightGray, 2), 0, 50 * i, MyCanvas.Width, 50 * i);
			}

			var funchNumbers = GetNumbersSelectedFunctions();
			GetBounds(funchNumbers);
			foreach (var number in funchNumbers)
			{
				List<Point> pointsList = _graphList[number];
				_colorsList[number] = new System.Drawing.Pen(System.Drawing.Color.LightGray, 2);
				panelGraphics.DrawLines(_colorsList[number], pointsList.Select(_ => new PointF { X = (float)GetNewCoordinates(_).X, Y = (float) GetNewCoordinates(_).Y }).ToArray());
			}
		}

		private void MyCanvasY_Paint(object sender, PaintEventArgs e)
		{
			
			
		}

		private void MyCanvasX_Paint(object sender, PaintEventArgs e)
		{

		}

		private Point GetNewCoordinates(Point point)
		{
			double x = MyCanvas.Width * (point.X - _xMin) / (_xMax - _xMin);
			double y = MyCanvas.Height * (1 - (point.Y - _yMin) / (_yMax - _yMin));
			return new Point(x, y);
		}

		private void GetBounds(IEnumerable<int> numGraph)
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

				double addLenX = (localXMax - localXMin) / 4;
				double addLenY = (localYMax - localYMin) / 4;
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

		private void DrawButton_Click(object sender, EventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems.Count != 0)
				{
					MyCanvas.Invalidate();
				}
				else
				{
					MessageBox.Show("Выберите фукцию!","", MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
			}
			else
			{
				if (MessageBox.Show("Список функций пуст!\nДобавить из файла?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					AddButton_Click(null, null);
				}
			}
		}

		private IEnumerable<int> GetNumbersSelectedFunctions()
		{
			return
				from object funch in MyListBox.SelectedItems
				select Int32.Parse(funch.ToString())
					into k
					select k - 1;
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
