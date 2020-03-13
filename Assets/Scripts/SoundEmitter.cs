using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    [HideInInspector]
    public float currentSoundLevel = 0f;

    // used for the sound to emit
    public AudioClip sourceAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // decrease the emitting sound as time goes. The decrease rate should be same as the audio length
        if (currentSoundLevel > 0f)
        {
            currentSoundLevel -= Time.deltaTime;
        }
        else if (currentSoundLevel < 0)
        {
            // prevent from going to minus
            currentSoundLevel = 0;
        }
    }
}
