// Antwoord.cs
// ------------
// Dit model stelt een antwoord voor dat gegeven wordt op een vraag binnen een rapport.
// Elk antwoord bevat een RapportId, VraagId en de eigenlijke tekst van het antwoord.
// Wordt gebruikt voor opslag en verwerking van ingevulde vragenlijsten/rapporten.

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Antwoord
    {
        // Unieke identificatie van het antwoord (optioneel: alleen getter, niet altijd gebruikt)
        public int Id { get; }

        // Het rapport waartoe dit antwoord behoort
        public int RapportId { get; set; }

        // De bijhorende vraag (vraagnummer of id)
        public int VraagId { get; set; }

        // De tekst van het antwoord; mag niet leeg zijn
        private string _antwoordTekst;
        public string AntwoordTekst
        {
            get => _antwoordTekst;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Antwoordtekst mag niet leeg zijn.");
                _antwoordTekst = value;
            }
        }

        // Standaard constructor: initialiseer AntwoordTekst met lege string
        public Antwoord()
        {
            AntwoordTekst = string.Empty;
        }

        // Constructor met parameters voor rapport, vraag en tekst
        public Antwoord(int rapportId, int vraagId, string antwoordTekst)
        {
            RapportId = rapportId;
            VraagId = vraagId;
            AntwoordTekst = antwoordTekst; // Zet via setter, dus validatie wordt uitgevoerd
        }
    }
}
