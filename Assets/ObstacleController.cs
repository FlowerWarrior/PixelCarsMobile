using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] boolDir randomizeDir;
    [SerializeField] Transform meshT;

    [System.Serializable]
    struct boolDir 
    {
        public bool x;
        public bool y;
        public bool z;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        int dir = Random.Range(0, 4);
        Vector3 rot = transform.rotation.eulerAngles;

        if (randomizeDir.x)
        {
            rot.x = dir * 90;
        }
        if (randomizeDir.y)
        {
            rot.y = dir * 90;
        }
        if (randomizeDir.z)
        {
            rot.z = dir * 90;
        }      
        

        meshT.rotation = Quaternion.Euler(rot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            PlayerController.PlayerHit?.Invoke();
            Destroy(gameObject);
        }
    }
}
