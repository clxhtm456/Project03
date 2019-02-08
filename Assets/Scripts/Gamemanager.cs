using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gamemanager : Singleton<Gamemanager> {
    private void Start()
    {
        Screen.SetResolution(1280, 720, true);
        Debug.Log("타임" + Time.timeScale);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIBase.topUI != null)
                UIBase.topUI.CloseUI();
            else
                Application.Quit();

        }
    }
}
