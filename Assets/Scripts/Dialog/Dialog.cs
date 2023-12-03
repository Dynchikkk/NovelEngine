using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    public event Action<Dialog> OnDialogFinish;
    public int CurrentStep => _currentStep;
    public int StepCount => _config.DialogSteps.Count;

    private DialogConfig _config;
    private DialogVisualiser _visualiser;

    private int _currentStep = 0;
    private NarratorConfig _lastNarrator;

    private List<NarratorConfig> _actors = new();
    private Dictionary<NarratorConfig, NarratorPlaces> _places = new();

    public Dialog(DialogConfig config, DialogVisualiser visualiser)
    {
        _config = config;
        _visualiser = visualiser;
        _visualiser.OnActorNeedToRemove += RemoveActor;
    }

    public void StartDialog(int step = 0)
    {
        _currentStep = step;
        _visualiser.SetUpDialog();
        _visualiser.SetUpBg(_config.DefaultBg);

        if(_lastNarrator == null)
            _lastNarrator = _config.DialogSteps[0].Actor;

        foreach (var item in _config.DialogSteps)
        {
            if(item.Actor != null && !_actors.Contains(item.Actor))
                _actors.Add(item.Actor);
        }

        MakeStep();
    }

    private void SetUpActor(NarratorConfig act)
    {
        if (act == null)
            return;

        if (_places.ContainsKey(act))
            return;

        var tmpPlace = NarratorPlaces.Left;
        foreach (var item in _places.Values)
        {
            if (tmpPlace == item)
                tmpPlace += 1;
        }

        _places.Add(act, tmpPlace);
        _visualiser.SetUpActor(act, tmpPlace);
    }

    private void RemoveActor(NarratorPlaces actorPlace)
    {
        foreach (var item in _places.Keys)
        {
            if (_places[item] == actorPlace)
            {
                _places.Remove(item);
                _actors.Remove(item);
                break;
            }
        }
    }

    private void ShadeListener(NarratorPlaces actorPlace)
    {
        foreach (NarratorPlaces place in Enum.GetValues(typeof(NarratorPlaces)))
        {
            if (place == actorPlace) continue;

            var actByPlace = _visualiser.GetActorByPlace(place);
            if (actByPlace == null) continue;
            if (actByPlace.Image.color.a == 0) continue;

            _visualiser.ChangeActorLight(place, NarratorColorStates.Shaded);
        }
    }

    public void MakeStep()
    {
        if (_currentStep >= _config.DialogSteps.Count)
        {
            OnDialogFinish?.Invoke(this);
            return;
        }

        // if text is not fully write yet force write it
        if (_visualiser.IsTyping)
        {
            _visualiser.ForceStopWriting();
            return;
        }

        var curStep = _config.DialogSteps[_currentStep];

        NarratorConfig curActor = curStep.IsTellerStep ? _config.Teller : curStep.Actor;
        if (curActor == null)
            curActor = _lastNarrator;
        else
            _lastNarrator = curActor;

        if (curStep.IsTellerStep)
        {
            // shade both actor because of teller talk
            _visualiser.ChangeActorLight(NarratorPlaces.Left, NarratorColorStates.Shaded);
            _visualiser.ChangeActorLight(NarratorPlaces.Right, NarratorColorStates.Shaded);
        }
        else
        {
            SetUpActor(curActor);
            var actorPlace = _places[curActor];
            // change transparency of current actor
            _visualiser.ChangeActorLight(actorPlace, curStep.ColorState);
            // change transparency of other actor
            if (curStep.ColorState == NarratorColorStates.Default)
                ShadeListener(actorPlace);

            // set up other parametres of current actor
            _visualiser.ChangeActorAction(actorPlace, curStep.Action);
            _visualiser.ChangeMood(actorPlace, curStep.Mood);

            if(curStep.Action == NarratorAction.Destroy)
                _places.Remove(curActor);
        }

        // type text
        _visualiser.SetText(curStep.Text, curActor.NarratorName);

        _currentStep++;
        if (curStep.Text == "")
            MakeStep();
    }
}
