using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	public class Game
	{
		public GameTime GameTime { get; set; }

		public Team Visitor { get; set; }

		public Team Home { get; set; }

		public override string ToString()
		{
			string data = "Home: " + Home.Name + "\nVis:  " + Visitor.Name + "\nDay:  " + GameTime.GameDay.ToString() + "\nTime: " + GameTime.TimeOfDay.ToString();
			return data;
		}
	}
}
