using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Scheduler
{
	public enum Month
	{
		Jan,
		Feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sep,
		Oct,
		Nov,
		Dec
	}

	public enum TimeOfDay
	{
		EarlyMorning,
		Morning,
		Afternoon,
		Evening,
		LateEvening
	}

	[DataContract()]
	class Date
	{
		[DataMember]
		public Month Month { get; set; }

		[DataMember]
		public int Day { get; set; }

		[DataMember]
		public TimeOfDay TimeOfDay { get; set; }

		public Date()
		{
			this.Month = Month.Jan;
			this.Day = 1;
			this.TimeOfDay = TimeOfDay.Afternoon;
		}
	}
}
