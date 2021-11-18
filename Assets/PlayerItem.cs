using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI tmp;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    Player player;

    public void Awake()
    {
        playerProperties["PlayerShip"] = 0;
        playerProperties["FastestLap"] = 0f;
        playerProperties["FinishedRace"] = false;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void SetPlayerInfo(Player thePlayer)
    {
        tmp.text = thePlayer.NickName;
        player = thePlayer;
        UpdatePlayerItem(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("PlayerShip"))
        {
            Debug.Log("PlayerShip is there");
        }
        else
            playerProperties["PlayerShip"] = 0;
    }

}
