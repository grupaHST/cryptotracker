# Cryptotracker

Projekt aplikacji biznesowej do śledzenia kursów walut i kryptowalut.

## Spis treści

* [Wprowadzenie](#wprowadzenie "Wprowadzenie")

    * [Cel](#cel "Cel")

    * [Zakres](#zakres "Zakres")

    * [Definicje, akronimy i skróty](#definicje,-akronimy-i-skróty "Definicje, akronimy i skróty")

* [Opis wymagań](#opis-wymagań "Opis wymagań")

    * [Wymagania fukcjonalne](#wymagania-fukcjonalne "Wymagania fukcjonalne")

    * [Wymagania niefukcjonalne](#wymagania-niefukcjonalne "Wymagania fukcjonalne")

* [Osoby uczestniczące w projekcie](#osoby-uczestniczące-w-projekcie "Osoby uczestniczące w projekcie")

* [Technologie](#technologie-użyte-w-projekcie "Technologie")

* [Podział ról i odpowiedzialności w projekcie](#podział-ról-i-odpowiedzialności-w-projekcie "Podział ról i odpowiedzialności w projekcie")

# Wprowadzenie

Dokument ten został stworzony na potrzeby projektu Cryptotracker. Przedstawia wynik fazy analizy oraz specyfikację wymagań dostarczoną przez klienta w dogodny i przejrzysty sposób.

## Cel

Dokument ten ma za zadanie przedstawione przez klienta wymagania przedstawić w formie zwięzłej, sformalizowanej i dla każdego przystępnej.

## Zakres

Dokument ten obejmuje czynniki, których wymaga klient od sporządzanej aplikacji. Czynniki te obejmują zaplanowany na stworzenie systemu czas, środki czy platformy które muszą być obsługiwane oraz wszystko to, co ogranicza projekt.

## Definicje, akronimy i skróty

* Architektura – schemat ogólny budowy systemu komputerowego lub jego części, określający jego elementy, układy ich łączące i zasady współpracy między nimi.

* API - zbiór reguł ściśle opisujący, w jaki sposób programy lub podprogramy komunikują się ze sobą.

# Opis wymagań

## Wymagania fukcjonalne

* Aplikacja ma przedstawiać klientowi w formie graficznej aktualne oraz zarchiwizowane (z ostatnich dziesięciu lat) dane giełdowe walut i kryptowalut uzupełnione o podstawowe wskaźniki giełdowe. Na wyświetlanych wykresach ma być widoczna zmiana roczna, miesięczna, tygodniowa i dniowa oraz wolumen porównywanych walut. Użytkownik ma mieć możliwość ustawienia dokładnych ram czasowych wyświetlanych danych. Wykres ma posiadać funkcjonalność prostego zbliżania i oddalania. Program musi również umożliwiać szybkie przeliczenie wybranej waluty na inną.

* Aplikacja powinna dawać możliwość powiadamiania użytkowników o ruchach na giełdach według ustawionych przez niego wytycznych w programie.
Program ma również posiadać funkcjonalność profili (presetów) dla powiadomień.

* Aplikacja ma być dwujęzyczna, w języku polskim oraz w angielskim.

## Wymagania niefunkcjonalne

* Aplikacja musi być możliwa do uruchomienia zarówno pod systemem Linux jak i Microsoft Windows.

* Aplikacja ma korzystać z danych udostępnianych przez znane serwisy monitorujące waluty poprzez wystawione przez nie internetowe API ([Narodowy Bank Polski](https://www.nbp.pl/), [Bitbay](https://bitbay.net/pl)).

# Osoby uczestniczące w projekcie

* *Dawid Heinrich*
* *Szymon Świstek*
* *Oswald Toma*

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