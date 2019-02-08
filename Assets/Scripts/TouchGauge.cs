using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGauge : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(wp, Vector2.zero);
            RaycastHit2D[] rayHit;
            rayHit = Physics2D.RaycastAll(ray.origin, ray.direction);
            if (rayHit.Length > 0 && rayHit[0] && rayHit[0] == this)
            {
                
            }
        }
    }
}
