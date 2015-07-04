
namespace SheepDawg
{
	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Media;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public VisualContainer Host;

		public MainWindow()
		{
			Host = new VisualContainer();
			InitializeComponent();

			this.RenderContent.Content = Host;
			Host.AddVisual();
		}

		private void BackgroundGrid_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
		{
			double x = (double)this.BackgroundGrid.LayoutTransform.GetValue(ScaleTransform.ScaleXProperty);
			x = Math.Min(2.0, x + (double)e.Delta / 1200.0);
			x = Math.Max(x, 0);
			this.BackgroundGrid.LayoutTransform.SetValue(ScaleTransform.ScaleXProperty, x);

			double y = (double)this.BackgroundGrid.LayoutTransform.GetValue(ScaleTransform.ScaleYProperty);
			y = Math.Min(2.0, y + (double)e.Delta / 1200.0);
			y = Math.Max(x, 0);
			this.BackgroundGrid.LayoutTransform.SetValue(ScaleTransform.ScaleYProperty, x);

			e.Handled = true;
		}

		private void BackgroundGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var app = App.Current as SheepDawg.App;
			app.OnMouseDown(e.GetPosition(this.BackgroundGrid));
			Debug.WriteLine(e.GetPosition(this.BackgroundGrid).ToString());
		}
	}
}
