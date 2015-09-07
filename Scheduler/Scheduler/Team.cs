using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Scheduler
{
	[DataContract()]
	class Team
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Stadium { get; set; }

		[DataMember]
		public Division Division { get; set; }

		[DataMember]
		public int Wins { get; set; }

		[DataMember]
		public int TVMarket { get; set; }

		public Team()
		{
			this.Name = string.Empty;
			this.Stadium = string.Empty;
			this.Division = Division.AFCEast;
			this.Wins = 0;
			this.TVMarket = 0;
		}
	}
}
