using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Kost
    {
        public int Id { get; private set; }

        public int RapportId { get; set; }
        public int TypeId { get; set; }
        public int CategorieId { get; set; }
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
        /// Parameterloze constructor
        /// </summary>
        public Kost()
        {
            _bedrag = 0.0;
            Opmerking = string.Empty;
        }

        /// <summary>
        /// Constructor zonder Id (voor toevoegen van een nieuwe kost).
        /// </summary>
        public Kost(int rapportId, int typeId, int categorieId, double bedrag, string opmerking)
        {
            RapportId = rapportId;
            TypeId = typeId;
            CategorieId = categorieId;
            Bedrag = bedrag;
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
