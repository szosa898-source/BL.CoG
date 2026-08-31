# Status kompilacji — ZWERYFIKOWANE ✅

Po dograniu przez Ciebie brakujących DLL do repo `szosa898-source/test`
udało się **faktycznie skompilować oba warianty poprawki** w tym środowisku.

## Wyniki kompilacji (Roslyn csc, referencje = prawdziwe DLL gry z repo `test`)

| Wariant | Pliki | Wynik |
|--------|-------|-------|
| Pasek tylko po użyciu many | `CnKMpBarVM.cs` + `CnKMpBarView.cs` | ✅ **kompiluje się** (`mp_on_use.dll`) |
| Przełącznik w MCM | `CnKSettings.full.cs` + `CnKMpBarVM.full.cs` | ✅ **kompiluje się** (`mcm_toggle.dll`) |

Ostrzeżenia, które są OCZEKIWANE i nieszkodliwe:
- **CS1701** (netstandard 2.0 vs 2.1) — standard dla modów Bannerlord; runtime gry to rozwiązuje.
- **CS0436** (typ CnKMpBarVM konfliktuje z tym w BannerlordCnk.dll) — bo do testu
  referuję ORYGINALNY mod DLL zawierający stare wersje klas. W finalnym buildzie,
  gdzie te pliki ZASTĘPUJĄ oryginały, ostrzeżenie znika.

## Użyte referencje (wszystkie z repo `test`)

Wspólne:
- TaleWorlds.Library / Core / CampaignSystem / ObjectSystem / Localization
- TaleWorlds.Engine / Engine.GauntletUI / ScreenSystem
- TaleWorlds.MountAndBlade / **TaleWorlds.MountAndBlade.View** (dograne przez Ciebie)
- BannerlordCnk.dll (oryginalny — dla typu MagicBehavior)

Dodatkowo dla wariantu MCM:
- **MCMv5.dll** (dograne przez Ciebie) — zawiera cały MCM.Abstractions.* + MCM.Common.Dropdown
- TaleWorlds.InputSystem

## Co to oznacza dla Ciebie

Kod poprawek jest **potwierdzony jako kompilowalny i zgodny z API**. Aby uzyskać
finalny `BannerlordCnk.dll` do gry, wystarczy podmienić/dodać te pliki w
ORYGINALNYM projekcie moda i zbudować całość (Twój znajomy autor ma ten projekt).

Dekompilat w `_decompiled/` nie nadaje się do produkcyjnego pełnego buildu bez
ręcznych poprawek artefaktów IL — dlatego finalny build rób z oryginalnych źródeł.
Same nasze pliki są gotowe i zweryfikowane.
