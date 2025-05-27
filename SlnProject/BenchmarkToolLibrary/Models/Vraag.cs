using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Vraag
    {
        public int Id { get; }

        public int CategorieId { get; set; }

        private string _vraagTekst;
        public string VraagTekst
        {
            get => _vraagTekst;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                _vraagTekst = value;
            }
        }

        private string _typeVraag = "open";
        public string TypeVraag
        {
            get => _typeVraag;
            set => _typeVraag = value ?? "open";
        }

        public Vraag()
        {
            _typeVraag = "open";
        }

        public Vraag(int categorieId, string vraagTekst, string typeVraag)
        {
            CategorieId = categorieId;
            VraagTekst = vraagTekst;
            TypeVraag = typeVraag ?? "open";
        }
    }
}
