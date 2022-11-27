using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static System.Action Collected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            Collected?.Invoke();
            Destroy(gameObject);
        }
    }
}
