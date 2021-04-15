namespace Cryptotracker.Languages
{
    /*
     * W przypadku dodania nowego języka w aplikacji dodać do każdej funkcji linijkę ze zwrotem w nowym języku.
     * Tworzyć tylko funkcje statyczne!
     */

    /// <summary>
    /// Klasa przechowująca wszystkie napisy aplikacji. Domyślny język to <see cref="Language.English"/>
    /// </summary>
    public static class AppMessages
    {
        public static string Welcome(Language lang) => lang switch
        {
            Language.Polski => "Witamy w Cryptotrackerze !!!",
            _ => "Welcome to the Cryptotracker !!!"
        };

        public static string Settings(Language lang) => lang switch
        {
            Language.Polski => "Ustawienia",
            _ => nameof(Settings)
        };

        public static string Nbp(Language lang) => lang switch
        {
            Language.Polski => "Narodowy Bank Polski",
            _ => "The National Bank of Poland"
        };

        public static string SelectDate(Language lang) => lang switch
        {
            Language.Polski => "Wybierz datę",
            _ => "Select a date"
        };       
        
        public static string Currency(Language lang) => lang switch
        {
            Language.Polski => "Waluta",
            _ => "Currency"
        };

        public static string Download(Language lang) => lang switch
        {
            Language.Polski => "Pobierz",
            _ => "Download"
        };

        public static string StartDate(Language lang) => lang switch
        {
            Language.Polski => "Data początkowa",
            _ => "Start date"
        };
        
        public static string EndDate(Language lang) => lang switch
        {
            Language.Polski => "Data końcowa",
            _ => "End date"
        };
    }
}
