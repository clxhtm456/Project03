using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageFont : MonoBehaviour
{
    public float showTimer;
    private float reShowTimer;
    public float upSpeed;
    public float fontSize;
    [HideInInspector]
    public Text text;
    // Use this for initialization
    public void SetText(string _text)
    {
        if(!text)
            text = GetComponent<Text>();
        text.text = _text;
    }
    private void Awake()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1) * fontSize;
        text = GetComponent<Text>();
    }
    void OnEnable()
    {
        reShowTimer = showTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if (reShowTimer > 0.0f)
        {
            reShowTimer -= Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += upSpeed * Time.deltaTime;
            transform.position = pos;
            Color color = text.color;
            color.a = reShowTimer / showTimer;
            text.color = color;
        }
        else
            gameObject.SetActive(false);
    }
}
