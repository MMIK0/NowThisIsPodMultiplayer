using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public enum PhotonEventCodes
{
    timerUpdate = 1,
}

public class RaceController : MonoBehaviourPunCallbacks
{
    public static RaceController instance;

    public int lapsInRace;
    public Text LapInfoText;
    public Text CheckpointInfoText;

    private int nextCheckpointNumber;
    private int checkpointCount;
    private int lapCount;
    private float lapStartTime;
    private bool isRaceActive;
    // Laptimes get stored in a list
    private List<float> lapTimes = new List<float>();
    private Checkpoint activeCheckpoint;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        //lapsInRace = (int)PhotonNetwork.CurrentRoom.CustomProperties["LapAmount"];
        if (lapsInRace < 3)
            lapsInRace = 3;

        StartCoroutine(CountDown(4.20));
    }

    IEnumerator CountDown(double seconds)
    {
        double count = seconds;
        while (count > 0) { 
         lapStartTime = Time.time;
        yield return new WaitForSeconds(1);
        count --;
     }
        isRaceActive = true; 
        nextCheckpointNumber = 0;
        lapCount = 0;
        checkpointCount = this.transform.childCount;
        
        // Assign each of the checkpoints its own number in order in Hierarchy
         
        for (int i = 0; i < checkpointCount; i++)
        {
            Checkpoint cp = transform.GetChild(i).GetComponent<Checkpoint>();
            cp.checkpointNumber = i;
            cp.isActiveCheckpoint = false;
        }
        StartRace(); 
    }
   
    // Update is called once per frame
    void Update()
    {
        if(isRaceActive)
        {
            LapInfoText.text = TimeParser(Time.time - lapStartTime);
        }
        else
        {
            LapInfoText.text = "";
            CheckpointInfoText.text = "";
        }
    }

    public void StartRace()
    {
        activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
        activeCheckpoint.isActiveCheckpoint = true;
        lapStartTime = Time.time;

    }
   
    public void CheckpointPassed(Player player)
    {
        if (player != PhotonNetwork.LocalPlayer)
            return;

        CheckpointInfoText.text = ("CHECKPOINT " + (nextCheckpointNumber + 1) + " / " + checkpointCount + "\nLAP " + (lapCount + 1) + " / " + lapsInRace);
        activeCheckpoint.isActiveCheckpoint = false;
        nextCheckpointNumber++;


        if (nextCheckpointNumber < checkpointCount)
        {
            activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
            activeCheckpoint.isActiveCheckpoint = true;
        }
        // If a lap was finished, we enter the new lap, and the checkpoint-counter is reset
        else
        {
            // Add the laptime to the list of laptimes
            lapTimes.Add(Time.time - lapStartTime);
            float fastestLapTime = lapTimes[0];
            foreach(float time in lapTimes)
            {
                if (time <= fastestLapTime)
                {
                    fastestLapTime = time;
                    player.CustomProperties["FastestLap"] = fastestLapTime;
                    object[] content = new object[] { player.NickName, fastestLapTime };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCodes.timerUpdate, content, raiseEventOptions, SendOptions.SendReliable);
                }
            }
            lapCount++;

            // Reset the lap timer
            lapStartTime = Time.time;
            nextCheckpointNumber = 0;
            // If the finished lap wasn't the last lap
            if (lapCount < lapsInRace)
            {
                activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
                activeCheckpoint.isActiveCheckpoint = true;
            }
            // If final lap, end the game and calculate results
            else
            {
                isRaceActive = false;
                player.CustomProperties["FinishedRace"] = true;
                Debug.Log((bool)player.CustomProperties["FinishedRace"] + " " + player.NickName);
                EndMenu.instance.CheckPlayers();
                float raceTotalTime = 0.0f;
                for (int i = 0; i < lapsInRace; i++)
                {
                    // Compare the laptimes to pick fastest
                    if (lapTimes[i] < fastestLapTime)
                    {
                        fastestLapTime = lapTimes[i];
                    }
                    // Count total time
                    raceTotalTime += lapTimes[i];
                }
            }
        }
    }

    public string TimeParser(float time)
    {
       
        float minutes = Mathf.Floor((time) / 60);
        float seconds = Mathf.Floor((time) % 60);
        float msecs = Mathf.Floor(((time) * 100) % 100);

        return (minutes.ToString() + ":" + seconds.ToString("00") + ":" + msecs.ToString("00"));

         }
}

        

