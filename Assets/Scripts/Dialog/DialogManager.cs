using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public Dialog CurrentDialog { get; private set; }
    public bool IsInDialog { get; private set; }

    [SerializeField] private DialogVisualiser _visualizer;
    [SerializeField] private DialogConfig _dialogConfigTest;

    private Dictionary<DialogConfig, Dialog> _dialogs = new();

    private void Start()
    {
        _visualizer.Init();
        _visualizer.ChangeVisualiserCondition(false);
    }

    public void PickDialog(DialogConfig dialogConfig)
    {
        _dialogs.TryAdd(dialogConfig, new Dialog(dialogConfig, _visualizer));
        var dialog = _dialogs[dialogConfig];
        CurrentDialog = dialog;

        dialog.StartDialog();
        IsInDialog = true;
    }

    [ContextMenu("Make step")]
    public void Test()
    {
        PickDialog(_dialogConfigTest);
    }
}
