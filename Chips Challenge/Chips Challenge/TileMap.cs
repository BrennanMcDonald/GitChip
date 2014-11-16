using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace TileEngine
{
    public class MapCell
    {
        public int TileID { get; set; }

        public MapCell(int tileID)
        {
            TileID = tileID;
        }
    }

    public class MapRow
    {
        public List<MapCell> Columns = new List<MapCell>();
    }

    public class TileMap
    {
        public List<MapRow> Rows = new List<MapRow>();
        public int MapWidth = 9,
            MapHeight = 9,
            chipCount = 0;
        public Point startPos;
        public TileMap(string levelFile)
        {
            // Create Map Data
            using (StreamReader sr = new StreamReader(levelFile))
            {
                string Line;
                MapHeight = int.Parse(sr.ReadLine());
                MapWidth = int.Parse(sr.ReadLine());
                for (int h = 0; h < MapHeight; h++)
                {
                    Line = sr.ReadLine();
                    MapRow thisRow = new MapRow();
                    for (int w = 0; w < MapWidth; w++)
                    {
                        if (Line[w] == 83)
                        {
                            startPos = new Point(w, h);
                            thisRow.Columns.Add(new MapCell(48));
                        }
                        else
                            thisRow.Columns.Add(new MapCell(Line[w]));
                        if (Line[w] == 42)
                            chipCount++;

                    }
                    Rows.Add(thisRow);
                }

            }

            // End Map Data

            //for (int y = 0; y < MapHeight; y++)
            //{
            //    MapRow thisRow = new MapRow();
            //    for (int x = 0; x < MapWidth; x++)
            //    {
            //        thisRow.Columns.Add(new MapCell(0));
            //    }
            //    Rows.Add(thisRow);
            //}

            //// Create Map Data
            //using (StreamReader sr = new StreamReader(levelFile))
            //{
            //    string Line;
            //    MapHeight = int.Parse(sr.ReadLine());
            //    MapWidth = int.Parse(sr.ReadLine());
            //    for(int h = 0; h < MapHeight; h++)
            //    {
            //        Line = sr.ReadLine();
            //        for(int w = 0; w < MapWidth; w++)
            //        {
            //            if (Line[w] == 83)
            //            {
            //                startPos = new Point(w, h);
            //                Rows[h].Columns[w].TileID = 48;
            //            }
            //            else
            //                Rows[h].Columns[w].TileID = Line[w];
            //            if (Line[w] == 42)
            //                chipCount++;

            //        }
            //    }
                        
            //}
            //// End Map Data
        }

    }
}
