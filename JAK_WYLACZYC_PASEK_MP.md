# Jak wyłączyć pasek MP (mana) — Calradian Old Gods

Poprawka na własny użytek (za zgodą autora). Pasek MP to fioletowy pasek z liczbą
pojawiający się w bitwie, gdy bohater zna magię. Nie ma go w ustawieniach MCM,
dlatego wyłączenie systemu "Sanity" go nie usuwało — to osobny system (magia).

Masz do wyboru dwie metody.

---

## ✅ Metoda 1 — GOTOWA, działa od razu (edycja prefabu, bez rekompilacji)

To już zostało zrobione w tym repo. Wystarczy podmienić jeden plik w instalacji moda.

**Co zrobić:**
1. Znajdź folder moda z prefabami UI, np.:
   `...\Steam\steamapps\workshop\content\261550\<ID_moda>\GUI\Prefabs\`
   (lub `...\Mount & Blade II Bannerlord\Modules\<Calradian Old Gods>\GUI\Prefabs\`)
2. Zrób kopię zapasową oryginalnego `CnKMpBar.xml` (np. zmień nazwę na `CnKMpBar.xml.orig`).
3. Skopiuj tam plik `GUI/Prefabs/CnKMpBar.xml` z tego repo (z poprawką `IsVisible="false"`).
4. Uruchom grę. Pasek MP nie będzie się już pokazywać w bitwach.

**Cofnięcie zmiany:** przywróć oryginalny plik (albo w pliku zmień
`IsVisible="false"` z powrotem na `IsVisible="@IsVisible"`).
Backup oryginału jest tu: `GUI/Prefabs/CnKMpBar.xml.bak`.

**Uwaga:** to twarde ukrycie — pasek jest zawsze niewidoczny. Prosto i pewnie.
Nie wymaga restartu ani nowej gry; zadziała po ponownym uruchomieniu gry
(prefaby wczytują się przy starcie).

---

## ⚙️ Metoda 2 — prawdziwy przełącznik w MCM (wymaga rekompilacji DLL)

Jeśli chcesz mieć w MCM opcję **Mission Spells → Show MP bar** (włącz/wyłącz
bez edycji plików), trzeba przebudować `BannerlordCnk.dll` z oryginalnych źródeł.
Tego nie da się zrobić bez projektu autora + referencji do DLL gry, MCM i Harmony,
dlatego dostarczam gotowe zmiany kodu do wklejenia. Pliki w `_patch_mcm_toggle/`:

- `CnKSettings.snippet.cs` — nowa właściwość `ShowMpBar` (checkbox w grupie
  "Mission Spells"), domyślnie włączona.
- `CnKMpBarVM.Refresh.snippet.cs` — zmodyfikowana metoda `Refresh`, która na
  początku sprawdza `CnKSettings.Instance.ShowMpBar` i przy `false` ustawia
  `IsVisible = false`.
- `mcm_toggle.patch` — te same zmiany w formie diffa.

**Kroki (na maszynie z oryginalnym projektem moda):**
1. Dodaj właściwość `ShowMpBar` do klasy `CnKSettings` (z `CnKSettings.snippet.cs`).
2. Podmień metodę `Refresh(Hero)` w `CnKMpBarVM` (z `CnKMpBarVM.Refresh.snippet.cs`).
3. (Opcjonalnie) dodaj tłumaczenia kluczy w `ModuleData/Languages/...`:
   - `cnk_settings_show_mp_bar_name` = "Show MP bar"
   - `cnk_settings_show_mp_bar_hint` = "When off, the mana (MP) bar is never shown..."
   Nie jest to konieczne — MCM użyje tekstu domyślnego zapisanego po `}` w kluczu.
4. Zbuduj projekt → wygenerowany `BannerlordCnk.dll` wstaw do
   `...\Modules\<mod>\bin\Win64_Shipping_Client\`.
5. W grze: Opcje → Mod Configuration Menu → Calradian Old Gods →
   Mission Spells → odznacz **Show MP bar**.

---

## Pliki pomocnicze w tym repo (NIE publikować)

- `_decompiled/` — zdekompilowane źródła DLL, użyte tylko do diagnozy. Ze względu
  na licencję moda **nie udostępniaj ich publicznie**.
- `BUG_ANALYSIS_MP_BAR.md` — szczegółowa analiza przyczyny.
- `_patch_mcm_toggle/` — materiały do Metody 2.

Zmiana z Metody 1 (`CnKMpBar.xml`) jest jedyną, która realnie modyfikuje mod
i jest przeznaczona wyłącznie do Twojego prywatnego użytku.
