using KrakenGamingTest.Player;
using UnityEngine;

namespace KrakenGamingTest.LevelObjects
{
    public class Stairs : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerModel player))
                player.SetPhysicsToClimb(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerModel player))
                player.SetPhysicsToClimb(false);
        }
    }
}
