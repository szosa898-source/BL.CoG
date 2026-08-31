# Analiza buga: pasek MP ("14") zawsze widoczny w bitwie

## Podsumowanie
Fioletowy pasek z liczbą (np. "14") wyświetlany w bitwie to **pasek MP (Magic Points)**
systemu magii moda — **NIE** pasek Sanity. Dlatego wyłączenie opcji
"Enable sanity system" w MCM nie usuwa go: to dwa niezależne systemy.

## Dowody (dekompilacja BannerlordCnk.dll v1.1.2.2)

### 1. Pasek pochodzi z systemu magii, nie sanity
Klasy widoku/VM leżą w `Magic/Mission/`, nie w `Sanity/`:
- `D:\Dev\Bannerlord-CnK\BL-CnK-Dev\Magic\Mission\CnKMpBarView.cs`
- `D:\Dev\Bannerlord-CnK\BL-CnK-Dev\Magic\Mission\CnKMpBarVM.cs`

### 2. Widok jest DODAWANY BEZWARUNKOWO
`CnKMpBarView.OnMissionScreenInitialize()` tworzy warstwę Gauntlet i ładuje
prefab `CnKMpBar` przy każdej inicjalizacji ekranu misji — bez żadnego
sprawdzenia ustawień:

```csharp
public override void OnMissionScreenInitialize()
{
    base.OnMissionScreenInitialize();
    _vm = new CnKMpBarVM();
    _gauntletLayer = new GauntletLayer("GauntletLayer", 20, false);
    _movie = _gauntletLayer.LoadMovie("CnKMpBar", _vm);
    MissionScreen.AddLayer(_gauntletLayer);
}
```

### 3. Widoczność zależy WYŁĄCZNIE od posiadania zdolności magicznych
`CnKMpBarVM.Refresh(Hero)` ustawia `IsVisible` na podstawie liczby znanych
"capacity" (zdolności/mocy) bohatera — nie ma tu żadnej flagi konfiguracyjnej:

```csharp
public void Refresh(Hero hero)
{
    MagicBehavior magicBehavior = Campaign.Current?.GetCampaignBehavior<MagicBehavior>();
    if (magicBehavior == null || hero == null) { IsVisible = false; return; }

    IsVisible = magicBehavior.GetKnownCapacityIds(hero).Count > 0;   // <-- jedyny warunek
    if (IsVisible)
    {
        int max = MaxMP = MagicBehavior.GetMaxMP(hero);
        CurrentMP = magicBehavior.GetCurrentMP(hero);
        MPRatio = (max > 0) ? (float)CurrentMP / max : 0f;
    }
}
```

Prefab `GUI/Prefabs/CnKMpBar.xml` wiąże `IsVisible="@IsVisible"`, więc pasek
pokazuje się, gdy tylko bohater zna ≥1 zdolność magiczną.

### 4. Brak opcji MCM do wyłączenia paska/magii
Wszystkie opcje MCM zapisane w DLL:
- Events: common/incursion daily chance
- Sanity: `Enable sanity system` (tylko sanity!)
- Locations: tower shop refresh/stock/mage gear stock
- Mission Spells: `Power wheel key`

Nie istnieje żaden przełącznik "Enable magic system" ani "Show MP bar".

## Przyczyna źródłowa (root cause)
Pasek MP nie respektuje żadnego ustawienia widoczności. Gdy Twój bohater
zdobył choć jedną zdolność magiczną (np. przez pakt/questa "The Carmine
Evanescence"/Hastura z zapisu w logach), `GetKnownCapacityIds(hero).Count > 0`
zwraca true i pasek jest trwale widoczny w każdej bitwie.

## Propozycje naprawy (dla autora moda)
1. Dodać opcję MCM np. `cnk_settings_show_mp_bar` (grupa "Mission Spells")
   i sprawdzać ją w `CnKMpBarVM.Refresh` oraz/lub `OnMissionScreenInitialize`.
2. Alternatywnie: pokazywać pasek MP tylko, gdy gracz trzyma klawisz Power Wheel
   lub ma aktywną zdolność rzucania, zamiast trwale.
3. Ewentualnie powiązać widoczność z tym samym master-switchem co reszta magii,
   jeśli taki powstanie.

## Obejście dla gracza (bez modyfikacji kodu)
Ponieważ widoczność zależy od `GetKnownCapacityIds(hero).Count`, pasek zniknie
tylko, gdy bohater nie zna żadnej zdolności magicznej. Nie da się tego wyłączyć
z MCM w obecnej wersji (v1.1.2.2). Najlepszą drogą jest zgłoszenie autorowi
(Anate0) prośby o dodanie przełącznika widoczności paska MP.
