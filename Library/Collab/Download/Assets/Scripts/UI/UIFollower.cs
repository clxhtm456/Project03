using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    public GameObject target;
    public float height;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (gameObject.activeInHierarchy == true && (target == null || target.activeInHierarchy == false))
            gameObject.SetActive(false);
        else
            Movefunc();
    }
    void Movefunc()
    {
        Vector3 repPos = target.transform.position;
        repPos.y += height;
        Vector3 pos = Camera.main.WorldToScreenPoint(repPos);
        transform.position = pos;
    }
}

