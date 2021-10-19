using System;
using System.Windows.Forms;

namespace PasswordManager
{
    public partial class Parametre : Form
    {
        public Parametre()
        {
            InitializeComponent();
        }

        private void Parametre_Load(object sender, EventArgs e)
        {
            txtCle.Text = Properties.Settings.Default.CleSecrete;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Not the safest thing to do.
            Properties.Settings.Default.CleSecrete = txtCle.Text;
            Properties.Settings.Default.Save();
        }
    }
}
