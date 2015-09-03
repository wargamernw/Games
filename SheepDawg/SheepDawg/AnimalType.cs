namespace SheepDawg
{
	public enum Animal
	{
		None,

		Dog,

		Sheep
	}

	public class AnimalType
	{
		public double Affinity { get; set; }

		public Animal Type { get; set; }

		public AnimalType()
		{
			this.Type = Animal.None;
			this.Affinity = 0;
		}
	}
}
