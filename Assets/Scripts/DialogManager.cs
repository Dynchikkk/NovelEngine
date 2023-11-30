using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private DialogVisualiser _visualizer;
    [SerializeField] private List<DialogConfig> _dialogConfigs = new();

    private readonly List<Dialog> _dialogs = new();

    private void Start()
    {
        _visualizer.Init();

        foreach (var dialogConf in _dialogConfigs)
        {
            var temp = new Dialog(dialogConf, _visualizer);
            _dialogs.Add(temp);
        }

        _dialogs[0].StartDialog();
    }

    [ContextMenu("Make step")]
    public void Test()
    {
        _dialogs[0].MakeStep();
    }
}
