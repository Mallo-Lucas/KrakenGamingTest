using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public bool gamePlayed;

    public float GeneralVolume;
    public float MusicVolume;
    public float EffectsVolume;
    public float currentGameScore;

    public List<float> maxScores;
    public List<string> playerNames;
}
