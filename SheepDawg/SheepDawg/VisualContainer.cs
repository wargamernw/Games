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
					var brush = new SolidColorBrush(obj.Pos.Color);

					dc.DrawEllipse(new SolidColorBrush(), new Pen(brush, 1), obj.Pos.Location, obj.Pos.Radius, obj.Pos.Radius);

					Vector facing = new Vector();

					facing.X = Math.Cos(obj.Pos.Facing);
					facing.Y = Math.Sin(obj.Pos.Facing);

					facing *= obj.Pos.Radius;

					dc.DrawLine(new Pen(brush, 1), obj.Pos.Location, obj.Pos.Location + facing);
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
