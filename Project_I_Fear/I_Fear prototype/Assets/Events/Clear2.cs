using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Clear2 : MonoBehaviour
{
    // Start is called before the first frame updat

	[SerializeField, TooltipAttribute("討伐数")] public int DeadCount = 1;
	[SerializeField, TooltipAttribute("討伐数UI")] Image DeadText;
	[SerializeField, TooltipAttribute("討伐数UI")] Sprite ClaerUI;
	[SerializeField]
	//　ポーズした時に表示するUIのプレハブ
	public GameObject fadePrefab;
	[SerializeField] Sprite[] DeadCountSprite;
	PlayerScript player;
	public GameObject canvas;
	public float Rtime = 3;
	//　ポーズUIのインスタンス
	private GameObject setCanvas;
	// Update is called once per frame
	private int DeadEnemy = 0;
	private Fade fade;
	private FadeImage FadeImage;
	private GameObject image;
	private Image GameClaerText;
	private bool ReStartON = false;
	[SerializeField] short nowStageNumber;
	float Nowtime = 0;
	float nextTime=5;
	bool nextOn;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
		DeadText.sprite = DeadCountSprite[DeadEnemy];
		fade = fadePrefab.GetComponent<Fade>();
		FadeImage = fadePrefab.GetComponent<FadeImage>();
		setCanvas = canvas.transform.GetChild(1).gameObject;
		image = setCanvas.transform.GetChild(0).gameObject;
		GameClaerText = image.GetComponent<Image>();
	}
	void Update()
	{
		if (DeadCount ==DeadEnemy)
		{
			if (Nowtime == 0)
			{
				if (player.Fade)
				{
					fade.FadeOut(2);
					player.Fade = false;
					GameClaerText.sprite = ClaerUI;
					image.SetActive(true);
					setCanvas.SetActive(true);
				}
				else
				{
					GameClaerText.sprite = ClaerUI;
					image.SetActive(true);
					setCanvas.SetActive(true);
				}
				//Time.timeScale = 0f;
				//ReStartON = true;
				Nowtime = Time.time;
			}
			NextStage();
		}
		//ReStart();
	}

	public int Dead()
	{
	
		DeadEnemy += 1;
		DeadText.sprite = DeadCountSprite[DeadEnemy];
		return DeadEnemy;

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

	void NextStage()
	{
		if (Time.time - Nowtime > nextTime)
		{
			switch (nowStageNumber)
			{
				case 0:
					SceneManager.LoadScene("Stage2");
					break;
				case 2:
					SceneManager.LoadScene("Stage3");
					break;
			}
		}
	}
}
