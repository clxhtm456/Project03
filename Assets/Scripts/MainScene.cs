using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour {
    public void GameSave()
    {
        SaveListManager.instance.GameSave(SaveListManager.instance.playerdata.index);
    }

    public void goBattle()
    {
        LoadingScene.LoadScene("BattleScene");
        //SceneManager.LoadScene("BattleScene");
    }

    public void editParty()
    {
        Debug.Log("Update Soon");
    }

    public void Shop()
    {
        Debug.Log("Update Soon");
    }

    public void Option()
    {
        Debug.Log("Update Soon");
    }

    // Use this for initialization
    void Start () {
        AudioManager.instance.PlayBGM("02 The Hamlet");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
