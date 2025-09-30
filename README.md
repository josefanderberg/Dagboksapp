# Dagboksapp

Dagboksapp är en enkel konsolapplikation skriven i C# för att skriva, spara och hantera dagboksanteckningar. Programmet använder JSON för lagring och erbjuder grundläggande funktioner för att lägga till, visa, söka, uppdatera och ta bort anteckningar.

---

## Funktioner

- Lägg till ny anteckning med datum och text
- Lista alla anteckningar
- Sök anteckning på datum
- Uppdatera eller ta bort anteckning
- Spara anteckningar till fil (JSON)
- Läs anteckningar från fil
- Felhantering och input-validering

---

## Kom igång

### Förutsättningar

- [.NET 8 SDK](https://dotnet.microsoft.com/download) installerat

### Installation och körning

1. Klona detta repo:
   ```
   git clone https://github.com/ditt-användarnamn/Dagboksapp.git
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
2. Lista alla anteckningar
3. Sök anteckning på datum
4. Ta bort eller uppdatera en anteckning
5. Spara till fil
6. Läs från fil
7. Avsluta
===================================
Välj ett alternativ: 1

=== Skriv ny anteckning ===
Skriv din anteckning: Tränade på gymmet.
Skriv datum (yyyy-mm-dd): 2024-05-01
Anteckning tillagd!
```

---

## Mappstruktur

```
/Dagboksapp
  /Dagboksapp/    # C#-projektfiler
    Program.cs
    Diary.cs
    DiaryEntry.cs
  README.md
  .gitignore
```

---

## Reflektion

Jag valde att lagra anteckningarna i både en `List<DiaryEntry>` och en `Dictionary<DateTime, DiaryEntry>`. Listan används för att enkelt iterera och spara/läsa från fil, medan dictionaryn ger snabbare uppslag på datum (O(1)), vilket är användbart vid sökning, uppdatering och borttagning. Filformatet är JSON för enkel serialisering och läsbarhet. Felhantering sker med try/catch, t.ex. vid filoperationer och inmatningsfel. Input valideras med `DateTime.TryParse()` och kontroll av tomma texter. Programmet ger tydliga felmeddelanden till användaren.

---
