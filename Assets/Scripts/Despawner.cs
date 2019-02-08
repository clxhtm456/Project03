using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour {
    public float despawnTimer;
    private float timer;
    // Use this for initialization
    private void OnEnable()
    {
        timer = 0.0f;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > despawnTimer)
            gameObject.SetActive(false);

    }
}
