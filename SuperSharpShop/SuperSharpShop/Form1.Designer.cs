using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Windows.Forms.VisualStyles;
using MySql.Data.MySqlClient;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace SuperSharpShop
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void panelSwitch(object sender, EventArgs e)
        {
            clickedPanel = null;
            Button button = (Button)sender;
            list.Controls.Clear();
            list.Visible = false;
            int index = 0;
            int count = 0;
            foreach (Panel panel in this.panels)
            {
                if (panel.Name == button.Name)
                {
                    //Console.WriteLine(panel.Name + " | " + button.Name);
                    index = count;
                }
                count = count + 1;
            }
            //Console.WriteLine(index);
            //Console.WriteLine(panels.Count);
            Panel Panel1 = panels[index];
            clickedPanel = Panel1;
            lastPanel.Controls.Clear();
            //List<String> friendsPanels = new List<String>(new []{"Friends", "Requests", "Rejected", "Add"});
            if (this.Controls.Count > 2)
            {
                this.Controls.RemoveAt(2);
            }
            if (panelTitles.Contains(Panel1.Name))
            {
                lastPanel = Panel1;
                //Console.WriteLine(lastPanel.Text);
            } else if (friendsPanels.Contains(Panel1.Name))
            {
                friendsPanel = Panel1;
                setFriends(new object(), new EventArgs());
            }
            this.Controls.Add(Panel1);
            if (Panel1 == profilePanel)
            {
                userButton.Visible = false;
            }
            else
            {
                userButton.Visible = true;
            }
            searchBar.Text = "";
            if (panelTitles.Contains(Panel1.Name))
            {
                setComboBox();
            }
            
        }

        public void setPanelButton(Panel panel, Button button, String name)
        {
            panel.Controls.Add(button);
            //Console.WriteLine(panel.ToString() + " | " + name);
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = panel.BackColor;
            button.ForeColor = Color.Honeydew;
            button.Font = new Font("Helvetica Neue", 12);
            button.FlatAppearance.BorderColor = button.BackColor;
            button.FlatStyle = FlatStyle.Flat;
            if (panel.Name == "Friends")
            {
                button.Location = new Point(0, panel.Controls.IndexOf(button) * 40 + 20 * panel.Controls.IndexOf(button) + 50);
            }
            else
            {
                button.Location = new Point(0, panel.Controls.IndexOf(button) * 40 + 10 * panel.Controls.IndexOf(button));
            }

            button.Size = new Size(250, 50);
            button.Name = name;
            button.Text = name;
            //Console.WriteLine($"{button.Name}: X: {button.Location.X}, Y: {button.Location.Y}, Text: {button.Text}");
            button.Click += new EventHandler(panelSwitch);
        }

        public void clickItem(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox) sender;
            Control control = pb.Parent;
            Panel panel = new Panel();
            setItemPanel(panel, pb.Name);
            if (this.Controls.Count > 2)
            {
                //if (control.Parent.Text != "Search" && control.Parent.Text != "SearchClick")
                //{
                //    lastPanel = control;
                //}
                this.Controls.RemoveAt(2);
            }
            this.Controls.Add(panel);
        }
        
        public void clickItemList(object sender, EventArgs e)
        {
            this.list.Controls.Clear();
            this.list.Visible = false;
            searchBar.Text = "";
            //Console.WriteLine(126);
            Button button = (Button) sender;
            //Console.WriteLine(128);
            Control control = this.Controls[2];
            //Console.WriteLine(130);
            Control item = null;
            //Console.WriteLine(132 + ": " + lastPanel.Name + " | " + lastPanel.Controls.Count);
            foreach (Control c in lastPanel.Controls)
            {
                //Console.WriteLine(135 + ": " + c.Name);
                if (c.Name == button.Name)
                {
                    //Console.WriteLine(138 + ": " + c.Name);
                    item = c;
                }
            }
            //Console.WriteLine(142);
            Panel panel = new Panel();
            setItemPanel(panel, item.Name);
            if (this.Controls.Count > 2)
            {
                //if (control.Parent.Text != "Search" && control.Parent.Text != "SearchClick")
                //{
                //    lastPanel = control;
                //}
                this.Controls.RemoveAt(2);
            }
            this.Controls.Add(panel);
            //Console.WriteLine(lastPanel.Name);
        }

        public void Buy(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            double price = double.Parse(button.Text.Substring(1));
            Item item = null;
            foreach (Item i in getPanelList())
            {
                if (i.Name == button.Name)
                {
                    item = i;
                }
            }
            Console.WriteLine(price);
            PayForm payForm = new PayForm(price, item);
            payForm.Show();
        }

        public void Install(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            Console.WriteLine("Installing");
            String dirName = button.Name.Replace(" ", "").Replace(":", "");
            String path = Program.getDirectory() + "SuperSharpShop/Common/";
            Directory.SetCurrentDirectory(path);
            Directory.CreateDirectory(dirName);
            Panel panel = new Panel();
            Program.setAllItems();
            setItemPanel(panel, button.Name);
            if (this.Controls.Count > 2)
            {
                this.Controls.RemoveAt(2);
            }
            Console.WriteLine(this.Controls.Count);
            this.Controls.Add(panel);
        }

        public void setItemPanel(Panel panel, String name)
        {
            setPanel(panel, name);
            Item item = null;
            foreach (Item i in getPanelList())
            {
                if (i.Name == name)
                {
                    item = i;
                }
            }
            Label itemName = new Label();
            itemName.Location = new Point(50, 50);
            itemName.Size = new Size(350, 50);
            itemName.Text = item.Name;
            itemName.TextAlign = ContentAlignment.MiddleCenter;
            itemName.BackColor = ColorTranslator.FromHtml("#454545");
            itemName.Font = new Font("Arial", 15, FontStyle.Bold);
            panel.Controls.Add(itemName);
            PictureBox pictureBox = new PictureBox();
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.Name = name;
            pictureBox.Location = new Point(50, 150);
            pictureBox.Size = new Size(500, 300);
            pictureBox.BackgroundImage = Image.FromStream( new MemoryStream(item.Image));
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            panel.Controls.Add(pictureBox);
            Label descriptionBox = new Label();
            Label description = new Label();
            descriptionBox.BackColor = ColorTranslator.FromHtml("#303030");
            descriptionBox.SuspendLayout();
            descriptionBox.TabIndex = 0;
            descriptionBox.TabStop = false;
            descriptionBox.ResumeLayout(false);
            descriptionBox.PerformLayout();
            descriptionBox.FlatStyle = FlatStyle.Flat;
            description.Text = item.Description;
            descriptionBox.Size = new Size(400, 175);
            description.Size = new Size(descriptionBox.Size.Width - 10, 170);
            descriptionBox.Location = new Point(pictureBox.Location.X + pictureBox.Size.Width + 100, pictureBox.Location.Y);
            description.Location = new Point(5, 13);
            descriptionBox.BorderStyle = BorderStyle.Fixed3D;
            description.Font = new Font("Arial", 12);
            descriptionBox.Controls.Add(description);
            panel.Controls.Add(descriptionBox);
            Label title = new Label();
            title.Text = item.Name;
            title.Size = new Size(340, 50);
            title.Location = new Point(descriptionBox.Location.X, descriptionBox.Location.Y + descriptionBox.Size.Height + 50);
            title.BorderStyle = BorderStyle.Fixed3D;
            title.Font = new Font("Arial", 12);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.BackColor = ColorTranslator.FromHtml("#303030");
            panel.Controls.Add(title);
            Button price = new Button();
            bool library = false;
            bool install = false;
            foreach (Item libraryItem in Program.libraryItems)
            {
                if (libraryItem.Name == item.Name)
                {
                    library = true;
                }
            }
            if (library)
            {
                foreach (Item installedItem in Program.installedItems)
                {
                    if (installedItem.Name == item.Name)
                    {
                        install = true;
                    }
                }
                
            }
            if (install)
            {
                price.Enabled = false;
                price.Text = "Installed";
                price.BackColor = ColorTranslator.FromHtml("#454545");
            } else if (library)
            {
                price.Text = "Install";
                price.Click += new EventHandler(Install);
                price.BackColor = Color.OrangeRed;
            }
            else
            {
                price.Text = item.Price;
                price.Click += new EventHandler(Buy);
                price.BackColor = Color.OrangeRed;
            }
            price.Name = item.Name;
            price.Size = new Size(descriptionBox.Size.Width - title.Size.Width, title.Size.Height);
            price.Location = new Point(title.Location.X + title.Size.Width, title.Location.Y);
            panel.Controls.Add(price);
        }

        public void setPanel(Panel panel, String name)
        {
            panels.Add(panel);
            List<String> friends = new List<string>(new []{"Friends", "Requests", "Rejected", "Awaiting", "Add"});
            panel.BackColor = ColorTranslator.FromHtml("#656565");
            panel.ForeColor = Color.Honeydew;
            panel.Location = new Point(250, menu.Size.Height);
            panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - panel.Location.X, Screen.PrimaryScreen.Bounds.Height - (Screen.PrimaryScreen.Bounds.Height / 5));
            panel.Name = name;
            panel.Text = name;
            List<String> list = new List<string>();
            foreach (List<Label> title in titles)
            {
                foreach (Label label in title)
                {
                    list.Add(label.Text);
                }
            }
            if (name != "Search" && name != "SearchClick" && !list.Contains(name) && name != Program.user.Name && !friends.Contains(name))
            {
                setPanelButton(this.panel, new Button(), name);
            } else if (friends.Contains(name)) {
                //Console.WriteLine(sidePanel.Name);
                setPanelButton(sidePanel, new Button(), name);
                Label title = new Label();
                title.Location = new Point(25, 25);
                title.Size = new Size(200, 50);
                title.Text = name;
                title.TextAlign = ContentAlignment.MiddleCenter;
                title.BackColor = ColorTranslator.FromHtml("#454545");
                title.Font = new Font("Arial", 20, FontStyle.Bold);
                panel.Controls.Add(title);
                friendsPanels.Add(name);
            } else if (name == Program.user.Name)
            {
                setProfile();
            } else if (list.Contains(name))
            {
                List<int> indexes = new List<int>();
                foreach (Panel pane in panels)
                {
                    if (pane.Name == name)
                    {
                        panels.IndexOf(pane);
                    }
                }
                foreach (int index in indexes)
                {
                    panels.RemoveAt(index);
                }
            }
            panel.AutoScroll = false;
            panel.HorizontalScroll.Enabled = false;
            panel.HorizontalScroll.Visible = false;
            panel.HorizontalScroll.Maximum = 0;
            panel.AutoScrollPosition = new Point(Screen.PrimaryScreen.Bounds.Width - 100, 0);
            panel.AutoScrollMinSize = new Size(10, panel.Size.Height);
            panel.AutoScroll = true;
            panel.MouseEnter += new EventHandler(Scrolling);
        }

        public void Scrolling(object sender, EventArgs e)
        {
            Panel panel = (Panel) sender;
            panel.Focus();
        }

        public void setPanelSize(Control panel)
        {
            //Console.WriteLine(panel.Controls.Count);
            if (Screen.PrimaryScreen.Bounds.Height > (panel.Controls.Count / 3) * 300 - 350)
                panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, Screen.PrimaryScreen.Bounds.Height);
            else
            {
                panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, (panel.Controls.Count / 3) * 300 - 350);
            }
        }
        
        public void showCard(Label title, Label priceCard)
        {
            title.Visible = true;
            //Console.WriteLine(title.Text);
            if (lastPanel.Text == "Shop")
            {
                priceCard.Visible = true;
            }
        }

        public void hideCard(Label title, Label priceCard)
        {
            title.Visible = false;
            priceCard.Visible = false;
        }

        public void setCard(object sender, EventArgs e)
        {
            //Console.WriteLine(lastPanel.Controls.Count);
            if (lastPanel.Controls.Count > 1)
            {
                GroupBox item = (GroupBox) sender;
                if ($"{item.Parent}" != "")
                {
                    int index = item.Parent.Controls.IndexOf(item);
                    //Console.WriteLine(233);
                    List<Label> titles = getTitlesList(item.Parent);
                    Label title = titles[index];
                    List<Label> priceCards = getPriceCardList(item.Parent);
                    Label priceCard = priceCards[index];
                    //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
                    showCard(title, priceCard);
                }
            }
        }

        public void removeCard(object sender, EventArgs e)
        {
            if (lastPanel.Controls.Count > 1)
            {
                GroupBox item = (GroupBox) sender;
                if ($"{item.Parent}" != "")
                {
                    int index = item.Parent.Controls.IndexOf(item);
                    List<Label> titles = getTitlesList(item.Parent);
                    Label title = titles[index];
                    List<Label> priceCards = getPriceCardList(item.Parent);
                    Label priceCard = priceCards[index];
                    //Console.WriteLine(item.Name + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
                    hideCard(title, priceCard);
                }
            }
        }
        
        public void setCardChild(object sender, EventArgs e)
        {
            
            if (lastPanel.Controls.Count > 1)
            {
                Control child = (Control) sender;
                Control item = child.Parent;
                if ($"{item.Parent}" != "")
                {
                    int index = item.Parent.Controls.IndexOf(item);
                    List<Label> titles = getTitlesList(item.Parent);
                    Label title = titles[index];
                    List<Label> priceCards = getPriceCardList(item.Parent);
                    Label priceCard = priceCards[index];
                    //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
                    showCard(title, priceCard);
                }
            }
        }
        

        public void removeCardChild(object sender, EventArgs e)
        {
            if (lastPanel.Controls.Count > 1)
            {
                Control child = (Control) sender;
                Control item = child.Parent;
                if ($"{item.Parent}" != "")
                {
                    int index = item.Parent.Controls.IndexOf(item);
                    List<Label> titles = getTitlesList(item.Parent);
                    Label title = titles[index];
                    List<Label> priceCards = getPriceCardList(item.Parent);
                    Label priceCard = priceCards[index];
                    hideCard(title, priceCard);
                }
            }
        }

        public void setTitleList(Panel panel, Label title)
        {
            if (panel.Text == "Shop")
            {
                storeTitles.Add(title);
            } else if (panel.Text == "Library")
            {
                libraryTitles.Add(title);
            } else if (panel.Text == "Installed")
            {
                installedTitles.Add(title);
            } else if (panel.Text == "Search" || panel.Text == "SearchClick")
            {
                searchTitles.Add(title);
            }
        }

        public List<Label> getTitlesList(Control panel)
        {
            if (panel.Text == "Shop")
            {
                return storeTitles;
            } else if (panel.Text == "Library")
            {
                return libraryTitles;
            } else if (panel.Text == "Installed")
            {
                return installedTitles;
            }else if (panel.Text == "Search" || panel.Text == "SearchClick")
            {
                return searchTitles;
            }
            return new List<Label>();
        }
        
        public void setPriceCardList(Panel panel, Label priceCard)
        {
            if (panel.Text == "Shop")
            {
                storePriceCards.Add(priceCard);
            } else if (panel.Text == "Library")
            {
                libraryPriceCards.Add(priceCard);
            } else if (panel.Text == "Installed")
            {
                installedPriceCards.Add(priceCard);
            } else if (panel.Text == "Search" || panel.Text == "SearchClick")
            {
                searchPriceCards.Add(priceCard);
            }
        }

        public List<Label> getPriceCardList(Control panel)
        {
            if (panel.Text == "Shop")
            {
                return storePriceCards;
            } else if (panel.Text == "Library")
            {
                return libraryPriceCards;
            } else if (panel.Text == "Installed")
            {
                return installedPriceCards;
            }else if (panel.Text == "Search" || panel.Text == "SearchClick")
            {
                return searchPriceCards;
            }
            return new List<Label>();
        }

        public void setProfile()
        {
            Label username = new Label();
            username.Location = new Point(25, 25);
            username.Size = new Size(200, 50);
            username.Text = profilePanel.Name;
            username.TextAlign = ContentAlignment.MiddleCenter;
            username.BackColor = ColorTranslator.FromHtml("#454545");
            username.Font = new Font("Arial", 20, FontStyle.Bold);
            profilePanel.Controls.Add(username);
        }

        public void setCardEvents(GroupBox item)
        {
            item.MouseEnter += new EventHandler(setCard);
            item.MouseLeave += new EventHandler(removeCard);
            foreach (Control child in item.Controls)
            {
                //Console.WriteLine($"{item.Parent.Text}: {item.Name}");
                child.MouseEnter += new EventHandler(setCardChild);
                child.MouseLeave += new EventHandler(removeCardChild);
            }
        }
        

        public void setItem(Panel panel, GroupBox item, String name, String description, String price, byte[] image, String type)
        {
            //Console.WriteLine(name);
            panel.Controls.Add(item);
            item.Name = name;
            //Console.WriteLine(403);
            item.Size = new Size((Screen.PrimaryScreen.Bounds.Width - 450) / 3, (Screen.PrimaryScreen.Bounds.Height - 25) / 3);
            //Console.WriteLine(405);
            if (panelTitles.Contains(panel.Name))
            {
                item.Location = new Point(50 + panel.Controls.IndexOf(item) % 3 * (item.Size.Width + 50), panel.Controls.IndexOf(item) / 3 * (item.Size.Height + 25) + 100);
            }
            else
            {
                item.Location = new Point(50 + panel.Controls.IndexOf(item) % 3 * (item.Size.Width + 50), panel.Controls.IndexOf(item) / 3 * (item.Size.Height + 25) + 25);
            }

            item.SuspendLayout();
            item.TabIndex = 0;
            item.TabStop = false;
            item.ResumeLayout(false);
            item.PerformLayout();
            item.FlatStyle = FlatStyle.Flat;
            //Console.WriteLine($"{item.Name}: X: {item.Location.X}, Y: {item.Location.Y}, Width: {item.Size.Width}, Height: {item.Size.Height}");
            Label title = new Label();
            title.Text = name;
            title.Name = type;
            title.Size = new Size(200, 25);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(2, 8);
            title.BackColor = ColorTranslator.FromHtml("#202020");
            item.Controls.Add(title);
            title.Visible = false;
            setTitleList(panel, title);
            //Console.WriteLine(424);
            PictureBox pb = new PictureBox();
            //Console.WriteLine(426);
            pb.Name = name;
            //Console.WriteLine(428);
            pb.Size = new Size(item.Size.Width - 4, item.Size.Height - 100);
            pb.Location = new Point(2, 8);
            MemoryStream buf = new MemoryStream(image);
            pb.BackgroundImage = Image.FromStream(buf);
            pb.BackgroundImageLayout = ImageLayout.Stretch;
            pb.Click += new EventHandler(clickItem);
            //Console.WriteLine(pb.ToString());
            item.Controls.Add(pb);
            //Console.WriteLine(437);
            Label priceCard = new Label();
            priceCard.Name = $"{price}";
            priceCard.Text = $"{price}";
            priceCard.Size = new Size(50, 25);
            priceCard.TextAlign = ContentAlignment.MiddleCenter;
            //Console.WriteLine(443);
            priceCard.Location = new Point(item.Size.Width - (priceCard.Size.Width + 2), item.Size.Height - (priceCard.Size.Height + 2));
            priceCard.BackColor = ColorTranslator.FromHtml("#202020");
            priceCard.Visible = false;
            //Console.WriteLine(447);
            setPriceCardList(panel, priceCard);
            item.Controls.Add(priceCard);
            Label descriptionCard = new Label();
            descriptionCard.Name = name;
            descriptionCard.Text = description;
            descriptionCard.TextAlign = ContentAlignment.TopLeft;
            descriptionCard.Size = new Size(pb.Size.Width, item.Size.Height - (pb.Size.Height + pb.Location.Y));
            descriptionCard.Location = new Point(pb.Location.X, pb.Size.Height + pb.Location.Y - 2);
            descriptionCard.BackColor = ColorTranslator.FromHtml("#303030");
            item.Controls.Add(descriptionCard);
            //Console.WriteLine($"{priceCard.Name}: X: {priceCard.Location.X}, Y: {priceCard.Location.Y}, Width: {priceCard.Size.Width}, Height: {priceCard.Size.Height}");
            setCardEvents(item);
            //setPanelSize(panel);
        }

        public void search(object sender, EventArgs e)
        {
            Control control = this.Controls[2];
            if (searchBar.Text.Trim() != "") {
                //Console.WriteLine(control.Text);
                Panel panel = new Panel();
                setPanel(panel,"Search" );
                list.Controls.Clear();
                list.Visible = true;
                list.Size = new Size(searchBar.Size.Width, 0);
                list.Location = new Point(searchBar.Location.X, searchBar.Location.Y + searchBar.Size.Height);
                userButton.Visible = true;
                searchTitles.Clear();
                searchPriceCards.Clear();
                foreach (Control child in lastPanel.Controls)
                {
                    bool check = false;
                    int index = 0;
                    int i = 0;
                    
                    foreach (String word in child.Name.Split())
                    {
                        if (word.ToLower().StartsWith(searchBar.Text.ToLower()))
                        {
                            check = true;
                            index = i;
                        }
                        i++;
                    }
                    //Console.WriteLine($"{child.Name.ToLower().Contains(searchBar.Text.ToLower())} {child.Name} {searchBar.Text}");
                    if (check)
                    {
                        MemoryStream ms = new MemoryStream();
                        Image imageIn = child.Controls[1].BackgroundImage;
                        imageIn.Save(ms,imageIn.RawFormat);
                        setItem(panel,new GroupBox(),  child.Name, child.Controls[3].Text, child.Controls[2].Text, ms.ToArray(), child.Controls[0].Name);
                        Button button = new Button();
                        if (child.Name.Contains(":"))
                        {
                            button.Text = $"\t{child.Name.Split(':')[1]}";
                        }
                        else
                        {
                            button.Text = $"\t{child.Name}";
                        }

                        button.Name = child.Name;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        button.Size = new Size(list.Size.Width, 30);
                        button.TextAlign = ContentAlignment.MiddleCenter;
                        button.Font = new Font("Arial", 9);
                        PictureBox pb = new PictureBox();
                        pb.Size = new Size(26,20);
                        pb.Location = new Point(2, 5);
                        MemoryStream buf = new MemoryStream(getItem(child.Name).Image);
                        pb.BackgroundImage = Image.FromStream(buf);
                        pb.BackgroundImageLayout = ImageLayout.Stretch;
                        button.Controls.Add(pb);
                        list.Controls.Add(button);
                        list.Size = new Size(list.Size.Width, button.Size.Height * (list.Controls.IndexOf(button) + 1) + button.Size.Height / 2);
                        button.Location = new Point(0, button.Size.Height * list.Controls.IndexOf(button));
                        //`Console.WriteLine("Button: "+ list.Controls.IndexOf(button));
                        button.Click += new EventHandler(clickItemList);
                        Console.WriteLine(list.Size.Height);
                    }
                }
                if (list.Controls.Count < 1)
                {
                    list.Visible = false;
                }
                //Console.WriteLine(this.Controls.Count);
                //Console.WriteLine(this.Controls[2].Text);
                if (this.Controls.Count > 2)
                {
                    if (panelTitles.Contains(control.Name))
                    {
                        lastPanel = control;
                    }

                    this.Controls.RemoveAt(2);
                }
                //setPanelSize(panel);
                this.Controls.Add(panel);
                Console.WriteLine("Search Ended");
            }
            else
            {
                list.Visible = false;
                if (this.Controls.Count > 2)
                {
                    if (panelTitles.Contains(control.Name))
                    {
                        lastPanel = control;
                        this.Controls.RemoveAt(2);
                        
                    }
                    else if (control.Name != "SearchClick")
                    {
                        this.Controls.RemoveAt(2);
                    }
                }
                if (control.Name != "SearchClick")
                {
                    //setPanelSize(lastPanel);
                    this.Controls.Add(lastPanel);
                }
            }
        }

        public Item getItem(String name)
        {
            Item result = new Item("", "", "", new byte[64], "", "");
            foreach (Item item in getPanelList())
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return result;
        }
        
        public void searchClick(object sender, EventArgs e)
        {
            if (searchBar.Text.Trim() != "")
            {
                Control control = this.Controls[2];
                //Console.WriteLine(control.Text);
                Panel panel = new Panel();
                setPanel(panel, "SearchClick");
                list.Controls.Clear();
                list.Visible = false;
                userButton.Visible = true;
                searchTitles.Clear();
                searchPriceCards.Clear();
                Console.WriteLine("SearchBar: " + searchBar.Text);
                foreach (Control child in lastPanel.Controls)
                {
                    bool check = false;
                    foreach (String word in child.Name.Split())
                    {
                        if (word.ToLower().StartsWith(searchBar.Text.ToLower()))
                        {
                            //Console.WriteLine("Child: " + child.Name);
                            check = true;
                        }
                    }

                    //Console.WriteLine($"{child.Name.ToLower().Contains(searchBar.Text.ToLower())} {child.Name} {searchBar.Text}");
                    if (check)
                    {
                        MemoryStream ms = new MemoryStream();
                        Image imageIn = child.Controls[1].BackgroundImage;
                        imageIn.Save(ms, imageIn.RawFormat);
                        //Console.WriteLine("Item: " + child.Name);
                        //Console.WriteLine($"{child.Name}, {child.Controls[3].Text}, {child.Controls[2].Text}");
                        setItem(panel, new GroupBox(), child.Name, child.Controls[3].Text, child.Controls[2].Text,
                            ms.ToArray(), child.Controls[0].Name);
                    }
                }

                //Console.WriteLine(this.Controls.Count);
                //Console.WriteLine(panel.Controls.Count);
                if (this.Controls.Count > 2)
                {
                    if (panelTitles.Contains(control.Name))
                    {
                        lastPanel = control;
                    }

                    this.Controls.RemoveAt(2);
                }
                this.Controls.Add(panel);
                searchBar.Text = "";
                Console.WriteLine("Search Click Ended");
            }
        }

        public void getHelp(object sender, EventArgs e)
        {
            MessageBox.Show("Stop it. Get some Help");
        }

        public void setShop(object sender, EventArgs e)
        {
            if (this.Controls.Count > 2)
            {
                this.Controls.RemoveAt(2);
            }
            this.Controls.RemoveAt(1);
            this.Controls.Add(panel);
            Program.setItems(lastPanel);
            this.Controls.Add(lastPanel);
            setComboBox();
        }

        public void requestClick(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            int id = int.Parse(button.Name);
            if (button.Text == "Accept")
            {
                Program.conn.Open();
                String sql = $"UPDATE friends SET answered = 1, accepted = 1 WHERE applicant_ID = {id} AND receiver_ID = {Program.userId};";
                MySqlCommand command = new MySqlCommand(sql, Program.conn);
                command.ExecuteNonQuery();
                Program.conn.Close();
            } else if (button.Text == "Reject")
            {
                Program.conn.Open();
                String sql = $"UPDATE friends SET answered = 1, accepted = 0 WHERE applicant_ID = {id} AND receiver_ID = {Program.userId};";
                MySqlCommand command = new MySqlCommand(sql, Program.conn);
                command.ExecuteNonQuery();
                Program.conn.Close();
            } else if (button.Text == "Add")
            {
                Program.conn.Open();
                String sql = $"INSERT INTO friends (applicant_ID, receiver_ID) VALUES ({Program.userId}, {id})";
                MySqlCommand command = new MySqlCommand(sql, Program.conn);
                command.ExecuteNonQuery();
                Program.conn.Close();
            }
            setFriends(new object(), new EventArgs());
        }

        public void setFriend(Panel panel, GroupBox item, int id, String name, String email,String role, bool active)
        {
            item.Name = name;
            panel.Controls.Add(item);
            item.Size = new Size(825, 65);
            item.Location = new Point(100, (item.Size.Height - 8) * panel.Controls.IndexOf(item) + 100);
            item.SuspendLayout();
            item.TabIndex = 0;
            item.TabStop = false;
            item.ResumeLayout(false);
            item.PerformLayout();
            Label label = new Label();
            label.BackColor = ColorTranslator.FromHtml("#353535");
            item.Controls.Add(label);
            label.Size = new Size(item.Size.Width - 4, item.Size.Height - 10);
            label.Location = new Point(2, 8);
            Label username = new Label();
            username.Text = name;
            username.Font = new Font("Arial", 14);
            username.Size = new Size(200, label.Size.Height);
            username.TextAlign = ContentAlignment.MiddleCenter;
            label.Controls.Add(username);
            Label emailBox = new Label();
            emailBox.Text = email;
            emailBox.Font = new Font("Arial", 10);
            emailBox.Location = new Point(200, 0);
            emailBox.Size = new Size(200, label.Size.Height);
            emailBox.TextAlign = ContentAlignment.MiddleCenter;
            label.Controls.Add(emailBox);
            Label roleBox = new Label();
            roleBox.Text = $"Role: {role}";
            roleBox.Font = new Font("Arial", 11);
            roleBox.Location = new Point(400, 0);
            roleBox.Size = new Size(200, label.Size.Height);
            roleBox.TextAlign = ContentAlignment.MiddleCenter;
            label.Controls.Add(roleBox);
            if (panel.Name == "Friends" || panel.Name == "Awaiting")
            {
                Label statusBox = new Label();
                String status = "";
                if (active)
                {
                    status = "Online";
                }
                else
                {
                    status = "Offline";
                }
                statusBox.Text = $"Status: {status}";
                statusBox.Font = new Font("Arial", 12);
                statusBox.Size = new Size(150, label.Size.Height);
                statusBox.Location = new Point(label.Size.Width - statusBox.Size.Width, 0);
                statusBox.TextAlign = ContentAlignment.MiddleCenter;
                label.Controls.Add(statusBox);
            } else if (panel.Name == "Requests")
            {
                Button acceptButton = new Button();
                acceptButton.Text = "Accept";
                acceptButton.Name = id.ToString();
                acceptButton.BackColor = Color.LimeGreen;
                acceptButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                acceptButton.Location = new Point(label.Size.Width - (acceptButton.Size.Width * 2), 0);
                acceptButton.Click += new EventHandler(requestClick);
                label.Controls.Add(acceptButton);
                Button rejectButton = new Button();
                rejectButton.Text = "Reject";
                rejectButton.Name = id.ToString();
                rejectButton.BackColor = Color.Red;
                rejectButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                rejectButton.Location = new Point(label.Size.Width - rejectButton.Size.Width, 0);
                rejectButton.Click += new EventHandler(requestClick);
                label.Controls.Add(rejectButton);
            } else if (panel.Name == "Add")
            {
                String list = getFriendsList(name);
                if (list == "friends" || list == "awaiting")
                {
                    Button addButton = new Button();
                    addButton.Text = "Added";
                    addButton.Name = id.ToString();
                    addButton.BackColor = ColorTranslator.FromHtml("#454545");
                    addButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                    addButton.Location = new Point(label.Size.Width - (addButton.Size.Width), 0);
                    //addButton.Click += new EventHandler(requestClick);
                    addButton.Enabled = false;
                    label.Controls.Add(addButton);
                }
                else if (list == "request")
                {
                    Button acceptButton = new Button();
                    acceptButton.Text = "Accept";
                    acceptButton.Name = id.ToString();
                    acceptButton.BackColor = Color.LimeGreen;
                    acceptButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                    acceptButton.Location = new Point(label.Size.Width - (acceptButton.Size.Width * 2), 0);
                    acceptButton.Click += new EventHandler(requestClick);
                    label.Controls.Add(acceptButton);
                    Button rejectButton = new Button();
                    rejectButton.Text = "Reject";
                    rejectButton.Name = id.ToString();
                    rejectButton.BackColor = Color.Red;
                    rejectButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                    rejectButton.Location = new Point(label.Size.Width - rejectButton.Size.Width, 0);
                    rejectButton.Click += new EventHandler(requestClick);
                    label.Controls.Add(rejectButton);
                }
                else if (list == "None")
                {
                    Button addButton = new Button();
                    addButton.Text = "Add";
                    addButton.Name = id.ToString();
                    addButton.BackColor = Color.LimeGreen;
                    addButton.Size = new Size(label.Size.Height + 25, label.Size.Height);
                    addButton.Location = new Point(label.Size.Width - (addButton.Size.Width), 0);
                    addButton.Click += new EventHandler(requestClick);
                    label.Controls.Add(addButton);
                }
            }
        }

        public String getFriendsList(String name)
        {
            foreach (User user in Program.friends)
            {
                if (user.Name == name)
                {
                    return "friends";
                }
            }
            foreach (User user in Program.awaitingUsers)
            {
                if (user.Name == name)
                {
                    return "awaiting";
                }
            }
            foreach (User user in Program.requestUsers)
            {
                if (user.Name == name)
                {
                    return "request";
                }
            }
            return "None";
        }

        public void searchFriend(object sender, EventArgs e)
        {
            TextBox searchBar = (TextBox) sender;
            Panel panel = (Panel) searchBar.Parent;
            searchList.Controls.Clear();
            searchFriendsClear(panel);
            if (searchBar.Text.Trim() != "")
            {
                foreach (User user in Program.users)
                {
                    if (user.Name.ToLower().StartsWith(searchBar.Text.ToLower()))
                    {
                        user.setFriend(panelAdd);
                    }
                }
            }
        }

        public void searchFriendsClear(Panel panel)
        {
            List<int> indexes = new List<int>();
            Console.WriteLine(panel.Controls.Count);
            foreach (Control control in panel.Controls)
            {
                if (panel.Controls.IndexOf(control) > 1)
                {
                    indexes.Add(panel.Controls.IndexOf(control));
                }
            }
            indexes.Reverse();
            foreach (int index in indexes)
            {
                panel.Controls.RemoveAt(index);
            }
        }

        public void addFriendsPanel(Panel panel)
        {
            TextBox searchBar = new TextBox();
            searchBar.Size = new Size(500, 50);
            searchBar.Location = new Point(300, 50);
            searchBar.Font = new Font("Arial", 15);
            searchBar.TextChanged += new EventHandler(searchFriend);
            panel.Controls.Add(searchBar);
            //searchList.Visible = false;
            //searchList.Location = new Point(searchBar.Location.X, searchBar.Location.Y + searchBar.Size.Height);
            //searchList.Size = new Size(searchBar.Size.Width, 0);
            //panel.Controls.Add(searchList);
            Program.setFriends(panel);
            
        }

        public void setFriends(object sender, EventArgs e)
        {
            sidePanel.Name = "Friends";
            //Console.WriteLine(sidePanel.Name);
            sidePanel.Size = new Size(250, Screen.PrimaryScreen.Bounds.Height - menu.Size.Height);
            sidePanel.Location = new Point(0, menu.Size.Height);
            sidePanel.BackColor = ColorTranslator.FromHtml("#454545");
            sidePanel.Controls.Clear();
            friendsPanels.Clear();
            panelFriends.Controls.Clear();
            panelRequests.Controls.Clear();
            panelAwaiting.Controls.Clear();
            setPanel(panelFriends, "Friends");
            Program.setFriends(panelFriends);
            setPanel(panelRequests, "Requests");
            Program.setFriends(panelRequests);
            //setPanel(panelRejected, "Rejected");
            setPanel(panelAwaiting, "Awaiting");
            Program.setFriends(panelAwaiting);
            setPanel(panelAdd, "Add");
            addFriendsPanel(panelAdd);
            //setPanel(new Panel(), "Accepted");
            //panel.BackColor = Color.Aquamarine;
            if (this.Controls.Count > 2)
            {
                this.Controls.RemoveAt(2);
            }
            this.Controls.RemoveAt(1);
            this.Controls.Add(sidePanel);
            this.Controls.Add(friendsPanel);
        }
        
        public void setMenu(Panel menu, String name)
        {
            menu.BorderStyle = BorderStyle.None;
            menu.BackColor = ColorTranslator.FromHtml("#353535");
            menu.Size = new Size(Screen.PrimaryScreen.Bounds.Width, 35);
            Button button = new Button();
            button.Text = "SuperSharpShop";
            button.Font = new Font("Arial", 8, FontStyle.Bold);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Click += new EventHandler(setShop);
            menu.Controls.Add(button);
            Button button1 = new Button();
            button1.Text = "Add Item";
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            button1.Click += new EventHandler(addItem);
            menu.Controls.Add(button1);
            Button button2 = new Button();
            button2.Text = "Friends";
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.Click += new EventHandler(setFriends);
            menu.Controls.Add(button2);
            Button button3 = new Button();
            button3.Text = "Help";
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.Click += new EventHandler(getHelp);
            menu.Controls.Add(button3);
            foreach (Control item in menu.Controls)
            {
                item.ForeColor = Color.Honeydew;
                item.Size = new Size(100, menu.Size.Height);
                item.Location = new Point(50 + (item.Size.Width + 25) * menu.Controls.IndexOf(item), 0);
            }
        }

        public void setFooter(Panel parent, Panel panel)
        {
            
        }
        
        public void addItem(object sender, EventArgs e)
        {
            if (itemForm == null)
            {
                itemForm = new ItemForm();
                itemForm.Show();
            }
            else
            {
                itemForm.BringToFront();
            }
        }

        public void setComboBox()
        {
            type = new ComboBox();
            type.Text = "All";
            type.FlatStyle = FlatStyle.Flat;
            type.Size = new Size(200, 30);
            type.Location = new Point(50, 20);
            type.TextChanged += new EventHandler(typeChanged);
            type.Items.Clear();
            type.Items.Add("All");
            type.Items.Add("Games");
            type.Items.Add("Films");
            //Panel panel = (Panel) this.Controls[2];
            //Control.ControlCollection controls = lastPanel.Controls;
            //lastPanel.Controls.Clear();
            //lastPanel.Controls.Add(this.type);
            setItemType();
        }


        public List<Item> getPanelList()
        {
            if (lastPanel.Name == "Shop")
            {
                return Program.shopItems;
            } else if (lastPanel.Name == "Library")
            {
                return Program.libraryItems;
            } else if (lastPanel.Name == "Installed")
            {
                return Program.installedItems;
            }
            else
            {
                return new List<Item>();
            }
        }

        public void setItemType()
        {
            Program.setItems(lastPanel);
            List<String> types = new List<String>();
            types.AddRange(new []{"All", "Games", "Films"});
            String type = this.type.Text;
            if (types.Contains(type)) {
                foreach (List<Label> labels in titles)
                {
                    labels.Clear();
                }
                foreach (List<Label> labels in priceCards)
                {
                    labels.Clear();
                }
                Panel panel = (Panel) this.Controls[2];
                List<Item> controls = getPanelList();
                //foreach (Control control in panel.Controls)
                //{
                //    controls.Add(control);
                //}
                //Console.WriteLine("Panel:" + panel.Name);
                panel.Controls.Clear();
                if (type == "All")
                {
                    //Console.WriteLine("Count:" + controls.Count);
                    foreach (Item item in controls)
                    {
                        //Console.WriteLine("Item:" + item.Description);
                        setItem(panel, new GroupBox(), item.Name, item.Description, item.Price, item.Image, item.Type);
                    }
                } else if (type == "Games")
                {
                    foreach (Item item in controls)
                    {
                        if (item.Type == "Game")
                        {
                            setItem(panel, new GroupBox(), item.Name, item.Description, item.Price, item.Image, item.Type);
                        }
                    }
                } else if (type == "Films")
                {
                    foreach (Item item in controls)
                    {
                        if (item.Type == "Film")
                        {
                            
                            setItem(panel, new GroupBox(), item.Name, item.Description, item.Price, item.Image, item.Type);
                        }
                    }
                }
                panel.Controls.Add(this.type);
            }
        }
        
        private void KeyEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchClick(sender, new EventArgs());
            }
        }

        public void typeChanged(object sender, EventArgs e)
        {
            setItemType();
        }
        
        public void CloseForm(object sender, EventArgs e)
        {
            // UPDATE `items` SET `image` = (SELECT image FROM (SELECT * FROM `items`) AS `tablie` WHERE name = 'cdijvi') WHERE ID = 4;
            Program.conn.Open();
            String sql = $"UPDATE users SET active = 0 WHERE ID = {Program.userId}";
            MySqlCommand command = new MySqlCommand(sql, Program.conn);
            command.ExecuteNonQuery();
            Program.conn.Close();
            Console.WriteLine("Exit");
            Environment.Exit(0);
        }

        public void startForm(object sender, EventArgs e)
        {
            Application.ApplicationExit += Program.Exit;
        }
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.Text = "Super Sharp Shop";
            this.BackColor = Color.DimGray;
            setMenu(menu, "menu");
            this.Controls.Add(menu);
            this.Controls.Add(panel);
            this.panel.Size = new Size(250, Screen.PrimaryScreen.Bounds.Height - menu.Size.Height);
            this.panel.Location = new Point(0, menu.Size.Height);
            this.panel.BackColor = ColorTranslator.FromHtml("#454545");
            this.panel.Controls.Add(userButton);
            //this.Menu = new MainMenu();
            this.panel.Controls.Add(searchBar);
            //this.panel.Controls.Add(searchButton);
            this.panel.Controls.Add(list);
            panelTitles.AddRange(new []{"Shop", "Installed", "Library"});
            list.Visible = false;
            list.BackColor = Color.Azure;
            //searchBar.AppendText("Search...");
            searchBar.Location = new Point(20, 100);
            searchBar.Size = new Size(200, 21);
            setPanel(profilePanel, Program.user.Name);
            userButton.Name = Program.user.Name;
            userButton.Text = Program.user.Name;
            userButton.FlatAppearance.BorderSize = 0;
            userButton.FlatStyle = FlatStyle.Flat;
            userButton.Size = new Size(250, 50);
            userButton.Location = new Point(0, 25);
            userButton.ForeColor = Color.Honeydew;
            userButton.Font = new Font("Arial", 15);
            userButton.Click += new EventHandler(panelSwitch);
            searchBar.TextChanged += new EventHandler(search);
            searchBar.BorderStyle = BorderStyle.FixedSingle;
            searchBar.KeyDown += new KeyEventHandler(KeyEnter);
            searchButton.Name = "Search";
            searchButton.Text = "Search";
            searchButton.Size = new Size(55, 21);
            searchButton.Location = new Point(180, 100);
            searchButton.BackColor = ColorTranslator.FromHtml("#858585");
            searchButton.Font = new Font("Arial", 7);
            searchButton.Click += new EventHandler(searchClick);
            setPanel(storePanel, "Shop");
            setPanel(ownedPanel, "Library");
            setPanel(installedPanel, "Installed");
            //setPanelButton(storeButton, "Shop");
            //setPanelButton(ownedButton, "Library");
            //setPanelButton(installedButton, "Installed");
            this.WindowState = FormWindowState.Maximized;
            //storePanel.BackColor = Color.Aqua;
            //ownedPanel.BackColor = Color.Gold;
            //installedPanel.BackColor = Color.Green;
            this.Controls.Add(storePanel);
            this.lastPanel = storePanel;
            this.friendsPanel = panelFriends;
            titles.Add(storeTitles);
            titles.Add(libraryTitles);
            titles.Add(installedTitles);
            titles.Add(searchTitles);
            priceCards.Add(storePriceCards);
            priceCards.Add(libraryPriceCards);
            priceCards.Add(installedPriceCards);
            priceCards.Add(searchPriceCards);
            this.Closing += new CancelEventHandler(CloseForm);
            //this.Load += new EventHandler(startForm);
        }

        #endregion
        
        public Panel panel = new System.Windows.Forms.Panel();
        public Panel storePanel = new System.Windows.Forms.Panel();
        public Panel ownedPanel = new System.Windows.Forms.Panel();
        public Panel installedPanel = new System.Windows.Forms.Panel();
        //public Button storeButton = new System.Windows.Forms.Button();
        //public Button ownedButton = new System.Windows.Forms.Button();
        //public Button installedButton = new System.Windows.Forms.Button();
        public TextBox searchBar = new System.Windows.Forms.TextBox();
        public Button searchButton = new System.Windows.Forms.Button();
        public Button userButton = new System.Windows.Forms.Button();
        public List<Panel> panels = new List<Panel>();
        public List<Label> storeTitles = new List<Label>();
        public List<Label> libraryTitles = new List<Label>();
        public List<Label> installedTitles = new List<Label>();
        public List<Label> searchTitles = new List<Label>();
        public List<List<Label>> titles = new List<List<Label>>();
        public List<Label> storePriceCards = new List<Label>();
        public List<Label> libraryPriceCards = new List<Label>();
        public List<Label> installedPriceCards = new List<Label>();
        public List<Label> searchPriceCards = new List<Label>();
        public List<List<Label>> priceCards = new List<List<Label>>();
        public Control lastPanel;
        public Control friendsPanel;
        public Panel menu = new Panel();
        public Panel profilePanel = new Panel();
        public ItemForm itemForm;
        public ListBox list = new ListBox();
        public ComboBox type;
        public Control clickedPanel;
        public List<String> panelTitles = new List<string>();
        public Panel sidePanel = new Panel();
        public List<String> friendsPanels = new List<String>();
        public Panel panelFriends = new Panel();
        public Panel panelRequests = new Panel();
        //public Panel panelRejected = new Panel();
        public Panel panelAwaiting = new Panel();
        public Panel panelAdd = new Panel();
        public ListBox searchList = new ListBox();
    }
}

