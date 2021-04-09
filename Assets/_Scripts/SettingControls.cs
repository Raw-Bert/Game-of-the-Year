using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class SettingControls : MonoBehaviour
{

    [SerializeField] Toggle muteMaster_UI;
    [SerializeField] Slider volMaster_UI;
    [SerializeField] Slider volMusic_UI;
    [SerializeField] Slider volSFX_UI;

    Bus master;
    Bus sfx;
    Bus music;

    private void Awake()
    {
        master = RuntimeManager.GetBus("bus:/Master");
        sfx = RuntimeManager.GetBus("bus:/Master/SFX");
        music = RuntimeManager.GetBus("bus:/Master/BGM");

        muteMaster_UI.isOn = getMute();
        volMaster_UI.value = getMaster();
        volMusic_UI.value = getMusic();
        volSFX_UI.value = getSFX();
    }

    public bool getMute() { bool val; master.getMute(out val); return val; }
    public float getMaster() { float val; master.getVolume(out val); return val; }
    public float getMusic() { float val; music.getVolume(out val); return val; }
    public float getSFX() { float val; sfx.getVolume(out val); return val; }

    public void mute(bool val) { master.setMute(val); }
    public void volMaster(float val) { master.setVolume(val); }
    public void volMusic(float val) { music.setVolume(val); }
    public void volSFX(float val) { sfx.setVolume(val); }
}