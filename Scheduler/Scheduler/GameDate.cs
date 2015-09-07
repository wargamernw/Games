using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	class DateObject
	{
		public Date Date { get; set; }

		public bool Used { get; set; }
	}

	class GameDate
	{
		public DateObject ThursdayNight = null;
		public DateObject ThursdayAfternoon = null;
		public DateObject ThursdayLateNight = null;

		public DateObject SundayEarlyMorning = null;
		public DateObject SundayMorning = null;
		public DateObject SundayAfternoon = null;
		public DateObject SundayNight = null;

		public DateObject MondayNight = null;
		public DateObject MondayAfternoon = null;
		public DateObject MondayLateNight = null;

		bool ExtraThursday { get; set; }

		bool ExtraMonday { get; set; }


		public GameDate(int gameWeek, bool hasThursday, bool extraThursday, bool hasMonday, bool extraMonday)
		{
			Debug.Assert(hasThursday || extraThursday == false);
			Debug.Assert(hasMonday || extraMonday == false);

			this.ExtraThursday = extraThursday;
			this.ExtraMonday = extraMonday;

			if (hasThursday)
			{
				if (this.ExtraThursday)
				{
					this.ThursdayAfternoon = new DateObject() { Date = new Date() { GameDay = GameDay.Thursday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Afternoon } };
					this.ThursdayLateNight = new DateObject() { Date = new Date() { GameDay = GameDay.Thursday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.LateNight } };
				}
				else
				{
					this.ThursdayNight = new DateObject() { Date = new Date() { GameDay = GameDay.Thursday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Night } };
				}
			}

			if (hasMonday)
			{
				if (this.ExtraMonday)
				{
					this.MondayAfternoon = new DateObject() { Date = new Date() { GameDay = GameDay.Monday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Afternoon } };
					this.MondayLateNight = new DateObject() { Date = new Date() { GameDay = GameDay.Monday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.LateNight } };
				}
				else
				{
					this.MondayNight = new DateObject() { Date = new Date() { GameDay = GameDay.Monday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Night } };
				}
			}

			this.SundayMorning = new DateObject() { Date = new Date() { GameDay = GameDay.Sunday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Morning } };
			this.SundayAfternoon = new DateObject() { Date = new Date() { GameDay = GameDay.Sunday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Afternoon } };
			this.SundayNight = new DateObject() { Date = new Date() { GameDay = GameDay.Sunday, GameWeek = gameWeek, TimeOfDay = TimeOfDay.Night } };
		}

		public bool IsUsed(GameTime gameTime)
		{
			bool used = false;

			switch (gameTime.GameDay)
			{
				case GameDay.Thursday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Afternoon)
						{
							used = this.ThursdayAfternoon == null || this.ThursdayAfternoon.Used;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.LateNight)
						{
							used = this.ThursdayLateNight == null || this.ThursdayLateNight.Used;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.Night)
						{
							used = this.ThursdayNight == null || this.ThursdayNight.Used;
						}
						else
						{
							used = true;
						}
					}
					break;

				case GameDay.Sunday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Afternoon
							|| gameTime.TimeOfDay == TimeOfDay.Morning)
						{
							used = false;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.EarlyMorning)
						{
							used = this.SundayEarlyMorning == null || this.SundayEarlyMorning.Used;
						}
						else if(gameTime.TimeOfDay == TimeOfDay.Night)
						{
							used = this.SundayNight == null || this.SundayNight.Used;
						}
						else
						{
							used = true;
						}
					}
					break;

				case GameDay.Monday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Afternoon)
						{
							used = this.MondayAfternoon == null || this.MondayAfternoon.Used;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.LateNight)
						{
							used = this.MondayLateNight == null || this.MondayLateNight.Used;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.Night)
						{
							used = this.MondayNight == null || this.MondayNight.Used;
						}
						else
						{
							used = true;
						}
					}
					break;
			}

			return used;
		}

		public void MarkUsed(GameTime gameTime)
		{
			switch (gameTime.GameDay)
			{
				case GameDay.Thursday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Afternoon)
						{
							this.ThursdayAfternoon.Used = true;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.LateNight)
						{
							this.ThursdayLateNight.Used = true;
						}
						else
						{
							this.ThursdayNight.Used = true;
						}
					}
					break;

				case GameDay.Sunday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Night)
						{
							this.SundayNight.Used = true;
						}
					}
					break;

				case GameDay.Monday:
					{
						if (gameTime.TimeOfDay == TimeOfDay.Afternoon)
						{
							this.MondayAfternoon.Used = true;
						}
						else if (gameTime.TimeOfDay == TimeOfDay.LateNight)
						{
							this.MondayLateNight.Used = true;
						}
						else
						{
							this.MondayNight.Used = true;
						}
					}
					break;
			}
		}
	}
}
