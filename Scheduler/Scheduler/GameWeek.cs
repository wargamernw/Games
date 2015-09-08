using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	public class GameWeek
	{
		public List<Game> Games = new List<Game>();

		public GameWeek(int numTeamsPlaying)
		{
			Debug.Assert(numTeamsPlaying % 2 == 0);

			for (int i = 0; i < numTeamsPlaying / 2; i++)
			{
				this.Games.Add(new Game());
			}
		}
	}
}
