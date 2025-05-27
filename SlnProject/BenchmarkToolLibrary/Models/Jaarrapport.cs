using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Jaarrapport
    {
        public int Id { get; }

        public int BedrijfId { get; set; }
        public int Jaar { get; set; }
        public DateTime Rapportdatum { get; set; }

        private string _status = "concept";
        public string Status
        {
            get => _status;
            set => _status = value ?? "concept";
        }

        public Jaarrapport()
        {
            _status = "concept";
            Rapportdatum = DateTime.Now;
        }

        public Jaarrapport(int bedrijfId, int jaar, DateTime rapportdatum, string status)
        {
            BedrijfId = bedrijfId;
            Jaar = jaar;
            Rapportdatum = rapportdatum;
            Status = status ?? "concept";
        }
    }
}
