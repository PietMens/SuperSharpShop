using System.Windows.Forms;

namespace SuperSharpShop
{
    public partial class PayForm : Form
    {
        public PayForm(double price, Item item)
        {
            Price = price;
            Item = item;
            InitializeComponent();
        }
    }
}