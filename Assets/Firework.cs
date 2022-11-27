using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    public static event System.Action Collected;

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            Collected?.Invoke();
            Destroy(gameObject);
        }
    }
}
