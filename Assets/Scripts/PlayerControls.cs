using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerControls : MonoBehaviour
{
    public static GameObject LocalPlayerInstance;

    public float thrustSpeed;
    public float turnSpeed;
    public float hoverPower;
    public float hoverHeight;

    private float thrustInput;
    private float turnInput;
    private Rigidbody shipRigidBody;
    PhotonView view;
    public Camera cam;
    public bool finishedRace;

    public void Awake()
    {
        PhotonNetwork.LocalPlayer.CustomProperties["FinishedRace"] = false;
        view = GetComponent<PhotonView>();

        if (view.IsMine)
            LocalPlayerInstance = this.gameObject;

        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        shipRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            if((bool)PhotonNetwork.LocalPlayer.CustomProperties["FinishedRace"] != true)
            {
                thrustInput = Input.GetAxis("Vertical_3");
                turnInput = Input.GetAxis("Horizontal_3");

                if (GameManager.instance.countDownDone == true ) 
                {
                    // Turning the ship
                    shipRigidBody.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);

                    // Moving the ship
                    shipRigidBody.AddRelativeForce(0f, 0f, thrustInput * thrustSpeed);

                    // Hovering
                    Ray ray = new Ray(transform.position, -transform.up);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, hoverHeight))
                    {
                        float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                        Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverPower;
                        shipRigidBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
                    }
                }
            }
        }
    }
}
