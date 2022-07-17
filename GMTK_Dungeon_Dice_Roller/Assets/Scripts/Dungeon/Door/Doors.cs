using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorsTween))]
public class Doors : MonoBehaviour
{
    private DoorsTween _doorsTween;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Doors Subscribing");
        LevelManager.OnLevelStart += new LevelManager.CallbackAction(LockDoors);
        LevelManager.OnLevelBeaten +=new LevelManager.CallbackAction(OpenDoors);

        _doorsTween = GetComponent<DoorsTween>();
    }

    void LockDoors() {
        Debug.Log("Locking Doors");
        _doorsTween.Lock();
    }

    void OpenDoors() {
         Debug.Log("Opening Doors");
        _doorsTween.Open();
        
    }
}
