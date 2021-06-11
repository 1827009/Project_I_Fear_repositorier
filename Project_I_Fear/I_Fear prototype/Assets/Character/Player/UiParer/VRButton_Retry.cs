using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VRButton_Retry : VRUIButton
{
    [SerializeField] MenuObjectScript parent;
    [SerializeField] Image cursorImage;
    GameObject canvas;
    bool onRetry = false;
    Fade fade;
    const float fadeTime = 0;
    float nowFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (onRetry)
        {
            nowFadeTime -= Time.deltaTime;
            if (nowFadeTime < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);                
            }
        }
    }

    public override void InCursorPoint()
    {
        cursorImage.enabled = true;
    }
    public override void OutCursorPoint()
    {
        cursorImage.enabled = false;
    }
    public override void OnVRButton()
    {
        onRetry = true;
        nowFadeTime = fadeTime;
        Time.timeScale = 1;
    }
}
