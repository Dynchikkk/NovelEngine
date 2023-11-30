using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarratorConfig", menuName = "Configs/Narrator Config")]
public class NarratorConfig : ScriptableObject
{
    public string NarratorName;
    public List<NarratorState> MoodPics = new(); 
}

[Serializable]
public class NarratorState
{
    public NarratorMoods Mood;
    public Sprite Sprite;
}

public enum NarratorMoods
{
    Same = 0,
    Default = 1,
    Angry = 2, 
    Sad = 3
}
