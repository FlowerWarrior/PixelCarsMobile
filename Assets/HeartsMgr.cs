using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsMgr : MonoBehaviour
{
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;
    int lastHealth;
    void OnEnable() 
    {
        PlayerController.PlayerTookDamage += UpdateHearts;
    }

    void OnDisable() 
    {
        PlayerController.PlayerTookDamage -= UpdateHearts;
    }

    void Start()
    {
        lastHealth = PlayerController.health;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        int maxHealth = PlayerController.maxHealth;
        int health = PlayerController.health;

        // duplicate hearts if not enough
        for (int i = transform.childCount; i < maxHealth; i++)
        {
            Instantiate(transform.GetChild(0), transform.position, Quaternion.identity, transform);
        }

        for (int i = 0; i < health; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = heartFull;
        }
        for (int i = health; i < maxHealth; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = heartEmpty;
        }

        if (health < maxHealth)
        {
            int lostHealth = maxHealth - health;
            transform.GetChild(lastHealth - lostHealth).GetComponent<Animator>().Play("HeartLost", 0, 0);
        }
    }
}
