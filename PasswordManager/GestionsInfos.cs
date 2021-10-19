namespace PasswordManager
{
    public class GestionsInfos
    {
        public string URL { get; set; }
        public string Mail { get; set; }
        public string Utilisateur { get; set; }
        public string MDP { get; set; }

        public GestionsInfos() { }

        public GestionsInfos(string _URL, string _Mail, string _Utilisateur, string _MDP)
        {
            URL = _URL;
            Mail = _Mail;
            Utilisateur = _Utilisateur;
            MDP = _MDP;
        }
    }
}
