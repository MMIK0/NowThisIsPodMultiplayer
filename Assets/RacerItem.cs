using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RacerItem : MonoBehaviour, IOnEventCallback
{
    public string playerName;
    public int playerNumber;
    public float lapTime;
    public TextMeshProUGUI tmp;
    public Player player;
    public PhotonView pv;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Awake()
    {
        Debug.Log(PhotonNetwork.PlayerList.Length);
        if (PhotonNetwork.PlayerList.Length <= playerNumber)
        {
            gameObject.SetActive(false);
            return;
        }

        player = PhotonNetwork.PlayerList[playerNumber];
        playerName = player.NickName;
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == (byte)PhotonEventCodes.timerUpdate)
        {
            object[] data = (object[])photonEvent.CustomData;

            if ((string)data[0] != playerName)
                return;

            lapTime = (float)data[1];
            UpdateTimes();
        }
    }

    public void Start()
    {
        UpdateTimes();
    }

    public void Update()
    {
        tmp.text = playerName + " " + TimeParser(lapTime);
    }

    public void UpdateTimes()
    {
        pv.RPC("PLT", RpcTarget.All, playerName, lapTime);
    }

    [PunRPC]
    void PLT(string kek, float time)
    {
        if (playerName == kek)
            lapTime = time;
    }

    public string TimeParser(float time)
    {

        float minutes = Mathf.Floor((time) / 60);
        float seconds = Mathf.Floor((time) % 60);
        float msecs = Mathf.Floor(((time) * 100) % 100);

        return (minutes.ToString() + ":" + seconds.ToString("00") + ":" + msecs.ToString("00"));

    }
}
