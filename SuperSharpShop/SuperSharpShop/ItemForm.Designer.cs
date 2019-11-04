using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SuperSharpShop
{
    partial class ItemForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        public void setTextBox(TextBox box, String name)
        {
            box.Name = name;
            controls.Add(box);
            box.BorderStyle = BorderStyle.FixedSingle;
            int height = 0;
            for (int i = 0; i < controls.IndexOf(box); i++)
            {
                Control control = controls[i];
                Console.WriteLine($"{box.Name}: {control.Name}: {height}");
                height = height + control.Size.Height + 25;
            }
            box.Location = new Point(100, 75 + height);
            this.Controls.Add(box);
            setLabel(box);
        }

        public void setLabel(Control box)
        {
            Label label = new Label();
            label.Text = box.Name;
            label.Location = new Point(20, box.Location.Y);
            label.Size = new Size(75, 21);
            this.Controls.Add(label);
        }

        public void setComboBox(ComboBox box, String name)
        {
            box.Name = name;
            controls.Add(box);
            box.FlatStyle = FlatStyle.Flat;
            int height = 0;
            for (int i = 0; i < controls.IndexOf(box); i++)
            {
                Control control = controls[i];
                Console.WriteLine($"{box.Name}: {control.Name}: {height}");
                height = height + control.Size.Height + 25;
            }
            box.Location = new Point(100, 75 + height);
            this.Controls.Add(box);
            setLabel(box);
        }

        public void Browse(object sender, EventArgs e)
        {
            browser = new OpenFileDialog();
            browser.ShowDialog();
            if (browser.FileName.Trim() != "")
            {
                image.Text = browser.FileName;
            }
        }

        public void Save(object sender, EventArgs e)
        {
            bool check = false;
            byte[] photo = new byte[] {};
            String message = "";
            String sql = "INSERT INTO items (name, type, description, image, price) VALUES (";
            foreach (Control control in controls)
            {
                if (control.Text.Trim() == "")
                {
                    check = true;
                    message += $"{control.Name} is empty\n";
                }
                else
                {
                    String text = "";
                    if (control.Name == "Image")
                    {
                        photo = File.ReadAllBytes(control.Text);
                        text = "@Content";
                        // UPDATE `items` SET `image` = (SELECT image FROM (SELECT * FROM `items`) AS `tablie` WHERE name = 'cdijvi') WHERE ID = 4;
                    }
                    else
                    {
                        text = $"'{control.Text}'";
                    }
                    if (controls.IndexOf(control) == 0) {
                        sql += $"{text}";
                    }
                    else
                    {
                        sql += $", {text}";
                    }
                    Console.WriteLine(sql);
                }
            }
            sql += ");";
            if (check)
            {
                MessageBox.Show(message);
                this.BringToFront();
            }
            else
            {
                Program.conn.Open();
                MySqlCommand command = new MySqlCommand(sql, Program.conn);
                MySqlParameter param = command.Parameters.Add("@Content", MySqlDbType.VarBinary);
                param.Value = photo;
                command.ExecuteNonQuery();
                Program.conn.Close();
                Program.setItems(Program.App.lastPanel);
                Program.App.setComboBox();
                this.Close();
            }
        }

        public void CloseForm(object sender, EventArgs e)
        {
            Program.App.itemForm = null;
        }
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Text = "Add Item";
            Console.WriteLine(this.Text);
            this.BackColor = ColorTranslator.FromHtml("#858585");
            this.CenterToScreen();
            this.Controls.Add(image);
            this.Controls.Add(ImageButton);
            this.Controls.Add(price);
            this.Controls.Add(saveButton);
            setTextBox(name, "Name");
            setComboBox(type, "Type");
            setTextBox(decription, "Description");
            image.Location = new Point(100, 290);
            image.Name = "Image";
            image.Size = new Size(200, 30);
            ImageButton.Location = new Point(image.Location.X + image.Size.Width + 20, image.Location.Y);
            ImageButton.Size = new Size(60, 21);
            ImageButton.Text = "Browse";
            ImageButton.Name = "Browse";
            ImageButton.ForeColor = Color.Honeydew;
            ImageButton.Font = new Font("Arial Black", 7);
            ImageButton.Click += new EventHandler(Browse);
            ImageButton.BackColor = ColorTranslator.FromHtml("#555555");
            setLabel(image);
            controls.Add(image);
            decription.Multiline = true;
            type.Text = "Game";
            type.Items.Add("Game");
            type.Items.Add("Film");
            name.Size = new Size(200, 30);
            type.Size = new Size(200, 30);
            decription.Size = new Size(200, 100);
            price.Size = new Size(200, 30);
            price.Location = new Point(100, 340);
            price.DecimalPlaces = 2;
            price.Name = "Price";
            setLabel(price);
            controls.Add(price);
            saveButton.Size = new Size(60, 30);
            saveButton.Location = new Point(170, 385);
            saveButton.Text = "Save";
            saveButton.Name = "Save";
            saveButton.BackColor = ColorTranslator.FromHtml("#555555");
            saveButton.ForeColor = Color.Honeydew;
            saveButton.Font = new Font("Arial Black", 7);
            saveButton.Click += new EventHandler(Save);
            this.Closing += new CancelEventHandler(CloseForm);
        }

        #endregion
        
        public TextBox name = new TextBox();
        public ComboBox type = new ComboBox();
        public TextBox decription = new TextBox();
        public TextBox image = new TextBox();
        public NumericUpDown price = new NumericUpDown();
        public Button ImageButton = new Button();
        public Button saveButton = new Button();
        public List<Control> controls = new List<Control>();
        public OpenFileDialog browser;

    }
}