namespace WinFormsGraphApplication
{
	partial class Form1
	{
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.MyCanvas = new System.Windows.Forms.Panel();
			this.MyStatusBar = new System.Windows.Forms.StatusStrip();
			this.TextBlockStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.TextBlockPoint = new System.Windows.Forms.ToolStripStatusLabel();
			this.MyCanvasY = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.DrawButton = new System.Windows.Forms.Button();
			this.ClearButton = new System.Windows.Forms.Button();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.MyListBox = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.MyStatusBar.SuspendLayout();
			this.MyCanvasY.SuspendLayout();
			this.SuspendLayout();
			// 
			// MyCanvas
			// 
			this.MyCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.MyCanvas.BackColor = System.Drawing.Color.White;
			this.MyCanvas.Location = new System.Drawing.Point(161, 0);
			this.MyCanvas.Name = "MyCanvas";
			this.MyCanvas.Size = new System.Drawing.Size(500, 500);
			this.MyCanvas.TabIndex = 0;
			// 
			// MyStatusBar
			// 
			this.MyStatusBar.BackColor = System.Drawing.SystemColors.MenuHighlight;
			this.MyStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TextBlockStatus,
            this.TextBlockPoint});
			this.MyStatusBar.Location = new System.Drawing.Point(0, 525);
			this.MyStatusBar.Name = "MyStatusBar";
			this.MyStatusBar.Size = new System.Drawing.Size(662, 22);
			this.MyStatusBar.TabIndex = 1;
			this.MyStatusBar.Text = "statusStrip1";
			// 
			// TextBlockStatus
			// 
			this.TextBlockStatus.ForeColor = System.Drawing.Color.White;
			this.TextBlockStatus.Name = "TextBlockStatus";
			this.TextBlockStatus.Size = new System.Drawing.Size(46, 17);
			this.TextBlockStatus.Text = "Статус:";
			// 
			// TextBlockPoint
			// 
			this.TextBlockPoint.ForeColor = System.Drawing.Color.White;
			this.TextBlockPoint.Margin = new System.Windows.Forms.Padding(400, 3, 0, 2);
			this.TextBlockPoint.Name = "TextBlockPoint";
			this.TextBlockPoint.Size = new System.Drawing.Size(30, 17);
			this.TextBlockPoint.Text = "X: Y:";
			// 
			// MyCanvasY
			// 
			this.MyCanvasY.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.MyCanvasY.Controls.Add(this.panel1);
			this.MyCanvasY.Location = new System.Drawing.Point(119, 0);
			this.MyCanvasY.Name = "MyCanvasY";
			this.MyCanvasY.Size = new System.Drawing.Size(41, 525);
			this.MyCanvasY.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(47, 499);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(477, 23);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
			this.panel2.Location = new System.Drawing.Point(159, 499);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(503, 26);
			this.panel2.TabIndex = 3;
			// 
			// DrawButton
			// 
			this.DrawButton.Image = global::WinFormsGraphApplication.Properties.Resources.Draw;
			this.DrawButton.Location = new System.Drawing.Point(11, 11);
			this.DrawButton.Name = "DrawButton";
			this.DrawButton.Size = new System.Drawing.Size(40, 35);
			this.DrawButton.TabIndex = 0;
			this.DrawButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.DrawButton.UseVisualStyleBackColor = true;
			// 
			// ClearButton
			// 
			this.ClearButton.Image = global::WinFormsGraphApplication.Properties.Resources.Clear;
			this.ClearButton.Location = new System.Drawing.Point(61, 11);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(40, 35);
			this.ClearButton.TabIndex = 4;
			this.ClearButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ClearButton.UseVisualStyleBackColor = true;
			// 
			// DeleteButton
			// 
			this.DeleteButton.Image = global::WinFormsGraphApplication.Properties.Resources.Delete;
			this.DeleteButton.Location = new System.Drawing.Point(62, 62);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(40, 35);
			this.DeleteButton.TabIndex = 6;
			this.DeleteButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.DeleteButton.UseVisualStyleBackColor = true;
			// 
			// AddButton
			// 
			this.AddButton.Image = global::WinFormsGraphApplication.Properties.Resources.Add;
			this.AddButton.Location = new System.Drawing.Point(12, 62);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(40, 35);
			this.AddButton.TabIndex = 5;
			this.AddButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.AddButton.UseVisualStyleBackColor = true;
			// 
			// MyListBox
			// 
			this.MyListBox.FormattingEnabled = true;
			this.MyListBox.Location = new System.Drawing.Point(11, 115);
			this.MyListBox.Name = "MyListBox";
			this.MyListBox.Size = new System.Drawing.Size(90, 95);
			this.MyListBox.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Image = global::WinFormsGraphApplication.Properties.Resources.Union;
			this.button1.Location = new System.Drawing.Point(62, 242);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(40, 35);
			this.button1.TabIndex = 8;
			this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.button2.Image = global::WinFormsGraphApplication.Properties.Resources.Revert;
			this.button2.Location = new System.Drawing.Point(12, 242);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(39, 35);
			this.button2.TabIndex = 7;
			this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.button2.UseVisualStyleBackColor = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(662, 547);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.MyListBox);
			this.Controls.Add(this.DeleteButton);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.ClearButton);
			this.Controls.Add(this.DrawButton);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.MyCanvasY);
			this.Controls.Add(this.MyStatusBar);
			this.Controls.Add(this.MyCanvas);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(678, 586);
			this.MinimumSize = new System.Drawing.Size(678, 586);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Draw Graph Application";
			this.MyStatusBar.ResumeLayout(false);
			this.MyStatusBar.PerformLayout();
			this.MyCanvasY.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel MyCanvas;
		private System.Windows.Forms.StatusStrip MyStatusBar;
		private System.Windows.Forms.ToolStripStatusLabel TextBlockStatus;
		private System.Windows.Forms.ToolStripStatusLabel TextBlockPoint;
		private System.Windows.Forms.Panel MyCanvasY;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button DrawButton;
		private System.Windows.Forms.Button ClearButton;
		private System.Windows.Forms.Button DeleteButton;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.ListBox MyListBox;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}

