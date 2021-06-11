using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleEvent : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    bool playerStart;
    [SerializeField] GameObject messagePrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerStart)
        {
            PlayerScript.FadeVolume = 1;
            playerStart = true;
            player.Update();
            player.ControlFrieze = true;
            Time.timeScale = 1;
            GameObject menuWindow = Instantiate(messagePrefab, player.transform.position + player.transform.forward, player.transform.rotation);
            menuWindow.GetComponent<MenuObjectScript>().Open(player);
        }
        if(OVRInput.Get(OVRInput.RawButton.A)|| OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            SceneManager.LoadScene("SampleScene");

    }
}
