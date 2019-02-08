using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class UIBase : MonoBehaviour {
    protected RectTransform recttransform;
    protected UIBase rootUI = null;
    static public UIBase topUI = null;
    // Use this for initialization
    virtual protected void Awake()
    {
        recttransform = GetComponent<RectTransform>();
    }
    void Start () {
	}
    virtual public void ResetPanel()
    {

    }
   
    protected void OnDisable()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
    virtual public void OpenUI()//UI를 아래로 정렬후 활성화
    {
        gameObject.SetActive(true);
        recttransform.SetAsLastSibling();
        rootUI = topUI;
        topUI = this;
        
        ResetPanel();
    }
    virtual public void CloseUI()
    {
        if (gameObject.activeInHierarchy == true)
            gameObject.SetActive(false);
        topUI = rootUI;

        if (rootUI)
        {
            rootUI.ResetPanel();
        }
    }
    static public void s_CloseAllUI()
    {
        while (topUI.ShutDownUI()) ;
    }
    public void CloseAllUI()
    {
        while (topUI != null && topUI.ShutDownUI()) ;
    }
    private bool ShutDownUI()
    {
        if (topUI == null)
            return false;
        else
        {
            topUI.CloseUI();
            return true;
        }
    }
}
