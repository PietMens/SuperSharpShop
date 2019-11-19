using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
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
        public static AuthForm authForm;
        public static MySqlConnection conn;
        public static int userId = 0;
        public static User user;
        public static List<User> users = new List<User>();
        public static List<User> awaitingUsers = new List<User>();
        public static List<User> friends = new List<User>();
        public static List<User> requestUsers = new List<User>();
        public static List<List<Item>> items = new List<List<Item>>();
        public static List<Item> shopItems = new List<Item>();
        public static List<Item> libraryItems = new List<Item>();
        public static List<Item> installedItems = new List<Item>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                String dir = getDirectory();
                authForm = new AuthForm();
                String path = Directory.GetCurrentDirectory();
                if (!Directory.Exists(dir + "SuperSharpShop"))
                {
                    Directory.SetCurrentDirectory(dir);
                    Directory.CreateDirectory("SuperSharpShop");
                    Directory.CreateDirectory("SuperSharpShop/Common");
                    Directory.SetCurrentDirectory(path);
                } else if (!Directory.Exists(dir + "SuperSharpShop/Common"))
                {
                    Directory.SetCurrentDirectory(dir);
                    Directory.CreateDirectory("SuperSharpShop/Common");
                    Directory.SetCurrentDirectory(path);
                }
                items.Add(shopItems);
                items.Add(libraryItems);
                items.Add(installedItems);
                setConnection();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(authForm);
                //App = new Form1();
                //Item Skyrim = new Item("Elder Scrolls V: Skyrim", "Game", 59.99, "../../IMG/skyrim.jpg", "The Elder Scrolls V: Skyrim is an action role-playing game, playable from either a first or third-person perspective. The player may freely roam over the land of Skyrim which is an open world environment consisting of wilderness expanses, dungeons, cities, towns, fortresses, and villages.", "shop");
                //Item BattlefieldV = new Item("Star Wars Battlefront II", "Game", 59.99, "../../IMG/starwarsbattlefront.jpg", "Star Wars Battlefront II features a single-player story mode, a customizable character class system, and content based on The Force Awakens and The Last Jedi movies. It also features vehicles and locations from the original, prequel, and sequel Star Wars movie trilogies.", "shop");
                //Item GTAV = new Item("Grand Theft Auto V", "Game", 59.99, "../../IMG/gtaV.jpg", "Grand Theft Auto V is an action-adventure game played from either a third-person or first-person perspective. Players complete missions—linear scenarios with set objectives—to progress through the story. Outside of the missions, players may freely roam the open world.", "shop");
                //Item Ark = new Item("Ark Survival Evolved", "Game", 54.99, "../../IMG/ark.jpg", "Ark: Survival Evolved is an action-adventure survival game set in an open world environment with a dynamic day-night cycle and played either from a third-person or first-person perspective.", "shop");
                //Item RedDeadRedemption = new Item("Red Dead Redemption 2", "Game", 59.99, "../../IMG/rdr2.jpeg", "Red Dead Redemption 2 is a Western-themed action-adventure game. Played from a first or third-person perspective, the game is set in an open-world environment featuring a fictionalized version of the Western, Midwestern and Southern United States.", "shop");
                //Item BattlefieldI = new Item("Battlefield I", "Game", 59.99, "../../IMG/battliefieldI.jpeg", "Battlefield 1 is a first-person shooter game that emphasizes teamwork. It is set in the period of World War I, and is inspired by historical events. Melee combat was reworked, with DICE introducing new melee weapons such as sabres, trench clubs, and shovels into the game.", "shop");
                //Item Minecraft = new Item("Minecraft", "Game", 19.99, "../../IMG/minecraft.jpg", "Minecraft is a sandbox video game created by Swedish game developer Markus Persson and released by Mojang in 2011. The game allows players to build with a variety of different blocks in a 3D procedurally generated world, requiring creativity from players.", "shop");
                //Item RainbowSixSiege = new Item("Tom Clancy's: Rainbow Six Siege", "Game", 19.99, "../../IMG/rainbowsixsiege.jpg", "Tom Clancy's Rainbow Six Siege is a first-person shooter game, in which players utilize many different operators from the Rainbow team. An in-game shop allows players to purchase operators or cosmetics using the in-game currency.", "shop");
                //Item AssassinsCreed = new Item("Assassin's Creed Odyssey", "Game", 39.99, "../../IMG/assassinscreed.jpg", "Assassin's Creed Odyssey is a cloud-based title on the Nintendo Switch, which launched on the same day as the other platforms, but in Japan only. The game's season pass will include two DLC stories spread across six episodes as well as remastered editions of Assassin's Creed III and Assassin's Creed Liberation.", "shop");
                //setItems(App.lastPanel);
                //App.setComboBox();
                //Application.Run(App);
            }
            catch (BadImageFormatException exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static void Exit(object sender, EventArgs e)
        {
            if (userId != 0)
            {
                conn.Open();
                String sql = $"UPDATE users SET active = 0 WHERE ID = {userId}";
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public static void setFriends(Panel panel)
        {
            if (panel.Name == "Friends")
            {
                friends.Clear();
                conn.Open();
                String sql = $"SELECT * FROM friends WHERE applicant_ID = {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                var da = new MySqlDataAdapter(command);
                var ds = new DataSet();
                da.Fill(ds, "users");
                List<int> receivers = new List<int>();
                foreach (DataRow row in ds.Tables["users"].Rows)
                {
                    if (bool.Parse(row[4].ToString()))
                    {
                        receivers.Add(int.Parse(row[2].ToString()));
                    }
                }
                conn.Close();
                foreach (int receiver in receivers)
                {
                    conn.Open();
                    sql = $"SELECT * FROM users WHERE ID = {receiver};";
                    command = new MySqlCommand(sql, conn);
                    da = new MySqlDataAdapter(command);
                    ds = new DataSet();
                    da.Fill(ds);
                    DataRow row = ds.Tables[0].Rows[0];
                    User user = new User(int.Parse(row[0].ToString()), row[1].ToString(), row[3].ToString(), int.Parse(row[4].ToString()), bool.Parse(row[5].ToString()));
                    friends.Add(user);
                    conn.Close();
                    user.setFriend(panel);
                }
                conn.Open();
                sql = $"SELECT * FROM friends WHERE receiver_ID = {userId};";
                command = new MySqlCommand(sql, conn);
                da = new MySqlDataAdapter(command);
                ds = new DataSet();
                da.Fill(ds, "users");
                List<int> applicants = new List<int>();
                foreach (DataRow row in ds.Tables["users"].Rows)
                {
                    if (bool.Parse(row[4].ToString()))
                    {
                        applicants.Add(int.Parse(row[1].ToString()));
                    }
                }
                conn.Close();
                foreach (int applicant in applicants)
                {
                    conn.Open();
                    sql = $"SELECT * FROM users WHERE ID = {applicant};";
                    command = new MySqlCommand(sql, conn);
                    da = new MySqlDataAdapter(command);
                    ds = new DataSet();
                    da.Fill(ds);
                    DataRow row = ds.Tables[0].Rows[0];
                    User user = new User(int.Parse(row[0].ToString()), row[1].ToString(), row[3].ToString(), int.Parse(row[4].ToString()), bool.Parse(row[5].ToString()));
                    friends.Add(user);
                    conn.Close();
                    user.setFriend(panel);
                }
            } else if (panel.Name == "Requests")
            {
                requestUsers.Clear();
                conn.Open();
                String sql = $"SELECT * FROM friends WHERE receiver_ID = {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds, "users");
                List<int> applicants = new List<int>();
                foreach (DataRow row in ds.Tables["users"].Rows)
                {
                    if (!bool.Parse(row[3].ToString()))
                    {
                        applicants.Add(int.Parse(row[1].ToString()));
                    }
                }
                conn.Close();
                foreach (int applicant in applicants)
                {
                    conn.Open();
                    sql = $"SELECT * FROM users WHERE ID = {applicant};";
                    command = new MySqlCommand(sql, conn);
                    da = new MySqlDataAdapter(command);
                    ds = new DataSet();
                    da.Fill(ds);
                    DataRow row = ds.Tables[0].Rows[0];
                    User user = new User(int.Parse(row[0].ToString()), row[1].ToString(), row[3].ToString(), int.Parse(row[4].ToString()), bool.Parse(row[5].ToString()));
                    requestUsers.Add(user);
                    conn.Close();
                    user.setFriend(panel);
                }
            } else if (panel.Name == "Awaiting") {
                awaitingUsers.Clear();
                conn.Open();
                String sql = $"SELECT * FROM friends WHERE applicant_ID = {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                DataSet ds = new DataSet();
                da.Fill(ds, "users");
                List<int> receivers = new List<int>();
                foreach (DataRow row in ds.Tables["users"].Rows)
                {
                    if (!bool.Parse(row[3].ToString()))
                    {
                        receivers.Add(int.Parse(row[2].ToString()));
                    }
                }
                conn.Close();
                foreach (int receiver in receivers)
                {
                    conn.Open();
                    sql = $"SELECT * FROM users WHERE ID = {receiver};";
                    command = new MySqlCommand(sql, conn);
                    da = new MySqlDataAdapter(command);
                    ds = new DataSet();
                    da.Fill(ds);
                    DataRow row = ds.Tables[0].Rows[0];
                    User user = new User(int.Parse(row[0].ToString()), row[1].ToString(), row[3].ToString(),
                        int.Parse(row[4].ToString()), bool.Parse(row[5].ToString()));
                    awaitingUsers.Add(user);
                    conn.Close();
                    user.setFriend(panel);
                }
            }
            else if (panel.Name == "Add")
            {
                users.Clear();
                conn.Open();
                String sql = $"SELECT * FROM users WHERE ID != {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                var da = new MySqlDataAdapter(command);
                var ds = new DataSet();
                da.Fill(ds, "users");
                Console.WriteLine(ds.Tables["users"].Rows);
                conn.Close();
                foreach (DataRow row in ds.Tables["users"].Rows)
                {
                    User user = new User(int.Parse(row[0].ToString()), row[1].ToString(), row[3].ToString(), int.Parse(row[4].ToString()), bool.Parse(row[5].ToString()));
                    users.Add(user);
                }
            }
        }

        public static void getUser()
        {
            conn.Open();
            String sql = $"SELECT * FROM users WHERE ID = {userId};";
            MySqlDataReader reader;
            MySqlCommand command = new MySqlCommand(sql, conn);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                user = new User(int.Parse(reader["ID"].ToString()), reader["username"].ToString(), reader["email"].ToString(), int.Parse(reader["role"].ToString()), bool.Parse(reader["active"].ToString()));
            }
            conn.Close();
        }

        public static String getDirectory()
        {
            String path = "";
            OperatingSystem system = Environment.OSVersion;
            if (system.Platform == PlatformID.Unix)
            {
                return $"/home/{SystemInformation.UserName}/.local/share/";
            } else if (system.Platform == PlatformID.MacOSX)
            {
                return $"/Users/{SystemInformation.UserName}/Library/Application Support/";
            }
            path =  Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            SetFolderPermission(path);
            return path;
        }
        
        public static void SetFolderPermission(string folderPath)
        {
            var directoryInfo = new DirectoryInfo(folderPath);
            var directorySecurity = directoryInfo.GetAccessControl();
            var currentUserIdentity = WindowsIdentity.GetCurrent();
            var fileSystemRule = new FileSystemAccessRule(currentUserIdentity.Name, 
                FileSystemRights.Read, 
                InheritanceFlags.ObjectInherit |
                InheritanceFlags.ContainerInherit, 
                PropagationFlags.None,
                AccessControlType.Allow);
          
            directorySecurity.AddAccessRule(fileSystemRule);
            directoryInfo.SetAccessControl(directorySecurity);
        }

        public static void setConnection()
        {
            String connectionString = "SERVER=localhost;DATABASE=SuperSharpShop;UID=root;PASSWORD=;";
            //String connectionString = "SERVER=192.168.0.113;DATABASE=SuperSharpShop;UID=root;PASSWORD=;";
            conn = new MySqlConnection(connectionString);
            //conn.StateChange += new StateChangeEventHandler(chenaged);
        }

        public static void chenaged(object sender, EventArgs e)
        {
            Console.WriteLine(conn.State + " | " + e.ToString());
        }
        
        public static void setAllItems()
        {
            setItems(App.storePanel);
            setItems(App.ownedPanel);
            setItems(App.installedPanel);
        }

        public static void setItems(Control panel)
        {
            foreach (List<Label> labels in App.titles)
            {
                labels.Clear();
            }
            foreach (List<Label> labels in App.priceCards)
            {
                labels.Clear();
            }
            if (panel.Name == "Shop")
            {
                App.storePanel.Controls.Clear();
                shopItems.Clear();
                conn.Open();
                String sql = "SELECT * FROM items;";
                MySqlCommand command = new MySqlCommand(sql, conn);
                var da = new MySqlDataAdapter(command);
                var ds = new DataSet();
                da.Fill(ds, "items");
                foreach (DataRow row in ds.Tables["items"].Rows)
                {
                    Item item = new Item(row[1].ToString(), row[2].ToString(), $"\x20ac{row[5]}", (byte[]) row[4],
                        row[3].ToString(), "shop");
                    shopItems.Add(item);
                    item.setItem();
                }
                conn.Close();
            } else if (panel.Name == "Library") {
                App.ownedPanel.Controls.Clear();
                libraryItems.Clear();
                conn.Open();
                String sql = $"SELECT item_ID FROM owned WHERE user_ID = {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                List<String> owned = new List<string>();
                while (reader.Read())
                {
                    owned.Add(reader.GetValue(0).ToString());
                }

                conn.Close();
                foreach (String itemID in owned)
                {
                    conn.Open();
                    sql = $"SELECT * FROM items WHERE ID = '{itemID}';";
                    MySqlCommand newcommand = new MySqlCommand(sql, conn);
                    MySqlDataReader newreader = newcommand.ExecuteReader();
                    while (newreader.Read())
                    {
                        Item item = new Item(newreader.GetValue(1).ToString(), newreader.GetValue(2).ToString(),
                            $"\x20ac{newreader.GetValue(5).ToString()}", (byte[]) newreader.GetValue(4),
                            newreader.GetValue(3).ToString(), "library");
                        libraryItems.Add(item);
                        item.setItem();
                    }

                    conn.Close();
                }
            } else if (panel.Name == "Installed") {
                App.installedPanel.Controls.Clear();
                installedItems.Clear();
                conn.Open();
                String sql = $"SELECT item_ID FROM owned WHERE user_ID = {userId};";
                MySqlCommand command = new MySqlCommand(sql, conn);
                MySqlDataReader reader = command.ExecuteReader();
                List<String> owned = new List<string>();
                while (reader.Read())
                {
                    owned.Add(reader.GetValue(0).ToString());
                }
                String path = getDirectory() + $"SuperSharpShop/Common/{user.Name}/";
                List<String> dirs =  new List<string>(Directory.GetDirectories(path));
                conn.Close();
                foreach (String itemID in owned)
                {
                    conn.Open();
                    sql = $"SELECT * FROM items WHERE ID = '{itemID}';";
                    MySqlCommand newcommand = new MySqlCommand(sql, conn);
                    MySqlDataReader newreader = newcommand.ExecuteReader();
                    while (newreader.Read())
                    {
                        String dirName = newreader.GetValue(1).ToString().Replace(" ", "").Replace(":", "");
                        String  pathName = path + dirName;
                        if (dirs.Contains(pathName))
                        {
                            Item item = new Item(newreader.GetValue(1).ToString(), newreader.GetValue(2).ToString(),
                                $"\x20ac{newreader.GetValue(5).ToString()}", (byte[]) newreader.GetValue(4),
                                newreader.GetValue(3).ToString(), "installed");
                            installedItems.Add(item);
                            item.setItem();
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}
