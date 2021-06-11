using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIButton : MonoBehaviour
{
    [SerializeField] AudioClip pushSE;
    public virtual void InCursorPoint()
    {
        
    }

    public virtual void OutCursorPoint()
    {

    }

    public virtual void OnCursorPoint()
    {

    }

    /// <summary>
    /// ボタンとして押されたとき
    /// </summary>
    public virtual void OnVRButton()
    {
        if(pushSE!=null)
        AudioSource.PlayClipAtPoint(pushSE, transform.position);
    }
}
