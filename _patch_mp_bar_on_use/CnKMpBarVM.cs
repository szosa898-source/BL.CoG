using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace BL_CnK_Dev;

// ============================================================================
//  WERSJA: pasek MP pojawia sie TYLKO po uzyciu many i chowa sie po chwili.
// ----------------------------------------------------------------------------
//  Zastap oryginalny plik CnKMpBarVM.cs ta wersja (wymaga rekompilacji DLL).
//
//  Zasada dzialania:
//   - Wykrywamy uzycie many jako SPADEK CurrentMP wzgledem poprzedniego odczytu
//     (w trakcie misji mana sie nie regeneruje, wiec kazdy spadek = rzucenie).
//   - Po wykryciu spadku pokazujemy pasek i ustawiamy licznik _hideTimer.
//   - Dopoki _hideTimer > 0 pasek jest widoczny; potem znika.
//   - Refresh(hero, dt) dostaje deltaTime z widoku, by odliczac czas.
//
//  Uwaga: jesli w MCM dodasz przelacznik ShowMpBar (patrz _patch_mcm_toggle),
//  mozesz laczyc obie logiki - tu zostawiam sam "pojawianie sie po uzyciu".
// ============================================================================

public class CnKMpBarVM : ViewModel
{
	private bool _isVisible;
	private int _currentMP;
	private int _maxMP;
	private float _mpRatio;

	// --- stan dla logiki "pokaz po uzyciu" ---
	private const float VisibleSeconds = 4f;   // jak dlugo pasek pozostaje widoczny po uzyciu many
	private int _lastMP = -1;                   // ostatnio widziane CurrentMP (-1 = jeszcze nie znane)
	private float _hideTimer;                   // ile sekund jeszcze pokazywac pasek

	[DataSourceProperty]
	public bool IsVisible
	{
		get { return _isVisible; }
		set { _isVisible = value; ((ViewModel)this).OnPropertyChangedWithValue(value, "IsVisible"); }
	}

	[DataSourceProperty]
	public int CurrentMP
	{
		get { return _currentMP; }
		set { _currentMP = value; ((ViewModel)this).OnPropertyChangedWithValue(value, "CurrentMP"); }
	}

	[DataSourceProperty]
	public int MaxMP
	{
		get { return _maxMP; }
		set { _maxMP = value; ((ViewModel)this).OnPropertyChangedWithValue(value, "MaxMP"); }
	}

	[DataSourceProperty]
	public float MPRatio
	{
		get { return _mpRatio; }
		set { _mpRatio = value; ((ViewModel)this).OnPropertyChangedWithValue(value, "MPRatio"); }
	}

	// Nowa sygnatura: przyjmuje deltaTime, by odliczac czas widocznosci.
	// (Wywolanie zaktualizowane w CnKMpBarView.OnMissionScreenTick.)
	public void Refresh(Hero hero, float dt)
	{
		Campaign current = Campaign.Current;
		MagicBehavior magicBehavior = current != null ? current.GetCampaignBehavior<MagicBehavior>() : null;

		// Brak magii/bohatera => nic nie pokazujemy i resetujemy stan.
		if (magicBehavior == null || hero == null || magicBehavior.GetKnownCapacityIds(hero).Count == 0)
		{
			_lastMP = -1;
			_hideTimer = 0f;
			IsVisible = false;
			return;
		}

		int max = MagicBehavior.GetMaxMP(hero);
		int cur = magicBehavior.GetCurrentMP(hero);

		// Pierwszy odczyt w tej misji - tylko zapamietaj, nie pokazuj.
		if (_lastMP < 0)
		{
			_lastMP = cur;
		}
		else if (cur < _lastMP)
		{
			// MP spadlo => gracz wlasnie uzyl many. Pokaz pasek i odswiez licznik.
			_hideTimer = VisibleSeconds;
		}
		_lastMP = cur;

		// Odliczanie czasu widocznosci.
		if (_hideTimer > 0f)
		{
			_hideTimer -= dt;
			if (_hideTimer < 0f)
			{
				_hideTimer = 0f;
			}
		}

		IsVisible = _hideTimer > 0f;
		if (IsVisible)
		{
			MaxMP = max;
			CurrentMP = cur;
			MPRatio = (max > 0) ? ((float)cur / (float)max) : 0f;
		}
	}
}
