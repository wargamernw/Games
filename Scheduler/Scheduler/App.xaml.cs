using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Scheduler
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public Model dataModel = new Model();

		public Calendar calendar = null;

		public App()
		{
			string[] args = Environment.GetCommandLineArgs();

			if (args.Length == 2)
			{
				if (string.Compare(args[1].ToLower(), "/gen") == 0)
				{
					this.dataModel.CreateSchema();
					System.Environment.Exit(1);
				}
			}

			this.Activated += (o, e) => 
			{
				if (calendar == null)
				{
					this.calendar = new Calendar(this.dataModel);
					this.calendar.CreateSchedule();

					(this.MainWindow as Scheduler.MainWindow).DrawSchedule(calendar);
				}
			};

			this.InitializeComponent();
		}
	}
}
