using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PauseMenu : MonoBehaviourPunCallbacks
{
    public bool gameIsPaused = false;
    public GameObject pauseMenuUi, Restartbutton;
    //public PhotonView view;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
        /*if (view.IsMine)
        {
        }*/
    }
    
    public void Resume()
    {
        /*if (!view.IsMine)
            return;*/

        if (PhotonNetwork.IsMasterClient)
            Restartbutton.SetActive(false);
        gameIsPaused = false;
        pauseMenuUi.SetActive(false);
        Debug.Log("We should resume" + pauseMenuUi.activeInHierarchy);
    }

    void Pause ()
    {
        /*if (!view.IsMine)
            return;*/

        pauseMenuUi.SetActive(true);
        gameIsPaused = true;
        if (PhotonNetwork.IsMasterClient)
            Restartbutton.SetActive(true);
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
}
