/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;


namespace MCForge.Commands {
    public class CmdKick : Command {
        public override string name { get { return "kick"; } }
        public override string shortcut { get { return "k"; } }
        public override string type { get { return "trusted"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.AdvBuilder; } }
        public CmdKick() { }

        public override void Use(Player p, string message) {
            if (message == "") { Help(p); return; }
            Player who = Player.Find(message.Split(' ')[0]);
            if (who == null) { Player.SendMessage(p, "Could not find player specified."); return; }
            if (message.Split(' ').Length > 1)
                message = message.Substring(message.IndexOf(' ') + 1);
            else if(p != null)
                message = "You were kicked by " + p.name + "!";
            if (p != null)
            {
                if (who == p)
                {
                    Player.SendMessage(p, "You cannot kick yourself!");
                    return;
                }
                if (who.@group.Permission >= p.@group.Permission && !who.hidden)
                {
                    Player.GlobalChat(p,p.color + p.name + Server.DefaultColor + " tried to kick " + who.color + who.name +" but failed.", false);
                    return;
                }
                else if(who.hidden && who.@group.Permission >= p.@group.Permission)
                {
                    Player.SendMessage(p, "Could not find player specified."); return;
                }
            }
            if (who.money >= 10) who.money -= 10;
            else who.money = 0;
            if (message.Contains("@"))
            {
                string testing = "0";
                testing = Server.GetRuleReason(p, message);
                if (testing == "0") return;
                else
                {
                    message = testing;
                    who.Kick("Kicked for breaking rule: " + message);
                }
            }
            else
                who.Kick("Kicked for: "+ message);
            who.OffenseNote(DateTime.Now, "%5kicked%c", message, p.name);
        }
        public override void Help(Player p) 
        {
            Player.SendMessage(p, "/kick <player> <reason> - Kicks a player for reason.");
            Player.SendMessage(p, "You can use for reason @rulenumber to point to a special rule");
        }
    }
}