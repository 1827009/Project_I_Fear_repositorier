using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePape_ItemScript : MonoBehaviour
{
    //public AudioClip audioClip;
    //AudioSource audioSource;
    [SerializeField, TooltipAttribute("表示するUI")] GameObject messagePrefab;
    [SerializeField] GameObject itemGraphic;

    PlayerScript player;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerScript>();
            transform.parent = other.transform;
            player.Fade = false;

            //AudioSource.PlayClipAtPoint(audioClip, transform.position);

            GameObject menuWindow = Instantiate(messagePrefab, player.transform.position + player.transform.forward, player.transform.rotation);
            menuWindow.GetComponent<MenuObjectScript>().Open(player);
            Destroy(gameObject);
        }
    }
}
