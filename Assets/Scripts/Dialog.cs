using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    public event Action<DialogStep> ChangeStep;
    private int _startStep = 0;

    private int _currentStep = 0;
    private DialogConfig _dialog;

    public Dialog(DialogConfig dialogConfig)
    {
        _dialog = dialogConfig;
        _startStep = dialogConfig.StartStep;
    }

    public void StartDialog()
    {
        _currentStep = _startStep;
        ChangeStep?.Invoke(_dialog.DialogSteps[_currentStep]);
    }

    public void NextStep()
    {
        _currentStep++;
        ChangeStep?.Invoke(_dialog.DialogSteps[_currentStep]);
    }
}
