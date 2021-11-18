using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField userName;

    public void Start()
    {
        userName.characterLimit = 10;
    }
    public void Update()
    {
        if (userName.text.Length > userName.characterLimit)
        {
            userName.text.Remove(userName.text.Length - 1);
        }
    }

    public void OnClickConnectToLobby()
    {
        userName.characterLimit = 10;

        if(userName.text.Length >= 1)
        {
            PhotonNetwork.NickName = userName.text;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
