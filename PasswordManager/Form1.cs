using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Data;
using System.Drawing;

namespace PasswordManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<GestionsInfos> GestionsInfosListe;
        bool saved;
        string fileName;
        private void Form1_Load(object sender, EventArgs e)
        {
            btnModifier.Enabled = false;
            btnSupprimer.Enabled = false;
            modifierToolStripMenuItem1.Enabled = false;
            supprimerToolStripMenuItem1.Enabled = false;
            this.KeyPreview = true;

            datas.AutoGenerateColumns = false;
            datas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            datas.RowHeadersVisible = false;
            datas.MultiSelect = false;
            datas.BorderStyle = BorderStyle.None;
            datas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(119, 175, 206);

            GestionsInfosListe = new List<GestionsInfos>();
            bindingSource1.DataSource = GestionsInfosListe;
            datas.DataSource = bindingSource1;

            saved = false;

        }
        
        private void exporterEnCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = ".csv";
            saveFileDialog1.Filter = "(*.csv)|*.csv|(*.*)|*.*";
            saveFileDialog1.Title = "Enregistrer sous ...";
            saveFileDialog1.FileName = "untitled";
            saveFileDialog1.RestoreDirectory = true;
            if (datas.Rows.Count > 0)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    exporter(fileName, GestionsInfosListe);
                    saved = true;
                }
            }
            else
            {
                MessageBox.Show("Il n'y a aucune donnée à exporter", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    private void datas_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowCount > 0)
            {
                btnModifier.Enabled = true;
                btnSupprimer.Enabled = true;
                modifierToolStripMenuItem1.Enabled = true;
                supprimerToolStripMenuItem1.Enabled = true;
            }

            NbElementStatusLabel.Text = Convert.ToString(GestionsInfosListe.Count) + " élement(s)";
            saved = false;
        }

        private void datas_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (e.RowCount == 0)
            {
                btnModifier.Enabled = false;
                btnSupprimer.Enabled = false;
                modifierToolStripMenuItem1.Enabled = false;
                supprimerToolStripMenuItem1.Enabled = false;
            }
            NbElementStatusLabel.Text = Convert.ToString(GestionsInfosListe.Count) + " élement(s)";
            saved = false;
        }

        private void datas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (Gestion g = new Gestion(true))
            {
                g.GestionInfo = GetSelectedGestionsInfos(datas.SelectedRows[0]);
                g.ShowDialog();
            }
        }

        private void importerCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "CSV (*.csv)|*.csv";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                importer(fileName);
                saved = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(datas.Rows.Count > 0 && !saved)
            {
                if (MessageBox.Show("Voulez-vous quitter?" + Environment.NewLine + "(Vos données non sauvegardés seront perdus)", Application.ProductName, MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && (e.KeyCode == Keys.N))
            {
                btnAjouter.PerformClick();
            }

            if(e.Control && (e.KeyCode == Keys.M))
            {
                if(datas.Rows.Count == 0)
                {
                    MessageBox.Show("Aucune ligne n'est sélectionné", Application.ProductName, MessageBoxButtons.OK);
                }
                else
                {
                    btnModifier.PerformClick();
                }
            }

            if(e.KeyCode == Keys.Delete)
            {
                if (datas.Rows.Count == 0)
                {
                    MessageBox.Show("Aucune ligne n'est sélectionné", Application.ProductName, MessageBoxButtons.OK);
                }
                else
                {
                    btnSupprimer.PerformClick();
                }
            }

            if(e.Control && (e.KeyCode == Keys.S))
            {
                exporterEnCSVToolStripMenuItem.PerformClick();
            }

            //if(e.Control && (e.KeyCode == Keys.F))
            //{
            //    Recherche();
            //}
        }

        private void parametresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(Parametre p = new Parametre())
            {
                p.ShowDialog();
            }
        }

        private void btnApropos_Click(object sender, EventArgs e)
        {
            using (About about = new About())
            {
                about.ShowDialog();
            }
        }

        private void datas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if(e.ColumnIndex == 0)
            {
                //MessageBox.Show(datas[0, e.RowIndex].Value.ToString());
                try
                {
                    Process.Start();
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }*/
        }

        #region " regions "
        #region " Clic droit "
        private void ajouterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Gestion g = new Gestion(false))
            {
                if (g.ShowDialog() == DialogResult.OK)
                {
                    GestionsInfosListe.Add(g.GestionInfo);
                    bindingSource1.ResetBindings(false);
                }
            }

        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Gestion g = new Gestion(false))
            {
                g.GestionInfo = GetSelectedGestionsInfos(datas.SelectedRows[0]);
                int indexGestion = datas.SelectedRows[0].Index;
                GestionsInfos tmp = g.GestionInfo;
                if (g.ShowDialog() == DialogResult.OK)
                {
                    GestionsInfosListe.Remove(tmp);
                    GestionsInfosListe.Insert(indexGestion, g.GestionInfo);
                    bindingSource1.ResetBindings(false);
                }
            }
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionsInfos tmp = GetSelectedGestionsInfos(datas.SelectedRows[0]);
            GestionsInfosListe.Remove(tmp);
            bindingSource1.ResetBindings(false);
        }
        #endregion

        #region " Edition "
        private void ajouterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ajouterToolStripMenuItem_Click(sender, e);
        }

        private void modifierToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            modifierToolStripMenuItem_Click(sender, e);
        }

        private void supprimerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            supprimerToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region "Fonctions custom"
        public GestionsInfos GetSelectedGestionsInfos(DataGridViewRow row)
        {
            GestionsInfos tmp = row.DataBoundItem as GestionsInfos;
            return tmp;
        }

        /// <summary>
        /// Permet de sauvegarder une liste de données dans un fichier, au format .csv.
        /// </summary>
        /// <param name="name">Nom du fichier.</param>
        /// <param name="gestions">Liste de GestionsInfos.</param>

        public void exporter(string name, List<GestionsInfos> gestions)
        {
            using (StreamWriter sw = new StreamWriter(name))
            {
                sw.AutoFlush = true;
                sw.WriteLine("Site;Mail;Nom d'utilisateur;Mot de passe;Hash");
                for (int i = 0; i < gestions.Count; i++)
                {
                    sw.WriteLine(gestions[i].URL + ";" + gestions[i].Mail + ";" + gestions[i].Utilisateur + ";" + gestions[i].MDP + ";" + HashMDP(gestions[i].MDP));
                }
            }

            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "Enregistrement";
            notifyIcon1.BalloonTipText = "Vos mots de passes ont bien été enregistrés!";
            notifyIcon1.ShowBalloonTip(1000);
        }

        /// <summary>
        /// Permet d'importer un fichier .csv.
        /// </summary>
        /// <param name="fichier">Fichier à importer.</param>

        public void importer(string fichier)
        {
            int i = 0;
            string ligne = "";
            using (StreamReader sr = new StreamReader(fichier))
            {
                while ((i = sr.Peek()) > -1)
                {
                    ligne = sr.ReadLine();
                    if (ligne == "Site;Mail;Nom d'utilisateur;Mot de passe;Hash")
                        continue;
                    string[] splitted = ligne.Split(';');
                    GestionsInfosListe.Add(new GestionsInfos(splitted[0], splitted[1], splitted[2], splitted[3]));
                    bindingSource1.ResetBindings(false);
                }
            }
        }

        /// <summary>
        /// Permet de calculé le hash MD5 d'un mot de passe.
        /// </summary>
        /// <param name="MDP">Hash à calculer</param>
        /// <returns></returns>
        public string HashMDP(string MDP)
        {
            byte[] codeSecret = Encoding.ASCII.GetBytes(Properties.Settings.Default.CleSecrete);
            HMACMD5 md5 = new HMACMD5(codeSecret);
            byte[] hashedPass = md5.ComputeHash(Encoding.UTF8.GetBytes(MDP));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < hashedPass.Length; i++)
            {
                sBuilder.Append(hashedPass[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }


        #endregion

        #region " btns "
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            using (Gestion g = new Gestion(false))
            {
                if (g.ShowDialog() == DialogResult.OK)
                {
                    GestionsInfosListe.Add(g.GestionInfo);
                    bindingSource1.ResetBindings(false);
                }
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            using (Gestion g = new Gestion(false))
            {
                g.GestionInfo = GetSelectedGestionsInfos(datas.SelectedRows[0]);
                int indexGestion = datas.SelectedRows[0].Index;
                GestionsInfos tmp = g.GestionInfo;
                if (g.ShowDialog() == DialogResult.OK)
                {
                    GestionsInfosListe.Remove(tmp);
                    GestionsInfosListe.Insert(indexGestion, g.GestionInfo);
                    bindingSource1.ResetBindings(false);
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            GestionsInfos tmp = GetSelectedGestionsInfos(datas.SelectedRows[0]);
            GestionsInfosListe.Remove(tmp);
            bindingSource1.ResetBindings(false);
        }

        private void txtRecherche_TextChanged(object sender, EventArgs e)
        {
            var result = from infos in GestionsInfosListe
                         where infos.URL.ToLower().Contains(txtRecherche.Text.ToLower()) || infos.Utilisateur.ToLower().Contains(txtRecherche.Text.ToLower())
                         || infos.Mail.ToLower().Contains(txtRecherche.Text) || infos.MDP.ToLower().Contains(txtRecherche.Text.ToLower())
                         select infos;

            List<GestionsInfos> res = result.ToList();
            bindingSource1.DataSource = res;
            datas.DataSource = bindingSource1;
            NbElementStatusLabel.Text = res.Count.ToString() + " élement(s)";
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                datas.DefaultCellStyle.SelectionBackColor = colorDialog1.Color;
        }

        private void themeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Ajouter un éditeur de thèmes
        }
    }

    #endregion

    #endregion
}
