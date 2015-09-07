using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
	class Model
	{
		public const int TeamsMax = 32;

		public const int StadiumsMax = 32;

		public List<Team> Teams = new List<Team>();

		public List<Stadium> Stadiums = new List<Stadium>();

		public Model()
		{

		}

		public void CreateSchema()
		{
			var team = new Team();

			try
			{
				using (var fs = new FileStream("./Data/Teams.xml", FileMode.OpenOrCreate))
				{
					var ser = new DataContractSerializer(typeof(Team));

					for (int i = 0; i < TeamsMax; i++)
					{
						ser.WriteObject(fs, team);
					}
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("The serialization operation failed: {0} StackTrace: {1}", exc.Message, exc.StackTrace);
			}

			var stadium = new Stadium();

			try
			{
				using (var fs = new FileStream("./Data/Stadiums.xml", FileMode.OpenOrCreate))
				{
					var ser = new DataContractSerializer(typeof(Stadium));

					for (int i = 0; i < TeamsMax; i++)
					{
						ser.WriteObject(fs, stadium);
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
