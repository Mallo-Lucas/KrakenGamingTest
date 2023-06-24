using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLevelAuidioPlayer : MonoBehaviour
{
    [SerializeField] private AudioPoolData poolData;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PlayerModel playerModel;

    private void Start()
    {
        LevelEventsHandler.Instance.SubscribeToFadeOutEvent(PlayMainTheme);
        playerModel.OnGetDamage += PlayLostTheme;
    }

    private void PlayMainTheme()
    {
        audioSource.clip = poolData.GetClipFromId("LevelTheme");
        audioSource.Play();
        audioSource.loop = true;
    }

    private void PlayLostTheme(int value)
    {
        audioSource.clip = poolData.GetClipFromId("LostTheme");
        audioSource.Play();
        audioSource.loop = false;
    }
}
