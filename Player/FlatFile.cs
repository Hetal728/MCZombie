using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MCForge
{
    public class FlatFile
    {
        public static bool Load(Player p)
        {
            if (!Directory.Exists("players"))
                Directory.CreateDirectory("players");
            if (File.Exists("players/" + p.name.ToLower() + "DB.txt"))
            {
                foreach (string line in File.ReadAllLines("players/" + p.name.ToLower() + "DB.txt"))
                {
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string key = line.Split('=')[0].Trim();
                        string value = line.Split('=')[1].Trim();
                        string section = "nowhere yet...";

                        try
                        {
                            switch (key.ToLower())
                            {
                                case "nick":
                                    p.DisplayName = value;
                                    section = key;
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Server.s.Log("Loading " + p.name + "'s FlatFile Database failed at section: " + section);
                            Server.ErrorLog(e);
                        }

                        p.timeLogged = DateTime.Now;
                    }
                }
                return true;
            }
            else
            {
                p.DisplayName = p.name;
                Save(p);
                return false;
            }
        }

        public static void Save(Player p)
        {
            if (!Directory.Exists("players"))
                Directory.CreateDirectory("players");
            StreamWriter sw = new StreamWriter(File.Create("players/" + p.name.ToLower() + "DB.txt"));
            sw.WriteLine("Nick = " + p.DisplayName);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
    }
}