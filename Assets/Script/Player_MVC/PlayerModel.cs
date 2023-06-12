using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrakenGamingTest.Player
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;

        private void Move(int axis)
        {
            rb.MovePosition(rb.transform.position + rb.transform.forward * axis * Time.deltaTime);
        }
    }
}
