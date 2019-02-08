using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour {
    public AudioClip clip;
    
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    // Use this for initialization
    void Start () {
        button.onClick.AddListener(()=> { AudioManager.instance.PlayEffect(clip); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
