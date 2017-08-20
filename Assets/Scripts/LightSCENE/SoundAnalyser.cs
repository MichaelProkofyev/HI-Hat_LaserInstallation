using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnalyser : MonoBehaviour {


    public AudioClip[] clips;
    float currentClipEndTime;

    //public float newPosition;
    int currentClipIdx = 0;

    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    AudioSource audioSource;

    public float smoothing = 0.5f;

    public float clipLoudness;
    private float prevLoudness;
    private float[] clipSampleData;

    public float NoiseDurationMultiplier = 1;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];
        
        currentClipIdx = 0;
        // if(clips.Length > 0) {
        //     audioSource.clip = clips[currentClipIdx];
        //     currentClipEndTime = Time.time + audioSource.clip.length;
        //     audioSource.Play();
        // }
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space)) {
         //   print(clipLoudness);
        //}

        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
            //SMOOTHING
            clipLoudness = Mathf.Lerp(prevLoudness, clipLoudness, smoothing);
            LightController.Instance.noiseTimeLeft += clipLoudness - prevLoudness;
            // print(clipLoudness);
            prevLoudness = clipLoudness;

        }

        // if(Time.time > currentClipEndTime) {
        //     currentClipIdx = (currentClipIdx + 1) % clips.Length;
        //     audioSource.clip = clips[currentClipIdx];
        //     currentClipEndTime = Time.time + audioSource.clip.length;
        //     audioSource.Play();
        // }

    }
}
