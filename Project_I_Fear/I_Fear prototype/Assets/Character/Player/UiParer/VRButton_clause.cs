using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRButton_clause : VRUIButton
{
    [SerializeField] MenuObjectScript parent;
    [SerializeField] Image cursorImage;
    GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.root.gameObject;
    }

    public override void InCursorPoint()
    {
        cursorImage.enabled = true;
    }
    public override void OutCursorPoint()
    {
        cursorImage.enabled = false;
    }
    public override void OnVRButton()
    {
        parent.Clause();
    }
}
