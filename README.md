**📚 Blazor Hash Dictionary App**
*Blazor WebAssembly aplikacija* za rad, testiranje i benchmark različitih hash metoda u sopstvenoj implementaciji rečnika sa bucket-ima i kolizijama.

Omogućava unos podataka, generisanje slučajnih vrednosti, pregled strukture hash tablice i merenje performansi.

**✨ Funkcionalnosti**
- Više hash metoda (Default .NET, Division, MidSquare, Multiplication, Fibonacci)
- Dodavanje i brisanje unosa sa prikazom vremena operacije
- Detaljan prikaz bucket-a (bucket index, ključ, vrednost) korisćenjem MudDataGrid za hash tablice i MudTable za tabelu sa kljucevima pre smeštanja u tablicu
- Dugme Remove za brisanje pojedinacnog elementa iz tablice
- Dugme Clear za brisanje svih elemenata iz tablice
- Pretraga po ključu, provera ključa i provera vrednosti sa merenjem vremena
- Generisanje random podataka u velikom broju za testiranje algoritama (npr. 1.000 ili više)

*Automatski benchmark:*
- Vreme izračunavanja hash bucket-a
- Minimalna, prosečna i maksimalna popunjenost bucket-a
- Broj kolizija (više od jednog ključa u istom bucket-u)

**🖼️ Izgled aplikacije**

- Aplikacija koristi MudBlazor komponentni framework za moderan UI:
- Radio dugmad za izbor hash metode
- Tabela sa trenutnim unosima
- DataGrid sa prikazom bucket detalja
- Sekcija sa rezultatima benchmark-a
- Polja za pretragu i provere

**🛠️ Tehnologije**

- Blazor WebAssembly (.NET 9)
- C#
- MudBlazor za UI
- Stopwatch za precizno merenje performansi
- Sopstvena klasa MySimpleDictionary<TKey, TValue> sa različitim hash funkcijama

**🚀 Kako pokrenuti projekat**

1. Kloniraj repozitorijum:
   git clone https://github.com/username/BlazorAppDictionaryHash.git
   cd BlazorAppDictionaryHash
   dotnet restore

2. Pokreni aplikaciju:
   na dugme run iz programa VS ili dotnet run --project BlazorAppDictionaryHash

3. Otvori u browser-u:
   automatski se otvara stranica: https://localhost:7194/

**📂 Struktura koda**

Data/MySimpleDictionary.cs – Implementacija rečnika i hash funkcija

Pages/Dictionary.razor – Glavna stranica sa UI logikom i povezivanjem sa rečnikom

Layout/ MainLayout.razor i NavManu.razor za prikaz hedera i odabir stranica

wwwroot/ – Statički fajlovi aplikacije

**📊 Benchmark primer**

Nakon generisanja 1.000 unosa i odabira Multiplication hash metode, rezultati mogu izgledati ovako:

Benchmark za 1000 unosa koristeći Multiplication.

Vreme izračunavanja: 15792 ticks (1 ms)

Buckets min/avg/max: 0/1.05/4

Broj kolizija (više od 1 ključa po bucketu): 47
