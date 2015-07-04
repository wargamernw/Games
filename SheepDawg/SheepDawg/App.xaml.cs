namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Data;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Threading;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		MovingObjectController mover = new MovingObjectController();

		MovingObject dog;

		List<MovingObject> sheep = new List<MovingObject>();

		

		public App()
		{
			string[] args = Environment.GetCommandLineArgs();

			if (args.Length == 3)
			{
				if (string.Compare(args[1].ToLower(), "/gen") == 0)
				{
					this.CreateObjects(args[2]);
					System.Environment.Exit(1);
				}
			}

			this.GenerateMap();

			this.mover.Start();

			var drawTimer = new DispatcherTimer();
			drawTimer.Tick += (s, e) => { (this.MainWindow as SheepDawg.MainWindow).Host.Draw(); };
			drawTimer.Interval = TimeSpan.FromMilliseconds(33);
			drawTimer.Start();
		}

		public void OnMouseDown(Point pos)
		{
			this.dog.Pos.Destination = pos;
		}

		public IEnumerable<MovingObject> GetDrawList()
		{
			return mover.Objects;
		}

		private void CreateObjects(string path)
		{
			System.IO.Directory.CreateDirectory(path);
			
			var dog = new MovingObject(null);
			dog.Data.SetProperties(100, Colors.Blue, 50, 5, Math.PI);
			dog.Save(path + "/SheepDog.xml");


			var sheep = new MovingObject(null);
			sheep.Data.SetProperties(100, Colors.White, 50, 5, Math.PI);
			sheep.Save(path + "/Sheep.xml");
		}

		private void GenerateMap()
		{
			var rand = new Random();

			this.dog = new MovingObject(mover);
			this.dog.Load("./Data/SheepDog.xml");
			this.mover.Objects.Add(this.dog);

			this.dog.Pos.Location = new Point(50, 50);
			this.dog.Pos.Destination = this.dog.Pos.Location;
			this.dog.Pos.Facing = rand.NextDouble() * Math.PI * 2.0 - Math.PI;

			for (int i = 0; i < 10; i++)
			{
				var sheep = new MovingObject(mover);
				sheep.Load("./Data/Sheep.xml");
				this.mover.Objects.Add(sheep);

				var x = rand.Next(200) - 100;
				var y = rand.Next(200) - 100;
				sheep.Pos.Location = new Point(200 + x, 200 + y);
				sheep.Pos.Destination = sheep.Pos.Location;
				sheep.Pos.Facing = rand.NextDouble() * Math.PI * 2.0 - Math.PI;
			}
		}
	}
}
