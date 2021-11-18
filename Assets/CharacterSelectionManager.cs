using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    public Button playButton;

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            playButton.interactable = true;
        }
        else
            playButton.interactable = false;
    }

    public void PlayButtonOnClick()
    {
        PhotonNetwork.LoadLevel("StageOne");
    }
}
