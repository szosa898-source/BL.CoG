using System;
using System.Collections.Generic;
using System.Linq;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using MCM.Common;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace BL_CnK_Dev;

// ============================================================================
//  Pelna klasa CnKSettings z DODANYM przelacznikiem "Show MP bar".
//  Odtworzona 1:1 z oryginalu (dekompilat) + nowa property ShowMpBar.
//  Uzyta do weryfikacji kompilacji wariantu MCM.
// ============================================================================

public class CnKSettings : AttributeGlobalSettings<CnKSettings>
{
	private static readonly InputKey[] _keyboardKeys = (from InputKey k in Enum.GetValues(typeof(InputKey))
		where (int)k != -1 && (int)Key.GetInputType(k) == 0
		select k).OrderBy<InputKey, string>((InputKey k) => k.ToString(), StringComparer.OrdinalIgnoreCase).ToArray();

	public override string Id => "BLCnK_settings";

	public override string DisplayName => new TextObject("{=cnk_settings_display_name}Calradian Old Gods", null).ToString();

	public override string FolderName => "BLCnK";

	public override string FormatType => "json2";

	[SettingPropertyFloatingInteger("{=cnk_settings_common_event_chance_name}Common event daily chance", 0f, 1f, "#0.0%", Order = 0, RequireRestart = false, HintText = "{=cnk_settings_common_event_chance_hint}Chance rolled each day to start a new common event. Default: 2%.")]
	[SettingPropertyGroup("{=cnk_settings_group_events}Events", GroupOrder = 0)]
	public float CommonEventDailyChance { get; set; } = 0.02f;

	[SettingPropertyFloatingInteger("{=cnk_settings_incursion_chance_name}Incursion daily chance", 0f, 1f, "#0.0%", Order = 1, RequireRestart = false, HintText = "{=cnk_settings_incursion_chance_hint}Chance rolled each day to start a new incursion. Default: 1%.")]
	[SettingPropertyGroup("{=cnk_settings_group_events}Events", GroupOrder = 0)]
	public float IncursionDailyChance { get; set; } = 0.01f;

	[SettingPropertyBool("{=cnk_settings_sanity_enabled_name}Enable sanity system", Order = 0, RequireRestart = false, HintText = "{=cnk_settings_sanity_enabled_hint}Master switch for the sanity system. When off, sanity never changes.")]
	[SettingPropertyGroup("{=cnk_settings_group_sanity}Sanity", GroupOrder = 1)]
	public bool SanityEnabled { get; set; } = true;

	[SettingPropertyInteger("{=cnk_settings_tower_refresh_name}Tower shop refresh interval (days)", 1, 30, "0 days", Order = 2, RequireRestart = false, HintText = "{=cnk_settings_tower_refresh_hint}How often an astrology tower's stock of books and magic items is randomized.")]
	[SettingPropertyGroup("{=cnk_settings_group_locations}Locations", GroupOrder = 2)]
	public int TowerShopRefreshDays { get; set; } = 3;

	[SettingPropertyInteger("{=cnk_settings_tower_stock_name}Tower shop stock size", 1, 12, "0 items", Order = 3, RequireRestart = false, HintText = "{=cnk_settings_tower_stock_hint}How many books/magic items an astrology tower offers for sale at once.")]
	[SettingPropertyGroup("{=cnk_settings_group_locations}Locations", GroupOrder = 2)]
	public int TowerShopStockSize { get; set; } = 5;

	[SettingPropertyDropdown("{=cnk_settings_power_wheel_key_name}Power wheel key", Order = 0, RequireRestart = false, HintText = "{=cnk_settings_power_wheel_key_hint}Key held to open the mission power wheel. Opens instantly and releases/activates the highlighted power on release.")]
	[SettingPropertyGroup("{=cnk_settings_group_mission_spells}Mission Spells", GroupOrder = 3)]
	public Dropdown<InputKey> PowerWheelKey { get; set; } = new Dropdown<InputKey>(_keyboardKeys, Math.Max(0, Array.IndexOf(_keyboardKeys, (InputKey)16)));

	// >>> NOWA OPCJA <<<
	[SettingPropertyBool("{=cnk_settings_show_mp_bar_name}Show MP bar", Order = 1, RequireRestart = false, HintText = "{=cnk_settings_show_mp_bar_hint}When off, the mana (MP) bar is never shown during missions, even if your hero knows magic.")]
	[SettingPropertyGroup("{=cnk_settings_group_mission_spells}Mission Spells", GroupOrder = 3)]
	public bool ShowMpBar { get; set; } = true;

	public InputKey PowerWheelKeyValue
	{
		get
		{
			Dropdown<InputKey> powerWheelKey = PowerWheelKey;
			return (InputKey)((powerWheelKey == null) ? 16 : ((int)powerWheelKey.SelectedValue));
		}
	}

	[SettingPropertyInteger("{=cnk_settings_tower_gear_stock_name}Tower shop mage gear stock size", 0, 4, "0 items", Order = 4, RequireRestart = false, HintText = "{=cnk_settings_tower_gear_stock_hint}How many mage gear pieces (robe/cape/hood/staff) an astrology tower offers for sale at once, independent of book stock.")]
	[SettingPropertyGroup("{=cnk_settings_group_locations}Locations", GroupOrder = 2)]
	public int TowerShopMageGearStockSize { get; set; } = 2;
}
