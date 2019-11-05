using System.ComponentModel;
using System.Drawing;

namespace SuperSharpShop
{
    partial class PayForm
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

        public double Price
        {
            get => price;
            set => price = value;
        }

        public Item Item
        {
            get => item;
            set => item = value;
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
            this.ClientSize = new System.Drawing.Size(400, 500);
            this.Text = "Pay";
            this.BackColor = Color.DimGray;
            this.CenterToScreen();
        }

        #endregion

        private double price;
        private Item item;
    }
}