namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using System.Windows.Shapes;

	public class VisualContainer : FrameworkElement
	{
		public VisualCollection children;

		private DrawingVisual visual = new DrawingVisual();

		public VisualContainer()
		{
			this.children = new VisualCollection(this);
		}

		public void AddVisual()
		{
			this.children.Add(this.visual);
		}

		public void Draw()
		{
			var app = App.Current as SheepDawg.App;

			using (var dc = this.visual.RenderOpen())
			{
				foreach (var obj in app.GetDrawList())
				{
					dc.DrawEllipse(new SolidColorBrush(), new Pen(Brushes.Red, 1), obj.Location, 5, 5);

					Vector facing = new Vector();

					facing.X = Math.Cos(obj.Facing);
					facing.Y = Math.Sin(obj.Facing);

					facing *= 5.0;

					dc.DrawLine(new Pen(Brushes.Red, 1), obj.Location, obj.Location + facing);
				}
			}
		}

		protected override int VisualChildrenCount
		{
			get { return this.children.Count; }
		}

		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= this.children.Count)
			{
				throw new ArgumentOutOfRangeException();
			}

			return this.children[index];
		}
	}
}
