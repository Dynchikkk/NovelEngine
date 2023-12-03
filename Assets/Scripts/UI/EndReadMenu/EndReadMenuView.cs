using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndReadMenuView : UIView<EndReadMenuModel>
{
    [SerializeField] private TMP_Text _congratText;
    [SerializeField] private Image _dialogIcon;
    [SerializeField] private Button BackToMenuButton;

    public override void Init(EndReadMenuModel uiModel)
    {
        base.Init(uiModel);

        BackToMenuButton.onClick.AddListener(UIManager.Instance.ShowMainMenu);
    }

    private void OnDestroy()
    {
        BackToMenuButton.onClick.RemoveAllListeners();
    }

    public override void UpdateView(EndReadMenuModel uiModel)
    {
        base.Init(uiModel);
        _congratText.text = 
            $"Ñongratulations on reading the {uiModel.Config.Name} from the {uiModel.Config.Author}";

        _dialogIcon.sprite = uiModel.Config.Icon;
    }
}

public class EndReadMenuModel : UIModel
{
    public DialogConfig Config;
}
