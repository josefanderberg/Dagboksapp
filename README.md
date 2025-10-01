# Dagboksapp

Dagboksapp är en enkel konsolapplikation skriven i C# för att skriva, spara och hantera dagboksanteckningar. Programmet använder JSON för lagring och erbjuder funktioner för att lägga till, visa, söka, stjärnmarkera, uppdatera och ta bort anteckningar.

---

## Funktioner

- Lägg till ny anteckning med datum och text
- Lista, sök och hantera anteckningar i en undermeny
- Stjärnmarkera/avmarkera anteckning
- Uppdatera eller ta bort anteckning
- Spara och ladda anteckningar från fil (JSON)
- Felhantering och input-validering

---

## Kom igång

### Förutsättningar

- [.NET 8 SDK](https://dotnet.microsoft.com/download) installerat

### Installation och körning

1. Klona detta repo:
   ```
   git clone https://github.com/josefanderberg/Dagboksapp.git
   ```
2. Gå till projektmappen:
   ```
   cd Dagboksapp/Dagboksapp
   ```
3. Bygg projektet:
   ```
   dotnet build
   ```
4. Starta applikationen:
   ```
   dotnet run
   ```

---

## Exempel på användning

```
===================================
          DAGBOKSAPP
===================================
1. Skriv ny anteckning
2. Lista och hantera anteckningar
3. Ladda
4. Spara
5. Avsluta

Välj ett alternativ: 2

=== Anteckningar ===
Navigera med piltangenterna. Tryck Enter för att välja.

2024-06-05: Avslutade ett stort projekt på jobbet.
2024-05-01: ★ Firade Valborg med familjen. ★
2024-03-12: Tränade på gymmet och kände mig stark.
...
Sök efter anteckning
Tillbaka till menyn...

(Välj en anteckning och tryck Enter)

=== Vald anteckning ===
★ Firade Valborg med familjen. ★
2024-05-01

Vad vill du göra?

> Avmarkera stjärna
  Ta bort anteckningen
  Uppdatera anteckningen
  Tillbaka till menyn...
```

---

## Mappstruktur

```
/Dagboksapp
  /Dagboksapp/
    Dagboksapp.csproj
    diary.json
    error.log
    /Data/
      DiaryEntry.cs
    /Logic/
      DiaryMenuHandler.cs
      DiaryService.cs
      Logger.cs
    /UI/
      AddEntryUI.cs
      ConsoleBoxDrawer.cs
      DiaryConsoleUI.cs
      EntriesMenuUI.cs
      EntryActionMenuUI.cs
      MainMenuUI.cs
    Program.cs
  README.md
  .gitignore
```

---

## Implementation

Alla anteckningar lagras i en `List<DiaryEntry>`. Vid sökning eller filtrering används LINQ för att filtrera och sortera listan. Detta är tillräckligt snabbt för ett normalt antal dagboksanteckningar och gör koden enkel och lättläst. Vid uppdatering, borttagning eller stjärnmarkering används datumet för att hitta rätt anteckning i listan.

Felhantering sker med try/catch, t.ex. vid filoperationer och inmatningsfel. Input valideras med `DateTime.TryParse()` och kontroll av tomma texter. Programmet ger tydliga felmeddelanden till användaren.

---
