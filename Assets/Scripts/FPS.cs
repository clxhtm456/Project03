using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour {
    private float fps = 0.0f;
    private float timeA = 0.0f;
    private int lastFps;
    private GUIStyle gUi = new GUIStyle();
    // Use this for initialization
    void Start () {
        timeA = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad - timeA <= 1.0f)
        {
            fps++;
        }
        else
        {
            lastFps = (int)fps;
            timeA = Time.timeSinceLevelLoad;
            fps = 0;
        }
	}
    private void OnGUI()
    {
        gUi.fontSize = 30;
        gUi.normal.textColor = Color.yellow;

        GUILayout.Label("FPS : " + lastFps.ToString(), gUi);
    }
}
