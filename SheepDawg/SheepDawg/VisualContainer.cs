﻿namespace SheepDawg
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
        private VisualCollection children;

        public VisualContainer()
        {
            children = new VisualCollection(this);

            var visual = new DrawingVisual();
            children.Add(visual);
            using (var dc = visual.RenderOpen())
            {
                dc.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 0), new Point(400, 400));
                dc.DrawLine(new Pen(Brushes.Black, 1), new Point(0, 400), new Point(400, 0));
            }
        }

        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return children[index];
        }
    }
}
