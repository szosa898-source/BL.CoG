// ============================================================================
//  PATCH (Ścieżka 2 – prawdziwy przełącznik MCM)  —  CnKMpBarVM.cs
// ----------------------------------------------------------------------------
//  Zastąp istniejącą metodę Refresh(Hero hero) w klasie BL_CnK_Dev.CnKMpBarVM
//  tą wersją. Jedyna różnica względem oryginału to sprawdzenie ustawienia
//  CnKSettings.Instance.ShowMpBar PRZED pokazaniem paska.
// ============================================================================

public void Refresh(Hero hero)
{
    // Master switch z MCM: gdy wyłączony, pasek MP nigdy się nie pokazuje.
    var settings = CnKSettings.Instance;
    if (settings != null && !settings.ShowMpBar)
    {
        IsVisible = false;
        return;
    }

    Campaign current = Campaign.Current;
    MagicBehavior magicBehavior = current != null ? current.GetCampaignBehavior<MagicBehavior>() : null;
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
