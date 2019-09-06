using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySoundLevel : MonoBehaviour
{
    private TextMeshProUGUI displayText;

    public SoundEmitter sourceObject;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void OnGUI()
    {
        //todo: change this to image rather than just text
        displayText.text = "Sound Level: " + Mathf.Round(sourceObject.currentSoundLevel);
        
    }
}
