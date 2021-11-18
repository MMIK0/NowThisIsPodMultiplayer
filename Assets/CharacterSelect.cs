using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class CharacterSelect : MonoBehaviour
{
    public int ShipNumber;

    public void SelectCurrentShip()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["PlayerShip"] = ShipNumber;
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["PlayerShip"]);
    }
}
