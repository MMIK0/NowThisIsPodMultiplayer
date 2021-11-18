using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public List<GameObject> shipList = new List<GameObject>();

    public GameObject playerShip;
    public GameObject[] spawnPoints;
    private int spawnIndex;
    //PhotonView pv;

    public void Awake()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            spawnPoints[i].gameObject.SetActive(true);
        }

        //pv = GetComponent<PhotonView>();

        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Start()
    {
        Debug.Log("How many instances is here?");

        for (int i = 0; i < shipList.Count; i++)
        {
            if (i == (int)PhotonNetwork.LocalPlayer.CustomProperties["PlayerShip"])
            {
                playerShip = shipList[i];
            }
        }

        if (spawnIndex >= spawnPoints.Length)
            spawnIndex = 0;

        Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        if (PlayerControls.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(playerShip.name, pos, Quaternion.identity);
        }
        else
        {
            PlayerControls.LocalPlayerInstance.gameObject.transform.position = pos;
            PlayerControls.LocalPlayerInstance.gameObject.transform.rotation = Quaternion.identity;
        }

    }
}
