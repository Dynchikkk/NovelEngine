using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private List<DialogConfig> _dialogConfigs = new();

    private readonly List<Dialog> _dialogs = new();

    private void Start()
    {
        foreach (var dialogConf in _dialogConfigs)
        {
            var temp = new Dialog(dialogConf);
            _dialogs.Add(temp);
        }
    }
}
