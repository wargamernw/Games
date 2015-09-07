using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	[DataContract()]
	class Stadium
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public List<Date> BlackoutDates = new List<Date>();

		public Stadium()
		{
			this.Name = string.Empty;
			this.BlackoutDates.Add(new Date());
			this.BlackoutDates.Add(new Date());
		}

	}
}
