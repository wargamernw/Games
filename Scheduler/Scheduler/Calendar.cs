using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	public class Calendar
	{
		public List<GameWeek> Schedule { get; set; }

		public const int GameWeeks = 4;

		private Model model;

		private Random rand = new Random();


		public Calendar(Model model)
		{
			this.model = model;
			this.Schedule = new List<GameWeek>();
		}

		public void CreateSchedule()
		{
			for (int gameWeek = 0; gameWeek < GameWeeks; gameWeek++)
			{
				this.Schedule.Add(new GameWeek(Model.TeamsMax));
				var gameDate = new GameDate(gameWeek, true, false, true, false);
				List<Team> usedTeams = new List<Team>();

				for (int game = 0; game < Model.TeamsMax / 2; game++)
				{
					var visitor = PickTeam(usedTeams);
					usedTeams.Add(visitor);
					var home = PickTeam(usedTeams);
					usedTeams.Add(home);

					var curGame = this.Schedule[gameWeek].Games[game];
					curGame.Home = home;
					curGame.Visitor = visitor;
					curGame.GameTime = PickGameTime(gameDate);
					gameDate.MarkUsed(curGame.GameTime);
				}
			}
		}

		Team PickTeam(List<Team> usedTeams)
		{
			Team pick = null;

			while (pick == null)
			{
				var which = this.rand.Next(Model.TeamsMax);

				var select = this.model.Teams[which];

				if (!usedTeams.Contains(select))
				{
					pick = select;
				}
			}

			return pick;
		}

		GameTime PickGameTime(GameDate gameDate)
		{
			GameTime gameTime = null;

			while (gameTime == null)
			{
				var testTime = new GameTime();
				testTime.GameDay = (GameDay)this.rand.Next(Enum.GetValues(typeof(GameDay)).Length);
				testTime.TimeOfDay = (TimeOfDay)this.rand.Next(Enum.GetValues(typeof(TimeOfDay)).Length);

				if (!gameDate.IsUsed(testTime))
				{
					gameTime = testTime;
				}
			}

			return gameTime;
		}
	}
}
