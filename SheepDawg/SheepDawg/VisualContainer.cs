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

        public VisualContainer()
        {
            this.children = new VisualCollection(this);
        }

        public void AddVisuals()
        {
            var visual = new DrawingVisual();
            this.children.Add(visual);
            using (var dc = visual.RenderOpen())
            {
                dc.DrawLine(new Pen(Brushes.Red, 1), new Point(0, 0), new Point(400, 400));
                dc.DrawLine(new Pen(Brushes.Red, 1), new Point(0, 400), new Point(400, 0));
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
