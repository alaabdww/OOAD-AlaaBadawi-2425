// Kost.cs
// --------
// Dit model stelt een enkele kost voor binnen een jaarrapport in het BenchmarkTool-systeem.
// Elke kost is gekoppeld aan een rapport, type, categorie en bevat een bedrag + optionele opmerking.

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Kost
    {
        // Uniek Id (meestal automatisch toegekend door de database)
        public int Id { get; private set; }

        // Id van het rapport waartoe deze kost behoort
        public int RapportId { get; set; }

        // Id van het type kost (bv. vast, variabel, ...)
        public int TypeId { get; set; }

        // Id van de categorie (bv. ICT, personeel, ...)
        public int CategorieId { get; set; }

        // Optionele extra info bij de kost (kan leeg zijn)
        public string Opmerking { get; set; }

        private double _bedrag = 0.0;

        /// <summary>
        /// Het bedrag van de kost. Mag niet negatief zijn.
        /// </summary>
        public double Bedrag
        {
            get { return _bedrag; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Bedrag mag niet negatief zijn.");
                _bedrag = value;
            }
        }

        /// <summary>
        /// Parameterloze constructor. Zet bedrag op 0 en opmerking op leeg.
        /// </summary>
        public Kost()
        {
            _bedrag = 0.0;
            Opmerking = string.Empty;
        }

        /// <summary>
        /// Constructor zonder Id (voor het toevoegen van een nieuwe kost).
        /// </summary>
        public Kost(int rapportId, int typeId, int categorieId, double bedrag, string opmerking)
        {
            RapportId = rapportId;
            TypeId = typeId;
            CategorieId = categorieId;
            Bedrag = bedrag; // Roep de property aan voor validatie!
            Opmerking = opmerking;
        }

        /// <summary>
        /// Constructor met Id (voor ophalen uit database).
        /// </summary>
        public Kost(int id, int rapportId, int typeId, int categorieId, double bedrag, string opmerking)
        {
            Id = id;
            RapportId = rapportId;
            TypeId = typeId;
            CategorieId = categorieId;
            Bedrag = bedrag;
            Opmerking = opmerking;
        }
    }
}
