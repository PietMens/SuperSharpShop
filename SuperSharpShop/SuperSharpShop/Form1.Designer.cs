using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;

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
            Console.WriteLine(e);
            int index = 0;
            int count = 0;
            foreach (Panel panel in this.panels)
            {
                if (panel.Name == button.Name)
                {
                    index = count;
                }
                count = count + 1;
            }
            if (this.Controls.Count > 1)
            {
                this.Controls.RemoveAt(1);
            }
            this.Controls.Add(panels[index]);
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
            
            button.Location = new Point(25, panel.Controls.IndexOf(button) * 40 + 20 * panel.Controls.IndexOf(button));
            button.Size = new Size(200, 50);
            button.Name = name;
            button.Text = name;
            Console.WriteLine($"{button.Name}: X: {button.Location.X}, Y: {button.Location.Y}, Text: {button.Text}");
            button.Click += new EventHandler(panelSwitch);
        }

        public void setPanel(Panel panel, String name)
        {
            panels.Add(panel);
            panel.BackColor = this.BackColor;
            panel.ForeColor = Color.Honeydew;
            panel.Location = new Point(250, 0);
            panel.Size = new Size(Screen.PrimaryScreen.Bounds.Width - 250, Screen.PrimaryScreen.Bounds.Height);
            panel.Name = name;
            panel.Text = name;
        }

        public void setCard(object sender, EventArgs e)
        {
            GroupBox item = (GroupBox) sender;
            int index = item.Parent.Controls.IndexOf(item);
            Label title = titles[index];
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            title.Visible = true;
            priceCard.Visible = true;
        }

        public void removeCard(object sender, EventArgs e)
        {
            GroupBox item = (GroupBox) sender;
            int index = item.Parent.Controls.IndexOf(item);
            Label title = titles[index];
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            title.Visible = false;
            priceCard.Visible = false;
        }
        
        public void setCardChild(object sender, EventArgs e)
        {
            Control child = (Control) sender;
            Control item = child.Parent;
            int index = item.Parent.Controls.IndexOf(item);
            Label title = titles[index];
            Label priceCard = priceCards[index];
            //Console.WriteLine(item.Name + ", " + item.GetType() + " |\t" + title.Text + " |\t" + item.Controls.Contains(title));
            title.Visible = true;
            priceCard.Visible = true;
        }

        public void removeCardChild(object sender, EventArgs e)
        {
            Control child = (Control) sender;
            Control item = child.Parent;
            int index = item.Parent.Controls.IndexOf(item);
            Label title = titles[index];
            Label priceCard = priceCards[index];
            title.Visible = false;
            priceCard.Visible = false;
        }
        
        

        public void setItem(Panel panel, GroupBox item, String name, String description, float price, String image)
        {
            panel.Controls.Add(item);
            item.Name = name;
            item.Size = new Size((Screen.PrimaryScreen.Bounds.Width - 450) / 3, 250);
            item.Location = new Point(50 + panel.Controls.IndexOf(item) % 3 * (item.Size.Width + 50),panel.Controls.IndexOf(item) / 3 * (item.Size.Height + 25) + 25);
            item.SuspendLayout();
            item.TabIndex = 0;
            item.TabStop = false;
            item.ResumeLayout(false);
            item.PerformLayout();
            item.FlatStyle = FlatStyle.Flat;
            Console.WriteLine($"{item.Name}: X: {item.Location.X}, Y: {item.Location.Y}, Width: {item.Size.Width}, Height: {item.Size.Height}");
            Label title = new Label();
            title.Text = name;
            title.Size = new Size(200, 25);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(1, 7);
            title.BackColor = ColorTranslator.FromHtml("#202020");
            item.Controls.Add(title);
            title.Visible = false;
            item.MouseEnter += new EventHandler(setCard);
            item.MouseLeave += new EventHandler(removeCard);
            titles.Add(title);
            PictureBox pb = new PictureBox();
            pb.Size = new Size(item.Size.Width - 3, item.Size.Height - 100);
            pb.Location = new Point(1, 7);
            pb.BackgroundImage = Image.FromFile(image);
            pb.BackgroundImageLayout = ImageLayout.Stretch;
            item.Controls.Add(pb);
            Label priceCard = new Label();
            priceCard.Name = $"${price}";
            priceCard.Text = $"${price}";
            priceCard.Size = new Size(50, 25);
            priceCard.TextAlign = ContentAlignment.MiddleCenter;
            priceCard.Location = new Point(item.Size.Width - (priceCard.Size.Width + 2), item.Size.Height - (priceCard.Size.Height + 2));
            priceCard.BackColor = ColorTranslator.FromHtml("#202020");
            priceCard.Visible = false;
            priceCards.Add(priceCard);
            item.Controls.Add(priceCard);
            Label descriptionCard = new Label();
            descriptionCard.Name = name;
            descriptionCard.Text = description;
            descriptionCard.TextAlign = ContentAlignment.MiddleLeft;
            descriptionCard.Size = new Size(pb.Size.Width, item.Size.Height - (pb.Size.Height + pb.Location.Y));
            descriptionCard.Location = new Point(pb.Location.X, pb.Size.Height + pb.Location.Y);
            descriptionCard.BackColor = ColorTranslator.FromHtml("#202020");
            item.Controls.Add(descriptionCard);
            Console.WriteLine($"{priceCard.Name}: X: {priceCard.Location.X}, Y: {priceCard.Location.Y}, Width: {priceCard.Size.Width}, Height: {priceCard.Size.Height}");
            foreach (Control child in item.Controls)
            {
                child.MouseEnter += new EventHandler(setCardChild);
                child.MouseLeave += new EventHandler(removeCardChild);
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
            this.Controls.Add(panel);
            this.panel.Size = new Size(250, Screen.PrimaryScreen.Bounds.Height);
            this.panel.BackColor = ColorTranslator.FromHtml("#454545");
            //this.Menu = new MainMenu();
            this.panel.Controls.Add(searchBar);
            this.panel.Controls.Add(searchButton);
            //searchBar.AppendText("Search...");
            searchBar.Location = new Point(20, 50);
            searchBar.Size = new Size(150, 21);
            searchButton.Name = "Search";
            searchButton.Text = "Search";
            searchButton.Size = new Size(55, 21);
            searchButton.Location = new Point(180, 50);
            searchButton.BackColor = ColorTranslator.FromHtml("#858585");
            searchButton.Font = new Font("Helvetica Neue", 7);
            setPanel(storePanel, "Shop");
            setPanel(ownedPanel, "Library");
            setPanel(installedPanel, "Installed");
            setPanelButton(storeButton, "Shop");
            setPanelButton(ownedButton, "Library");
            setPanelButton(installedButton, "Installed");
            //storePanel.BackColor = Color.Aqua;
            //ownedPanel.BackColor = Color.Gold;
            //installedPanel.BackColor = Color.Green;
            this.Controls.Add(storePanel);
        }

        #endregion
        
        public Panel panel = new System.Windows.Forms.Panel();
        public Panel storePanel = new System.Windows.Forms.Panel();
        public Panel ownedPanel = new System.Windows.Forms.Panel();
        public Panel installedPanel = new System.Windows.Forms.Panel();
        public Button storeButton = new System.Windows.Forms.Button();
        public Button ownedButton = new System.Windows.Forms.Button();
        public Button installedButton = new System.Windows.Forms.Button();
        public TextBox searchBar = new System.Windows.Forms.TextBox();
        public Button searchButton = new System.Windows.Forms.Button();
        public List<Panel> panels = new List<Panel>();
        public List<Label> titles = new List<Label>();
        public List<Label> priceCards = new List<Label>();
    }
}

