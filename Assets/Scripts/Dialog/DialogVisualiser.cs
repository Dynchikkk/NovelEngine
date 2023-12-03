using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogVisualiser : MonoBehaviour
{
    public event Action<NarratorPlaces> OnActorNeedToRemove;
    public bool IsTyping { get; private set; } = false;

    [SerializeField] private TMP_Text _textPlace;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Image _bg;
    [SerializeField, Range(0, 1)] private float _textWriteCD = 0.5f;

    [Header("Step action pararms")]
    [SerializeField, Range(0, 255)] private int _shadeForce = 150;
    [SerializeField] private float _shadeSpeed = 2f;
    [SerializeField] private float _moveSpeed = 2f;

    [Header("PLaces")]
    [SerializeField] private PlacePararms _leftPlace;
    [SerializeField] private PlacePararms _rightPlace;

    private Dictionary<NarratorPlaces, PlacePararms> _actors = new();

    public void Init()
    {
        _actors.Add(NarratorPlaces.Left, _leftPlace);
        _actors.Add(NarratorPlaces.Right, _rightPlace);
        ChangeVisualiserCondition(false);
    }

    // initialize dialog
    public void SetUpDialog()
    {
        ChangeVisualiserCondition(true);
        //_leftPlace.DefaultPlace = _leftPlace.Image.rectTransform.anchoredPosition;
        //_rightPlace.DefaultPlace = _rightPlace.Image.rectTransform.anchoredPosition;

        //_leftPlace.OutOfBoundsPlace = 
    }

    public void SetUpBg(Sprite img) =>
        _bg.sprite = img;
    
    public void ChangeVisualiserCondition(bool condition)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(condition);
        }
    }

    public void ForceStop()
    {
        IsTyping = false;

        DOTween.CompleteAll(true);

    }

    // initialize actor
    public void SetUpActor(NarratorConfig actor, NarratorPlaces narratorPlace, bool prevShow = false)
    {
        var tmp = GetActorByPlace(narratorPlace);
        tmp.moodPics = actor.MoodPics;

        ChangeMood(narratorPlace, NarratorMoods.Default);

        //ChangeActorLight(narratorPlace, prevShow == false ?
        //    NarratorColorStates.Transparent : NarratorColorStates.Shaded);

        //ChangeActorAction(narratorPlace, prevShow == false ? NarratorAction.MoveOut : NarratorAction.MoveIn);
    }

    // set actor name and text
    public void SetText(string text, string name)
    {
        _nameText.text = name;
        StartCoroutine(WriteText(text));
    }

    //public void ForceStopWriting() =>
    //    IsTyping = false;

    private IEnumerator WriteText(string text)
    {
        _textPlace.text = "";
        IsTyping = true;
        for (int i = 0; i < text.Length; i++)
        {
            _textPlace.text += text[i];
            yield return new WaitForSeconds(_textWriteCD);
            if (!IsTyping)
                break;
        }

        _textPlace.text = text;
        IsTyping = false;
    }

    public PlacePararms GetActorByPlace(NarratorPlaces place)
    {
        return _actors[place];
    }

    // Move actor
    public void ChangeActorAction(NarratorPlaces place, NarratorAction action)
    {
        var actor = GetActorByPlace(place);

        switch (action)
        {
            case NarratorAction.MoveIn:
                actor.Image.rectTransform.DOAnchorPos(actor.DefaultPlace.anchoredPosition, _moveSpeed);
                break;

            case NarratorAction.MoveOut:
                actor.Image.rectTransform.DOAnchorPos(actor.OutOfBoundsPlace.anchoredPosition, _moveSpeed);
                break;

            case NarratorAction.Destroy:
                OnActorNeedToRemove?.Invoke(place);
                break;

            case NarratorAction.Stand:
                break;
        }
    }

    // Change actor light (by default a talking = 255, a listener if not transpare = shade force)
    public void ChangeActorLight(NarratorPlaces place, NarratorColorStates state)
    {
        var actor = GetActorByPlace(place).Image;
        var color = actor.color;

        switch (state)
        {
            case NarratorColorStates.Default:
                color.a = 1;
                break;

            case NarratorColorStates.Shaded:
                color.a = (float)(_shadeForce / 255.0);
                break;

            case NarratorColorStates.Transparent:
                color.a = 0;
                break;
        }

        actor.DOColor(color, _shadeSpeed);
    }

    // Change actor mood pics
    public void ChangeMood(NarratorPlaces place, NarratorMoods mood)
    {
        if (mood == NarratorMoods.Same)
            return;

        var actor = GetActorByPlace(place);
        var moodPic = actor.moodPics.Find(pic => pic.Mood == mood).Sprite;
        actor.Image.sprite = moodPic;
    }
}

[Serializable]
public class PlacePararms
{
    public Image Image;

    public List<NarratorState> moodPics;
    public RectTransform DefaultPlace;
    public RectTransform OutOfBoundsPlace;
}

public enum NarratorPlaces
{
    Left = 0,
    Right = 1
}
