using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Categorie
    {
        public int Id { get; }

        public string Naam { get; set; }
        public int? ParentCategorieId { get; set; }
        public string Beschrijving { get; set; }

        public Categorie()
        {
            Beschrijving = string.Empty;
        }

        public Categorie(string naam, int? parentCategorieId, string beschrijving)
        {
            Naam = naam;
            ParentCategorieId = parentCategorieId;
            Beschrijving = beschrijving ?? string.Empty;
        }
    }
}
