using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSclipt : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerScript player;
    public Image image;

    private Sprite sprite;
    float oldHP;
    void Start()
    {
        image = this.GetComponent<Image>();
        HPChek();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Health!=oldHP)
        {
            HPChek();
        }

        oldHP = player.Health;


    }
    void HPChek()
    {
        switch (player.Health)
        {
            case 0:
                sprite = Resources.Load<Sprite>("UI1124");
                break;
            case 1:
                sprite = Resources.Load<Sprite>("UI1124(1)");
                break;
            case 2:
                sprite = Resources.Load<Sprite>("UI1124(2)");
                break;
            case 3:
                sprite = Resources.Load<Sprite>("UI1124(3)");
                break;
            default:
                break;
        }
        image.sprite = sprite;
    }
}

