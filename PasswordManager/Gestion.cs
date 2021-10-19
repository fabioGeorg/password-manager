using System;
using System.Windows.Forms;

namespace PasswordManager
{
    public partial class Gestion : Form
    {
        private bool m_readOnly;
        public GestionsInfos GestionInfo { get; set; }

        public Gestion(bool readOnly)
        {
            InitializeComponent();

            if (readOnly)
            {
                txtSite.ReadOnly = true;
                txtMail.ReadOnly = true;
                txtUtilisateur.ReadOnly = true;
                txtMDP.ReadOnly = true;
            }
            else
            {
                txtSite.ReadOnly = false;
                txtMail.ReadOnly = false;
                txtUtilisateur.ReadOnly = false;
                txtMDP.ReadOnly = false;
            }
            m_readOnly = readOnly;
        }

        private void Gestion_Load(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            if (GestionInfo != null)
            {
                txtSite.Text = GestionInfo.URL;
                txtMail.Text = GestionInfo.Mail;
                txtUtilisateur.Text = GestionInfo.Utilisateur;
                txtMDP.Text = GestionInfo.MDP;
            }
            else
            {
                txtSite.Text = "";
                txtMail.Text = "";
                txtUtilisateur.Text = "";
                txtMDP.Text = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!m_readOnly)
                GestionInfo = new GestionsInfos(txtSite.Text, txtMail.Text, txtUtilisateur.Text, txtMDP.Text);
        }

        private void txtSite_TextChanged(object sender, EventArgs e)
        {
            OnChange();
            if (txtSite.Text != "")
                errorProvider1.SetError(txtSite, "");
            else
                errorProvider1.SetError(txtSite, "Veuillez renseignez un site");
        }

        private void txtMail_TextChanged(object sender, EventArgs e)
        {
            OnChange();
            if (txtMail.Text != "")
                errorProvider1.SetError(txtMail, "");
            else
                errorProvider1.SetError(txtMail, "Veuillez renseignez un mail");
        }

        private void txtUtilisateur_TextChanged(object sender, EventArgs e)
        {
            OnChange();
            if (txtUtilisateur.Text != "")
                errorProvider1.SetError(txtUtilisateur, "");
            else
                errorProvider1.SetError(txtUtilisateur, "Veuillez renseignez un utilisateur");
        }

        private void txtMDP_TextChanged(object sender, EventArgs e)
        {
            OnChange();
            if (txtMDP.Text != "")
                errorProvider1.SetError(txtMDP, "");
            else
                errorProvider1.SetError(txtMDP, "Veuillez renseignez un MDP");
        }

        private void OnChange()
        {
            if (txtSite.Text != "" && txtMail.Text != "" && txtUtilisateur.Text != "" && txtMDP.Text != "")
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }
    }
}
