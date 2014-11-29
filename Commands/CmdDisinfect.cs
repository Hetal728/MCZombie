using System;
using System.IO;


namespace MCForge.Commands
{
    /// <summary>
    /// This is the command /disinfect
    /// use /help disinfect in-game for more info
    /// </summary>
    public class CmdDisInfect : Command
    {
        public override string name { get { return "disinfect"; } }
        public override string shortcut { get { return "di"; } }
        public override string type { get { return "headop"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdDisInfect() { }
        public override void Use(Player p, string message)
        {
            Player who = null;
            if (message == "") { who = p; message = p.name; } 
            else { who = Player.Find(message); }
            if (!who.infected || !Server.zombie.GameInProgess())
            {
                Player.SendMessage(p,"Cannot disinfect player");
            }
            else
            {
                if (!who.referee)
                {
                    Server.zombie.DisinfectPlayer(who);
                    Player.GlobalMessage(who.color + who.name + Server.DefaultColor + " just got Disnfected!");
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/disinfect [name] - disinfects [name]");
        }
    }
}