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
    [KnownType(typeof(MovingObjectDataTemporal))]
	public class MovingObjectData
	{
		// Pixel Points per second/second
		[DataMember]
		public double Accelleration { get; private set; }

		[DataMember]
		public Color Color { get; private set; }

		// Pixel Points per second
		[DataMember]
		public double MaxVelocity { get; private set; }

		// Pixel size
		[DataMember]
		public double Radius { get; private set; }

		// Radians per second
		[DataMember]
		public double TurnRate { get; private set; }

		public void SetProperties(double accel, Color color, double maxVel, double rad, double turn)
		{
			this.Accelleration = accel;
			this.Color = color;
			this.MaxVelocity = maxVel;
			this.Radius = rad;
			this.TurnRate = turn;
		}
	}
}
