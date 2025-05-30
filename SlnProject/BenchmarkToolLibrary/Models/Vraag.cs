// Vraag.cs
// --------
// Deze klasse stelt een vraag voor uit de vragenlijst van de BenchmarkTool.
// Elke vraag behoort tot een categorie, heeft een tekst, en een type (standaard 'open').

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Vraag
    {
        // Uniek Id van de vraag (wordt uit de database opgehaald)
        public int Id { get; private set; }

        // Id van de categorie waar deze vraag toe behoort
        public int CategorieId { get; set; }

        // De tekst van de vraag zelf (mag niet leeg zijn)
        private string _vraagTekst;
        public string VraagTekst
        {
            get { return _vraagTekst; }
            set
            {
                // Gooi een fout als de vraagtekst leeg is of alleen whitespace bevat
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vraagtekst mag niet leeg zijn.");
                _vraagTekst = value;
            }
        }

        // Het type vraag (bv. 'open', 'meerkeuze'...). Default is 'open'.
        private string _typeVraag = "open";
        public string TypeVraag
        {
            get { return _typeVraag; }
            set { _typeVraag = value ?? "open"; }
        }

        /// <summary>
        /// Parameterloze constructor (voor binding of aanmaak van een nieuwe lege vraag)
        /// </summary>
        public Vraag()
        {
            _typeVraag = "open";
        }

        /// <summary>
        /// Constructor voor toevoegen van een nieuwe vraag (zonder Id)
        /// </summary>
        public Vraag(int categorieId, string vraagTekst, string typeVraag)
        {
            CategorieId = categorieId;
            VraagTekst = vraagTekst;
            TypeVraag = typeVraag ?? "open";
        }

        /// <summary>
        /// Constructor voor ophalen van een vraag uit de database (met Id)
        /// </summary>
        public Vraag(int id, int categorieId, string vraagTekst, string typeVraag)
        {
            Id = id;
            CategorieId = categorieId;
            VraagTekst = vraagTekst;
            TypeVraag = typeVraag ?? "open";
        }
    }
}
