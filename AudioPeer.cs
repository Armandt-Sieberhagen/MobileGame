using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    AudioSource Sourceaudio;
    public float[] Samples = new float[512];
    public float SamplesAmount;
    public float[] frequenceBand = new float[8];
    // Start is called before the first frame update
    void Start()
    {
        Sourceaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        Sourceaudio.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
        MakeFrequencyBands();
    }

    void MakeFrequencyBands()
    {
        /*
         22050 / 512 = 43 hertsz per sample
         
         20 - 60 hertz
         60- 250 hertz
         250 - 500 hertz
         500 - 2000 hertz
         2000 - 4000 hertz
         4000 - 6000 hertz
         6000- 20000 hertz

         0-2 = 86hertz
         1 - 4 = 172 hertz, range 87-258
         2 - 8 = 344 hertz, range 259-602
         3 - 16 = 688 hertz, range 603-1290
         4 - 32 = 1376 hertz, range 1291-2666
         5 - 64 = 2752 hertz, range 2667-5418
         6 - 128 = 5504 hertz, range 5419,10922
         7 - 256 = 11008 hertz, range 10923-21930

         */

        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float averahe = 0;
            int sampleCount = (int)Mathf.Pow(2,i) * 2;

            if (i==7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                averahe += Samples[count] * (count+1);
                count++;
            }
            averahe /= count;
            frequenceBand[i] = averahe*20;
        }
    }
}
