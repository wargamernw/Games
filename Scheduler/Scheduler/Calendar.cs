using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	class Calendar
	{
		public List<GameWeek> Schedule { get; set; }

		public const int GameWeeks = 4;

		private Model model;

		public Calendar(Model model)
		{
			this.model = model;
			this.Schedule = new List<GameWeek>();

			for (int i = 0; i < GameWeeks; i++)
			{
				this.Schedule.Add(new GameWeek(Model.TeamsMax));
			}
		}

		public void CreateSchedule()
		{
			for (int gameWeek = 0; gameWeek < GameWeeks; gameWeek++)
			{
				var gameDate = new GameDate(gameWeek, true, false, true, false);
				List<GameTime> usedTimes = new List<GameTime>();
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
					curGame.GameTime = PickGameTime(gameDate, usedTimes);
					usedTimes.Add(curGame.GameTime);
				}
			}
		}

		Team PickTeam(List<Team> usedTeams)
		{
			var rand = new Random();

			Team pick = null;

			while (pick == null)
			{
				var which = rand.Next(Model.TeamsMax);

				var select = this.model.Teams[which];

				if (!usedTeams.Contains(select))
				{
					pick = select;
				}
			}

			return pick;
		}

		GameTime PickGameTime(GameDate gameDate, List<GameTime> usedTimes)
		{
			var rand = new Random();

			GameTime gameTime = null;

			while (gameTime == null)
			{
				var testTime = new GameTime();
				testTime.GameDay = (GameDay)rand.Next(Enum.GetValues(typeof(GameDay)).Length);
				testTime.TimeOfDay = (TimeOfDay)rand.Next(Enum.GetValues(typeof(TimeOfDay)).Length);

				if (!gameDate.IsUsed(testTime))
				{
					gameTime = testTime;
				}
			}

			return gameTime;
		}
	}
}
