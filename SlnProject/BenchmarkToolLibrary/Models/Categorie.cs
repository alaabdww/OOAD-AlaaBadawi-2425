// Categorie.cs
// -------------
// Dit model stelt een categorie voor in het BenchmarkTool-systeem.
// Categorieën worden gebruikt om kosten, vragen of andere entiteiten te groeperen (bijv. "ICT", "Personeel", ...).
// Ondersteunt hiërarchieën via ParentCategorieId (subcategorieën).

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Categorie
    {
        // Unieke identificatie van de categorie (primary key in de database)
        public int Id { get; private set; }

        // Naam van de categorie (bv. "ICT", "Personeel", ...)
        public string Naam { get; set; }

        // Optionele ParentCategorieId: verwijst naar bovenliggende categorie (voor hiërarchie/nesting)
        public int? ParentCategorieId { get; set; }

        // Korte beschrijving van de categorie (optioneel, nooit null)
        public string Beschrijving { get; set; }

        /// <summary>
        /// Parameterloze constructor (nodig voor data-binding of om een lege categorie aan te maken).
        /// </summary>
        public Categorie()
        {
            Beschrijving = string.Empty;
        }

        /// <summary>
        /// Constructor zonder Id (meestal voor nieuwe categorieën die nog niet in de database staan).
        /// </summary>
        public Categorie(string naam, int? parentCategorieId, string beschrijving)
        {
            Naam = naam;
            ParentCategorieId = parentCategorieId;
            Beschrijving = beschrijving ?? string.Empty;
        }

        /// <summary>
        /// Constructor met Id (meestal gebruikt bij ophalen uit de database).
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
