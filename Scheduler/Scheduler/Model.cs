using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Scheduler
{
	public class Model
	{
		public const int TeamsMax = 4;

		public const int StadiumsMax = 4;

		public List<Team> Teams = new List<Team>();

		public List<Stadium> Stadiums = new List<Stadium>();

		public Model()
		{
			try
			{
				using (var fs = new FileStream("./Data/Teams.xml", FileMode.Open))
				{
					var ser = new DataContractSerializer(typeof(List<Team>));

					using (var reader = XmlReader.Create(fs))
					{
						this.Teams = (List<Team>)ser.ReadObject(reader);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}

			try
			{
				using (var fs = new FileStream("./Data/Stadiums.xml", FileMode.Open))
				{
					var ser = new DataContractSerializer(typeof(List<Stadium>));

					using (var reader = XmlReader.Create(fs))
					{
						this.Stadiums = (List<Stadium>)ser.ReadObject(reader);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}

		public void CreateSchema()
		{
			this.Teams.Clear();
			var team = new Team();

			for (int i = 0; i < TeamsMax; i++)
			{
				this.Teams.Add(team);
			}

			try
			{
				using (var fs = new FileStream("./Data/Teams.xml", FileMode.Create))
				{
					var ser = new DataContractSerializer(typeof(List<Team>));

					var settings = new XmlWriterSettings() { Indent = true, IndentChars = "\t" };

					using (var writer = XmlWriter.Create(fs, settings))
					{
						ser.WriteObject(writer, this.Teams);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}

			this.Stadiums.Clear();
			var stadium = new Stadium();

			for (int i = 0; i < StadiumsMax; i++)
			{
				this.Stadiums.Add(stadium);
			}

			try
			{
				using (var fs = new FileStream("./Data/Stadiums.xml", FileMode.Create))
				{
					var ser = new DataContractSerializer(typeof(List<Stadium>));

					var settings = new XmlWriterSettings() { Indent = true, IndentChars = "\t" };

					using (var writer = XmlWriter.Create(fs, settings))
					{
						ser.WriteObject(writer, this.Stadiums);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}
		}
	}
}
