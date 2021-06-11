using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject fadePrefab;
    public PlayerScript player;
    public GameObject canvas;
    public float Rtime = 5;
    private GameObject image;

    //　ポーズUIのインスタンス
    private GameObject setCanvas;
    // Update is called once per frame
    private Fade fade;
    private FadeImage FadeImage;
    private GameObject gameover;
    private Text text;
    private bool ReStartON=false;
    float Nowtime = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Health <= 0)
        {
            if (setCanvas == null)
            {
                if (player.Fade)
                {
                    player.Fade = false;
                    setCanvas = canvas.transform.GetChild(1).gameObject;
                    image = setCanvas.transform.GetChild(1).gameObject;
                    image.SetActive(true);
                    setCanvas.SetActive(true);
                }
                else
                {
                    setCanvas = canvas.transform.GetChild(1).gameObject;
                    image = setCanvas.transform.GetChild(0).gameObject;
                    image.SetActive(true);
                    setCanvas.SetActive(true);
                }
                //Time.timeScale = 0f;
                ReStartON = true;
                Nowtime = Time.time;
            }
        }
        ReStart();
    }

    void ReStart()
    {
        if (ReStartON)
        {
            if (Time.time - Nowtime > Rtime)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
