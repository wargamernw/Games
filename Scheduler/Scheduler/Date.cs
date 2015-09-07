using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Scheduler
{
	public enum GameDay
	{
		Thursday,
		Sunday,
		Monday
	}

	public enum TimeOfDay
	{
		EarlyMorning,
		Morning,
		Afternoon,
		Night,
		LateNight
	}

	[DataContract()]
	class Date
	{
		[DataMember]
		public int GameWeek 
		{ 
			get; 
			set; 
		}

		[DataMember]
		public GameDay GameDay { get; set; }

		[DataMember]
		public TimeOfDay TimeOfDay { get; set; }

		public Date()
		{
			this.GameWeek = 0;
			this.GameDay = GameDay.Sunday;
			this.TimeOfDay = TimeOfDay.Afternoon;
		}
	}
}
