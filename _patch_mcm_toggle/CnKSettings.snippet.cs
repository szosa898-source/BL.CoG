// ============================================================================
//  PATCH (Ścieżka 2 – prawdziwy przełącznik MCM)  —  CnKSettings.cs
// ----------------------------------------------------------------------------
//  Dodaj poniższą właściwość do klasy BL_CnK_Dev.CnKSettings w ORYGINALNYM
//  projekcie moda (tam gdzie są już inne [SettingProperty...]).
//  Wstaw ją np. tuż obok PowerWheelKey, w tej samej grupie "Mission Spells".
//
//  Domyślnie true => zachowanie jak dotychczas (pasek widoczny, gdy bohater
//  zna >=1 zdolność). Ustaw na false w MCM, aby pasek MP nigdy się nie pokazał.
// ============================================================================

[SettingPropertyBool(
    "{=cnk_settings_show_mp_bar_name}Show MP bar",
    Order = 1,
    RequireRestart = false,
    HintText = "{=cnk_settings_show_mp_bar_hint}When off, the mana (MP) bar is never shown during missions, even if your hero knows magic.")]
[SettingPropertyGroup("{=cnk_settings_group_mission_spells}Mission Spells", GroupOrder = 3)]
public bool ShowMpBar { get; set; } = true;
