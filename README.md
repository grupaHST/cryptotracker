# Cryptotracker

## Spis treści

* [Osoby uczestniczące w projekcie](#osoby-uczestniczące-w-projekcie "Osoby uczestniczące w projekcie")
* [Opis wymagań](#opis-wymagań "Opis wymagań")
* [Technologie](#technologie-użyte-w-projekcie "Technologie")
* [Podział ról i odpowiedzialności w projekcie](#podział-ról-i-odpowiedzialności-w-projekcie "Podział ról i odpowiedzialności w projekcie")

# Osoby uczestniczące w projekcie

* *Dawid Heinrich*
* *Szymon Świstek*
* *Oswald Toma*

# Opis wymagań

Aplikacja ma przedstawiać klientowi w formie graficznej aktualne oraz zarchiwizowane dane giełdowe walut i kryptowalut uzupełnione o podstawowe wskaźniki giełdowe. Możliwe ma być ustawienie ram czasowych wyświetlanych danych. 
Aplikacja ma umożliwić analizę danych z ostatnich dziesięciu lat oraz porównywanie wybranych przez użytkownika walut. Program ma również umożliwiać szybkie przeliczenie jednej waluty na drugą.
Aplikacja ma  korzystać z danych udostępnianych przez znane serwisy monitorujące waluty poprzez wystawione przez nie API. Program powinien dawać możliwość powiadamiania użytkowników o ruchach na giełdach wg ustawionych przez niego wytycznych w programie.
Aplikacja ma również posiadać funkcjonalność profili (presetów) dla powiadomień. Na wykresach ma być widoczna zmiana tygodniowa, dniowa i godzinowa oraz wolumen porównywanych walut. Wykres ma mieć możliwość prostego zbliżania i oddalania. Aplikacja ma być dwujęzyczna, w języku polskim oraz w angielskim.

# Technologie użyte w projekcie

Projekt zostanie wykonany w języku [C#](https://docs.microsoft.com/pl-pl/dotnet/csharp/whats-new/csharp-9) w wersji 9.0. 
Bazowymi technologiami będą [.NET 5.0](https://docs.microsoft.com/pl-pl/dotnet/core/dotnet-five) oraz Windows Presention Foundation [(WPF)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0). Za graficzne przedstawienie użytkownikowi danych odpowiedzialna będzie biblioteka [LiveCharts](https://lvcharts.net/). Do testów jednostkowych użyty zostanie framework 
[UnitTesting](https://en.wikipedia.org/wiki/Visual_Studio_Unit_Testing_Framework) udostępniony przez firmę Microsoft. Projekt będzie wykonywany zgodnie z wzorcem projektowym MVVM.

# Podział ról i odpowiedzialności w projekcie

| Zadanie | Osoba odpowiedzialna |
| :- | :-: |
| Frontend - tworzenie wykresów (biblioteka LiveCharts) | *Dawid Heinrich* |
| Frontend - widoki, design aplikacji | *Szymon Świstek* |
| Tłumaczenie aplikacji na różne języki | *Szymon Świstek* |
| Backend - requesty HTTP + deserializacja danych | *Oswald Toma* |
| Backend - kalkulacje, przeliczenia walut | *Dawid Heinrich* |
| System powiadomień użytkownika | *Oswald Toma* |
| Integracja frontendu z backendem | *Szymon Świstek* |
| Testy jednostkowe aplikacji | Każdy dla swoich funkcjonalności |