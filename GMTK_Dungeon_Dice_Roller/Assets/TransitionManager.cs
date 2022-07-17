using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    public float transitionTime;
    public CanvasGroup transition;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) {
            Instance = this;
            LeanTween.alphaCanvas(transition, 0f, transitionTime).setEaseInSine();
            return;
        }

        Destroy(this.gameObject);
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

}
