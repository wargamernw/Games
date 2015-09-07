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
		private Model dataModel = new Model();

		private Calendar calendar;

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

			this.calendar = new Calendar(this.dataModel);
			this.calendar.CreateSchedule();
		}
	}
}
