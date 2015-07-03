
namespace SheepDawg
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var host = new VisualContainer();
            this.RenderContent.Content = host;
            host.AddVisuals();
        }

        private void BackgroundGrid_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            double x = (double)this.BackgroundGrid.LayoutTransform.GetValue(ScaleTransform.ScaleXProperty);
            x = Math.Min(1.0, x + (double)e.Delta / 1200.0);
            x = Math.Max(x, 0);
            this.BackgroundGrid.LayoutTransform.SetValue(ScaleTransform.ScaleXProperty, x);

            double y = (double)this.BackgroundGrid.LayoutTransform.GetValue(ScaleTransform.ScaleYProperty);
            y = Math.Min(1.0, y + (double)e.Delta / 1200.0);
            y = Math.Max(x, 0);
            this.BackgroundGrid.LayoutTransform.SetValue(ScaleTransform.ScaleYProperty, x);

            e.Handled = true;
        }
    }
}
