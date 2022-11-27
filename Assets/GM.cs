using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [SerializeField] Canvas canvasMenu;
    [SerializeField] Canvas canvasGameplay;
    [SerializeField] Canvas canvasGameOver;
    [SerializeField] ObstacleMgr _ObstacleMgr;
    [SerializeField] Rigidbody _playerRb;
    [SerializeField] TMPro.TextMeshProUGUI _textScore;
    [SerializeField] Transform explodeParticles;
    [SerializeField] Text finalScore;
    float startZ = 0F;
    int score = 0;
    bool isGame = false;

    void GameOver(PlayerController player)
    {
        Instantiate(explodeParticles, player.gameObject.transform.position, Quaternion.identity);
        finalScore.text = score.ToString();
        
        canvasGameOver.gameObject.SetActive(true);
        canvasGameplay.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        PlayerController.PlayerDied += GameOver;
    }

    void OnDisable()
    {
        PlayerController.PlayerDied -= GameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        if (PlayerPrefs.GetInt("isRestarting", 0) == 1)
        {
            PlayerPrefs.SetInt("isRestarting", 0);
            StartGame();
            return;
        }

        canvasGameOver.gameObject.SetActive(false);
        canvasMenu.gameObject.SetActive(true);
        canvasGameplay.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        score = (int) (_playerRb.transform.position.z - startZ);
        _textScore.text = score.ToString();
    }

    public void StartGame()
    {
        if (isGame)
            return;

        startZ = _playerRb.transform.position.z;
        _ObstacleMgr.gameObject.SetActive(true);
        
        canvasGameOver.gameObject.SetActive(false);
        canvasMenu.gameObject.SetActive(false);
        canvasGameplay.gameObject.SetActive(true);

        isGame = true;
        PlayerController.isGame = true;
    }
}
