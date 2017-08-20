using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : SingletonComponent<LightController> {

    public float[] multipliers;
    public SoundAnalyser soundAnalyser;
    public Light[] lights;
    public float generalMultiplier = 1f;
    public bool[] lockedLights;
    //CONTROLS
    public bool doNumbersLock;
    KeyCode[] controlKeys = new KeyCode[7]{KeyCode.Alpha1, KeyCode.Alpha2,KeyCode.Alpha3,KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6,KeyCode.Alpha7};

    //NOISE
    public float generalNoiseMultiplier = .001f;
    public float noiseTimeLeft;
    public float noiseDecreaseFactor = 1f;

	// Use this for initialization
	void Start () {
        lights = GetComponentsInChildren<Light>();
        lockedLights = new bool[lights.Length];
        multipliers = new float[lights.Length];
        for (int i = 0; i < multipliers.Length; i++) {
            multipliers[i] = 1f;
        }
    }

    // Update is called once per frame
    void Update () {
        //HANDLE INPUT
        for (int keyIdx = 0; keyIdx < controlKeys.Length; keyIdx++) {
            if(Input.GetKeyDown(controlKeys[keyIdx])) {
                lockedLights[keyIdx] = true;
            }
            if(Input.GetKeyUp(controlKeys[keyIdx])) {
                lockedLights[keyIdx] = false;
            }
        }
        


        float noise = 0;
        if(noiseTimeLeft > 0) {
            noise = Random.Range(-generalNoiseMultiplier, generalNoiseMultiplier);
            noiseTimeLeft -= Time.deltaTime * noiseDecreaseFactor;
        }else {
            noiseTimeLeft = 0;
        }

        for (int i = 0; i < lights.Length; i++) {
            if (!lockedLights[i]) {
                lights[i].intensity = (generalMultiplier + multipliers[i]) * soundAnalyser.clipLoudness + noise;
            }
        }
	}
}
