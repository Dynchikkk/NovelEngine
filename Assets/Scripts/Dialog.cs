using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
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
    }

    public void StartDialog(int step = 0)
    {
        _currentStep = step;
        _visualiser.SetUpDialog();

        _lastNarrator = _config.DialogSteps[0].Actor;

        foreach (var item in _config.DialogSteps)
        {
            if(item.Actor != null && !_actors.Contains(item.Actor))
                _actors.Add(item.Actor);
        }

        _visualiser.SetUpActor(_actors[0], NarratorPlaces.Left, false);
        _places.Add(_actors[0], NarratorPlaces.Left);

        _visualiser.SetUpActor(_actors[1], NarratorPlaces.Right, false);
        _places.Add(_actors[1], NarratorPlaces.Right);

        MakeStep();
    }

    public void MakeStep()
    {
        // if text is not fully write yet force write it
        if(_visualiser.IsTyping)
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
            var actorPlace = _places[curActor];
            // change transparency of current actor
            _visualiser.ChangeActorLight(actorPlace, curStep.ColorState);

            // change transparency of second actor
            if (curStep.ColorState == NarratorColorStates.Default)
            {
                // if second acctor is not transparent make him shaded
                var secondActorPlace = actorPlace == NarratorPlaces.Left ? NarratorPlaces.Right : NarratorPlaces.Left;
                if (_visualiser.GetActorByPlace(secondActorPlace).Image.color.a != 0)
                    _visualiser.ChangeActorLight(secondActorPlace, NarratorColorStates.Shaded);
            }

            // set up other parametres of current actor
            _visualiser.ChangeActorAction(actorPlace, curStep.Action);
            _visualiser.ChangeMood(actorPlace, curStep.Mood);
        }

        // type text
        _visualiser.SetText(curStep.Text, curActor.NarratorName);

        _currentStep++;
        if (curStep.Text == "")
            MakeStep();
    }
}
