using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUIController : MonoBehaviour
{
    [SerializeField] GameObject beamPoint;
    [SerializeField] GameObject hitPointEffect;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public bool OnMenuMode;
    GameObject effect;
    VRUIButton tompButton;

    PlayerScript player;

    private void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.OnMenuMode)
            RayPush();
        else
        {
            Destroy(effect);
            effect = null;
        }

    }

    void RayPush()
    {
        // rayを飛ばす
        RaycastHit hitObj;
        Ray ray = new Ray(beamPoint.transform.position, beamPoint.transform.forward);
        Physics.Raycast(ray, out hitObj, 10, LayerMask.GetMask("FadeFlont"));

        // 衝突位置にエフェクトを生成・追従
        if (hitObj.transform != null)
        {
            if (effect)
            {
                effect.transform.position = hitObj.point;
            }
            else
            {
                effect = Instantiate(hitPointEffect, hitObj.point, canvas.transform.rotation);
                effect.transform.parent = canvas.transform;
            }
        }
        else
        {
            Destroy(effect);
            effect = null;
        }

        VRUIButton button = null;
        if (hitObj.transform != null && hitObj.transform.gameObject.GetComponent<VRUIButton>()) 
        {
            button = hitObj.transform.gameObject.GetComponent<VRUIButton>();

            // Gizmos表示
            Debug.DrawRay(ray.origin, ray.direction, Color.green);

            // 押されたボタンの動作
            if (button)
            {
                if (tompButton != button)
                {
                    button.InCursorPoint();
                }

                button.OnCursorPoint();

                if (InputController.UIDecision())
                    button.OnVRButton();
            }
        }
        if ((tompButton != null) && (tompButton != button))
        {
            tompButton.OutCursorPoint();
        }

        tompButton = button;
    }

}
