using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : UIView<MainMenuModel>
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _author;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _readProgress;
    [SerializeField] private Button _startReadButton;

    public override void UpdateView(MainMenuModel uiModel)
    {
        base.UpdateView(uiModel);

        _name.text = uiModel.Config.Name;
        _author.text = uiModel.Config.Author;
        _description.text = uiModel.Config.Description;
        var dialog = DialogManager.Instance.GetDialogByConfig(uiModel.Config);

        _readProgress.text = 
            $"{dialog.CurrentStep} out of {dialog.StepCount}";

        _startReadButton.onClick.RemoveAllListeners();
        _startReadButton.onClick.AddListener(() => DialogManager.Instance.PickDialog(uiModel.Config));
    }

    public override void Hide()
    {
        base.Hide();
        _startReadButton.onClick.RemoveAllListeners();
    }
}

public class MainMenuModel : UIModel
{
    public DialogConfig Config;
}
