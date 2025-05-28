using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Categorie
    {
        public int Id { get; private set; }

        public string Naam { get; set; }
        public int? ParentCategorieId { get; set; }
        public string Beschrijving { get; set; }

        /// <summary>
        /// Parameterloze constructor (voor bv. nieuwe, lege categorie of voor binding).
        /// </summary>
        public Categorie()
        {
            Beschrijving = string.Empty;
        }

        /// <summary>
        /// Constructor zonder Id (voor toevoegen van nieuwe categorieën).
        /// </summary>
        public Categorie(string naam, int? parentCategorieId, string beschrijving)
        {
            Naam = naam;
            ParentCategorieId = parentCategorieId;
            Beschrijving = beschrijving ?? string.Empty;
        }

        /// <summary>
        /// Constructor met Id (voor ophalen uit database).
        /// </summary>
        public Categorie(int id, string naam, int? parentCategorieId, string beschrijving)
        {
            Id = id;
            Naam = naam;
            ParentCategorieId = parentCategorieId;
            Beschrijving = beschrijving ?? string.Empty;
        }
    }
}
