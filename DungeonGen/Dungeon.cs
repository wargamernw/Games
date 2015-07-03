using System;
using System.Drawing;
using System.Collections.Generic;

namespace DungeonGen
{
	public class GenerationParameters
	{
		public bool m_bSmallRooms;
		public bool m_bMedRooms;
		public bool m_bLargeRooms;
		public bool m_bHugeRooms;
		public int m_CorridorSize;
		public Floor m_floor;
		public int m_entrances;
		public int m_exits;
		public Percentage m_roomDensity = new Percentage();
		public Percentage m_secretDoorDensity = new Percentage();
		public Percentage m_trapDensity = new Percentage();
	};
	
	public class SpawnParameters
	{
		public int m_level;
		public Percentage m_monsterDensity = new Percentage();
	};

	/// <summary>
	/// Summary description for Dungeon.
	/// </summary>
	public class Dungeon
	{
		private iCell m_dungeonMax = new iCell(0, 0);
		private iCell m_dungeonMin = new iCell(0, 0);
		protected Cell[,] m_dungeonCells;
		Room[] m_rooms;
		Floor m_floor;

		public Dungeon()
		{

		}
		
		public int GetRoomCount()
		{
			return m_rooms.Length;
		}
		
		public Room GetRoom(int index)
		{
			return m_rooms[index];
		}

		public void Render(Graphics toGraphics)
		{
			if (m_dungeonCells != null)
			{
				for (int y = 0; y < m_dungeonCells.GetLength(0); y++)
				{
					for (int x = 0; x < m_dungeonCells.GetLength(1); x++)
					{
						m_dungeonCells[y, x].Draw(toGraphics);
					}
				}
			}
			
			CreateLabels(toGraphics);
//			CreateGrid(toGraphics);
		}

		private void CreateLabels(Graphics toGraphics)
		{
			if (m_rooms == null)
			{
				return;
			}
		
			SolidBrush myBrush = new SolidBrush(Color.BlueViolet);
			Font myFont = new Font(FontFamily.GenericSerif, 8);
			
			for (int i = 0; i < m_rooms.GetLength(0); i++)
			{
				if (m_rooms[i].IsRoomValid())
				{
					toGraphics.DrawString(m_rooms[i].GetDungeonIndex().ToString(), myFont, myBrush, 
						m_rooms[i].Center().mScreenX - 4, m_rooms[i].Center().mScreenY - 4);
				}
			}
		}

		private void CreateGrid(Graphics toGraphics)
		{
			SolidBrush myBrush = new SolidBrush(Color.Black);

			for (int x = m_dungeonMin.X; x < m_dungeonMax.X; x++)
			{
				for (int y = m_dungeonMin.Y; y < m_dungeonMax.Y; y++)
				{
					toGraphics.FillRectangle(myBrush, x * iCell.CellSizeX, y * iCell.CellSizeY, 1, 1);
				}
			}

			myBrush.Dispose();
		}

		public void ClearDungeon()
		{
			if (m_dungeonCells != null)
			{
				for (int y = 0; y < m_dungeonCells.GetLength(0); y++)
				{
					for (int x = 0; x < m_dungeonCells.GetLength(1); x++)
					{
						m_dungeonCells[y, x].m_iIndex = -1;
						m_dungeonCells[y, x].m_iObject = -1;
					}
				}
			}
		}

		public void Generate(GenerationParameters genParms, iCell cellSize)
		{
			CreateArea(new iCell(0, 0), cellSize);

			m_floor = genParms.m_floor;
			m_rooms = new Room[(int)((cellSize.X * cellSize.Y / 8) * genParms.m_roomDensity) + genParms.m_entrances + genParms.m_exits];
			
			for (int i = 0; i < genParms.m_entrances; i++)
			{
				m_rooms[i] = new Room();
				PlaceEntrance(m_rooms[i], genParms.m_floor, i);
			}
			
			for (int i = genParms.m_entrances; i < genParms.m_exits + genParms.m_entrances; i++)
			{
				m_rooms[i] = new Room();
				PlaceExit(m_rooms[i], genParms.m_floor, i);
			}
			
			int roomIndex = genParms.m_entrances + genParms.m_exits;

			for (int i = genParms.m_entrances + genParms.m_exits; i < m_rooms.GetLength(0); i++)
			{
				m_rooms[i] = new Room();
				
				int roomSizeX = 8;
				int roomSizeY = 8;

				int size = SelectSize(genParms.m_bSmallRooms, genParms.m_bMedRooms, 
						genParms.m_bLargeRooms, genParms.m_bHugeRooms, i / m_rooms.GetLength(0));
				
				if (size == 0)
				{
					roomSizeX = 3 + Form1.sRand.Next(3);
					roomSizeY = 3 + Form1.sRand.Next(3);
				}
				else if (size == 1)
				{
					roomSizeX = 5 + Form1.sRand.Next(5);
					roomSizeY = 5 + Form1.sRand.Next(5);
				}
				else if (size == 2)
				{
					roomSizeX = 8 + Form1.sRand.Next(8);
					roomSizeY = 8 + Form1.sRand.Next(8);
				}
				else if (size == 3)
				{
					roomSizeX = 12 + Form1.sRand.Next(10);
					roomSizeY = 12 + Form1.sRand.Next(10);
				}

				Point origin = FindOpenSpace(roomSizeX, roomSizeY);
				
				if (origin.X >= 0)
				{
					Point max = new Point(origin.X + roomSizeX * iCell.CellSizeX, 
											origin.Y + roomSizeY * iCell.CellSizeY);
					m_rooms[i].Create(m_dungeonCells, origin, max, "Room", roomIndex);
					roomIndex += 1;
				}
			}
			
			ConnectRooms(genParms.m_entrances, genParms.m_exits, genParms);
		
			for (int y = 0; y < m_dungeonCells.GetLength(0); y++)
			{	
				for (int x = 0; x < m_dungeonCells.GetLength(1); x++)
				{
					if (m_dungeonCells[y, x].m_iIndex == -1)
					{
						m_dungeonCells[y, x].m_iIndex = 0;
					}
				}
			}
			
			AddSecretDoors(genParms.m_secretDoorDensity);
		}
		
		private int SelectSize(bool small, bool med, bool large, bool huge, float percentBuilt)
		{
			int totSelect = 0;
			if (small)
			{
				totSelect += (int)(6 * percentBuilt);
			}
			
			if (med)
			{
				totSelect += 5;
			}
			
			if (large)
			{
				totSelect += (int)(6 * (1 - percentBuilt));
			}
			
			if (huge)
			{
				totSelect += (int)(4 * (1 - percentBuilt));
			}
			
			int select = Form1.sRand.Next(totSelect);
			
			totSelect = 0;
			
			if (small)
			{
				totSelect += 6;
				
				if (totSelect > select)
				{
					return 0;
				}
			}
			
			if (med)
			{
				totSelect += 5;

				if (totSelect > select)
				{
					return 1;
				}
			}
			
			if (large)
			{
				totSelect += 3;

				if (totSelect > select)
				{
					return 2;
				}
			}
			
			if (huge)
			{
				totSelect += 2;

				if (totSelect > select)
				{
					return 3;
				}
			}
			
			return 0;
		}
		
		private void PlaceEntrance(Room room, Floor floor, int index)
		{
			Point origin = FindOpenSpace(3, 3);
			Point max = new Point(origin.X + 3 * iCell.CellSizeX, origin.Y + 3 * iCell.CellSizeY);
			room.Create(m_dungeonCells, origin, max, "Entrance", index);
		}
		
		private void PlaceExit(Room room, Floor floor, int index)
		{
			Point origin = FindOpenSpace(3, 3);
			Point max = new Point(origin.X + 3 * iCell.CellSizeX, origin.Y + 3 * iCell.CellSizeY);
			room.Create(m_dungeonCells, origin, max, "Exit", index);
		}

		public void SpawnDungeon(SpawnParameters spawnParms)
		{
		}

		public void ConnectRooms(int entrances, int exits, GenerationParameters genParms)
		{
			// Generate paths from each entrance / exit to each and every room
			for (int start = 0; start < entrances + exits; start++)
			{
				for (int end = entrances + exits; end < m_rooms.GetLength(0); end++)
				{
					if (m_rooms[end].IsRoomValid())
					{
						int spreadWeight = 3;
					
						if (genParms.m_CorridorSize > 5 && Form1.sRand.Next(3) == 0)
						{
							spreadWeight = 1;
						}
						else if (genParms.m_CorridorSize > 10 && Form1.sRand.Next(3) == 0)
						{
							spreadWeight = 0;
						}
			
						AStar path = new AStar(spreadWeight);
						
						if(path.FindPath(m_dungeonCells, m_rooms[start].Center(), m_rooms[end].Center()))
						{
							for (int i = 1; i < path.m_path.Count; i++)
							{
								BuildCorridor(path.m_path[i -1], path.m_path[i]);
							}
						}
					}
				}
			}
		}
		
		private void BuildCorridor(iCell from, iCell to)
		{
			Dir moveDir = from.MoveDir(to);
			
			Cell fromCell = m_dungeonCells[from.Y, from.X];
			Cell toCell = m_dungeonCells[to.Y, to.X];
			
			if (toCell.m_iIndex == -1)
			{
				Opening[] openings = new Opening[(int)Dir.Max] { Opening.Open, Opening.Open, Opening.Open, Opening.Open };
				
				toCell.m_iIndex = Cell.FindCell(CellType.Corridor, openings);
				
				if (toCell.m_iIndex == -1)
				{
					throw new ArgumentException("Unable to create corridor");
				}
			}

			Opening fromOpening = AddOpening(fromCell, moveDir, Opening.Max);
			AddOpening(toCell, AStar.ReverseDir(moveDir), fromOpening);
		}

		private Opening AddOpening(Cell cell, Dir openDir, Opening fromOpening)
		{
			Opening open = cell.GetOpening(openDir);
			
			if (open == Opening.Closed)
			{
				Opening[] openings = new Opening[(int)Dir.Max] { Opening.Open, Opening.Open, Opening.Open, Opening.Open };
				
				CellType targetCell = cell.CellType;
				
				if (targetCell == CellType.Room)
				{
					for (Dir dir = 0; dir < Dir.Max; dir += 1)
					{
						openings[(int)dir] = cell.GetOpening(dir);
					}
				
					if (fromOpening != Opening.Door)
					{
						targetCell = Form1.sRand.Next(2) == 0 ? CellType.WoodDoor : CellType.IronDoor;
						openings[(int)openDir] = Opening.Door;
						open = Opening.Door;
					}
					else
					{
						openings[(int)openDir] = Opening.Open;
						open = Opening.Open;
					}						
				}
				
				int index = Cell.FindCell(targetCell, openings);
				
				if (index != -1)
				{
					cell.m_iIndex = index;
					cell.m_iObject = -1;
				}
			}
			
			return open;
		}

		public void Edit(iCell editCell, int iNewCell)
		{
			m_rooms[0].Set(m_dungeonCells, editCell, iNewCell);
		}
		
		public void Toggle(iCell editCell)
		{
			if (m_dungeonCells[editCell.Y, editCell.X].m_iIndex == 0)
			{
				m_dungeonCells[editCell.Y, editCell.X].m_iIndex = 1;
			}
			else if (m_dungeonCells[editCell.Y, editCell.X].m_iIndex == 1)
			{
				m_dungeonCells[editCell.Y, editCell.X].m_iIndex = 0;
			}
		}
		
		public Point FindOpenSpace(int sizeX, int sizeY)
		{
			List<iCell> validCells = new List<iCell>();
			
			for (int Y = 2 + m_dungeonMin.Y; Y < m_dungeonMax.Y - sizeY - 2; Y++)
			{
				for (int X = 2 + m_dungeonMin.X; X < m_dungeonMax.X - sizeX - 2; X++)
				{
					bool bFound = true;

					for (int y = 0; y <= sizeY && bFound == true; y++)
					{
						for (int x = 0; x <= sizeX && bFound == true; x++)
						{
							if (m_dungeonCells[Y + y, X + x].m_iIndex != -1)
							{
								bFound = false;
							}
						}
					}
					
					if (bFound)
					{
						validCells.Add(new iCell(X, Y));
					}
				}
			}
			
			if (validCells.Count > 0)
			{
				int select = Form1.sRand.Next(validCells.Count);
				
				return new Point(validCells[select].mScreenX, validCells[select].mScreenY);
			}
			
			return new Point(-1, -1);
		}
		
		private void AddSecretDoors(Percentage secretPercent)
		{
			for (int i = 0; i < m_rooms.GetLength(0); i++)
			{
				int doorCount = 0;
				int targetCell = -1;
				int targetX = 0;
				int targetY = 0;
				Dir targetDir = Dir.Max;
			
				for (int y = 0; y < m_rooms[i].CellCountY(); y++)
				{
					for (int x = 0; x < m_rooms[i].CellCountX(); x++)
					{
						int cellIndex = m_dungeonCells[y + m_rooms[i].Origin.Y, x + m_rooms[i].Origin.X].m_iIndex;
						
						if (cellIndex > 0)
						{
							for (Dir dir = Dir.North; dir < Dir.Max; dir += 1)
							{
								if (Cell.GetCellData(cellIndex).m_bOpening[(int)dir] == Opening.Door)
								{
									doorCount += 1;
									targetCell = cellIndex;
									targetX = x + m_rooms[i].Origin.X;
									targetY = y + m_rooms[i].Origin.Y;
									targetDir = dir;
								}
							}
						}
					}
				}
				
				if (doorCount == 1
					&& secretPercent.Test())
				{
					iCell checkCell = new iCell(targetX, targetY);
					checkCell.Move(targetDir);
					
					Cell adjCell = m_dungeonCells[checkCell.Y, checkCell.X];
					
					int openCount = 0;
					
					// Find out how many doors or openings there are in the cell that
					// connects to the cell we are putting the secret door
					for (Dir dir = Dir.North; dir < Dir.Max; dir += 1)
					{
						iCell iTestCell = new iCell(checkCell);
						iTestCell.Move(dir);
						Cell testCell = m_dungeonCells[iTestCell.Y, iTestCell.X];
						
						if (testCell.GetOpening(AStar.ReverseDir(dir)) == Opening.Door
							|| testCell.GetOpening(AStar.ReverseDir(dir)) == Opening.Open)
						{
							openCount += 1;
						}
					}
				
					// Don't place a secret door at the end of a corridor unless there
					// is another door or opening to go on from there
					if (openCount >= 3)
					{
						int newCell = Cell.FindCell(CellType.SecretDoor, Cell.GetCellData(targetCell).m_bOpening);

						if (newCell > 0)
						{
							m_dungeonCells[targetY, targetX].m_iIndex = newCell;
						}
					}
				}
			}
		}

		public void CreateArea(iCell begin, iCell end)
		{
			m_dungeonMin = begin;
			m_dungeonMax = end;
			
			m_dungeonCells = new Cell[end.Y, end.X];
			
			for (int y = 0; y < m_dungeonCells.GetLength(0); y++)
			{
				for (int x = 0; x < m_dungeonCells.GetLength(1); x++)
				{
					m_dungeonCells[y, x] = new Cell();
					
					iCell theCell = new iCell(x, y);
					m_dungeonCells[y, x].InitCell(theCell);
				}
			}
		}
		
		public Room GetEditRoom()
		{
			return m_rooms[0];
		}
		
		public void SaveEditRoom()
		{
			m_rooms[0].SaveRoom(m_dungeonCells);
		}
		
		public bool UpdateEditRoom(string name)
		{
			if (name != m_rooms[0].GetName())
			{
				m_rooms[0].UpdateName(name);
				return true;
			}
			
			return false;
		}
		
		public bool UpdateEditRoom(int weight)
		{
			if (weight != m_rooms[0].GetWeight())
			{
				m_rooms[0].UpdateWeight(weight);
				return true;
			}
			
			return false;
		}
		
		public void SetEditRoom(int index)
		{
			m_rooms = new Room[1];
			m_rooms[0] = new Room();
			m_rooms[0].Select(m_dungeonCells, new Point(m_dungeonMin.mScreenX, m_dungeonMin.mScreenY), index, 0);
		}
	}
}

