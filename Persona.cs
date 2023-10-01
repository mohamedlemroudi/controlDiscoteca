using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDiscoteca
{
    public class Persona
    {
        // Propietats
        private string nom { get; set; }
        private int tempsCua { get; set; }
        private int tempsDintra { get; set; }
        private string urlFoto { get; set; }
        private int Id { get; }

        // Constructor
        public Persona(string nom, int tempsCua, int tempsDintra, string urlFoto)
        {
            this.Id = DateTime.Now.Millisecond;
            this.nom = nom;
            this.tempsCua = tempsCua;
            this.tempsDintra = tempsDintra;
            this.urlFoto = urlFoto;
        }

        // Metodes GETS
        public int getId()
        {
            return Id;
        }

        public string getNom()
        {
            return nom;
        }

        public int getTempsCua()
        { 
            return tempsCua; 
        }

        public int getTempsDintra() 
        {
            return tempsDintra;
        }

        public string getUrlFoto()
        {
            return urlFoto;
        }
    }
}
