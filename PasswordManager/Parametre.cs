﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Properties.Settings.Default.CleSecrete = txtCle.Text;
            Properties.Settings.Default.Save();
        }
    }
}
