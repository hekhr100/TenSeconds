using UnityEditor.Tilemaps;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Door : MonoBehaviour
{
    public bool OpenDoor;
    private Collider2D BoxColl;

    private void Start()
    {
        BoxColl = GetComponent<BoxCollider2D>(); 
    }
    public void UpdateDoor(bool dooropen)
    {
            OpenDoor = dooropen;
            if (OpenDoor)
            {
                //Open Door
                print("Open Door");
                BoxColl.enabled = false;
            }
            else
            {
                //CloseDoor
                print("Close Door");
                BoxColl.enabled = true;
            }
    }
}
