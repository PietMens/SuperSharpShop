using System;
using System.Windows.Forms;

namespace SuperSharpShop
{
    public class Item
    {
        private String name;
        private String type;
        private String price;
        private byte[] image;
        private String description;
        private String location;

        public Item(String name, String type, String price, byte[] image, String description, String where)
        {
            Name = name;
            Type = type;
            Price = price;
            Image = image;
            Description = description;
            Location = where;
        }

        public void setItem()
        {
            Panel panel;
            if (Location.ToLower() == "shop") {
                panel = Program.App.storePanel;
            } else if (Location.ToLower() == "library") {
                panel = Program.App.ownedPanel;
            } else if (Location.ToLower() == "installed")
            {
                panel = Program.App.installedPanel;
            } else {
                panel = new Panel();
            }
            Program.App.setItem(panel, new GroupBox(), Name, Description, Price, Image, Type);
        }


        public string Location
        {
            get => location;
            set => location = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Type
        {
            get => type;
            set => type = value;
        }

        public String Price
        {
            get => price;
            set => price = value;
        }

        public byte[] Image
        {
            get => image;
            set => image = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }
    }
}