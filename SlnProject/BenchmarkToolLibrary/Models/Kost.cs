using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Kost
    {
        public int Id { get; }

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
            get => _bedrag;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Bedrag mag niet negatief zijn.");
                _bedrag = value;
            }
        }

        public Kost()
        {
            _bedrag = 0.0;
            Opmerking = string.Empty;
        }

        public Kost(int rapportId, int typeId, int categorieId, double bedrag, string opmerking)
        {
            RapportId = rapportId;
            TypeId = typeId;
            CategorieId = categorieId;
            Bedrag = bedrag;
            Opmerking = opmerking;
        }
    }
}
