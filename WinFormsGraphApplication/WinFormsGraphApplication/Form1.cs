using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinFormsGraphApplication.Extensions;
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
		private bool _isClear;
		private bool _isMouseWhell;

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
			_zoom = 1.03;
			_isClear = false;
			_isMouseWhell = false;
		}

		private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
		{
			var mousePosition = new Point(e.X, e.Y);
			double xPosition = _xMin + mousePosition.X * (_xMax - _xMin) / MyCanvas.Width;
			double yPosition = _yMin + (MyCanvas.Height - mousePosition.Y) * (_yMax - _yMin) / MyCanvas.Height;
			TextBlockPoint.Text = string.Format("X: {0}\t  Y: {1}", Math.Round(xPosition, 4), Math.Round(yPosition, 4));
		}

		private void MyCanvas_Paint(object sender, PaintEventArgs e)
		{
				MyCanvasX.Invalidate();
				MyCanvasY.Invalidate();
			
			var rnd = new Random();

			Graphics myGraphics = MyCanvas.CreateGraphics();

			for (int i = 0; i <= 10; i++)
			{
				myGraphics.DrawLine(new Pen(Color.LightGray, 2), 50 * i, 0, 50 * i, MyCanvas.Height);
				myGraphics.DrawLine(new Pen(Color.LightGray, 2), 0, 50 * i, MyCanvas.Width, 50 * i);
			}
			//if (_isClear)
			//{
			//	_isClear = false;
			//	return;
			//}

			var funchNumbers = GetNumbersSelectedFunctions();
			if (!_isMouseWhell)
			{
				GetBounds(funchNumbers);
			}
			

			foreach (var number in funchNumbers)
			{
				List<Point> pointsList = _graphList[number];
				if (!_isMouseWhell)
				{
					_colorsList[number] = new Pen(Color.FromArgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)), 3);
				}

				myGraphics.DrawCurve(_colorsList[number], pointsList.Select(_ => new PointF { X = (float)GetNewCoordinates(_).X, Y = (float)GetNewCoordinates(_).Y }).ToArray());
			}
			if (!_isMouseWhell)
			{
				TextBlockStatus.Text = string.Format("Нарисовано графиков: {0}", funchNumbers.Count());
			}
			_isMouseWhell = false;
		}

		private void MyCanvasY_Paint(object sender, PaintEventArgs e)
		{
			if (MyListBox.SelectedItems.Count == 0)
			{
				return;
			}

			Graphics myGraphics = MyCanvasY.CreateGraphics();
			Brush brush = Brushes.White;
			Font font = new Font("Microsoft Sans Serif", 9F);

			double hy = (_yMax - _yMin) / 10.0;
			for (int i = 0; i < 10; i++)
			{
				myGraphics.DrawString(string.Format("{0}", Math.Round(_yMin + i * hy, 3)), font, brush, 0,MyCanvasY.Height - i*50 - 38);
			}

		}

		private void MyCanvasX_Paint(object sender, PaintEventArgs e)
		{
			if (MyListBox.SelectedItems.Count == 0)
			{
				return;
			}

			Graphics myGraphics = MyCanvasX.CreateGraphics();
			Brush brush = Brushes.White;
			Font font = new Font("Microsoft Sans Serif", 9F);

			double hx = (_xMax - _xMin) / 10.0;
			double hy = (_yMax - _yMin) / 10.0;
			for (int i = 0; i < 10; i++)
			{
				myGraphics.DrawString(string.Format("{0}", Math.Round(_xMin + i * hx, 3)), font, brush, i*50, 0);
			}

			
		}

		private Point GetNewCoordinates(Point point)
		{
			if (Math.Abs(_xMax - _xMin) < 0.000001 || Math.Abs(_yMax - _yMin) < 0.000001)
				return default(Point);
			double x = MyCanvas.Width * (point.X - _xMin) / (_xMax - _xMin);
			double y = MyCanvas.Height * (1 - (point.Y - _yMin) / (_yMax - _yMin));
			return new Point(x, y);
		}

		private void GetBounds(IEnumerable<int> numGraph)
		{
			if (!numGraph.Any())
			{
				_xMax = 0;
				_xMin = 0;
				_yMax = 0;
				_yMin = 0;
				return;
			}
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

			if (Math.Abs(_yMax - _yMin) < 0.0001)
			{
				_yMax = (_xMax - _xMin) / 2.0;
				_yMin = -(_xMax - _xMin) / 2.0;
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

		private void DeleteButton_Click(object sender, EventArgs e)
		{
			_graphList.Clear();
			MyListBox.Items.Clear();
			TextBlockStatus.Text = "Все файлы графиков удалены";
		}

		private void RevertButton_Click(object sender, EventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					var funchNumbers = GetNumbersSelectedFunctions();
					var funchNumbersAdd = new List<int>();
					funchNumbersAdd.AddRange(funchNumbers);
					funchNumbersAdd = RevertGraph(funchNumbersAdd).ToList();
					MyListBox.SelectedItems.Clear();
					foreach (var funch in funchNumbersAdd)
					{
						MyListBox.SelectedItems.Add(MyListBox.Items[funch]);
					}
					
					MyCanvas.Invalidate();
				}
				else
				{
					MessageBox.Show("Выберите фукцию!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private IEnumerable<int> RevertGraph(IEnumerable<int> funchNumbers)
		{
			var revertNumberList = new List<int>();
			int currentCountInList = _graphList.Count;
			foreach (var number in funchNumbers)
			{
				currentCountInList++;
				var newPointsList = _graphList[number].Select(_ => new Point(_.X, -_.Y));
				_graphList.Add(newPointsList.ToList());
				revertNumberList.Add(currentCountInList - 1);
				MyListBox.Items.Add(new GraphicInfo { Code = currentCountInList, Name = string.Format("-F{0}(x)", number + 1) });
			}
			return revertNumberList;
		}

		private void ClearButton_Click(object sender, EventArgs e)
		{
			MyListBox.SelectedItems.Clear();
			TextBlockStatus.Text = "Полотно очищено";
			_isClear = true;
			MyCanvas.Invalidate();
		}

		private void UnionButton_Click(object sender, EventArgs e)
		{
			if (_graphList.Any())
			{
				if (MyListBox.SelectedItems != null && MyListBox.SelectedItems.Count != 0)
				{
					var funchNumbers = GetNumbersSelectedFunctions();
					var funchNumbersAdd = new List<int>();
					funchNumbersAdd.AddRange(funchNumbers);
					funchNumbersAdd = UnionGraph(funchNumbersAdd).ToList();
					MyListBox.SelectedItems.Clear();
					foreach (var funch in funchNumbersAdd)
					{
						MyListBox.SelectedItems.Add(MyListBox.Items[funch]);
					}

					MyCanvas.Invalidate();
				}
				else
				{
					MessageBox.Show("Выберите фукцию!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private IEnumerable<int> UnionGraph(IEnumerable<int> funchNumbersAdd)
		{
			var unionNumberList = new List<int>();
			int currentCountInList = _graphList.Count;
			var unionGraph = new List<Point?>();
			foreach (var number in funchNumbersAdd)
			{
				var newPointsList = _graphList[number];
				foreach (var point in newPointsList)
				{
					var findResult = unionGraph.Find(_ => _.Value.X.IsEqual(point.X));
					if (findResult != null)
					{
						unionGraph.Remove(findResult);
						Point tmPoint = findResult.GetValueOrDefault();
						tmPoint.X = (tmPoint.X + point.X) / 2.0;
						tmPoint.Y = (tmPoint.Y + point.Y) / 2.0;
						unionGraph.Add(tmPoint);
					}
					else
					{
						unionGraph.Add(point);
					}
				}
			}
			currentCountInList++;
			unionGraph = unionGraph.OrderBy(_ => _.Value.X).ToList();
			_graphList.Add(unionGraph.ConvertAll(_ => _.Value));
			MyListBox.Items.Add(new GraphicInfo { Code = currentCountInList, Name = string.Format("Union(Fi(x))") });
			unionNumberList.Add(currentCountInList - 1);
			return unionNumberList;
		}

		private void MyCanvas_MouseWhell(object sender, MouseEventArgs e)
		{
			if (!GetNumbersSelectedFunctions().Any())
				return;
			var mousePoint = new Point(e.X, e.Y);

			double lenX = _xMax - _xMin;
			double lenY = _yMax - _yMin;
			double deltaXMin = 0;
			double deltaXMax = 0;
			double deltaYMin = 0;
			double deltaYMax = 0;

			if (e.Delta > 0)
			{
				deltaXMin = -(lenX - lenX / _zoom) * (mousePoint.X / MyCanvas.Width);
				deltaXMax = -(lenX - lenX / _zoom) * (1 - mousePoint.X / MyCanvas.Width); ;
				deltaYMin = -(lenY - lenY / _zoom) * ((MyCanvas.Height - mousePoint.Y) / MyCanvas.Height);
				deltaYMax = -(lenY - lenY / _zoom) * (1 - (MyCanvas.Height - mousePoint.Y) / MyCanvas.Height);
				TextBlockStatus.Text = string.Format("Увеличено в {0} раза", _zoom);
			}

			if (e.Delta < 0)
			{
				deltaXMin = -(lenX - lenX * _zoom) * (mousePoint.X / MyCanvas.Width);
				deltaXMax = -(lenX - lenX * _zoom) * (1 - mousePoint.X / MyCanvas.Width); ;
				deltaYMin = -(lenY - lenY * _zoom) * ((MyCanvas.Height - mousePoint.Y) / MyCanvas.Height);
				deltaYMax = -(lenY - lenY * _zoom) * (1 - (MyCanvas.Height - mousePoint.Y) / MyCanvas.Height);
				TextBlockStatus.Text = string.Format("Уменьшено в {0} раза", _zoom);
			}

			double oldXMin = _xMin;
			double oldXMAx = _xMax;
			double oldYMin = _yMin;
			double OldYMax = _yMax;

			_xMin = _xMin - deltaXMin;
			_xMax = _xMax + deltaXMax;
			_yMin = _yMin - deltaYMin;
			_yMax = _yMax + deltaYMax;

			if (Math.Abs(_xMax - _xMin) < 0.01 || Math.Abs(_yMax - _yMin) < 0.01)
			{
				_xMin = oldXMin;
				_xMax = oldXMAx;
				_yMin = oldYMin;
				_yMax = OldYMax;
				TextBlockStatus.Text = "Дальнейший зум невозможен!";
			}
			_isMouseWhell = true;
			MyCanvas.Invalidate();
		}

		private void MyCanvas_MouseEnter(object sender, EventArgs e)
		{
			MyCanvas.Focus();
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
