using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms.VisualStyles;
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
            Button button = (Button)sender;
            list.Controls.Clear();
            list.Visible = false;
            int index = 0;
            int count = 0;
            foreach (Panel panel in this.panels)
            {
                Console.WriteLine(panel.Name + " | " + button.Name);
                if (panel.Name == button.Name)
                {
                    Console.WriteLine(panel.Name + " | " + button.Name);
                    index = count;
                }
                count = count + 1;
            }
            Panel Panel1 = panels[index];
            if (this.Controls.Count > 2)
            {
                if (Panel1.Name != "Search" && Panel1.Name != "SearchClick")
                {
                    lastPanel = Panel1;
                    Console.WriteLine(lastPanel.Text);
                }
                this.Controls.RemoveAt(2);
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
            Program.setItems();
            searchBar.Text = "";
        }

        public void setPanelButton(Button button, String name)
        {
            this.panel.Controls.Add(button);
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = this.panel.BackColor;
            button.ForeColor = Color.Honeydew;
            button.Font = new Font("Helvetica Neue", 12);
            button.FlatAppearance.BorderColor = button.BackColor;
            button.FlatStyle = FlatStyle.Flat;
            
            button.Location = new Point(0, panel.Controls.IndexOf(button) * 40 + 10 * panel.Controls.IndexOf(button));
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
            setPanel(panel, control.Name);
            if (this.Controls.Count > 2)
            {
                if (control.Parent.Text != "Search" && control.Parent.Text != "SearchClick")
                {
                    lastPanel = control;
                }
                this.Controls.RemoveAt(2);
            }
            this.Controls.Add(panel);
            PictureBox pictureBox = new PictureBox();
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.Name = pb.Name;
            pictureBox.Location = new Point(100, 50);
            pictureBox.Size = new Size(panel.Size.Width - pictureBox.Location.X * 2, 400);
            pictureBox.BackgroundImage = pb.BackgroundImage;
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            panel.Controls.Add(pictureBox);
        }
        
        public void clickItemList(object sender, EventArgs e)
        {
            this.list.Controls.Clear();
            this.list.Visible = false;
            searchBar.Text = "";
            Button button = (Button) sender;
            Control control = this.Controls[2];
            Control item = null;
            foreach (Control c in control.Controls)
            {
                if (c.Name == button.Text)
                {
                    item = c;
                }
            }
            Panel panel = new Panel();
            setPanel(panel, item.Name);
            if (this.Controls.Count > 2)
            {
                if (control.Parent.Text != "Search" && control.Parent.Text != "SearchClick")
                {
                    lastPanel = control;
                }
                this.Controls.RemoveAt(2);
            }
            this.Controls.Add(panel);
            PictureBox pictureBox = new PictureBox();
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.Name = button.Name;
            pictureBox.Location = new Point(100, 50);
            pictureBox.Size = new Size(panel.Size.Width - pictureBox.Location.X * 2, 400);
            pictureBox.BackgroundImage = item.Controls[1].BackgroundImage;
            pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            panel.Controls.Add(pictureBox);
        }

        public void setPanel(Panel panel, String name)
        {
            panels.Add(panel);
            panel.BackColor = this.BackColor;
            panel.ForeColor = Color.Honeydew;
            panel.Location = new Point(250, menu.Size.Height);
            setPanelSize(panel);
            panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, Screen.PrimaryScreen.Bounds.Height);
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
            if (name != "Search" && name != "SearchClick" && !list.Contains(name) && name != Program.user.Name)
            {
                setPanelButton(new Button(), name);
            } else if (name == Program.user.Name)
            {
                setProfile();
            }
            //panel.VerticalScroll.Visible = true;
            //panel.VerticalScroll.Enabled = true;
            panel.AutoScroll = true;
            panel.AutoScrollPosition = new Point(Screen.PrimaryScreen.Bounds.Width - 100, 0);
            panel.AutoScrollMinSize = new Size(10, Screen.PrimaryScreen.Bounds.Height);
            panel.MouseEnter += new EventHandler(Scrolling);
        }

        public void Scrolling(object sender, EventArgs e)
        {
            Panel panel = (Panel) sender;
            panel.Focus();
        }

        public void setPanelSize(Control panel)
        {
            if (Screen.PrimaryScreen.Bounds.Height > (panel.Controls.Count / 3) * 300 + 50)
                panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, Screen.PrimaryScreen.Bounds.Height);
            else
            {
                panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, (panel.Controls.Count / 3) * 300 - 300);
            }
        }
        
        public void showCard(Label title, Label priceCard)
        {
            title.Visible = true;
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
            GroupBox item = (GroupBox) sender;
            int index = item.Parent.Controls.IndexOf(item);
            List<Label> titles = getTitlesList(item.Parent);
            Label title = titles[index];
            List<Label> priceCards = getPriceCardList(item.Parent);
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            showCard(title, priceCard);
        }

        public void removeCard(object sender, EventArgs e)
        {
            GroupBox item = (GroupBox) sender;
            int index = item.Parent.Controls.IndexOf(item);
            List<Label> titles = getTitlesList(item.Parent);
            Label title = titles[index];
            List<Label> priceCards = getPriceCardList(item.Parent);
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            hideCard(title, priceCard);
        }
        
        public void setCardChild(object sender, EventArgs e)
        {
            Control child = (Control) sender;
            Control item = child.Parent;
            int index = item.Parent.Controls.IndexOf(item);
            List<Label> titles = getTitlesList(item.Parent);
            Label title = titles[index];
            List<Label> priceCards = getPriceCardList(item.Parent);
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            showCard(title, priceCard);
        }
        

        public void removeCardChild(object sender, EventArgs e)
        {
            Control child = (Control) sender;
            Control item = child.Parent;
            int index = item.Parent.Controls.IndexOf(item);
            List<Label> titles = getTitlesList(item.Parent);
            Label title = titles[index];
            List<Label> priceCards = getPriceCardList(item.Parent);
            Label priceCard = priceCards[index];
            hideCard(title, priceCard);
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
        

        public void setItem(Panel panel, GroupBox item, String name, String description, String price, byte[] image)
        {
            panel.Controls.Add(item);
            item.Name = name;
            item.Size = new Size((Screen.PrimaryScreen.Bounds.Width - 450) / 3, (Screen.PrimaryScreen.Bounds.Height - 25) / 3);
            item.Location = new Point(50 + panel.Controls.IndexOf(item) % 3 * (item.Size.Width + 50),panel.Controls.IndexOf(item) / 3 * (item.Size.Height + 25) + 25);
            item.SuspendLayout();
            item.TabIndex = 0;
            item.TabStop = false;
            item.ResumeLayout(false);
            item.PerformLayout();
            item.FlatStyle = FlatStyle.Flat;
            //Console.WriteLine($"{item.Name}: X: {item.Location.X}, Y: {item.Location.Y}, Width: {item.Size.Width}, Height: {item.Size.Height}");
            Label title = new Label();
            title.Text = name;
            title.Size = new Size(200, 25);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(2, 2);
            title.BackColor = ColorTranslator.FromHtml("#202020");
            item.Controls.Add(title);
            title.Visible = false;
            setTitleList(panel, title);
            PictureBox pb = new PictureBox();
            pb.Name = image.ToString();
            pb.Size = new Size(item.Size.Width - 4, item.Size.Height - 100);
            pb.Location = new Point(2, 2);
            MemoryStream buf = new MemoryStream(image);
            pb.BackgroundImage = Image.FromStream(buf);
            pb.BackgroundImageLayout = ImageLayout.Stretch;
            pb.Click += new EventHandler(clickItem);
            item.Controls.Add(pb);
            Label priceCard = new Label();
            priceCard.Name = $"{price}";
            priceCard.Text = $"{price}";
            priceCard.Size = new Size(50, 25);
            priceCard.TextAlign = ContentAlignment.MiddleCenter;
            priceCard.Location = new Point(item.Size.Width - (priceCard.Size.Width + 2), item.Size.Height - (priceCard.Size.Height + 2));
            priceCard.BackColor = ColorTranslator.FromHtml("#202020");
            priceCard.Visible = false;
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
            setPanelSize(panel);
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
                        setItem(panel,new GroupBox(),  child.Name, child.Controls[3].Text, child.Controls[2].Text, ms.ToArray());
                        Button button = new Button();
                        button.Text = child.Name;
                        button.FlatAppearance.BorderSize = 0;
                        button.FlatStyle = FlatStyle.Flat;
                        button.Size = new Size(list.Size.Width, 30);
                        list.Controls.Add(button);
                        list.Size = new Size(list.Size.Width, button.Size.Height * (list.Controls.IndexOf(button) + 1) + button.Size.Height / 2);
                        button.Location = new Point(0, button.Size.Height * list.Controls.IndexOf(button));
                        Console.WriteLine("Button: "+ list.Controls.IndexOf(button));
                        button.Click += new EventHandler(clickItemList);
                    }
                }
                if (list.Controls.Count < 1)
                {
                    list.Visible = false;
                }
                Console.WriteLine(this.Controls.Count);
                Console.WriteLine(this.Controls[2].Text);
                if (this.Controls.Count > 2)
                {
                    if (control.Name != "Search")
                    {
                        lastPanel = control;
                    }

                    this.Controls.RemoveAt(2);
                }
                setPanelSize(panel);
                this.Controls.Add(panel);
                Console.WriteLine("Search Ended");
            }
            else
            {
                list.Visible = false;
                if (this.Controls.Count > 2)
                {
                    if (control.Name != "Search" && control.Name != "SearchClick")
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
                    setPanelSize(lastPanel);
                    this.Controls.Add(lastPanel);
                }
            }
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
                            Console.WriteLine("Child: " + child.Name);
                            check = true;
                        }
                    }

                    //Console.WriteLine($"{child.Name.ToLower().Contains(searchBar.Text.ToLower())} {child.Name} {searchBar.Text}");
                    if (check)
                    {
                        MemoryStream ms = new MemoryStream();
                        Image imageIn = child.Controls[1].BackgroundImage;
                        imageIn.Save(ms, imageIn.RawFormat);
                        Console.WriteLine("Item: " + child.Name);
                        //Console.WriteLine($"{child.Name}, {child.Controls[3].Text}, {child.Controls[2].Text}");
                        setItem(panel, new GroupBox(), child.Name, child.Controls[3].Text, child.Controls[2].Text,
                            ms.ToArray());
                    }
                }

                Console.WriteLine(this.Controls.Count);
                Console.WriteLine(panel.Controls.Count);
                if (this.Controls.Count > 2)
                {
                    if (control.Name != "Search" && control.Name != "SearchClick")
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

        public void setMenu(Panel menu, String name)
        {
            menu.BorderStyle = BorderStyle.None;
            menu.BackColor = ColorTranslator.FromHtml("#353535");
            menu.Size = new Size(Screen.PrimaryScreen.Bounds.Width, 35);
            Label label = new Label();
            label.Text = "SuperSharpShop";
            label.Font = new Font("Arial", 9, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleCenter;
            menu.Controls.Add(label);
            Button button = new Button();
            button.Text = "Add Item";
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Click += new EventHandler(addItem);
            menu.Controls.Add(button);
            Button button1 = new Button();
            button1.Text = "Friends";
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
            menu.Controls.Add(button1);
            Button button2 = new Button();
            button2.Text = "Help";
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.Click += new EventHandler(getHelp);
            menu.Controls.Add(button2);
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
            this.panel.Controls.Add(searchButton);
            this.panel.Controls.Add(list);
            list.Visible = false;
            list.BackColor = Color.Azure;
            //searchBar.AppendText("Search...");
            searchBar.Location = new Point(20, 100);
            searchBar.Size = new Size(150, 21);
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
            titles.Add(storeTitles);
            titles.Add(libraryTitles);
            titles.Add(installedTitles);
            titles.Add(searchTitles);
            priceCards.Add(storePriceCards);
            priceCards.Add(libraryPriceCards);
            priceCards.Add(installedPriceCards);
            priceCards.Add(searchPriceCards);
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
        public Panel menu = new Panel();
        public Panel profilePanel = new Panel();
        public ItemForm itemForm;
        public ListBox list = new ListBox();
    }
}

