using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellSwingGeter : MonoBehaviour
{
    [SerializeField] PlayerScript player;
    [SerializeField, TooltipAttribute("鈴の音")] AudioClip bellSound;
    [SerializeField, TooltipAttribute("鈴の先端")] GameObject bellObj;
    Vector3 oldPosition;
    Vector3 oldMoveVector;
    Quaternion oldQuaternion;
    bool swingCoolTime;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        oldPosition = transform.position;
        swingCoolTime = true;
    }

    /// <summary>
    /// 呼び出す際に座標など更新
    /// </summary>
    public void Initialized()
    {
        oldPosition = transform.position;
        swingCoolTime = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVector = bellObj.transform.position - oldPosition;
        if (player.Fade)
        {
            Vector3 moveDirection = (moveVector - oldMoveVector).normalized;
            if (swingCoolTime && moveVector.magnitude < 0.01f)
                swingCoolTime = false;

            if (moveVector.magnitude > 0.03f && !swingCoolTime)
            {
                float swingSize = moveVector.magnitude * 25;
                if (swingSize < 10)
                {
                    player.BellSwing(3);
                    audioSource.PlayOneShot(bellSound, swingSize);
                    swingCoolTime = true;
                }
            }

        }
            oldMoveVector = bellObj.transform.position - oldPosition;
            oldPosition = bellObj.transform.position;
    }
}
