using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordManager
{
    public class GestionsInfos
    {
        private string m_Site;
        private string m_Mail;
        private string m_Utilisateur;
        private string m_Mdp;

        public GestionsInfos()
        {

        }

        public GestionsInfos(string _Site, string _Mail, string _Utilisateur, string _MDP)
        {
            m_Site = _Site;
            m_Mail = _Mail;
            m_Utilisateur = _Utilisateur;
            m_Mdp = _MDP;
        }
       
        public string URL
        {
            get
            {
                return m_Site;
            }
            set
            {
                m_Site = value;
            }
        }

        public string Mail
        {
            get
            {
                return m_Mail;
            }
            set
            {
                m_Mail = value;
            }
        }

        public string Utilisateur
        {
            get
            {
                return m_Utilisateur;
            }
            set
            {
                m_Utilisateur = value;
            }
        }

        public string MDP
        {
            get
            {
                return m_Mdp;
            }
            set
            {
                m_Mdp = value;
            }
        }
    }
}
