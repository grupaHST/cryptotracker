# Cryptotracker

## Spis treści

* [Opis wymagań](#opis-wymagań)
* [Technologie](#technologie-użyte-w-projekcie)

# Opis wymagań

Aplikacja ma przedstawiać klientowi w formie graficznej aktualne oraz zarchiwizowane dane giełdowe walut i kryptowalut. Możliwa ma być czasowa filtracja danych z ostatnich dzięsięciu lat oraz porównywanie wybranych przez użytkownika walut. Program ma również umożliwiać szybkie przeliczeniej jednej waluty na drugą.
Aplikacja ma umożliwiać użytkownikowi połączenie się ze znanymi serwisami monitorującymi waluty poprzez wystawione przez nie API. Program powinien dawać możliwość powiadamiania użytkowników o ruchach na giełdach wg ustawionych przez niego wytycznych w programie.
Aplikacja ma również posiadać funkcjonalność profili (presetów) dla powiadomień. Na wykresach ma być widoczna zmiana tygodniowa, dniowa i godzinowa oraz wolumen obu walut porównywanych. Wykres ma oczywiście mieć możliwość zbliżania i oddalania.

# Technologie użyte w projekcie

Projekt zostanie wykonany w języku [C#](https://docs.microsoft.com/pl-pl/dotnet/csharp/whats-new/csharp-9) w wersji 9.0. 
Bazowymi technologiami będą [.NET 5.0](https://docs.microsoft.com/pl-pl/dotnet/core/dotnet-five) oraz Windows Presention Foundation [(WPF)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0). Za graficzne przedstawienie użytkownikowi danych odpowiedzialna będzie biblioteka [LiveCharts](https://lvcharts.net/). Do testów jednostkowych użyty zostanie framework 
[UnitTesting](https://en.wikipedia.org/wiki/Visual_Studio_Unit_Testing_Framework) udostępniony przez firmę Microsoft. Projekt będzie wykonywany zgodnie z architekturą MVVM.