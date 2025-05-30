// BenchmarkResultaat.cs
// ---------------------
// Deze klasse stelt een benchmarkresultaat voor, met een categorie (zoals Personeel, ICT, ...)
// en de bijhorende waarde. Wordt gebruikt om resultaten per categorie op te slaan of weer te geven.

namespace BenchmarkToolLibrary.Data
{
    public class BenchmarkResultaat
    {
        public string CategorieNaam { get; set; }   // Naam van de categorie (bijv. Personeel, ICT, ...)
        public double Waarde { get; set; }          // De waarde/score voor deze categorie
    }
}
