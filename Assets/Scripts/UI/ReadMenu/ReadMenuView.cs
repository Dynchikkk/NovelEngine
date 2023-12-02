using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadMenuView : UIView<ReadMenuModel>
{
    [SerializeField] private Button BackToMainMenuButton;

    public override void Show()
    {
        base.Show();
        BackToMainMenuButton.onClick.AddListener(UIManager.Instance.ShowMainMenu);
    }

    public override void Hide()
    {
        base.Hide();
        BackToMainMenuButton.onClick.RemoveAllListeners();
    }
}

public class ReadMenuModel : UIModel
{

}
