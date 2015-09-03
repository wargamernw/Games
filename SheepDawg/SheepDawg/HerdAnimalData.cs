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
	public class HerdAnimalData
	{
		// Pixel Points per second/second
		[DataMember]
		public Animal Type { get; set; }

		[DataMember]
		public IEnumerable<AnimalType> Affinity { get; set; }
	}
}
