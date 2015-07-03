using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace DungeonGen
{
	public class RoomData
	{
		public string	m_strRoomName;
		public int		m_weight;
		public List<List<int> > m_CellData = new List<List<int> >();
	}
	
	/// <summary>
	/// Summary description for Room.
	/// </summary>
	public class Room
	{
		protected Point m_Min = new Point(0, 0);
		protected Point m_Max = new Point(0, 0);
		protected iCell m_Origin = new iCell(0, 0);
		protected int m_dungeonIndex = -1;
		protected int m_roomIndex = -1;

		private static List<RoomData> sRoomDataList = new List<RoomData>();

		public Room()
		{
			m_Min.X = -1;
			m_Min.Y = -1;
			m_Max.X = -1;
			m_Max.Y = -1;
		}

		public Point Min
		{
			get
			{
				return m_Min;
			}
		}
		
		public iCell Origin
		{
			get
			{
				return m_Origin;
			}
		}

		public int CellCountX()
		{
			return 1 + (m_Max.X - m_Min.X) / iCell.CellSizeX;
		}
		
		public int CellCountY()
		{
			return 1 + (m_Max.Y - m_Min.Y) / iCell.CellSizeY;
		}

		public bool IsRoomValid()
		{
			if (m_roomIndex > -1)
			{
				return (m_Min.X >= 0 && m_Min.Y >= 0 && m_Max.X > m_Min.X && m_Max.Y > m_Min.Y);
			}
			
			return false;
		}
	
		public virtual void Create(Cell[,] dungeonCells, Point min, Point max, String match, int index)
		{
			m_Min = min;
			m_Max = max;
			m_Origin.X = min.X / iCell.CellSizeX;
			m_Origin.Y = min.Y / iCell.CellSizeY;
			m_dungeonIndex = index;
			
			PickRoomForSize(dungeonCells, match);
			PickObjects(dungeonCells);
		}

		public void Select(Cell[,] dungeonCells, Point min, int select, int index)
		{
			m_Min = min;
			m_dungeonIndex = index;
			
			Set(dungeonCells, select);
		}

		private void PickRoomForSize(Cell[,] dungeonCells, String match)
		{
			List<int> pickData = new List<int>();
			int roomDataIndex = 0;
			
			// Collect all the rooms that match our selection criteria
			foreach (RoomData roomData in sRoomDataList)
			{
				if (roomData.m_CellData.Count < CellCountX()
					&& roomData.m_CellData.Count >= CellCountX() - 2
					&& roomData.m_CellData[0].Count < CellCountY()
					&& roomData.m_CellData[0].Count >= CellCountY() - 2)
				{
					if (match.Length == 0
						|| roomData.m_strRoomName.Contains(match))
					{
						pickData.Add(roomDataIndex);
					}
				}
				
				roomDataIndex += 1;
			}
			
			// We have a match.  Pick one at random
			if (pickData.Count > 0)
			{
				int totalWeight = 0;
				
				for (int i = 0; i < pickData.Count; i++)
				{
					totalWeight += sRoomDataList[pickData[i]].m_weight;
				}
				
				if (totalWeight > 0)
				{
					int selectWeight = Form1.sRand.Next(totalWeight);
			
					totalWeight = 0;
					
					for (int i = 0; i < pickData.Count; i++)
					{
						totalWeight += sRoomDataList[pickData[i]].m_weight;

						if (totalWeight > selectWeight)
						{
							m_roomIndex = i;
							break;
						}
					}
					
					Set(dungeonCells, pickData[m_roomIndex]);
				}
			}
		}
		
		public void Set(Cell[,] dungeonCells, int select)
		{
			m_roomIndex = select;
			
			if (sRoomDataList[select].m_CellData.Count > 0)
			{
				m_Max.X = m_Min.X + sRoomDataList[select].m_CellData[0].Count * iCell.CellSizeX;
				m_Max.Y = m_Min.Y + sRoomDataList[select].m_CellData.Count * iCell.CellSizeY;
			}
			else
			{
				m_Max = m_Min;
			}
			
			for (int y = 0; y < CellCountY(); y++)
			{
				for (int x = 0; x < CellCountX(); x++)
				{
					if (y < sRoomDataList[select].m_CellData.Count
						&& x < sRoomDataList[select].m_CellData[y].Count)
					{
						dungeonCells[m_Origin.Y + y, m_Origin.X + x].m_iIndex = sRoomDataList[select].m_CellData[y][x];
					}
				}
			}
		}
		
		public void Set(Cell[,] dungeonCells, iCell cellIndex, int iNewCell)
		{
			m_Max.X = Math.Max(cellIndex.mScreenX + iCell.CellSizeX, m_Max.X);
			m_Max.Y = Math.Max(cellIndex.mScreenY + iCell.CellSizeY, m_Max.Y);
			
			dungeonCells[m_Origin.Y + cellIndex.Y, m_Origin.X + cellIndex.X].m_iIndex = iNewCell;

			while (m_Max.Y / iCell.CellSizeY >= sRoomDataList[m_roomIndex].m_CellData.Count)
			{
				sRoomDataList[m_roomIndex].m_CellData.Add(new List<int>());
			}
			
			for (int y = 0; y < m_Max.Y / iCell.CellSizeY; y++)
			{
				while (m_Max.X / iCell.CellSizeX >= sRoomDataList[m_roomIndex].m_CellData[y].Count)
				{
					sRoomDataList[m_roomIndex].m_CellData[y].Add(-1);
				}
			}
			
			sRoomDataList[m_roomIndex].m_CellData[cellIndex.Y][cellIndex.X] = iNewCell;
		}
		
		private void PickObjects(Cell[,] dungeonCells)
		{
			for (int y = 0; y < CellCountY(); y++)
			{
				for (int x = 0; x < CellCountX(); x++)
				{
					Cell checkCell = dungeonCells[y + m_Origin.Y, x + m_Origin.X];
					
					if (checkCell.m_iIndex >= 0
						&& Form1.sRand.Next(20) == 0)
					{
						checkCell.m_iObject = Object.PickRandomObject(Cell.GetCellData(checkCell.m_iIndex).m_bOpening);
					}
				}
			}
		}

		public iCell Center()
		{
			iCell center = new iCell((m_Origin.X + (CellCountX() - 1) / 2),
									(m_Origin.Y + (CellCountY() - 1) / 2));
												
			return center;
		}
		
		public string GetName()
		{
			return sRoomDataList[m_roomIndex].m_strRoomName;
		}
		
		public int GetWeight()
		{
			return sRoomDataList[m_roomIndex].m_weight;
		}
		
		public int GetDungeonIndex()
		{
			return m_dungeonIndex;
		}
		
		public int GetRoomIndex()
		{
			return m_roomIndex;
		}

		public void UpdateName(string name)
		{
			sRoomDataList[m_roomIndex].m_strRoomName = name;
		}
		
		public void UpdateWeight(int weight)
		{
			sRoomDataList[m_roomIndex].m_weight = weight;
		}
		
		public void SaveRoom(Cell[,] dungeonCells)
		{
			Point extents = new Point(0, 0);
		
			for (int y = 0; y < CellCountY(); y++)
			{
				for (int x = 0; x < CellCountX(); x++)
				{
					if (dungeonCells[m_Origin.Y + y, m_Origin.X + x].m_iIndex > 0)
					{
						extents.X = Math.Max(extents.X, x * iCell.CellSizeX);
						extents.Y = Math.Max(extents.Y, y * iCell.CellSizeY);
					}
				}
			}
			
			if (m_Max.X < extents.X
				|| m_Max.Y < extents.Y)
			{
				throw new Exception("Bad editing");
			}
			
			m_Max = extents;
			
			while (CellCountY() < sRoomDataList[m_roomIndex].m_CellData.Count)
			{
				sRoomDataList[m_roomIndex].m_CellData.RemoveAt(sRoomDataList[m_roomIndex].m_CellData.Count - 1);
			}
			
			for (int y = 0; y < CellCountY(); y++)
			{
				while (CellCountX() < sRoomDataList[m_roomIndex].m_CellData[y].Count)
				{
					sRoomDataList[m_roomIndex].m_CellData[y].RemoveAt(
							sRoomDataList[m_roomIndex].m_CellData[y].Count - 1);
				}
			
				for (int x = 0; x < CellCountX(); x++)
				{
					if (y < sRoomDataList[m_roomIndex].m_CellData.Count
						&& x < sRoomDataList[m_roomIndex].m_CellData[y].Count)
					{
						sRoomDataList[m_roomIndex].m_CellData[y][x] = dungeonCells[m_Origin.Y + y, m_Origin.X + x].m_iIndex;
					}
					else
					{
						throw new Exception("Why are we here?");
					}
				}
			}
		}
		
		public static int GetRoomDataCount()
		{
			return sRoomDataList.Count;
		}
		
		public static string GetRoomDataName(int index)
		{
			return sRoomDataList[index].m_strRoomName;
		}
		
		public static void LoadRoomData(Floor floor)
		{
			sRoomDataList.Clear();
		
			Cell.LoadCellData(floor);
			
			string fileName = "Config\\Room" + floor.ToString() + ".cfg";

			StreamReader sr = File.OpenText(fileName);
			
			string data = sr.ReadLine();
			
			while (data != null)
			{
				RoomData roomData = new RoomData();
				roomData.m_strRoomName = data;
				
				data = sr.ReadLine();
				roomData.m_weight = Convert.ToInt32(data);
				if (roomData.m_weight < 0)
				{
					roomData.m_weight = 0;
				}

				data = sr.ReadLine();	// First Row

				while (data != null
					&& data.Length > 0)
				{
					string[] data_entry = data.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					
					roomData.m_CellData.Add(new List<int>());
					
					for (int i = 0; i < data_entry.Length; i++)
					{
						roomData.m_CellData[roomData.m_CellData.Count - 1].Add(Cell.CellNameLookup(data_entry[i]));
					}
					
					data = sr.ReadLine();
				}
				
				sRoomDataList.Add(roomData);

				data = sr.ReadLine();	// Room Name
			}
			
			sr.Dispose();
		}

		public static int AddNewRoom()
		{
			int newIndex = sRoomDataList.Count;
			RoomData roomData = new RoomData();
			roomData.m_strRoomName = "Generic Room " + newIndex.ToString();
			roomData.m_weight = 10;
			sRoomDataList.Add(roomData);
			return newIndex;
		}

		public static void SaveRoomData(Floor floor)
		{
			string fileName = "Config\\Room" + floor.ToString() + ".cfg";
			
			string fileNameOld = fileName + ".old";

			if (File.Exists(fileNameOld))
			{
				File.Delete(fileNameOld);
			}
		
			File.Copy(fileName, fileNameOld);
			
			StreamWriter sr = File.CreateText(fileName);
			
			foreach(RoomData roomData in sRoomDataList)
			{
				sr.WriteLine(roomData.m_strRoomName);
				sr.WriteLine(roomData.m_weight.ToString());
				
				foreach(List<int> yList in roomData.m_CellData)
				{
					foreach(int xIndex in yList)
					{
						if (xIndex == -1)
						{
							sr.Write(Cell.GetCellName(0));
						}
						else
						{
							sr.Write(Cell.GetCellName(xIndex));
						}
						
						sr.Write("\t");
					}
					
					sr.WriteLine();
				}

				sr.WriteLine();
			}
			
			sr.Dispose();
		}
	}
}
