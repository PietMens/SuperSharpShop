using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SuperSharpShop
{
    partial class AuthForm
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

        public void ColorSwitch()
        {
            Console.WriteLine(lastPanel.Name);
            if (lastPanel.Name == "Login")
            {
                loginPanelButton.BackColor = ColorTranslator.FromHtml("#454545");
                registerPanelButton.BackColor = Color.DimGray;
            } else if (lastPanel.Name == "Register")
            {
                registerPanelButton.BackColor = ColorTranslator.FromHtml("#454545");
                loginPanelButton.BackColor = Color.DimGray;
            }
        }
        
        public void panelSwitch(object sender, EventArgs e)
        {
            Button button = (Button)sender;
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
            Panel Panel1 = panels[index];
            if (this.Controls.Count > 2)
            {
                lastPanel = Panel1;
                this.Controls.RemoveAt(2);
            }
            this.Controls.Add(Panel1);
            this.Text = Panel1.Text;
            ColorSwitch();
        }
        
        private void KeyEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (lastPanel.Name == "Login")
                {
                    Login(sender, new EventArgs());
                } 
                else if (lastPanel.Name == "Register")
                {
                    Register(sender, new EventArgs()); 
                }
            }
        }

        public String Hash(String password)
        {
            String result = "";
            List<Char> list = new List<Char>(new []{'-', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'});
            foreach (Char character in password)
            {
                if (list.Contains(character))
                {
                    String hash = $"{list.IndexOf(character)}";
                    if (hash.Length > 1)
                    {
                        if (Char.IsUpper(character))
                        {
                            hash = $"1{hash}";
                        }
                        else
                        {
                            hash = $"0{hash}";
                        }
                    }
                    else
                    {
                        if (Char.IsUpper(character))
                        {
                            hash = $"10{hash}";
                        }
                        else
                        {
                            hash = $"00{hash}";
                        }
                    }
                    result += hash;
                }
            }
            return result;
        }

        public void Login(object sender, EventArgs e)
        {
            String username = loginUsername.Text;
            String password = loginPassword.Text;
            if (username != "" && password != "")
            {
                String hashed = Hash(password);
                if (hashed.Length / 3 == password.Length)
                {
                    Program.conn.Open();
                    String sql = $"SELECT * FROM users WHERE username = '{username}';";
                    MySqlCommand command = new MySqlCommand(sql, Program.conn);
                    var da = new MySqlDataAdapter(command);
                    var ds = new DataSet();
                    da.Fill(ds, "users");
                    Program.conn.Close();
                    if (ds.Tables["users"].Rows.Count == 1)
                    {
                        DataRow row = ds.Tables["users"].Rows[0];
                        if (hashed == row[2].ToString())
                        {
                            Program.userId = int.Parse(row[0].ToString());
                            Program.getUser();
                            Program.conn.Close();
                            Program.App = new Form1();
                            Program.setItems(Program.App.lastPanel);
                            Program.App.setComboBox();
                            this.Close();
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(Program.App);
                        }
                    }
                }
            }
        }
        
        
        public void Register(object sender, EventArgs e)
        {
            String username = registerUsername.Text;
            String password = registerPassword.Text;
            String email = registerEmail.Text;
            if (username != "" && password != "" && email != "")
            {
                String hashed = Hash(password);
                if (hashed.Length / 3 == password.Length)
                {
                    Program.conn.Open();
                    String sql = $"SELECT * FROM users WHERE username = '{username}';";
                    MySqlCommand command = new MySqlCommand(sql, Program.conn);
                    var da = new MySqlDataAdapter(command);
                    var ds = new DataSet();
                    da.Fill(ds, "users");
                    Program.conn.Close();
                    if (ds.Tables["users"].Rows.Count < 1)
                    {
                        Program.conn.Open();
                        sql = $"INSERT INTO users (username, password, email) VALUES ('{username}', '{hashed}', '{email}');";
                        command = new MySqlCommand(sql, Program.conn);
                        command.ExecuteNonQuery();
                        sql = $"SELECT * FROM users WHERE username = '{username}';";
                        command = new MySqlCommand(sql, Program.conn);
                        da = new MySqlDataAdapter(command);
                        ds = new DataSet();
                        da.Fill(ds, "users");
                        Program.conn.Close();
                        if (ds.Tables["users"].Rows.Count == 1)
                        {
                            DataRow row = ds.Tables["users"].Rows[0];
                            Program.userId = int.Parse(row[0].ToString());
                            Program.getUser();
                            Program.conn.Close();
                            Program.App = new Form1();
                            Program.setItems(Program.App.lastPanel);
                            Program.App.setComboBox();
                            this.Close();
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(Program.App);
                        }
                    }
                }
            }
        }
        
        public void setLabel(Panel panel, Control box)
        {
            Label label = new Label();
            label.Text = box.Name;
            label.Location = new Point(100, box.Location.Y);
            label.Size = new Size(75, 21);
            panel.Controls.Add(label);
        }
        
        public void setPanelButton(Button button, String name)
        {
            this.Controls.Add(button);
            button.FlatAppearance.BorderSize = 1;
            button.BackColor = this.BackColor;
            button.ForeColor = Color.Honeydew;
            button.FlatStyle = FlatStyle.System;
            button.Font = new Font("Helvetica Neue", 11);
            button.FlatAppearance.BorderColor = Color.Black;
            button.Size = new Size(this.Size.Width / 2 - 4, 30);
            //Console.WriteLine(this.Controls.IndexOf(button) + " | " + button.Parent.Text);
            button.Location = new Point(this.Controls.IndexOf(button) * button.Size.Width,0);
            button.Name = name;
            button.Text = name;
            //Console.WriteLine($"{button.Name}: X: {button.Location.X}, Y: {button.Location.Y}, Text: {button.Text}");
            button.Click += new EventHandler(panelSwitch);
        }
        
        public void setRegisterPanel()
        {
            registerPanel.Name = "Register";
            setPanelButton(registerPanelButton, registerPanel.Name);
            registerPanel.Size = new Size(this.Size.Width, this.Size.Height - 30);
            registerPanel.Location = new Point(0, 30);
            register.Text = registerPanel.Name;
            register.TextAlign = ContentAlignment.MiddleCenter;
            register.Size = new Size(200, 50);
            register.Location = new Point(150, 50);
            register.Font = new Font("Arial", 20, FontStyle.Bold);
            register.ForeColor = Color.Honeydew;
            registerPanel.Controls.Add(register);
            registerPanel.BackColor = Color.DimGray;
            setTextBox(registerPanel, registerUsername, "Username");
            setTextBox(registerPanel, registerEmail, "Email");
            setTextBox(registerPanel, registerPassword, "Password");
            registerPassword.UseSystemPasswordChar = true;
            setButton(registerPanel, registerButton, "Register");
            registerButton.Click += new EventHandler(Register);
            registerUsername.KeyDown += new KeyEventHandler(KeyEnter);
            registerPassword.KeyDown += new KeyEventHandler(KeyEnter);
        }

        public void setLoginPanel()
        {
            loginPanel.Name = "Login";
            setPanelButton(loginPanelButton, loginPanel.Name);
            loginPanel.Size = new Size(this.Size.Width, this.Size.Height - 30);
            loginPanel.Location = new Point(0, 30);
            login.Text = loginPanel.Name;
            login.TextAlign = ContentAlignment.MiddleCenter;
            login.Size = new Size(200, 50);
            login.Location = new Point(150, 50);
            login.Font = new Font("Arial", 20, FontStyle.Bold);
            login.ForeColor = Color.Honeydew;
            loginPanel.Controls.Add(login);
            loginPanel.BackColor = Color.DimGray;
            setTextBox(loginPanel, loginUsername, "Username");
            setTextBox(loginPanel, loginPassword, "Password");
            loginPassword.UseSystemPasswordChar = true;
            setButton(loginPanel, loginButton, "Login");
            loginButton.Click += new EventHandler(Login);
            loginUsername.KeyDown += new KeyEventHandler(KeyEnter);
            loginPassword.KeyDown += new KeyEventHandler(KeyEnter);
        }
        
        
        public void CloseForm(object sender, EventArgs e)
        {
            //Application.Restart();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Program.App);
        }

        public void setTextBox(Panel panel, TextBox box, String name)
        {
            panel.Controls.Add(box);
            box.Name = name;
            box.Size = new Size(200, 21);
            box.Location = new Point(175, (box.Size.Height * 2) * (panel.Controls.Count / 2) + 100);
            setLabel(panel, box);
        }

        public void setButton(Panel panel, Button button, String name)
        {
            button.Text = name;
            //Console.WriteLine(this.Controls[this.Controls.Count -1] + " | " + button.Size.Width);
            button.Location = new Point((panel.Size.Width - button.Size.Width) / 2 + 20, panel.Controls[panel.Controls.Count -1].Location.Y + 45);
            panel.Controls.Add(button);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        ///
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 550);
            this.Text = "Login";
            this.BackColor = Color.DimGray;
            setLoginPanel();
            setRegisterPanel();
            this.Controls.Add(loginPanel);
            lastPanel = loginPanel;
            ColorSwitch();
            panels.AddRange(new []{registerPanel, loginPanel});
            this.CenterToScreen();
        }

        #endregion
        
        Panel loginPanel = new Panel();
        Button loginPanelButton = new Button();
        Panel registerPanel = new Panel();
        Button registerPanelButton = new Button();
        Panel lastPanel;
        Label login = new Label();
        Label register = new Label();
        TextBox loginUsername = new TextBox();
        TextBox registerUsername = new TextBox();
        TextBox registerEmail = new TextBox();
        TextBox loginPassword = new TextBox();
        TextBox registerPassword = new TextBox();
        Button loginButton = new Button();
        Button registerButton = new Button();
        List<Panel> panels = new List<Panel>();
    }
}