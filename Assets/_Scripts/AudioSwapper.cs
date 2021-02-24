using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")] //needed to display event search in editor
public class AudioSwapper : MonoBehaviour
{

    EventInstance busBright, busDark;
    void Start()
    {
        busBright = GetComponents<StudioEventEmitter>()[0].EventInstance;
        busDark = GetComponents<StudioEventEmitter>()[1].EventInstance;
    }

    void Update()
    {
        if (GetComponent<ChangeForm>().shadowForm)
        {
            busDark.setVolume(.5f);
            busBright.setVolume(0f);
        }
        else
        {
            busBright.setVolume(.5f);
            busDark.setVolume(0f);
        }
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}