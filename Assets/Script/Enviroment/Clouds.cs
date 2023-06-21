using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.position += (transform.forward + transform.right).normalized * speed * Time.deltaTime;
    }
}
