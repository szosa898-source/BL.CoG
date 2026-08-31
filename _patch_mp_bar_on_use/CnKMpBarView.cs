using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ScreenSystem;

namespace BL_CnK_Dev;

// ============================================================================
//  WERSJA: pasek MP pojawia sie TYLKO po uzyciu many i chowa sie po chwili.
// ----------------------------------------------------------------------------
//  Zastap oryginalny plik CnKMpBarView.cs ta wersja (wymaga rekompilacji DLL).
//
//  Zmiana wzgledem oryginalu:
//   - Refresh jest teraz wolane CO KLATKE i dostaje 'dt', dzieki czemu VM moze
//     plynnie odliczac czas widocznosci paska po uzyciu many.
//   - Usunieto throttling co 0.25s (odliczanie czasu wymaga pelnej rozdzielczosci).
//
//  Uwaga: to zdekompilowany kod z artefaktami rzutowan ((MissionView)this itd.).
//  W oryginalnym projekcie moda te rzutowania sa zbedne - user wkleja logike
//  do swojej wersji klasy. Ponizej zostawiam forme zgodna z dekompilatem, aby
//  bylo jednoznacznie widac, co i gdzie zmienic.
// ============================================================================

public class CnKMpBarView : MissionView
{
	private GauntletLayer _gauntletLayer;
	private GauntletMovieIdentifier _movie;
	private CnKMpBarVM _vm;

	public override MissionBehaviorType BehaviorType => (MissionBehaviorType)1;

	public override void OnMissionScreenInitialize()
	{
		((MissionView)this).OnMissionScreenInitialize();
		_vm = new CnKMpBarVM();
		_gauntletLayer = new GauntletLayer("GauntletLayer", 20, false);
		_movie = _gauntletLayer.LoadMovie("CnKMpBar", (ViewModel)(object)_vm);
		((ScreenBase)((MissionView)this).MissionScreen).AddLayer((ScreenLayer)(object)_gauntletLayer);
	}

	public override void OnMissionScreenTick(float dt)
	{
		((MissionView)this).OnMissionScreenTick(dt);
		if (_vm != null)
		{
			// Wolane co klatke z deltaTime - VM sam decyduje o widocznosci i odliczaniu.
			_vm.Refresh(Hero.MainHero, dt);
		}
	}

	public override void OnMissionScreenFinalize()
	{
		if (_gauntletLayer != null)
		{
			if (_movie != null)
			{
				_gauntletLayer.ReleaseMovie(_movie);
			}
			if (((MissionView)this).MissionScreen != null && ((ScreenBase)((MissionView)this).MissionScreen).HasLayer((ScreenLayer)(object)_gauntletLayer))
			{
				((ScreenBase)((MissionView)this).MissionScreen).RemoveLayer((ScreenLayer)(object)_gauntletLayer);
			}
			_movie = null;
			_gauntletLayer = null;
		}
		_vm = null;
		((MissionView)this).OnMissionScreenFinalize();
	}
}
