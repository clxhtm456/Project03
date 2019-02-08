using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStalker : MonoBehaviour {
    public GameObject Target;
    private Ray ray;
    private RaycastHit hit;
    static public CursorStalker GameCursor;
    // Use this for initialization
    private void Awake()
    {
        GameCursor = this;
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        //pos.z = -5.0f;
        transform.position = pos;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Target = hit.transform.gameObject;
            transform.position = hit.point;
            //Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 1f);
        }
        else
            Target = gameObject;

    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Unit")
    //    {
    //        Target = other.gameObject;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Unit")
    //    {
    //        Target = gameObject;
    //    }
    //}
}
