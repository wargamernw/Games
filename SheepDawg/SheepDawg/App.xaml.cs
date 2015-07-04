namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		MovingObjectController mover = new MovingObjectController();

		MovingObject dog = new MovingObject(new Point(0, 0), 50, 100, Math.PI, 0);

		public App()
		{
			this.mover.Objects.Add(this.dog);
			this.mover.Start();

			var drawTimer = new DispatcherTimer();
			drawTimer.Tick += (s, e) => { (this.MainWindow as SheepDawg.MainWindow).Host.Draw(); };
			drawTimer.Interval = TimeSpan.FromMilliseconds(33);
			drawTimer.Start();
		}

		public void OnMouseDown(Point pos)
		{
			this.dog.Destination = pos;
		}

		public IEnumerable<MovingObject> GetDrawList()
		{
			return mover.Objects;
		}
	}
}
