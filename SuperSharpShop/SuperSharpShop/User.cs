using System;

namespace SuperSharpShop
{
    public class User
    {
        private int id;
        private String name;
        private String email;
        
        public User(int id, String name, String email)
        {
            Id = id;
            Name = name;
            Email = email;
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
    }
}