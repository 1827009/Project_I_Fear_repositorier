using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, TooltipAttribute("カメラ")] GameObject cameraObject;
    [SerializeField, TooltipAttribute("追従する対象")] GameObject playerObject;
    [SerializeField, TooltipAttribute("クリックモードでの感度")] float clickRotaSpeed = 10;
    [SerializeField, TooltipAttribute("スムーズモードでの感度")] float smoothRotaSpeed = 1;

    bool clickInputOld;

    /// <summary>
    /// 向きの操作法
    /// </summary>
    public enum CameraControlMode
    {
        Click,
        Smooth,
        Tracking
    }
     CameraControlMode controlMode;
    /// <summary>
    /// 向きの操作法
    /// </summary>
    public CameraControlMode ControlMode
    {
        get { return controlMode; }
        set
        {
            controlMode = value;
            switch (controlMode)
            {
                case CameraControlMode.Click:
                    break;
                case CameraControlMode.Smooth:
                    break;
                case CameraControlMode.Tracking:
                    break;
            }
        }
    }
    Action updateAction;


    void ClickModeUpdate()
    {
        if (InputController.Camera().x > 0.5f)
        {
            //if (!clickInputOld)
            {
                Quaternion quaternion = playerObject.transform.rotation;
                quaternion.eulerAngles += new Vector3(0, clickRotaSpeed, 0);
                playerObject.transform.SetPositionAndRotation(playerObject.transform.position, quaternion);
            }
            clickInputOld = true;
        }
        else if (InputController.Camera().x < 0.5f)
        {
            //if (!clickInputOld)
            {
                Quaternion quaternion = playerObject.transform.rotation;
                quaternion.eulerAngles -= new Vector3(0, clickRotaSpeed, 0);
                playerObject.transform.SetPositionAndRotation(playerObject.transform.position, quaternion);
            }
            clickInputOld = true;
        }
        else
            clickInputOld = false;
    }
    void SmoothModeUpdate()
    {
        if (InputController.Camera().x != 0)
        {
            Quaternion quaternion = playerObject.transform.rotation;
            quaternion.eulerAngles += new Vector3(0, smoothRotaSpeed, 0);
            playerObject.transform.SetPositionAndRotation(playerObject.transform.position, quaternion);
            clickInputOld = true;
        }
    }
    void TrackingModeUpdate()
    {
        Quaternion cameraQuaternion = new Quaternion(0, -cameraObject.transform.rotation.y, 0, cameraObject.transform.rotation.w);
        playerObject.transform.SetPositionAndRotation(transform.position, cameraQuaternion);
    }

    // Start is called before the first frame update
    void Start()
    {
        updateAction =new Action(ClickModeUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = playerObject.transform.position;
        updateAction();
    }

}
