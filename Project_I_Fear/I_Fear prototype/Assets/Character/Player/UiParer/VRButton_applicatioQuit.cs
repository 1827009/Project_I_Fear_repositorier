using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton_applicatioQuit : VRUIButton
{
    public override void OnVRButton()
    {
        Application.Quit();
    }
}
