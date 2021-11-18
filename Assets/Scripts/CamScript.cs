using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CamScript : MonoBehaviour
{
    public GameObject playerShip;
    PhotonView view;
    [SerializeField] private Transform target = null;
    [SerializeField] private float distance = 3.0f;
    [SerializeField] private float height = 1.0f;
    [SerializeField] private float damping = 5.0f;
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float rotationDamping = 10.0f;
 
    [SerializeField] private Vector3 targetLookAtOffset; // allows offsetting of camera lookAt, very useful for low bumper heights
    [SerializeField] private float bumperDistanceCheck = 5f; // length of bumper ray
    [SerializeField] private float bumperCameraHeight = 1.0f; // adjust camera height while bumping
    [SerializeField] private Vector3 bumperRayOffset; // allows offset of the bumper ray from target origin

    void Start()
    {
        transform.position = playerShip.transform.TransformPoint(0f, height, -distance);
        view = GetComponent<PhotonView>();
    }
 
    private void FixedUpdate() 
    {
        if (view.IsMine)
        {
            /*Vector3 camDist = Vector3.zero;
            if (Physics.Raycast(transform.TransformDirection(0, 0, 1), Vector3.Normalize(transform.position - playerShip.transform.position), out RaycastHit hitto, 4f))
            {
                Debug.Log(hitto.collider);
                camDist = playerShip.transform.TransformDirection(0,0, hitto.distance);
            }*/
            Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
            // check to see if there is anything behind the target
            RaycastHit hit;
            Vector3 one = target.transform.TransformDirection(-1 * Vector3.zero);
            // cast the bumper ray out from rear and check to see if there is anything behind

            if (Physics.Raycast(target.TransformPoint(bumperRayOffset), one, out hit, bumperDistanceCheck)) // ignore ray-casts that hit the user. DR
            {
                // clamp wanted position to hit position
                wantedPosition.x = hit.point.x;
                wantedPosition.z = hit.point.z;
                wantedPosition.y = Mathf.Lerp(hit.point.y + bumperCameraHeight, wantedPosition.y, Time.deltaTime * damping);
            }
            //wantedPosition += camDist;
            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
            Vector3 lookPosition = target.TransformPoint(targetLookAtOffset);
            if (smoothRotation)
            {
                Quaternion wantedRotation = Quaternion.LookRotation(lookPosition - transform.position, target.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
            }
            else
                transform.rotation = Quaternion.LookRotation(lookPosition - target.position, target.up);
        }
        else
            this.gameObject.SetActive(false);
    }
 }
