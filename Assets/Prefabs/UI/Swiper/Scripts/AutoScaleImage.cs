using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaleImage : MonoBehaviour
{
    private const float TimeBetweenScreenChangeCalculations = 0.5f;
    private float _lastScreenChangeCalculationTime = 0;

    private RectTransform _image;
    private Vector2 _lastScreenWidth;

    private RectTransform _sizer;
    SwipeSnapMenu _swipeSnapMenu;

    public void Init(SwipeSnapMenu swipeSnapMenu, RectTransform sizer)
    {
        _lastScreenChangeCalculationTime = Time.time;
        _image = GetComponent<RectTransform>();
        _lastScreenWidth = new Vector2(Screen.width, Screen.height);

        _sizer = sizer;
        _swipeSnapMenu = swipeSnapMenu;

        _image.sizeDelta = new Vector2(sizer.rect.width , sizer.rect.height);
    }

    private void Update()
    {
        if (_lastScreenWidth.x == Screen.width && _lastScreenWidth.y == Screen.height)
            return;

        if (Time.time - _lastScreenChangeCalculationTime < TimeBetweenScreenChangeCalculations)
            return;

        _lastScreenChangeCalculationTime = Time.time;

        _image.sizeDelta = new Vector2(_sizer.rect.width, _sizer.rect.height);
        _swipeSnapMenu.SelectTab(_swipeSnapMenu.SelectedTabIndex);

        _lastScreenWidth = new Vector2(Screen.width, Screen.height);

        Debug.Log($"Window dimensions changed to {Screen.width}x{Screen.height}");
    }
}
