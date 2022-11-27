using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartGame()
    {
        Debug.Log("clicked restart");
        PlayerPrefs.SetInt("isRestarting", 1);

        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }
}
