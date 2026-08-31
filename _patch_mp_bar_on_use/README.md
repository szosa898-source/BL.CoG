# Wersja: pasek MP pojawia się tylko po użyciu many

Ta wersja realizuje Twoją prośbę: **pasek MP jest niewidoczny domyślnie i pokazuje
się dopiero po użyciu many, a po kilku sekundach sam znika.**

## Jak to działa (mechanizm)

Ustaliłem z kodu moda:
- Podczas misji mana **nie regeneruje się** (regeneracja tylko poza bitwą, w osadzie).
- `CnKMpBarVM.Refresh` jest wołane cyklicznie z widoku i ma dostęp do `CurrentMP`/`MaxMP`.

Dlatego "użycie many" wykrywamy jako **spadek `CurrentMP`** względem poprzedniego
odczytu. Po wykryciu spadku pasek pokazuje się na `VisibleSeconds` (domyślnie 4 s),
a licznik odnawia się przy każdym kolejnym rzuceniu. Gdy licznik dojdzie do zera —
pasek znika.

## ⚠️ Wymaga rekompilacji DLL

Tej logiki (timer + porównanie poprzedniej wartości) **nie da się zapisać w samym
prefabie XML** — prefab potrafi tylko pokazać/ukryć widget wg gotowej właściwości.
Trzeba więc podmienić dwie klasy w kodzie i przebudować `BannerlordCnk.dll`.
W tym środowisku nie ma referencji DLL gry, więc budujesz u siebie.

## Pliki

- `CnKMpBarVM.cs` — nowa wersja ViewModelu z logiką "pokaż po użyciu".
  Kluczowa zmiana: `Refresh(Hero hero, float dt)` — dochodzi parametr `dt`.
- `CnKMpBarView.cs` — woła `Refresh(hero, dt)` co klatkę (usunięty throttling 0.25 s,
  bo płynne odliczanie potrzebuje pełnej rozdzielczości czasu).

## Instalacja

1. W projekcie moda podmień zawartość klas `CnKMpBarVM` i `CnKMpBarView`
   na wersje z tego folderu (dopasuj `namespace`/`using` jeśli trzeba).
2. **WAŻNE:** przywróć oryginalny prefab `GUI/Prefabs/CnKMpBar.xml`
   (musi mieć `IsVisible="@IsVisible"`, NIE `false` — inaczej pasek nigdy się nie pokaże).
   Backup oryginału jest w repo: `GUI/Prefabs/CnKMpBar.xml.bak`.
3. Zbuduj projekt, wstaw nowy `BannerlordCnk.dll` do
   `...\Modules\<mod>\bin\Win64_Shipping_Client\`.
4. Wejdź do bitwy, rzuć zaklęcie kosztujące manę — pasek pojawi się na ~4 s i zniknie.

## Regulacja czasu

W `CnKMpBarVM.cs` zmień stałą:
```csharp
private const float VisibleSeconds = 4f;   // np. 2f = krócej, 6f = dłużej
```

## Opcjonalnie: płynne zanikanie (fade-out)

Twarde znikanie po 4 s jest OK, ale jeśli chcesz płynny fade, można rozbudować VM
o właściwość `Alpha` (0..1) malejącą w ostatniej sekundzie i zbindować ją w prefabie
przez `AlphaFactor="@Alpha"` na root widgecie. Daj znać, jeśli chcesz — dopiszę.
