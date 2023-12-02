using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MainMenuController : UIController<MainMenuView, MainMenuModel> 
{
    [SerializeField] private SwipeMenuController _swiper;

    public override void Init()
    {
        base.Init();

        _swiper.Init();

        foreach (var config in DialogManager.Instance.GetDialogConfigs())
        {
            _swiper.AddItems(config);
        }

        _swiper.OnTabSelected += TabSelected;
    }

    private void OnDestroy()
    {
        _swiper.OnTabSelected -= TabSelected;
    }

    public override void Show()
    {
        base.Show();
        var model = new MainMenuModel()
        {
            Config = _swiper.SelectedTab
        };

        _view.UpdateView(model);
    }

    private void TabSelected(DialogConfig config)
    {
        var model = new MainMenuModel()
        {
            Config = config
        };

        _view.UpdateView(model);
    }
}
