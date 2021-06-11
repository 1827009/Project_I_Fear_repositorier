using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUiController : MonoBehaviour
{
    //[SerializeField, TooltipAttribute("体力")] Text healthText;
    float fadeAlpha;
    float fadeSpeed = 2;
    [SerializeField, TooltipAttribute("ダメージ画面効果用のUIパネル")] GameObject damagePanel;
    float damageColorAlpha;
    float flashingSpan;
    [SerializeField, TooltipAttribute("時間経過テキスト")] Text countDownText;
    float countDown = 0;
    [SerializeField, TooltipAttribute("フェイドのプレハブ")] Fade fade;
    [SerializeField, TooltipAttribute("フェードアウトしたときに表示が消えるRenderer")] Renderer[] fadeOutRenderer;

    PlayerScript player;
    //Image fadeImage;
    Image damegeImage;

    delegate void UpdateDelegate();
    bool fadein=true;
    bool fadeOutComplete = false;
    UpdateDelegate updateDelegate;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponentする
        player = GetComponent<PlayerScript>();
        damegeImage = damagePanel.gameObject.GetComponent<Image>();
        
        // 更新イベント
        updateDelegate = new UpdateDelegate(FadeOutUpdate);
        updateDelegate += CountDownUpdate;

        // 表示のため一度更新
        HealthTextUpdate();

        // フェードインアウト時間の秒数返還
        fadeSpeed = 1 / fadeSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        updateDelegate();
    }

    /// <summary>
    /// ダメージエフェクト実行
    /// </summary>
    public void DamageEffect()
    {
        damageColorAlpha = 1;
        updateDelegate += DamageColorUpdate;
    }
    /// <summary>
    /// ダメージエフェクトを更新
    /// </summary>
    void DamageColorUpdate()
    {
        /*if (player.Health == 1)
        {
            // 点滅
            flashingSpan += Time.deltaTime;
            damageColorAlpha = Mathf.Sin(flashingSpan % Mathf.PI);
        }
        else*/
        {
            if (damageColorAlpha > 0)
            {
                damegeImage.color = new Color(damegeImage.color.r, damegeImage.color.g, damegeImage.color.b, damageColorAlpha);
                damageColorAlpha -= Time.deltaTime;
            }
            else
                updateDelegate -= DamageColorUpdate;
        }
    }

    /// <summary>
    /// フェードイン、フェードアウトのスクリーンを更新
    /// と、音の増減
    /// </summary>
    void FadeOutUpdate()
    {
        if (player.Fade)
        {
            if (fadeAlpha < 1)
                fadeAlpha += fadeSpeed * Time.deltaTime;
            else if (!fadeOutComplete)
            {
                fadeOutComplete = true;
                foreach (Renderer renderer in fadeOutRenderer)
                    renderer.enabled = false;
            }
            PlayerScript.FadeVolume = fadeAlpha;
            if (!fadein)
            {
                foreach (Renderer renderer in fadeOutRenderer)
                    renderer.enabled = true; 

                fade.FadeIn(2);
                fadein = true;
            }
        
        }
        if (!player.Fade)
        {
            if (fadeAlpha > 0)
                fadeAlpha -= fadeSpeed * Time.deltaTime;
            PlayerScript.FadeVolume = fadeAlpha;

            if (fadein)
            {
                fade.FadeOut(2);
                fadein = false;
                fadeOutComplete = false;
                foreach (Renderer renderer in fadeOutRenderer)
                    renderer.enabled = true;
            }
        }
    }

    public void HealthTextUpdate()
    {
        //healthText.text = "HP:" + player.Health;
    }

    public void CountDownUpdate()
    {
        countDown += Time.deltaTime;
        countDownText.text = "経過:" + countDown;
    }
}
