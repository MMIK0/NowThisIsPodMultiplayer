using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PlayerTimers : MonoBehaviourPunCallbacks
{
    public List<GameObject> racers = new List<GameObject>();
    public Transform playerTimeParent;
    public RacerItem racer;
    public PhotonView view;

    public void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    public void Start()
    {
        UpdateTimers();
    }

    public void UpdateTimers()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            RacerItem newRacer = Instantiate(racer, playerTimeParent);
            newRacer.transform.SetParent(playerTimeParent);
            newRacer.GetComponent<RacerItem>().player = p;
        }
    }    
}
