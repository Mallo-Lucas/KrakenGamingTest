using KrakenGamingTest.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace KrakenGamingTest.LevelObjects
{
    public class Stairs : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerModel player))
                player.SetCanClimb(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerModel player))
                player.SetCanClimb(false);
        }
    }
}
