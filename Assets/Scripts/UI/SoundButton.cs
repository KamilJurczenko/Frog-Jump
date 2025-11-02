using UnityEngine;

public class SoundButton : MonoBehaviour
{
    [SerializeField] GameObject soundOn;
    [SerializeField] GameObject soundOff;

    Settings settings;
    Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        settings = AudioManager.instance.setting;
        sounds = AudioManager.instance.sounds;

        if (settings.soundOn)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

    public void SoundOff()
    {
        settings.soundOn = false;
        SaveManager.SaveData(settings, "Setting");
        foreach (Sound s in sounds)
        {
            s.source.volume = 0;
        }
    }
    public void SoundOn()
    {
        settings.soundOn = true;
        SaveManager.SaveData(settings, "Setting");
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
        }
    }

}
