using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
//using System.Reflection.Metadata;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioSwapper : MonoBehaviour
{

    public float fadeTime = 5;
    public List<int> amountOfEnemyType { get; set; } = new List<int>();

    [EventRef]
    public List<string> sounds = new List<string>();

    List<int> lastAmountOfEnemyType = new List<int>();
    List<float> startVol = new List<float>();
    List<bool> switchDark = new List<bool>(), switchBright = new List<bool>();
    List<DateTime> fadeInit = new List<DateTime>();

    List<EventInstance> busBright = new List<EventInstance>(), busDark = new List<EventInstance>();
    private void OnDestroy()
    {
        foreach (var a in busBright)
        {
            a.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            a.release();
        }
        foreach (var a in busDark)
        {
            a.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            a.release();
        }
    }
    void Start()
    {
       
        busBright.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[0]));
        busBright.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[1]));
        busBright.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[2]));
        busDark.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[3]));
        busDark.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[4]));
        busDark.Add(FMODUnity.RuntimeManager.CreateInstance(sounds[5]));

        busDark[0].setVolume(0);
        busBright[0].setVolume(0);
        busDark[1].setVolume(0);
        busBright[1].setVolume(0);
        busDark[2].setVolume(0);
        busBright[2].setVolume(0.5f);

        for (int a = 0; a < 3; ++a)
        {
            startVol.Add(0);
            startVol.Add(0); //not a mistake

            fadeInit.Add(DateTime.Now);
            amountOfEnemyType.Add(0);
            lastAmountOfEnemyType.Add(0);
            switchBright.Add(false);
            switchDark.Add(false);

            busBright[a].start();
            busDark[a].start();
        }
    }

    void Update()
    {

        if (GetComponent<ChangeForm>().shadowForm)
        {
            for (int a = 0; a < 2; ++a)
            {
                if (!switchDark[a] || lastAmountOfEnemyType[a] != amountOfEnemyType[a])
                {
                    fadeInit[a] = DateTime.Now;

                    float tmp;
                    busDark[a].getVolume(out tmp);
                    startVol[a * 2] = tmp;
                    busBright[a].getVolume(out tmp);
                    startVol[a * 2 + 1] = tmp;

                    lastAmountOfEnemyType[a] = amountOfEnemyType[a];
                    switchDark[a] = true;
                    switchBright[a] = false;
                }

                fadeIn(busDark[a], fadeInit[a], fadeTime, startVol[a * 2],
                    0.5f * (0.1f * amountOfEnemyType[a] + (amountOfEnemyType[a] > 0 ? 1 : 0)));
                fadeOut(busBright[a], fadeInit[a], startVol[a * 2 + 1], fadeTime);
            }

            int lastIndex = switchDark.Count - 1;
            if (!switchDark[lastIndex])
            {
                fadeInit[lastIndex] = DateTime.Now;

                float tmp;
                busDark[lastIndex - 1].getVolume(out tmp);
                startVol[lastIndex * 2 - 1] = tmp;
                busBright[lastIndex].getVolume(out tmp);
                startVol[lastIndex * 2] = tmp;

                switchDark[lastIndex] = true;
                switchBright[lastIndex] = false;
            }

            fadeIn(busDark[lastIndex], fadeInit[lastIndex], fadeTime, startVol[lastIndex * 2 - 1], 0.5f);
            fadeOut(busBright[lastIndex], fadeInit[lastIndex], fadeTime, startVol[lastIndex * 2]);
        }
        else
        {
            for (int a = 0; a < 2; ++a)
            {
                if (!switchBright[a] || lastAmountOfEnemyType[a] != amountOfEnemyType[a])
                {
                    fadeInit[a] = DateTime.Now;

                    float tmp;
                    busBright[a].getVolume(out tmp);
                    startVol[a * 2] = tmp;
                    busDark[a].getVolume(out tmp);
                    startVol[a * 2 + 1] = tmp;

                    lastAmountOfEnemyType[a] = amountOfEnemyType[a];
                    switchBright[a] = true;
                    switchDark[a] = false;
                }

                fadeIn(busBright[a], fadeInit[a], fadeTime, startVol[a * 2],
                    0.5f * (0.1f * amountOfEnemyType[a] + (amountOfEnemyType[a] > 0 ? 1 : 0)));
                fadeOut(busDark[a], fadeInit[a], fadeTime, startVol[a * 2 + 1]);
            }

            int lastIndex = switchBright.Count - 1;
            if (!switchBright[lastIndex])
            {
                fadeInit[lastIndex] = DateTime.Now;

                float tmp;
                busBright[lastIndex - 1].getVolume(out tmp);
                startVol[lastIndex * 2 - 1] = tmp;
                busDark[lastIndex].getVolume(out tmp);
                startVol[lastIndex * 2] = tmp;

                switchBright[lastIndex] = true;
                switchDark[lastIndex] = false;
            }

            fadeIn(busBright[lastIndex], fadeInit[lastIndex], fadeTime, startVol[lastIndex * 2 - 1], 0.5f);
            fadeOut(busDark[lastIndex], fadeInit[lastIndex], fadeTime, startVol[lastIndex * 2]);
        }
    }

    void fadeIn(EventInstance e, DateTime startTime, float duration, float startVol, float vol)
    {
        e.setVolume(Mathf.Lerp(startVol, Mathf.Clamp(vol, 0, .8f),
            Mathf.Clamp((float)DateTime.Now.Subtract(startTime).TotalSeconds / duration, 0, 1)));
    }

    void fadeOut(EventInstance e, DateTime startTime, float duration, float startVol)
    {
        e.setVolume(Mathf.Lerp(startVol, 0,
            Mathf.Clamp((float)DateTime.Now.Subtract(startTime).TotalSeconds / duration, 0, 1)));
    }

}