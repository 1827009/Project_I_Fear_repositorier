using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemEvent : MonoBehaviour
{
    PlayerScript playerCanvas;
    [SerializeField] GameObject stageNoImagePrefab;
    [SerializeField] Sprite stageNoImage;
    OVRManager VRManager;

    // Start is called before the first frame update
    void Start()
    {
        playerCanvas = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        VRManager = new OVRManager();
        stageNoImagePrefab.GetComponentInChildren<Image>().sprite = stageNoImage;

        GameObject noImage = Instantiate(stageNoImagePrefab, playerCanvas.transform.position + (transform.forward*1.5f), playerCanvas.transform.rotation);

        noImage.transform.SetParent(playerCanvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();

    }
    private void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
