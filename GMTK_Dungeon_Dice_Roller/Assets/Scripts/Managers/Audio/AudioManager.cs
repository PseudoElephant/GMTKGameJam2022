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
        
        DontDestroyOnLoad(gameObject);
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

    public static void Stop(string name)
    {
        Sounds s = Array.Find(Instance.sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.Log("NO AUDIO FILE: "+name);
            return;
        }

        s.source.Stop();
    }

    public static void Loop(string name)
    {
        Sounds s = Array.Find(Instance.sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.Log("NO AUDIO FILE: "+name);
            return;
        }

        s.source.loop = true;
    }

    private void SubscribeToEvents()
    {
        LevelManager.OnLevelStart += LevelManagerOnOnLevelStart();
        LevelManager.OnPlayerDeath += LevelManagerOnOnPlayerDeath();
        LevelManager.OnPlayerHit += LevelManagerOnOnPlayerHit();
        LevelManager.OnDiceRoll += LevelManagerOnOnDiceRoll();
        LevelManager.OnEnemyKilled += LevelManagerOnOnEnemyKilled();
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelStart -= LevelManagerOnOnLevelStart();
        LevelManager.OnPlayerDeath -= LevelManagerOnOnPlayerDeath();
        LevelManager.OnPlayerHit -= LevelManagerOnOnPlayerHit();
        LevelManager.OnDiceRoll -= LevelManagerOnOnDiceRoll();
        LevelManager.OnEnemyKilled -= LevelManagerOnOnEnemyKilled();
    }

    private static LevelManager.CallbackAction LevelManagerOnOnLevelStart()
    {
        return () => Play("sfx_levelStart");
    }

    private static LevelManager.CallbackAction LevelManagerOnOnPlayerDeath()
    {
        return () => Play("sfx_onPlayerDie");
    }

    private static LevelManager.CallbackAction LevelManagerOnOnPlayerHit()
    {
        return () => Play("sfx_onPlayerHit");
    }

    private static LevelManager.CallbackAction LevelManagerOnOnDiceRoll()
    {
        return () => Play("sfx_DiceRoll");
    }

    private static LevelManager.CallbackAction LevelManagerOnOnEnemyKilled()
    {
        return () => Play("sfx_EnemyDeath");
    }
}
