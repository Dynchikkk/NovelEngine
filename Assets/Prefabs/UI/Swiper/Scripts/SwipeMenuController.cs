using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenuController : MonoBehaviour
{
    public event Action<DialogConfig> OnTabSelected;

    public DialogConfig SelectedTab
    {
        get
        {
            return GetConfigByIndex(_swipeSnapMenu.SelectedTabIndex);
        }
    }

    public int ElementsCount => _items.Count;

    [SerializeField] private SwipeMenuType _swipeMenuType;

    [Header("SwipeSnapMenu Attribute")]
    [SerializeField] private SwipeSnapMenu _swipeSnapMenu;
    [SerializeField] private RectTransform _contentConteiner;
    [SerializeField] private float _snapSpeed = 15f;
    [SerializeField] ScrollRect _scrollRect;

    [SerializeField] private GameObject _itemPrefab;
    private readonly List<GameObject> _items = new();

    [Header("UI")]
    [SerializeField] private Button _nextItemButton;
    [SerializeField] private Button _previousItemButton;


    private readonly Dictionary<int, DialogConfig> _dialogConfigs = new();

    public void Init()
    {
        switch (_swipeMenuType)
        {
            case SwipeMenuType.Horizontal:
                MakeHorizontal();
                break;

            case SwipeMenuType.Vertical:
                MakeVertical();
                break;
        }

        _swipeSnapMenu.OnTabSelected += TabSelected;

        SubUI();
    }

    private void TabSelected(int value) =>
        OnTabSelected?.Invoke(GetConfigByIndex(value));

    public void SelectTab(int num) =>
        _swipeSnapMenu.SelectTab(num);

    public void SelectTab(DialogConfig dialog)
    {
        for (int num = 0; num < _dialogConfigs.Count; num++)
        {
            if (_dialogConfigs[num] == dialog)
            {
                SelectTab(num);
                return;
            }
        }
    }

    public void MakeHorizontal()
    {
        if(_contentConteiner.TryGetComponent(out VerticalLayoutGroup toDestr))
            Destroy(toDestr);
        HorizontalLayoutGroup layoutGroup = null;
        if (!_contentConteiner.TryGetComponent(out layoutGroup))
            layoutGroup = _contentConteiner.AddComponent<HorizontalLayoutGroup>();

        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        _scrollRect.vertical = false;
        _scrollRect.horizontal = true;
        _swipeSnapMenu.Init(_contentConteiner, _scrollRect.horizontalScrollbar, _snapSpeed);
    }

    public void MakeVertical()
    {
        if (_contentConteiner.TryGetComponent(out HorizontalLayoutGroup toDestr))
            Destroy(toDestr);
        VerticalLayoutGroup layoutGroup = null;
        if (!_contentConteiner.TryGetComponent(out layoutGroup))
            layoutGroup = _contentConteiner.AddComponent<VerticalLayoutGroup>();

        layoutGroup.childControlHeight = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        _scrollRect.vertical = true;
        _scrollRect.horizontal = false;
        _swipeSnapMenu.Init(_contentConteiner, _scrollRect.verticalScrollbar, _snapSpeed);
    }

    public void SubUI()
    {
        _nextItemButton.onClick.AddListener(delegate
        {
            SlideNext();
        });

        _previousItemButton.onClick.AddListener(delegate
        {
            SlidePrevious();
        });
    }

    public void UnSubUI()
    {
        _nextItemButton.onClick.RemoveAllListeners();
        _previousItemButton.onClick.RemoveAllListeners();
    }

    private void SlideNext()
    {
        int index = _swipeSnapMenu.SelectedTabIndex;
        _swipeSnapMenu.SelectTab(index + 1);
    }

    private void SlidePrevious() 
    {
        int index = _swipeSnapMenu.SelectedTabIndex;
        _swipeSnapMenu.SelectTab(index - 1);
    }

    private DialogConfig GetConfigByIndex(int value)
    {
        return _dialogConfigs[value];
    }

    public void AddItems(DialogConfig config)
    {
        var item = Instantiate(_itemPrefab, _contentConteiner);
        item.GetComponent<Image>().sprite = config.Icon;

        _dialogConfigs.Add(_items.Count, config);
        _items.Add(item);

        _swipeSnapMenu.RecalculatePositions();

        item.AddComponent<AutoScaleImage>().Init(this, _scrollRect.viewport);

        _swipeSnapMenu.RecalculatePositions();
    }
}

public enum SwipeMenuType
{
    Horizontal = 0,
    Vertical = 1
}
