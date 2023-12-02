using Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("Contollers")]
    [SerializeField] private EndReadMenuController _endReadMenuController;
    [SerializeField] private ReadMenuController _readMenuController;
    [SerializeField] private MainMenuController _mainMenuController;

    private void Start()
    {
        _endReadMenuController.Init();
        _readMenuController.Init();
        _mainMenuController.Init();

        DialogManager.Instance.OnDialogPick += StartReading;
        DialogManager.Instance.OnDialogFinished += ShowEndReadMenu;

        HideAll();
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        HideAll();
        _mainMenuController.Show();
    }

    public void ShowEndReadMenu()
    {
        HideAll();
        _endReadMenuController.Show();
    }

    private void StartReading(DialogConfig config)
    {
        HideAll();
        _readMenuController.Show();
    }

    private void HideAll()
    {
        _endReadMenuController.Hide();
        _readMenuController.Hide();
        _mainMenuController.Hide();
    }
}
