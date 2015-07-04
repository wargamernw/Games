namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;

	[DataContract()]
	public class MovingObjectDataTemporal
	{
		[DataMember]
		public Point Location { get; set; }

		[DataMember]
		public Point Destination { get; set; }

		// Pixel Points per second
		[DataMember]
		public double Velocity { get; set; }

		// Radians
		[DataMember]
		public double Facing { get; set; }
	}
}
