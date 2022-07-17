using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    public static AudioManager Instance;
    public void Awake()
    {
        if (Instance == null) {
            Instance = this;
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;

            sound.source.pitch = sound.pitch == 0 ? 1f : sound.pitch;
        }
        
        Play("SongMenu");
        
        SubscribeToEvents();
    }

    public static void Play(string name)
    {
        Sounds s = Array.Find(Instance.sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.Log("NO AUDIO FILE: "+name);
            return;
        }

        s.source.Play();
    }

    public static void Pause(string name)
    {
        Sounds s = Array.Find(Instance.sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.Log("NO AUDIO FILE: "+name);
            return;
        }

        s.source.Stop();
    }

    private void SubscribeToEvents()
    {
        LevelManager.OnLevelStart += () => Play("sfx_levelStart");
        LevelManager.OnPlayerDeath += () => Play("sfx_onPlayerDie");
        LevelManager.OnPlayerHit += () => Play("sfx_onPlayerHit");
        LevelManager.OnDiceRoll += () => Play("sfx_DiceRoll");
        LevelManager.OnEnemyKilled += () => Play("sfx_EnemyDeath");
    }
}
