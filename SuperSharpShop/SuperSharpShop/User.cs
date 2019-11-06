using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SuperSharpShop
{
    public class User
    {
        private int id;
        private String name;
        private String email;
        private int role;
        private bool active;
        
        public User(int id, String name, String email, int role, bool active)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
        }

        public void setFriend(Panel panel)
        {
            List<String> roles = new List<string>(new []{"", "User", "Admin"});
            Program.App.setFriend(panel, new GroupBox(), Id, Name, Email, roles[Role], Active);
        }

        public bool Active
        {
            get => active;
            set => active = value;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public int Role
        {
            get => role;
            set => role = value;
        }
    }
}