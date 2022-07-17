using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class DoorTransport : MonoBehaviour 
{

    [Header("Scene Loader")] 
    [Tooltip("Loads a Scene")]
    public string sceneName = "";


    void Start()
    {
        LevelManager.OnLevelStart  += new LevelManager.CallbackAction(DisableTrigger);
        LevelManager.OnLevelBeaten += new LevelManager.CallbackAction(EnableTrigger);
    }

    void DisableTrigger() {
        gameObject.SetActive(false);
    }

    void EnableTrigger() {
        gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
          TransitionManager.LoadScene(sceneName);
        }
    }
}
