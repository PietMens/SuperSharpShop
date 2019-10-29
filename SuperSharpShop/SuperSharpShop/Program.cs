using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSharpShop
{
    static class Program
    {
        public static Form1 App;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            App = new Form1();
            Item Skyrim = new Item("Elder Scrolls V: Skyrim", "Game", 60, "../../IMG/skyrim.jpg", "The Elder Scrolls V: Skyrim is an action role-playing game, playable from either a first or third-person perspective. The player may freely roam over the land of Skyrim which is an open world environment consisting of wilderness expanses, dungeons, cities, towns, fortresses, and villages.", "shop");
            Item BattlefieldV = new Item("Star Wars Battlefront II", "Game", 60, "../../IMG/starwarsbattlefront.jpeg", "Star Wars Battlefront II features a single-player story mode, a customizable character class system, and content based on The Force Awakens and The Last Jedi movies. It also features vehicles and locations from the original, prequel, and sequel Star Wars movie trilogies.", "shop");
            Item GTAV = new Item("Grand Theft Auto V", "Game", 60, "../../IMG/gtaV.jpg", "Grand Theft Auto V is an action-adventure game played from either a third-person or first-person perspective. Players complete missions—linear scenarios with set objectives—to progress through the story. Outside of the missions, players may freely roam the open world.", "shop");
            Item Ark = new Item("Ark Survival Evolved", "Game", 60, "../../IMG/ark.jpg", "Ark: Survival Evolved is an action-adventure survival game set in an open world environment with a dynamic day-night cycle and played either from a third-person or first-person perspective.", "shop");
            Item RedDeadRedemption = new Item("Red Dead Redemption 2", "Game", 60, "../../IMG/rdr2.jpeg", "Red Dead Redemption 2 is a Western-themed action-adventure game. Played from a first or third-person perspective, the game is set in an open-world environment featuring a fictionalized version of the Western, Midwestern and Southern United States.", "shop");
            Item BattlefieldI = new Item("Battlefield I", "Game", 60, "../../IMG/battliefieldI.jpeg", "Battlefield 1 is a first-person shooter game that emphasizes teamwork. It is set in the period of World War I, and is inspired by historical events. ... Melee combat was reworked, with DICE introducing new melee weapons such as sabres, trench clubs, and shovels into the game.", "shop");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(App);
            
        }
    }
}
