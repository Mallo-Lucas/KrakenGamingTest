using KrakenGamingTest.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPowerUp : MonoBehaviour
{
    [SerializeField] private PlayerAbility powerUp;
    [SerializeField] private GameObject powerUpVisual;

    private bool _canUsePowerUp;

    private void Start()
    {
        _canUsePowerUp = true;
        LevelEventsHandler.Instance.SubscribeToFadeInEvent(() => EnablePowerUp(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_canUsePowerUp)
            return;
        if(other.TryGetComponent(out PlayerModel player))
        {
            player.AddAbility(powerUp);
            EnablePowerUp(false);
        }
    }

    public void EnablePowerUp(bool state)
    {
        _canUsePowerUp = state;
        if (!state)
        {
            powerUpVisual.SetActive(state);
            return;
        }
        powerUpVisual.SetActive(state);        
    }
}
