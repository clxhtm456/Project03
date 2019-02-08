using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float maxValue;
    [SerializeField]
    private float recentValue;
    public float workSpeed;
    public bool stop;
    public Image progress;
    public Text progressRate;
    private float stopRate = 100;//멈춰야하는 값
    public float StopRate
    {
        set
        {
            stopRate = value;
            stop = false;
        }
    }
    public bool Stop
    {
        get { return stop; }
        set
        {
            if (value != stop)
            {
                stop = value;
            }
        }
    }
    public float RecentValue
    {
        get { return recentValue; }
        set
        {
            if (value < 0)
                value = 0;
            recentValue = value;
            float rate = (recentValue / maxValue);
            Vector3 val = progress.transform.localScale;
            val.x = (1.0f - rate);
            progress.transform.localScale = val;
            if (progressRate != null)
                progressRate.text = ((int)(rate * 100.0f)).ToString() + "%";
        }
    }
    private void Awake()
    {
    }
    // Use this for initialization
    void Start()
    {
        float rate = (recentValue / maxValue);
        Vector3 val = progress.transform.localScale;
        val.x = (1.0f - rate);
        progress.transform.localScale = val;
        if (progressRate != null)
            progressRate.text = ((int)(rate * 100.0f)).ToString() + "%";
        if (((int)(rate * 100.0f)) >= stopRate)
        {
            Stop = true;
        }
    }
    private void OnDisable()
    {
        recentValue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (recentValue >= maxValue)
            return;
        if (!stop)
        {
            recentValue += workSpeed * Time.deltaTime;
            if (recentValue > maxValue)
                recentValue = maxValue;
        }
        float rate = (recentValue / maxValue);
        Vector3 val = progress.transform.localScale;
        val.x = (1.0f - rate);
        progress.transform.localScale = val;
        if (progressRate != null)
            progressRate.text = ((int)(rate * 100.0f)).ToString() + "%";
    }
}
