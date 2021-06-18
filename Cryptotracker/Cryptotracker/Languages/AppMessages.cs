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
            Language.Deutsch => "Anwendungseinstellungen",
            _ => nameof(Settings)
        };

        public static string Nbp(Language lang) => lang switch
        {
            Language.Polski => "Narodowy Bank Polski",
            Language.Deutsch => "Die Polnische Nationalbank",
            _ => "The National Bank of Poland"
        };

        public static string SelectDate(Language lang) => lang switch
        {
            Language.Polski => "Wybierz datę",
            Language.Deutsch => "Wählen Sie ein Datum",
            _ => "Select a date"
        };       
        
        public static string Currency(Language lang) => lang switch
        {
            Language.Polski => "Waluta",
            Language.Deutsch => "Währung",
            _ => "Currency"
        };        
        
        public static string Cryptocurrency(Language lang) => lang switch
        {
            Language.Polski => "Kryptowaluta",
            Language.Deutsch => "Kryptowährung",
            _ => "Cryptocurrency"
        };

        public static string Download(Language lang) => lang switch
        {
            Language.Polski => "Pobierz",
            Language.Deutsch => "Herunterladen",
            _ => "Download"
        };

        public static string StartDate(Language lang) => lang switch
        {
            Language.Polski => "Data początkowa",
            Language.Deutsch => "Anfangsdatum",
            _ => "Start date"
        };
        
        public static string EndDate(Language lang) => lang switch
        {
            Language.Polski => "Data końcowa",
            Language.Deutsch => "Endtermin",
            _ => "End date"
        };

        public static string ExchangeRates(Language lang) => lang switch
        {
            Language.Polski => "Kursy walut",
            Language.Deutsch => "Wechselkurse",
            _ => "Exchange rates"
        };

        public static string CurrencyConverter(Language lang) => lang switch
        {
            Language.Polski => "Przelicznik",
            Language.Deutsch => "Konverter",
            _ => "Converter"
        };

        public static string AboutApp(Language lang) => lang switch
        {
            Language.Polski => "O aplikacji",
            Language.Deutsch => "Über die App",
            _ => "About app"
        };

        public static string Description(Language lang) => lang switch
        {
            Language.Polski => "Cryptotracker to aplikacja biznesowa umożliwiająca śledzenie aktualnych kursów walut i kryptowalut.",
            Language.Deutsch => "Cryptotracker ist eine Geschäftsanwendung, mit der Sie die aktuellen Wechselkurse und Kryptowährungen verfolgen können.",
            _ => "Cryptotracker is a business application that allows you to track the current exchange rates and cryptocurrencies."
        };

        public static string ApiDescription(Language lang) => lang switch
        {
            Language.Polski => "Zewnętrzne API z których korzystamy",
            Language.Deutsch => "Externe API, die wir verwenden",
            _ => "External API which we use"
        };        
        public static string Developers(Language lang) => lang switch
        {
            Language.Polski => "Twórcy aplikacji",
            Language.Deutsch => "App-Entwickler",
            _ => "Application developers"
        };

        public static string Github(Language lang) => lang switch
        {
            Language.Polski => "Zobacz nasz projekt na Github'ie",
            Language.Deutsch => "Siehe unser Projekt auf der Github - Website",
            _ => "See our project on Github website"
        };        
        
        public static string NumberConversionError(Language lang) => lang switch
        {
            Language.Polski => "Oczekiwano liczby",
            Language.Deutsch => "Zahlen erwartet",
            _ => "Number expected"
        };        
        
        public static string ApiKeys(Language lang) => lang switch
        {
            Language.Polski => "Klucze do API",
            Language.Deutsch => "API Schlüssel",
            _ => "API Keys"
        };

        public static string ChartTitle(Language lang) => lang switch
        {
            Language.Polski => "Wykres kursu",
            Language.Deutsch => "Preisdiagramm",
            _ => "Stock Chart"
        };

        public static string YLabel(Language lang) => lang switch
        {
            Language.Polski => "Cena waloru",
            Language.Deutsch => "Der Preis des Vermögenswerts",
            _ => "Stock Price"
        };
        
        public static string Notifications(Language lang) => lang switch
        {
            Language.Polski => "Powiadomienia",
            Language.Deutsch => "Benachrichtigungen",
            _ => "Notifications"
        };        
        
        public static string AddCurrencyNotification(Language lang) => lang switch
        {
            Language.Polski => "Dodaj powiadomienie dla walut",
            Language.Deutsch => "Währungsbenachrichtigung hinzufügen",
            _ => "Add currency notification"
        };

        public static string AddCryptocurrencyNotification(Language lang) => lang switch
        {
            Language.Polski => "Dodaj powiadomienie dla kryptowalut",
            Language.Deutsch => "Kryptowährungsbenachrichtigung hinzufügen",
            _ => "Add cryptocurrency notification"
        };        
        
        public static string NotificationService(Language lang) => lang switch
        {
            Language.Polski => "Serwis powiadomienia",
            Language.Deutsch => "Benachrichtigungsdienst",
            _ => "Notification service"
        };        
        
        public static string NotificationServiceMessage(Language lang) => lang switch
        {
            Language.Polski => "Aby wybrać inny, zmień serwis w ustawieniach",
            Language.Deutsch => "Um einen anderen auszuwählen, ändern Sie den Dienst in den Einstellungen",
            _ => "To select a different one, change the service in the settings"
        };

        public static string ThresholdValue(Language lang) => lang switch
        {
            Language.Polski => "Wartość progowa",
            Language.Deutsch => "Schwellwert",
            _ => "Threshold value"
        };
        
        public static string ComparisonOperator(Language lang) => lang switch
        {
            Language.Polski => "Wybierz operator porównania",
            Language.Deutsch => "Wählen Sie einen Vergleichsoperator",
            _ => "Choose a comparision operator"
        };
        
        public static string NotificationTitle(Language lang) => lang switch
        {
            Language.Polski => "Dzięki aplikacji Cryptotracker możesz otrzymać powiadomienie gdy wybrana przez ciebie waluta osiągnie podany próg",
            Language.Deutsch => "Mit der Cryptotracker - Anwendung können Sie eine Benachrichtigung erhalten, wenn die Währung Ihrer Wahl den Schwellenwert erreicht",
            _ => "With the Cryptotracker application, you can receive a notification of the currency of your choice reaching the threshold"
        };
    }
}
