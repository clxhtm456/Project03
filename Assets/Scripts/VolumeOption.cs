using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOption : MonoBehaviour {
    public Slider bgmSlider;
    public Slider effectSlider;
    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        bgmSlider.value = AudioManager.instance.BGMVolume;
        effectSlider.value = AudioManager.instance.EFFECTVolume;
    }
	
	// Update is called once per frame
	void Update () {
        AudioManager.instance.BGMVolume = bgmSlider.value;
        AudioManager.instance.EFFECTVolume = effectSlider.value;
    }
}
