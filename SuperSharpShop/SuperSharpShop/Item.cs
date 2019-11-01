using System;
using System.Windows.Forms;

namespace SuperSharpShop
{
    public class Item
    {
        private String name;
        private String type;
        private double price;
        private byte[] image;
        private String description;

        public Item(String name, String type, double price, byte[] image, String description, String where)
        {
            Name = name;
            Type = type;
            Price = price;
            Image = this.image;
            Description = description;
            Panel panel;
            if (where.ToLower() == "shop") {
                panel = Program.App.storePanel;
            } else if (where.ToLower() == "library") {
                panel = Program.App.ownedPanel;
            } else if (where.ToLower() == "installed")
            {
                panel = Program.App.installedPanel;
            } else {
                panel = new Panel();
            }
            Program.App.setItem(panel, new GroupBox(), Name, Description, $"${Price}", image);
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

        public double Price
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