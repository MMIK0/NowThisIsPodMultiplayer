using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckView : MonoBehaviourPun
{
    public PhotonView view;

    public void Awake()
    {
        if (!view.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
