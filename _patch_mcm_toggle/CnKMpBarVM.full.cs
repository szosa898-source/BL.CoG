using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace BL_CnK_Dev;

// ============================================================================
//  Wariant MCM: pasek MP respektuje przelacznik CnKSettings.ShowMpBar.
//  Pelna wersja klasy do rekompilacji. Oryginalna sygnatura Refresh(Hero).
// ============================================================================

public class CnKMpBarVM : ViewModel
{
	private bool _isVisible;
	private int _currentMP;
	private int _maxMP;
	private float _mpRatio;

	[DataSourceProperty]
	public bool IsVisible
	{
		get { return _isVisible; }
		set { _isVisible = value; OnPropertyChangedWithValue(value, "IsVisible"); }
	}

	[DataSourceProperty]
	public int CurrentMP
	{
		get { return _currentMP; }
		set { _currentMP = value; OnPropertyChangedWithValue(value, "CurrentMP"); }
	}

	[DataSourceProperty]
	public int MaxMP
	{
		get { return _maxMP; }
		set { _maxMP = value; OnPropertyChangedWithValue(value, "MaxMP"); }
	}

	[DataSourceProperty]
	public float MPRatio
	{
		get { return _mpRatio; }
		set { _mpRatio = value; OnPropertyChangedWithValue(value, "MPRatio"); }
	}

	public void Refresh(Hero hero)
	{
		// Master switch z MCM: gdy wylaczony, pasek MP nigdy sie nie pokazuje.
		CnKSettings settings = CnKSettings.Instance;
		if (settings != null && !settings.ShowMpBar)
		{
			IsVisible = false;
			return;
		}

		Campaign current = Campaign.Current;
		MagicBehavior magicBehavior = (current != null) ? current.GetCampaignBehavior<MagicBehavior>() : null;
		if (magicBehavior == null || hero == null)
		{
			IsVisible = false;
			return;
		}
		IsVisible = magicBehavior.GetKnownCapacityIds(hero).Count > 0;
		if (IsVisible)
		{
			int num = MaxMP = MagicBehavior.GetMaxMP(hero);
			CurrentMP = magicBehavior.GetCurrentMP(hero);
			MPRatio = (num > 0) ? ((float)CurrentMP / (float)num) : 0f;
		}
	}
}
