using System;

namespace ProjectNet
{
    public class Patient
    {
        public int Id { get; set; }
        public string Tel { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Genre { get; set; }
        public DateTime DateNaissance { get; set; }
        public string Adresse { get; set; }
    }
}
