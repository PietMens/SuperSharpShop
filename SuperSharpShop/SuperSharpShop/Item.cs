using System;
using System.Windows.Forms;

namespace SuperSharpShop
{
    public class Item
    {
        private String name;
        private String type;
        private float price;
        private String imagePath;
        private String description;

        public Item(String name, String type, float price, String imagePath, String description, String where)
        {
            Name = name;
            Type = type;
            Price = price;
            ImagePath = imagePath;
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
            Program.App.setItem(panel, new GroupBox(), Name, Description, Price, ImagePath);
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

        public float Price
        {
            get => price;
            set => price = value;
        }

        public string ImagePath
        {
            get => imagePath;
            set => imagePath = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }
    }
}