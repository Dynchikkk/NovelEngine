using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarratorConfig", menuName = "Configs/Narrator Config")]
public class NarratorConfig : ScriptableObject
{
    public string NarratorName;
    public List<NarratorState> moodPics = new(); 
}

[Serializable]
public class NarratorState
{
    public NarratorMoods Mood;
    public Sprite Sprite;
}

public enum NarratorMoods
{
    Default = 0,
    Angry = 1, 
    Sad = 2
}
