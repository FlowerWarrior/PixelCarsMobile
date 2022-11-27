using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsMgr : MonoBehaviour
{
    [SerializeField] Text amountText;
    [SerializeField] Animator _animator;

    void CoinCollected()
    {
        int coins = PlayerPrefs.GetInt("coins", 0);
        coins++;
        PlayerPrefs.SetInt("coins", coins);
        
        _animator.Play("CoinCollected", 0, 0);

        amountText.text = coins.ToString();
    }

    void OnEnable()
    {
        Coin.Collected += CoinCollected;
    }

    void OnDisable()
    {
        Coin.Collected -= CoinCollected;
    }
}
