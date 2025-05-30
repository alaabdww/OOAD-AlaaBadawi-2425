// Nacecode.cs
// -----------
// Dit model stelt een NACE-code voor, een Europese classificatiecode voor economische activiteiten.
// Elke Nacecode heeft een unieke code (bv. "62.01"), een (korte) beschrijving en optioneel een parentcode voor hiërarchie.

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Nacecode
    {
        // Unieke code van de NACE-activiteit (bv. "62.01")
        public string Code { get; private set; }

        // Beschrijving van de code (max 200 tekens)
        private string _beschrijving = string.Empty;
        public string Beschrijving
        {
            get { return _beschrijving; }
            set
            {
                // Beschrijving mag maximaal 200 tekens lang zijn
                if (value != null && value.Length > 200)
                    throw new ArgumentException("Beschrijving mag niet langer zijn dan 200 tekens.");
                _beschrijving = value ?? string.Empty;
            }
        }

        // Code van de bovenliggende NACE-categorie (voor hiërarchische structuur), of null
        public string ParentCode { get; set; }

        /// <summary>
        /// Parameterloze constructor (voor bv. data-binding of nieuwe Nacecode aan te maken).
        /// </summary>
        public Nacecode()
        {
            _beschrijving = string.Empty;
            ParentCode = null;
        }

        /// <summary>
        /// Constructor voor volledige initialisatie van een Nacecode.
        /// </summary>
        public Nacecode(string code, string beschrijving, string parentCode)
        {
            Code = code;
            Beschrijving = beschrijving;
            ParentCode = parentCode;
        }
    }
}
