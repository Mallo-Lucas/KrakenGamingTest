using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFXAudioPoolPlayer : MonoBehaviour
{
    [SerializeField] private AudioPoolData poolData;
    [SerializeField] private AudioSource audioSource;

    public void PlayAuidioById(string id)
    {
        audioSource.Stop();
        var clip = poolData.GetClipFromId(id);
        if (clip == null)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
