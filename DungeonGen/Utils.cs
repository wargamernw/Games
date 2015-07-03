using System;
using System.Collections.Generic;

namespace DungeonGen
{
	public class Percentage
	{
		decimal m_value;
		
		public void Set(decimal value)
		{
			if (value <= 0)
			{
				m_value = 0;
			}
			else if (value > 1)
			{
				m_value = 1;
			}
			else
			{
				m_value = value;
			}
		}
		
		public bool Test()
		{
			return Form1.sRand.NextDouble() < (double)m_value;
		}
		
		public static decimal operator * (decimal left, Percentage right)
		{
			return left * right.m_value;
		}
	};

	public enum Dir
	{
		North,
		East,
		South,
		West,
		Max
	}
	
	public enum Floor
	{
		Stone,
		Wood,
		Earth,
		Max
	}
	
	public enum Opening
	{
		Closed,
		Open,
		Door,
		Blocked,
		Max
	}

	public enum CellType
	{
		Room,
		Corridor,
		WoodDoor,
		IronDoor,
		SecretDoor,
		Max
	}	

	public class AStar
	{
		struct PathData
		{
			public iCell m_cell;
			public float m_fTravel;
			public int m_fromCellIndex;
			public float m_fRating;
		}
		
		public static Dir ReverseDir(Dir inDir)
		{
			switch(inDir)
			{
				case Dir.North:	return Dir.South;
				case Dir.East:	return Dir.West;
				case Dir.South:	return Dir.North;
				case Dir.West:	return Dir.East;
			}
			
			return inDir;
		}
	
		public static Dir LeftDir(Dir inDir)
		{
			switch(inDir)
			{
				case Dir.North:	return Dir.West;
				case Dir.East:	return Dir.North;
				case Dir.South:	return Dir.East;
				case Dir.West:	return Dir.South;
			}
			
			return inDir;
		}

		public static Dir RightDir(Dir inDir)
		{
			switch(inDir)
			{
				case Dir.North:	return Dir.East;
				case Dir.East:	return Dir.South;
				case Dir.South:	return Dir.West;
				case Dir.West:	return Dir.North;
			}
			
			return inDir;
		}

		bool[,] m_bVisited;
		public List<iCell> m_path = new List<iCell>();
		private List<PathData> m_openList = new List<PathData>();
		private List<PathData> m_closedList = new List<PathData>();
		private int m_spreadWeight = 3;
	
		public AStar(int spreadWeight)
		{
			m_spreadWeight = spreadWeight;
		}
		
		public bool FindPath(Cell[,] dungeonCells, iCell origin, iCell dest)
		{
			m_path.Clear();
			m_path.Add(origin);
			m_bVisited = new bool[dungeonCells.GetLength(0), dungeonCells.GetLength(1)];
			m_bVisited[origin.Y, origin.X] = true;
			m_openList.Clear();
			m_closedList.Clear();
			
			PathData pathOrigin = new PathData();
			pathOrigin.m_cell = origin;
			pathOrigin.m_fromCellIndex = -1;
			pathOrigin.m_fTravel = 0;
			pathOrigin.m_fRating = 0;
			
			m_openList.Add(pathOrigin);
			
			while (m_openList.Count > 0)
			{
				if (ReachedDestination(m_openList[0].m_cell, dest, dungeonCells))
				{
					SavePath(m_openList[0]);
					return true;
				}
				
				m_closedList.Add(m_openList[0]);
				m_openList.RemoveAt(0);
				CollectNextMoves(dungeonCells, m_closedList[m_closedList.Count - 1], 
						m_closedList.Count - 1, dest);
			}
			
			return false;
		}
	
		private bool ReachedDestination(iCell location, iCell dest, Cell[,] dungeonCells)
		{
			if (location.X == dest.X
				&& location.Y == dest.Y)
			{
				return true;
			}
			
			return false;
		}
		
		private void SavePath(PathData end)
		{
			PathData temp = end;
			
			while (temp.m_fromCellIndex != -1)
			{
				m_path.Insert(1, temp.m_cell);
				temp = m_closedList[temp.m_fromCellIndex];
			}
		}
		
		private void CollectNextMoves(Cell[,] dungeonCells, PathData fromData, int fromIndex,
				iCell dest)
		{
			for (Dir dir = Dir.North; dir < Dir.Max; dir += 1)
			{
				iCell tempCell = new iCell(fromData.m_cell);
				tempCell.Move(dir);
				
				if (tempCell.X >= 0
					&& tempCell.Y >= 0
					&& tempCell.X < dungeonCells.GetLength(1)
					&& tempCell.Y < dungeonCells.GetLength(0)
					&& m_bVisited[tempCell.Y, tempCell.X] == false)
				{
					Cell fromCell = dungeonCells[fromData.m_cell.Y, fromData.m_cell.X];
					Cell toCell = dungeonCells[tempCell.Y, tempCell.X];
					
					Opening fromOpen = fromCell.GetOpening(dir);
					Opening toOpen = toCell.GetOpening(ReverseDir(dir));
					
					if (toOpen != Opening.Blocked
						&& fromOpen != Opening.Blocked)
					{
						int moveCost = 1;
						
						if (toOpen == Opening.Door)
						{
							moveCost += 1;
						}
						
						if (fromOpen == Opening.Door)
						{
							moveCost += 1;
						}

						if (toOpen == Opening.Closed)
						{
							moveCost += 8 - m_spreadWeight;
						}
						
						// Avoid corners
						if (toCell.GetOpening(LeftDir(dir)) == Opening.Closed
							|| toCell.GetOpening(RightDir(dir)) == Opening.Closed)
						{
							moveCost += 2;
						}
						
						// Avoid double doors
						if (toCell.GetOpening(LeftDir(dir)) == Opening.Door
							|| toCell.GetOpening(RightDir(dir)) == Opening.Door)
						{
							moveCost += 20;
						}

						if (fromOpen == Opening.Closed)
						{
							moveCost += 8 - m_spreadWeight;
						}
						
						if (toCell.CellType == CellType.Room)
						{
							moveCost += 2;
						}
						
						if (toCell.m_iIndex == -1)
						{
							moveCost += m_spreadWeight;
						}
					
						PathData toData = new PathData();
						toData.m_cell = tempCell;
						toData.m_fTravel = fromData.m_fTravel + moveCost;
						toData.m_fRating = toData.m_fTravel + tempCell.DistanceTo(dest);
						toData.m_fromCellIndex = fromIndex;
						
						m_bVisited[tempCell.Y, tempCell.X] = true;
						
						bool bAdded = false;
						for (int i = 0; i < m_openList.Count; i++)
						{
							if (m_openList[i].m_fRating > toData.m_fRating)
							{
								m_openList.Insert(i, toData);
								bAdded = true;
								break;
							}
						}
						
						if (!bAdded)
						{
							m_openList.Add(toData);
						}
					}
				}
			}
		}
	}
}
