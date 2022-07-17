using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    public float transitionTime;
    public CanvasGroup transition;

    private string startScene = "Start Room";

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) {
            Instance = this;
            LeanTween.alphaCanvas(transition, 0f, transitionTime).setEaseInSine();
            LevelManager.OnPlayerDeath += LevelManagerOnOnPlayerDeath();
            return;
        }

        Destroy(this.gameObject);
    }

    private LevelManager.CallbackAction LevelManagerOnOnPlayerDeath()
    {
        return () => LoadScene(startScene);
    }

    public static void NextScene()
    {
      LeanTween.alphaCanvas(TransitionManager.Instance.transition, 1f, TransitionManager.Instance.transitionTime).setEaseInSine()
        .setOnComplete(() =>  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public static void LoadScene(string name)
    {
      LeanTween.alphaCanvas(TransitionManager.Instance.transition, 1f, TransitionManager.Instance.transitionTime).setEaseInSine()
        .setOnComplete(() =>  SceneManager.LoadScene(name,LoadSceneMode.Single));
    }

    private void OnDestroy()
    {
        LevelManager.OnPlayerDeath -= LevelManagerOnOnPlayerDeath();
    }
}
