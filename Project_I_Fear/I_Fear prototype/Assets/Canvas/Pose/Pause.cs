using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
	
	[SerializeField]
	//　ポーズした時に表示するUIのプレハブ
	private GameObject pauseUIPrefab;
	//　ポーズUIのインスタンス
	private GameObject pauseUIInstance;

	// Update is called once per frame
	void Update()
	{
		
			if (Input.GetKeyDown("p"))
			{
				if (pauseUIInstance == null)
				{
				if (Time.timeScale == 1f)
				{
					pauseUIInstance = GameObject.Instantiate(pauseUIPrefab) as GameObject;
					Time.timeScale = 0f;
				}
				}
				else
				{
					Destroy(pauseUIInstance);
					Time.timeScale = 1f;
				}
			}
		
	}
}
