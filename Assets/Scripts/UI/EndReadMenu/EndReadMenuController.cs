using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndReadMenuController : UIController<EndReadMenuView, EndReadMenuModel>
{
    public override void Init()
    {
        base.Init();

        var model = new EndReadMenuModel()
        {
            Config = DialogManager.Instance.CurrentDialog
        };

        _view.Init(model);
    }

    public override void Show()
    {
        base.Show();
        UpdateView();
    }

    public override void UpdateView()
    {
        base.UpdateView();

        var model = new EndReadMenuModel()
        {
            Config = DialogManager.Instance.CurrentDialog
        };

        _view.UpdateView(model);
    }
}
