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
    [SerializeField, Range(0, 255)] private int _shadeForce;
    [SerializeField] private float _shadeSpeed = 2f;
    [SerializeField] private float _moveSpeed = 2f;

    [Header("PLaces")]
    [SerializeField] private ActorParams _leftPlace;
    [SerializeField] private ActorParams _rightPlace;

    private Dictionary<NarratorPlaces, ActorParams> _actors;

    public void Init()
    {
        _actors.Add(NarratorPlaces.Left, _leftPlace);
        _actors.Add(NarratorPlaces.Right, _rightPlace);
    }

    public void SetUpDialog(NarratorConfig leftActor, NarratorConfig rightActor, bool prevShowLeft = false, bool prevShowRight = false)
    {
        for (int i = 0; i < leftActor.moodPics.Count; i++)
        {
            _leftPlace.moodPics[i] = leftActor.moodPics[i];
            _rightPlace.moodPics[i] = rightActor.moodPics[i];
        }

        ChangeMood(NarratorPlaces.Left, NarratorMoods.Default);
        ChangeMood(NarratorPlaces.Right, NarratorMoods.Default);

        _leftPlace.DefaultPlace = _leftPlace.Image.rectTransform.position;
        _rightPlace.DefaultPlace = _rightPlace.Image.rectTransform.position;

        ChangeActorLight(NarratorPlaces.Left, prevShowLeft == false ? 
            NarratorColorStates.Transparent : NarratorColorStates.Shaded);

        ChangeActorLight(NarratorPlaces.Right, prevShowLeft == false ?
           NarratorColorStates.Transparent : NarratorColorStates.Shaded);

        MoveActor(NarratorPlaces.Left, prevShowLeft == false ? NarratorAction.MoveOut : NarratorAction.MoveIn);
        MoveActor(NarratorPlaces.Right, prevShowLeft == false ? NarratorAction.MoveOut : NarratorAction.MoveIn);
    }

    private ActorParams GetActorByPlace(NarratorPlaces place)
    {
        return _actors[place];
    }

    public void MoveActor(NarratorPlaces place, NarratorAction action)
    {
        var actor = GetActorByPlace(place);

        switch (action)
        {
            case NarratorAction.MoveIn:
                actor.Image.rectTransform.DOMove(actor.DefaultPlace, _moveSpeed);
                break;

            case NarratorAction.MoveOut:
                actor.Image.rectTransform.DOMove(actor.OutOfBoundsPlace, _moveSpeed);
                break;

            case NarratorAction.Stand:
                break;
        }
    }

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

    public void ChangeMood(NarratorPlaces place, NarratorMoods mood)
    {
        var actor = GetActorByPlace(place);
        var moodPic = actor.moodPics.Find(pic => pic.Mood == mood).Sprite;
        actor.Image.sprite = moodPic;
    }
}

[Serializable]
public class ActorParams
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
