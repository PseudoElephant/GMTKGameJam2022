using System;
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
        LevelManager.OnLevelStart += LevelManagerOnOnLevelStart();
        LevelManager.OnLevelBeaten += LevelManagerOnOnLevelBeaten();

        _doorsTween = GetComponent<DoorsTween>();
    }

    private LevelManager.CallbackAction LevelManagerOnOnLevelBeaten()
    {
        return new LevelManager.CallbackAction(OpenDoors);
    }

    private LevelManager.CallbackAction LevelManagerOnOnLevelStart()
    {
        return new LevelManager.CallbackAction(LockDoors);
    }

    void LockDoors()
    {
        AudioManager.Play("sfx_DoorClose");
        Debug.Log("Locking Doors");
        _doorsTween.Lock();
    }

    void OpenDoors() {
        AudioManager.Play("sfx_DoorOpen");
         Debug.Log("Opening Doors");
        _doorsTween.Open();
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelStart -= LevelManagerOnOnLevelStart();
        LevelManager.OnLevelBeaten -= LevelManagerOnOnLevelBeaten();
    }
}
