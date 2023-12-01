using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private KeyCode _makeStepKey;
    [SerializeField] private Button _makeStepButton;

    private DialogManager _dialogManager;

    private void Awake()
    {
        _dialogManager = GetComponent<DialogManager>();
        _makeStepButton?.onClick.AddListener(MakeStep);
    }

    private void Update()
    {
        if (Input.GetKeyDown(_makeStepKey))
        {
            MakeStep();
        }
    }

    private void MakeStep()
    {
        if (!_dialogManager.IsInDialog)
            return;

        _dialogManager.CurrentDialog.MakeStep();
    }
}
