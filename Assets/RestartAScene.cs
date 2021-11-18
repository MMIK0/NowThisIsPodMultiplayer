using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RestartAScene : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log(PhotonNetwork.PlayerList.Length);

        PhotonNetwork.LoadLevel("StageOne");
    }
}
