using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogConfig", menuName = "Configs/Dialog Config")]
public class DialogConfig : ScriptableObject
{
    public NarratorConfig Teller;
    public List<DialogStep> DialogSteps = new List<DialogStep>();
}

[Serializable]
public class DialogStep
{
    public bool IsTellerStep = false;
    public NarratorConfig Actor = null; // of null => actor from prev step
    public NarratorAction Action = NarratorAction.Stand;
    public NarratorColorStates ColorState = NarratorColorStates.Default;
    public NarratorMoods Mood = NarratorMoods.Same;
    public string Text;
}

public enum NarratorAction
{
    Stand = 0,
    MoveIn = 1,
    MoveOut = 2
}

public enum NarratorColorStates
{
    Default = 0, // who is silent is darkened
    Shaded = 1,
    Transparent = 2
}
