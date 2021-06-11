using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjectScript : MonoBehaviour
{
    [SerializeField] AudioClip openSE;
    [SerializeField] AudioClip clauseSE;
    [HideInInspector] public PlayerScript player;
    // Start is called before the first frame update
    private void Update()
    {
        if (InputController.MenuModeSwitch())
        {
            Clause();
        }
    }
    public void Open(PlayerScript p)
    {
        player = p;
        player.OnMenuMode = true;
        Time.timeScale = 0;

        player.vrGUiController.canvas = gameObject.transform.GetChild(0).gameObject.GetComponent<Canvas>();

        if (openSE != null)
            AudioSource.PlayClipAtPoint(clauseSE, transform.position);
    }
    public void Clause()
    {
        player.OnMenuMode = false;
        Time.timeScale = 1;
        Destroy(gameObject);

        player.vrGUiController.canvas = null;

        if (openSE != null)
            AudioSource.PlayClipAtPoint(clauseSE, transform.position);
    }
}
