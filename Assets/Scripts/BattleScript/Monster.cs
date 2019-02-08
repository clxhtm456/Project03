using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    protected ThreatList[] aggroList = new ThreatList[4];
    protected struct ThreatList
    {
        public Unit threatUnit;
        public float threatMount;
    };
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
