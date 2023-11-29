using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogConfig", menuName = "Configs/Dialog Config")]
public class DialogConfig : ScriptableObject
{
    public int StartStep = 1;

    public NarratorConfig Teller;
    public List<DialogStep> DialogSteps = new List<DialogStep>();
}

[Serializable]
public class DialogStep
{
    public NarratorConfig Actor = null;
    public NarratorMoods State = NarratorMoods.Default;
    public NarratorAction Action = NarratorAction.Stand;
    public NarratorColorStates ColorState = NarratorColorStates.Default;
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
