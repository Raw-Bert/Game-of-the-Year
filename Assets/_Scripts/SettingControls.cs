using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
public class SettingControls : MonoBehaviour
{

    Bus master;
    Bus sfx;
    Bus music;

    private void Awake()
    {
        master = RuntimeManager.GetBus("bus:/Master");
        sfx = RuntimeManager.GetBus("bus:/Master/SFX");
        music = RuntimeManager.GetBus("bus:/Master/BGM");
    }

    public void mute(bool val) { master.setMute(val); }
    public void volMaster(float val) { master.setVolume(val); }
    public void volMusic(float val) { music.setVolume(val); }
    public void volSFX(float val) { sfx.setVolume(val); }
}