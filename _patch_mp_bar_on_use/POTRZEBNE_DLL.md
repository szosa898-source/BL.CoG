# Jakie DLL są potrzebne do rekompilacji

## Status weryfikacji (w tym środowisku)

Pobrałem DLL z Twojego repo `szosa898-source/test` i **faktycznie skompilowałem**
ViewModel poprawki:

- ✅ `CnKMpBarVM.cs` (logika "pokaż pasek po użyciu many") — **KOMPILUJE SIĘ**
  poprawnie (tylko nieszkodliwe ostrzeżenia CS1701 netstandard 2.0/2.1).
- ❌ `CnKMpBarView.cs` — NIE skompilował się, bo brakuje **jednego** pliku:
  `TaleWorlds.MountAndBlade.View.dll` (zawiera `MissionView`, `MissionScreen`).

## Czego brakuje w Twoich repo

W `szosa898-source/test` jest komplet TaleWorlds.* i SandBox.*, ALE brakuje:

1. **`TaleWorlds.MountAndBlade.View.dll`**  ← WYMAGANE (baza klasy `CnKMpBarView`)
   Lokalizacja u Ciebie na dysku:
   `...\Mount & Blade II Bannerlord\Modules\Native\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.View.dll`

2. `MCM.Abstractions.dll` (+ `MCM.Common.dll`)  ← tylko jeśli chcesz też wariant
   z przełącznikiem w MCM (`_patch_mcm_toggle`). Dla wersji "pokaż po użyciu" NIE jest
   potrzebne.
   Lokalizacja: folder modułu MCM, np.
   `...\Modules\Bannerlord.MBOptionScreen\bin\Win64_Shipping_Client\MCM.Abstractions.dll`

## Pełna lista referencji potrzebnych do zbudowania obu plików poprawki

Z repo `test` (mam je / są dostępne):
- TaleWorlds.Library.dll
- TaleWorlds.Core.dll
- TaleWorlds.CampaignSystem.dll
- TaleWorlds.Engine.dll
- TaleWorlds.Engine.GauntletUI.dll
- TaleWorlds.ScreenSystem.dll
- TaleWorlds.MountAndBlade.dll
- TaleWorlds.Localization.dll
- TaleWorlds.ObjectSystem.dll
- 0Harmony.dll
- BannerlordCnk.dll (oryginalny — dla typu MagicBehavior)

Brakuje (dograj do repo `test` lub podaj):
- **TaleWorlds.MountAndBlade.View.dll**   (konieczne)
- MCM.Abstractions.dll, MCM.Common.dll     (tylko dla wariantu MCM)

## Co zrób, żebym dokończył kompilację TUTAJ

Wrzuć `TaleWorlds.MountAndBlade.View.dll` do repo `test` (najlepiej obok innych
TaleWorlds.*). Wtedy zbuduję i zweryfikuję cały patch (View + VM) do końca,
a jeśli dograsz też MCM.* — zbuduję również wariant z przełącznikiem.

Uwaga: pełny, finalny `BannerlordCnk.dll` do gry i tak najlepiej zbudować z
ORYGINALNEGO projektu autora (tu mamy tylko dekompilat, który do produkcyjnego
buildu wymagałby ręcznych poprawek artefaktów). Ale weryfikacja kompilacji
samej poprawki jest w pełni wykonalna po dograniu View.dll.
