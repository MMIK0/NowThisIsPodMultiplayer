using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public GameObject lobbyPanel, roomPanel;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Dropdown dropDown; //lapOptions;
    private byte playerCountOnRoom;
    //private int lapAmount;

    public List<PlayerItem> playerList = new List<PlayerItem>();
    public PlayerItem playerPrefab;
    public Transform playerItemParent;

    //ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.Log("We connected to lobby");
    }

    public void CreateRoom()
    {
        if (createInput.text.Length >= 1)
        {
            playerCountOnRoom = byte.TryParse(dropDown.options[dropDown.value].text, out playerCountOnRoom) ? playerCountOnRoom : (byte)2;
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers = playerCountOnRoom, BroadcastPropsChangeToAll = true});
        }
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        UpdatePlayerList();
    }

    /*public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        if (PhotonNetwork.IsMasterClient)
        {
            lapOptions.gameObject.SetActive(true);
            roomProperties["LapAmount"] = 0;
        }
        UpdateLaps();
    }*/

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    /*public void UpdateLaps()
    {
        lapAmount = int.TryParse(lapOptions.options[lapOptions.value].text, out lapAmount) ? lapAmount : (int)3;
        PhotonNetwork.CurrentRoom.CustomProperties["LapAmount"] = lapAmount;
        Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["LapAmount"]);
    }*/

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerList)
        {
            Destroy(item.gameObject);
        }
        playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
            return;
        
        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayer = Instantiate(playerPrefab, playerItemParent);
            newPlayer.SetPlayerInfo(player.Value);
            playerList.Add(newPlayer);
        }
    }

}
