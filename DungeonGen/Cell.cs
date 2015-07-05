using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace DungeonGen
{
	/// <summary>
	/// Summary description for Cell.
	/// </summary>
	/// 
	public class iCell
	{
		public const int CellSizeX = 15;
		public const int CellSizeY = 15;

		int mX;
		int mY;
		public int mScreenX;
		public int mScreenY;

		public iCell(int X, int Y)
		{
			mX = X;
			mY = Y;
			mScreenX = X * CellSizeX;
			mScreenY = Y * CellSizeY;
		}
		
		public iCell(Point screen)
		{
			mX = screen.X / CellSizeX;
			mY = screen.Y / CellSizeY;
			mScreenX = screen.X;
			mScreenY = screen.Y;
		}

		public iCell(iCell cell)
		{
			mX = cell.mX;
			mY = cell.mY;
			mScreenX = cell.mScreenX;
			mScreenY = cell.mScreenY;
		}
		
		public void Move(Dir dir)
		{
			if (dir == Dir.North)
			{
				mY -= 1;
				mScreenY -= CellSizeY;
			}
			else if (dir == Dir.East)
			{
				mX += 1;
				mScreenX += CellSizeX;
			}
			else if (dir == Dir.South)
			{
				mY += 1;
				mScreenY += CellSizeY;
			}
			else if (dir == Dir.West)
			{
				mX -= 1;
				mScreenX -= CellSizeX;
			}
		}
		
		public Dir MoveDir(iCell toCell)
		{
			if (mY - 1 == toCell.Y)
			{
				return Dir.North;
			}
			else if (mY + 1 == toCell.Y)
			{
				return Dir.South;
			}
			else if (mX + 1 == toCell.X)
			{
				return Dir.East;
			}
			else if (mX - 1 == toCell.X)
			{
				return Dir.West;
			}
			
			return Dir.Max;
		}
		
		public int DistanceTo(iCell toCell)
		{
			return Math.Abs(X - toCell.X) + Math.Abs(Y - toCell.Y);
		}

		public int X
		{
			get
			{
				return mX;
			}
			
			set
			{
				mX = value;
			}
		}

		public int Y
		{
			get
			{
				return mY;
			}
			
			set
			{
				mY = value;
			}
		}
	}

	public class CellData
	{
		public Opening[]	m_bOpening = new Opening[(int)Dir.Max] {Opening.Closed, Opening.Closed, Opening.Closed, Opening.Closed };
		public Bitmap	m_bmpImage;
		public string	m_strName;
		public CellType m_eType;
	}

	public class Cell
	{
		public static List<CellData> sCellDataList = new List<CellData>();

		private iCell	m_iCell;
		public int		m_iIndex;
		public int		m_iObject;
		
		public static int CellNameLookup(string strCell)
		{
			if (strCell == "Empty")
			{
				return -1;
			}
		
			int count = 0;
			foreach (CellData data in sCellDataList)
			{
				if (data.m_strName == strCell)
				{
					return count;
				}
				
				count++;
			}

			throw new ArgumentException(strCell + " not found in Cell Data");
		}
		
		public static int CellNameLookup(string strCell, CellType eType)
		{
			if (strCell == "Empty")
			{
				return -1;
			}
		
			int count = 0;
			foreach (CellData data in sCellDataList)
			{
				if (data.m_eType == eType
					&& data.m_strName == strCell)
				{
					return count;
				}
				
				count++;
			}
		
			throw new ArgumentException(strCell + " not found in Cell Data for type " + 
					eType.ToString());
		}
		
		public static int FindCell(CellType eType, Opening[] openings)
		{
			List<int> indices = new List<int>();
			
			int count = 0;
			foreach (CellData data in sCellDataList)
			{
				if (data.m_eType == eType)
				{
					bool bOpeningsValid = true;
					for (int i = 0; i < openings.GetLength(0) && i < data.m_bOpening.GetLength(0); i++)
					{
						if (openings[i] != Opening.Max
							&& openings[i] != data.m_bOpening[i])
						{
							bOpeningsValid = false;
							break;
						}
					}
					
					if (bOpeningsValid)
					{
						indices.Add(count);
					}
				}
				
				count++;
			}
			
			if (indices.Count > 0)
			{
				return indices[Form1.sRand.Next(indices.Count)];
			}
			
			return -1;
		}

		public static CellData GetCellData(int iIndex)
		{
			return sCellDataList[iIndex];
		}
		
		public static string GetCellName(int iIndex)
		{
			return sCellDataList[iIndex].m_strName;
		}

		public static int GetCellDataCount()
		{
			return sCellDataList.Count;
		}

		public Cell()
		{
			m_iIndex = -1;
			m_iObject = -1;
		}
		
		public CellType CellType
		{
			get
			{
				if (m_iIndex >= 0)
				{
					return sCellDataList[m_iIndex].m_eType;
				}
				else
				{
					return CellType.Max;
				}
			}
		}
		
		public void InitCell(iCell theCell)
		{
			m_iCell = theCell;
		}

		public void Draw(Graphics toGraphics)
		{
			if (m_iIndex >= 0)
			{
				toGraphics.DrawImage(sCellDataList[m_iIndex].m_bmpImage, 
						m_iCell.mScreenX, m_iCell.mScreenY);
						
				if (m_iObject >= 0)
				{
					Object.Draw(toGraphics, m_iObject, m_iCell);
				}
			}
		}
		
		public Opening GetOpening(Dir moveDir)
		{
			if (m_iIndex >= 0)
			{
				return sCellDataList[m_iIndex].m_bOpening[(int)moveDir];
			}
			
			return Opening.Max;
		} 

		public static void LoadCellData(Floor floor)
		{
			sCellDataList.Clear();
		
			string cellFile = "Config\\Cell" + floor.ToString() + ".cfg";
			StreamReader sr = File.OpenText(cellFile);

			string data = sr.ReadLine();
			data = sr.ReadLine();
			
			while (data != null)
			{
				CellData celldata = new CellData();
				
				string[] data_entry = data.Split(new Char[] { ' ' }, 5);
				celldata.m_strName = data_entry[4];

				for (int dir = 0; dir < (int)Dir.Max; dir++)
				{
					celldata.m_bOpening[dir] = (Opening)Convert.ToInt16(data_entry[dir]);
					
					if (celldata.m_bOpening[dir] < Opening.Closed
							|| celldata.m_bOpening[dir] >= Opening.Max)
					{
						throw new ArgumentException(celldata.m_strName + " does not have a valid open bit type ");
					}
				}
				
				celldata.m_eType = CellType.Room;
				
				if (celldata.m_strName.Contains("Corridor"))
				{
					celldata.m_eType = CellType.Corridor;
				}
				else if (celldata.m_strName.Contains("DoorSecret"))
				{
					celldata.m_eType = CellType.SecretDoor;
				}
				else if (celldata.m_strName.Contains("DoorWood"))
				{
					celldata.m_eType = CellType.WoodDoor;
				}
				else if (celldata.m_strName.Contains("DoorIron"))
				{
					celldata.m_eType = CellType.IronDoor;
				}
				
				string filename;
				filename = "Tiles\\Floor" + floor.ToString() + celldata.m_strName + ".bmp";
				
				if (File.Exists(filename))
				{
					celldata.m_bmpImage = new Bitmap(filename, false);
					sCellDataList.Add(celldata);
				}
				else
				{
					filename = "Tiles\\Floor" + floor.ToString() + celldata.m_strName + ".jpg";
					if (File.Exists(filename))
					{
						celldata.m_bmpImage = new Bitmap(filename, false);
						sCellDataList.Add(celldata);
					}
					else
					{
						throw new ArgumentException("Unable to find .bmp or .jpg image Floor" 
												+ floor.ToString() + celldata.m_strName + " in Tiles folder");
					}
				}
				
				data = sr.ReadLine();
			}
			
			sr.Dispose();
			
			Object.LoadObjectData();
		}
	}
}
