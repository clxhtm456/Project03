using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChangeText : MonoBehaviour {
    public bool loop = false;
    private Text text;
    private float timer;
    private int triggerCount;
    [System.Serializable]
    public struct TextList
    {
        public string text;
        public float timer;
    }
    public TextList[] textList;
    void quickSort(TextList[] arr, int left, int right)
    {
        if (arr.Length <= 0)
            return;
        int i = left, j = right;
        float pivot = arr[(left + right) / 2].timer;
        TextList temp;
        do
        {
            while (arr[i].timer < pivot)
                i++;
            while (arr[j].timer > pivot)
                j--;
            if (i <= j)
            {
                temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
                i++;
                j--;
            }
        } while (i <= j);

        /* recursion */
        if (left < j)
            quickSort(arr, left, j);

        if (i < right)
            quickSort(arr, i, right);
    }
    private void Awake()
    {
        text = GetComponent<Text>();
        if (textList.Length-1 > 0)
            quickSort(textList,0, textList.Length-1);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (triggerCount < textList.Length && timer >= textList[triggerCount].timer)
        {
            text.text = textList[triggerCount].text;
            triggerCount++;
        }
    }
    private void LateUpdate()
    {
        if(triggerCount == textList.Length && loop)
        {
            triggerCount = 0;
            timer = 0;
        }
    }
}
