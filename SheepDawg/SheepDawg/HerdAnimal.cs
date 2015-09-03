namespace SheepDawg
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	public class HerdAnimal
	{
		public AnimalType Type { get; set; }

		public HerdAnimal(MovingObjectController controller)
		{

		}


		public virtual void Load(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(HerdAnimalData));
					this.Type = (AnimalType)ser.ReadObject(fs);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}

		}

		public virtual void Save(string dataFile)
		{
			try
			{
				using (var fs = new FileStream(dataFile, FileMode.OpenOrCreate))
				{
					DataContractSerializer ser = new DataContractSerializer(typeof(MovingObjectData));
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}


	}
}
