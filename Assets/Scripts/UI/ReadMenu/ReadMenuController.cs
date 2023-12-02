using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMenuController : UIController<ReadMenuView, ReadMenuModel>
{
    public override void Init()
    {
        base.Init();

        var model = new ReadMenuModel();
        _view.Init(model);
    }
}
