using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Antwoord
    {
        public int Id { get; }

        public int RapportId { get; set; }
        public int VraagId { get; set; }

        private string _antwoordTekst;
        public string AntwoordTekst
        {
            get => _antwoordTekst;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                _antwoordTekst = value;
            }
        }

        public Antwoord()
        {
            AntwoordTekst = string.Empty;
        }

        public Antwoord(int rapportId, int vraagId, string antwoordTekst)
        {
            RapportId = rapportId;
            VraagId = vraagId;
            AntwoordTekst = antwoordTekst;
        }
    }
}
