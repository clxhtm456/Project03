using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour {
    private void Awake()
    {
    }
    public void GameStart()
    {
        int temp = SaveListManager.instance.LoadingSaveList();
        Debug.Log("세이브 파일 개수" + temp);
        if (temp > 0)
        {
            SaveListManager.instance.StartGame(0);
        }
        else
        {
            SaveListManager.instance.NewGame(0);
        }
    }
	// Use this for initialization
	void Start () {
        //for (int i = 0; i < easyTweenList.Length; i++)
        AudioManager.instance.PlayBGM("01 Darkest Dungeon (Theme)", 40.0f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
