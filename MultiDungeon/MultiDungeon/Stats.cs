using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MultiDungeon
{
    class Stats
    {
        public static int kills = 0;
        public static int deaths = 0;
        public static string classType = "";

        public static void PrintStats()
        {
            List<string> lines = new List<string>();
            if (File.Exists("stats.txt"))
            {
                StreamReader sr = new StreamReader("stats.txt");
                while (!sr.EndOfStream)
                {
                    lines.Add(sr.ReadLine());
                }
                sr.Close();
            }
            Stream stream = File.OpenWrite("stats.txt");
            StreamWriter sw = new StreamWriter(stream);
            foreach (string line in lines)
            { sw.WriteLine(line); }
            sw.Write("class: " + classType + "\tkills: " + kills + "\tdeaths: " + deaths + "\n");
            sw.Flush();
            sw.Close();
        }
    }
}
