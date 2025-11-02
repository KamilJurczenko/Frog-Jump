using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    public Settings setting;

    // Start is called before the first frame update
    void Awake()
    {
        setting = (Settings)SaveManager.LoadData("Setting");
        if (setting == null)
        {
            setting = new Settings
            {
                soundOn = true
            };
        }

        transform.parent = null;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            if (setting.soundOn) s.source.volume = s.volume;
            else s.source.volume = 0;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        Play("Music");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

}
