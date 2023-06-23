using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Audio/AudioPoolData", fileName = "AudioPoolData", order = 0)]
public class AudioPoolData : ScriptableObject
{
    [SerializeField] private List<AudioPool> audioPools;

    public AudioClip GetClipFromId(string id)
    {
        return audioPools.FirstOrDefault(x => x.auidio_ID == id).clip;
    }


    [Serializable]
    public class AudioPool 
    {
        public string auidio_ID;
        public AudioClip clip;
    }
}
