using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class UiListener : MonoBehaviour
{
    public Text lapInfo, cpInfo;
    PhotonView view;

    public void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void Update()
    {
        lapInfo.text = RaceController.instance.LapInfoText.text;
        cpInfo.text = RaceController.instance.CheckpointInfoText.text;
    }
}
