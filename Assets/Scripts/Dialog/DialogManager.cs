using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    public event Action<DialogConfig> OnDialogPick;
    public event Action OnDialogFinished;

    public DialogConfig CurrentDialog { get; private set; }
    public bool IsInDialog { get; private set; }

    [SerializeField] private DialogVisualiser _visualizer;
    [SerializeField] private List<DialogConfig> _dialogConfigs;

    private readonly Dictionary<DialogConfig, Dialog> _dialogs = new();

    protected override void Awake()
    {
        base.Awake();

        _visualizer.Init();
        _visualizer.ChangeVisualiserCondition(false);

        foreach (DialogConfig config in _dialogConfigs)
            _dialogs.Add(config, new Dialog(config, _visualizer));
    }

    public void PickDialog(DialogConfig dialogConfig)
    {
        var dialog = _dialogs[dialogConfig];
        dialog.OnDialogFinish += DialogEnd;
        CurrentDialog = dialogConfig;

        OnDialogPick?.Invoke(dialogConfig);

        dialog.StartDialog(_dialogs[dialogConfig].CurrentStep);
        IsInDialog = true;
    }

    public Dialog GetDialogByConfig(DialogConfig config)
    {
        return _dialogs[config];
    }

    public IList<DialogConfig> GetDialogConfigs()
    {
        return _dialogConfigs.AsReadOnly();
    }

    private void DialogEnd(Dialog d)
    {
        OnDialogFinished?.Invoke();
        d.OnDialogFinish -= DialogEnd;
    }

    //[ContextMenu("Make step")]
    //public void Test()
    //{
    //    PickDialog(_dialogConfigs[0]);
    //}
}
