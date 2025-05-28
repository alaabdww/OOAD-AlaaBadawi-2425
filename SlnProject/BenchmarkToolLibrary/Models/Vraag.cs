using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Vraag
    {
        public int Id { get; private set; }

        public int CategorieId { get; set; }

        private string _vraagTekst;
        public string VraagTekst
        {
            get { return _vraagTekst; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vraagtekst mag niet leeg zijn.");
                _vraagTekst = value;
            }
        }

        private string _typeVraag = "open";
        public string TypeVraag
        {
            get { return _typeVraag; }
            set { _typeVraag = value ?? "open"; }
        }

        /// <summary>
        /// Parameterloze constructor
        /// </summary>
        public Vraag()
        {
            _typeVraag = "open";
        }

        /// <summary>
        /// Constructor voor toevoegen van nieuwe vraag (zonder Id)
        /// </summary>
        public Vraag(int categorieId, string vraagTekst, string typeVraag)
        {
            CategorieId = categorieId;
            VraagTekst = vraagTekst;
            TypeVraag = typeVraag ?? "open";
        }

        /// <summary>
        /// Constructor voor ophalen uit database (met Id)
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
