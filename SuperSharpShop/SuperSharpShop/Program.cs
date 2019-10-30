using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Win32.SafeHandles;

namespace SuperSharpShop
{
    static class Program
    {
        public static Form1 App;
        public static MySqlConnection conn;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            App = new Form1();
            try
            {
                setConnection();
                //Item Skyrim = new Item("Elder Scrolls V: Skyrim", "Game", 59.99, "../../IMG/skyrim.jpg", "The Elder Scrolls V: Skyrim is an action role-playing game, playable from either a first or third-person perspective. The player may freely roam over the land of Skyrim which is an open world environment consisting of wilderness expanses, dungeons, cities, towns, fortresses, and villages.", "shop");
                //Item BattlefieldV = new Item("Star Wars Battlefront II", "Game", 59.99, "../../IMG/starwarsbattlefront.jpg", "Star Wars Battlefront II features a single-player story mode, a customizable character class system, and content based on The Force Awakens and The Last Jedi movies. It also features vehicles and locations from the original, prequel, and sequel Star Wars movie trilogies.", "shop");
                //Item GTAV = new Item("Grand Theft Auto V", "Game", 59.99, "../../IMG/gtaV.jpg", "Grand Theft Auto V is an action-adventure game played from either a third-person or first-person perspective. Players complete missions—linear scenarios with set objectives—to progress through the story. Outside of the missions, players may freely roam the open world.", "shop");
                //Item Ark = new Item("Ark Survival Evolved", "Game", 54.99, "../../IMG/ark.jpg", "Ark: Survival Evolved is an action-adventure survival game set in an open world environment with a dynamic day-night cycle and played either from a third-person or first-person perspective.", "shop");
                //Item RedDeadRedemption = new Item("Red Dead Redemption 2", "Game", 59.99, "../../IMG/rdr2.jpeg", "Red Dead Redemption 2 is a Western-themed action-adventure game. Played from a first or third-person perspective, the game is set in an open-world environment featuring a fictionalized version of the Western, Midwestern and Southern United States.", "shop");
                //Item BattlefieldI = new Item("Battlefield I", "Game", 59.99, "../../IMG/battliefieldI.jpeg", "Battlefield 1 is a first-person shooter game that emphasizes teamwork. It is set in the period of World War I, and is inspired by historical events. Melee combat was reworked, with DICE introducing new melee weapons such as sabres, trench clubs, and shovels into the game.", "shop");
                //Item Minecraft = new Item("Minecraft", "Game", 19.99, "../../IMG/minecraft.jpg", "Minecraft is a sandbox video game created by Swedish game developer Markus Persson and released by Mojang in 2011. The game allows players to build with a variety of different blocks in a 3D procedurally generated world, requiring creativity from players.", "shop");
                //Item RainbowSixSiege = new Item("Tom Clancy's: Rainbow Six Siege", "Game", 19.99, "../../IMG/rainbowsixsiege.jpg", "Tom Clancy's Rainbow Six Siege is a first-person shooter game, in which players utilize many different operators from the Rainbow team. An in-game shop allows players to purchase operators or cosmetics using the in-game currency.", "shop");
                //Item AssassinsCreed = new Item("Assassin's Creed Odyssey", "Game", 39.99, "../../IMG/assassinscreed.jpg", "Assassin's Creed Odyssey is a cloud-based title on the Nintendo Switch, which launched on the same day as the other platforms, but in Japan only. The game's season pass will include two DLC stories spread across six episodes as well as remastered editions of Assassin's Creed III and Assassin's Creed Liberation.", "shop");
                setItems();
                Application.Run(App);
                //conn.Close();
            }
            catch (BadImageFormatException exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static void setConnection()
        {
            String connectionString = "SERVER=localhost;DATABASE=SuperSharpShop;UID=root;PASSWORD=;";
            conn = new MySqlConnection(connectionString);
        }

        public static void setItems()
        {
            conn.Open();
            String sql = "SELECT * FROM items;";
            MySqlDataReader reader;
            MySqlCommand command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                new Item(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(), double.Parse(reader.GetValue(5).ToString()), reader.GetValue(4).ToString(), reader.GetValue(3).ToString(), reader.GetValue(6).ToString());
            }
            conn.Close();
        }
    }
}
