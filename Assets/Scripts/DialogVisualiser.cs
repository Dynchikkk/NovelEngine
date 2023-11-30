using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogVisualiser : MonoBehaviour
{
    [SerializeField] private TMP_Text _textPlace;
    [SerializeField] private TMP_Text _nameText;

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
    }

    // initialize dialog
    public void SetUpDialog()
    {
        _leftPlace.DefaultPlace = _leftPlace.Image.rectTransform.position;
        _rightPlace.DefaultPlace = _rightPlace.Image.rectTransform.position;
    }

    // initialize actor
    public void SetUpActor(NarratorConfig actor, NarratorPlaces narratorPlace, bool prevShow)
    {
        var tmp = GetActorByPlace(narratorPlace);
        tmp.moodPics = actor.MoodPics;

        ChangeMood(narratorPlace, NarratorMoods.Default);

        ChangeActorLight(narratorPlace, prevShow == false ?
            NarratorColorStates.Transparent : NarratorColorStates.Shaded);

        ChangeActorAction(narratorPlace, prevShow == false ? NarratorAction.MoveOut : NarratorAction.MoveIn);
    }

    // set actor name and text
    public void SetText(string text, string name)
    {
        _textPlace.text = text;
        _nameText.text = name;
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
                actor.Image.rectTransform.DOAnchorPos(actor.DefaultPlace, _moveSpeed);
                break;

            case NarratorAction.MoveOut:
                actor.Image.rectTransform.DOAnchorPos(actor.OutOfBoundsPlace, _moveSpeed);
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
                color.a = 255;
                break;

            case NarratorColorStates.Shaded:
                color.a = _shadeForce;
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
    public Vector3 OutOfBoundsPlace;
    public Vector3 DefaultPlace;
}

public enum NarratorPlaces
{
    Left = 0,
    Right = 1
}
