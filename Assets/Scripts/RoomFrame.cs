using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFrame : MonoBehaviour
{
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;

    private bool leftDoorClosed = true;
    private bool rightDoorClosed = true;
    private bool topDoorClosed = true;
    private bool bottomDoorClosed = true;

    public bool LeftDoorClosed
    {
        get { return leftDoorClosed; }
        set
        {
            leftDoorClosed = value;
            leftDoor.SetActive(leftDoorClosed);
        }
    }
    public bool RightDoorClosed 
    { 
        get { return rightDoorClosed; } 
        set 
        {
            rightDoorClosed = value;
            rightDoor.SetActive(rightDoorClosed);
        } 
    }

    public bool TopDoorClosed 
    { 
        get { return topDoorClosed; } 
        set
        {
            topDoorClosed = value;
            topDoor.SetActive(topDoorClosed);
        }
    }

    public bool BottomDoorClosed 
    { 
        get { return bottomDoorClosed; } 
        set 
        { 
            bottomDoorClosed = value;
            bottomDoor.SetActive(bottomDoorClosed);
        }
    }
}
