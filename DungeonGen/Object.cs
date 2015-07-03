using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace DungeonGen
{
	public class ObjectData
	{
		public Opening[]	m_bOpening = new Opening[(int)Dir.Max] {Opening.Closed, Opening.Closed, Opening.Closed, Opening.Closed };
		public string		m_strName;
		public int			m_weight;
		public Bitmap		m_bmpImage;
	}

	public class Object
	{
		public static List<ObjectData> sObjectDataList = new List<ObjectData>();

		public Object()
		{
		}

		public static void Draw(Graphics toGraphics, int iObject, iCell cell)
		{
			for (int y = 0; y < sObjectDataList[iObject].m_bmpImage.Size.Height; y++)
			{
				for (int x = 0; x < sObjectDataList[iObject].m_bmpImage.Size.Width; x++)
				{
					if (sObjectDataList[iObject].m_bmpImage.GetPixel(x, y).R != 255
							|| sObjectDataList[iObject].m_bmpImage.GetPixel(x, y).G != 255
							|| sObjectDataList[iObject].m_bmpImage.GetPixel(x, y).B != 255)
					{
						Pen myPen = new Pen(sObjectDataList[iObject].m_bmpImage.GetPixel(x, y));
						
						toGraphics.DrawRectangle(myPen, cell.mScreenX + x + 2, 
								cell.mScreenY + y + 2, 1, 1);
						myPen.Dispose();
					}
				}
			}
		}
		
		public static int PickRandomObject(Opening[] openings)
		{
			int totalWeight = 0;
			int count = 0;

			List<int> indices = new List<int>();

			foreach (ObjectData data in sObjectDataList)
			{
				bool bOpeningsValid = true;
				for (int i = 0; i < openings.GetLength(0) && i < data.m_bOpening.GetLength(0); i++)
				{
					if (openings[i] != Opening.Max
						&& data.m_bOpening[i] != Opening.Max
						&& openings[i] != data.m_bOpening[i])
					{
						bOpeningsValid = false;
						break;
					}
				}
				
				if (bOpeningsValid)
				{
					indices.Add(count);
					totalWeight += data.m_weight;
				}
		
				count++;
			}
			
			if (totalWeight > 0)
			{
				int select = Form1.sRand.Next(totalWeight);
					
				int curWeight = 0;
				
				for (int i = 0; i < indices.Count; i++)
				{
					curWeight += sObjectDataList[indices[i]].m_weight;
					
					if (curWeight >= select)
					{
						return indices[i];
					}
				}
			}
					
			return -1;
		}
		
		public static void LoadObjectData()
		{
			sObjectDataList.Clear();
			StreamReader sr = File.OpenText("Config\\Object.cfg");

			string data = sr.ReadLine();
			data = sr.ReadLine();
			
			while (data != null)
			{
				ObjectData objData = new ObjectData();
				
				string[] data_entry = data.Split(new Char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				
				if (data_entry.Length < 6)
				{
					throw new ArgumentException(data + " does not have enough properties set ");
				}

				objData.m_strName = data_entry[4];
				
				for (int dir = 0; dir < (int)Dir.Max; dir++)
				{
					objData.m_bOpening[dir] = (Opening)Convert.ToInt16(data_entry[dir]);
					
					if (objData.m_bOpening[dir] < Opening.Closed
							|| objData.m_bOpening[dir] > Opening.Max)
					{
						throw new ArgumentException(objData.m_strName + " does not have a valid open bit type ");
					}
				}
				
				objData.m_weight = Convert.ToInt32(data_entry[5]);

				string filename;
				filename = "Tiles\\" + objData.m_strName + ".bmp";
				
				if (File.Exists(filename))
				{
					objData.m_bmpImage = new Bitmap(filename, false);
					sObjectDataList.Add(objData);
				}
				else
				{
					filename = "Tiles\\" + objData.m_strName + ".jpg";
					if (File.Exists(filename))
					{
						objData.m_bmpImage = new Bitmap(filename, false);
						sObjectDataList.Add(objData);
					}
					else
					{
						throw new ArgumentException("Unable to find .bmp or .jpg image " 
											+ objData.m_strName + " in Tiles folder");
					}
				}
				
				data = sr.ReadLine();
			}
			
			sr.Dispose();
		}
	}
}
