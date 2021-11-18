using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class EndMenu : MonoBehaviourPunCallbacks
{
    public static EndMenu instance;

    Dictionary<Player, float> timeDict = new Dictionary<Player, float>();
    HashSet<Player> playerList = new HashSet<Player>();
    public TextMeshProUGUI tmp;
    private string playerName;
    private float playerTime;
    public GameObject endPanel;
    PhotonView view;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        view = GetComponent<PhotonView>();
    }


    public void CheckPlayers()
    {
        if (playerList.Count != PhotonNetwork.PlayerList.Length)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Debug.Log("Täällä ollaan loopíssa" + " " + playerList.Count + " " + PhotonNetwork.PlayerList.Length);
                if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["FinishedRace"] == true)
                {
                    Player _player = PhotonNetwork.PlayerList[i];
                    view.RPC(nameof(SetRaceCompleted), RpcTarget.All, _player);

                    if (playerList.Count >= PhotonNetwork.PlayerList.Length)
                    {
                        Debug.Log(playerList.Count);
                        endPanel.SetActive(true);
                        AddPlayersToDict();
                    }
                }
            }
        }
        else
        {
            endPanel.SetActive(true);
            AddPlayersToDict();
        }
    }

    [PunRPC]
    void SetRaceCompleted(Player player)
    {
        playerList.Add(player);
    }


    public void AddPlayersToDict()
    {
        foreach(Player pl in PhotonNetwork.PlayerList)
        {
            timeDict.Add(pl, (float)pl.CustomProperties["FastestLap"]);
        }
        CalculateWinner();
    }

    public void CalculateWinner()
    {
        var min = timeDict.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        playerName = min.NickName;
        playerTime = timeDict[min];
        Debug.Log(playerName + " " + playerTime);

        tmp.text = playerName + " " + TimeParser(playerTime);
    }

    public void RestartGame()
    {
        PhotonNetwork.LoadLevel("ReloadScene");
    }

    public void OnClickReturnToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }

    public string TimeParser(float time)
    {
        float minutes = Mathf.Floor((time) / 60);
        float seconds = Mathf.Floor((time) % 60);
        float msecs = Mathf.Floor(((time) * 100) % 100);

        return (minutes.ToString() + ":" + seconds.ToString("00") + ":" + msecs.ToString("00"));
    }
}
