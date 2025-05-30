// Jaarrapport.cs
// --------------
// Dit model stelt een jaar-rapport voor van een bedrijf in de BenchmarkTool.
// Elk rapport bevat een bedrijfsId, jaar, rapportdatum en status (bijvoorbeeld 'concept', 'afgerond', ...).

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Jaarrapport
    {
        // Unieke identificatie van het rapport (primary key)
        public int Id { get; private set; }

        // Id van het bedrijf waarvoor dit rapport geldt
        public int BedrijfId { get; set; }

        // Jaar van het rapport (bijv. 2024)
        public int Jaar { get; set; }

        // Datum waarop het rapport is aangemaakt
        public DateTime Rapportdatum { get; set; }

        // Status van het rapport ("concept", "afgerond", ...)
        private string _status = "concept";
        public string Status
        {
            get { return _status; }
            set { _status = value ?? "concept"; }
        }

        /// <summary>
        /// Parameterloze constructor (standaard status is 'concept', en rapportdatum is nu)
        /// </summary>
        public Jaarrapport()
        {
            _status = "concept";
            Rapportdatum = DateTime.Now;
        }

        /// <summary>
        /// Constructor voor toevoegen van een nieuw rapport (zonder Id).
        /// </summary>
        public Jaarrapport(int bedrijfId, int jaar, DateTime rapportdatum, string status)
        {
            BedrijfId = bedrijfId;
            Jaar = jaar;
            Rapportdatum = rapportdatum;
            Status = status ?? "concept";
        }

        /// <summary>
        /// Constructor voor ophalen uit de database (met Id).
        /// </summary>
        public Jaarrapport(int id, int bedrijfId, int jaar, DateTime rapportdatum, string status)
        {
            Id = id;
            BedrijfId = bedrijfId;
            Jaar = jaar;
            Rapportdatum = rapportdatum;
            Status = status ?? "concept";
        }
    }
}
