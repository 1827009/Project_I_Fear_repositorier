using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bell : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip audioClip;

    float time;
    AudioSource audioSource;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Vector3 pos = transform.position;
        float y = Mathf.PerlinNoise(time, 0);
        pos.y = y;
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            audioSource.Play();
            if (player)
            {
                player.ItemGetingBell();
                Destroy(gameObject);
            }
        }
    }
}
