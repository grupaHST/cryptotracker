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

        public static string ExchangeRates(Language lang) => lang switch
        {
            Language.Polski => "Kursy walut",
            _ => "Exchange rates"
        };

        public static string CurrencyConverter(Language lang) => lang switch
        {
            Language.Polski => "Przelicznik",
            _ => "Converter"
        };

        public static string AboutApp(Language lang) => lang switch
        {
            Language.Polski => "O aplikacji",
            _ => "About app"
        };

        public static string Description(Language lang) => lang switch
        {
            Language.Polski => "Cryptotracker to aplikacja biznesowa umożliwiająca śledzenie aktualnych kursów walut i kryptowalut.",
            _ => "Cryptotracker is a business application that allows you to track the current exchange rates and cryptocurrencies."
        };

        public static string ApiDescription(Language lang) => lang switch
        {
            Language.Polski => "Zewnętrzne API z których korzystamy",
            _ => "External API which we use"
        };        
        public static string Developers(Language lang) => lang switch
        {
            Language.Polski => "Twórcy aplikacji",
            _ => "Application developers"
        };

        public static string Github(Language lang) => lang switch
        {
            Language.Polski => "Zobacz nasz projekt na Github'ie",
            _ => "See our project on Github website"
        };        
        
        public static string NumberConversionError(Language lang) => lang switch
        {
            Language.Polski => "Oczekiwano liczby",
            _ => "Number expected"
        };        
        
        public static string ApiKeys(Language lang) => lang switch
        {
            Language.Polski => "Klucze do API",
            _ => "API Keys"
        };

        public static string ChartTitle(Language lang) => lang switch
        {
            Language.Polski => "Wykres kursu",
            _ => "Stock Chart"
        };

        public static string YLabel(Language lang) => lang switch
        {
            Language.Polski => "Cena waloru",
            _ => "Stock Price"
        };
        
        public static string Notifications(Language lang) => lang switch
        {
            Language.Polski => "Powiadomienia",
            _ => "Notifications"
        };        
        
        public static string AddCurrencyNotification(Language lang) => lang switch
        {
            Language.Polski => "Dodaj powiadomienie dla walut",
            _ => "Add currency notification"
        };

        public static string AddCryptocurrencyNotification(Language lang) => lang switch
        {
            Language.Polski => "Dodaj powiadomienie dla kryptowalut",
            _ => "Add cryptocurrency notification"
        };        
        
        public static string NotificationService(Language lang) => lang switch
        {
            Language.Polski => "Serwis powiadomienia",
            _ => "Notification service"
        };        
        
        public static string NotificationServiceMessage(Language lang) => lang switch
        {
            Language.Polski => "Aby wybrać inny, zmień serwis w ustawieniach",
            _ => "To select a different one, change the service in the settings"
        };

        public static string ThresholdValue(Language lang) => lang switch
        {
            Language.Polski => "Wartość progowa",
            _ => "Threshold value"
        };
        
        public static string ComparisonOperator(Language lang) => lang switch
        {
            Language.Polski => "Wybierz operator porównania",
            _ => "Choose a comparision operator"
        };
        
        public static string NotificationTitle(Language lang) => lang switch
        {
            Language.Polski => "Dzięki aplikacji Cryptotracker możesz otrzymać powiadomienie gdy wybrana przez ciebie waluta osiągnie podany próg",
            _ => "With the Cryptotracker application, you can receive a notification of the currency of your choice reaching the threshold"
        };
    }
}
