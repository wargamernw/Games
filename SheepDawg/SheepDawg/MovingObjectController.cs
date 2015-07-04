namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Threading;

	public class MovingObjectController
	{
		public List<MovingObject> Objects = new List<MovingObject>();


		private DispatcherTimer gameTimer = new DispatcherTimer();

		public MovingObjectController()
		{
			var interval = TimeSpan.FromMilliseconds(33);

			gameTimer.Tick += (s, e) =>
			{
				foreach(var obj in this.Objects)
				{
					obj.OnTick(33.0 / 1000.0);
				}
			};

			gameTimer.Interval = interval;
		}

		public void Start()
		{
			this.gameTimer.Start();
		}

		public void Stop()
		{
			this.gameTimer.Stop();
		}
	}
}
