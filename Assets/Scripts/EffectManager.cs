using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager> {
    public GameObject[] particleArray;
    public List<GameObject> particleList;
	// Use this for initialization
	void Start () {
		particleList = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private GameObject FindEffect(string _name)
    {
        for(int i = 0; i<particleArray.Length;i++)
        {
            if(particleArray[i].name == _name)
            {
                return particleArray[i];
            }
        }
        return null;
    }
    public GameObject MakeUIEffect(string _name)
    {
        ResetList();
        GameObject particle = particleList.Find(item => item.name == _name && item.activeInHierarchy == false);
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.SetParent(GameObject.Find("Canvas").transform);
            particle.transform.position = Vector3.zero;
            particle.transform.rotation = Quaternion.identity;
            return particle;
        }
        GameObject _object = FindEffect(_name);
        if (_object != null)
        {
            GameObject newList = Instantiate(_object);
            newList.transform.SetParent(GameObject.Find("Canvas").transform);
            newList.name = _object.name;
            particleList.Add(newList);
            return newList;
        }
        else
        {
            Debug.Log("이펙트없음");
            return null;
        }
    }
    public GameObject MakeUIEffect(string _name, Vector3 _position)
    {
        ResetList();
        GameObject particle = particleList.Find(item => item.name == _name && item.activeInHierarchy == false);
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.SetParent(GameObject.Find("Canvas").transform);
            particle.transform.localPosition = _position;
            particle.transform.rotation = Quaternion.identity;
            return particle;
        }
        GameObject _object = FindEffect(_name);
        if (_object != null)
        {
            GameObject newList = Instantiate(_object);
            newList.name = _object.name;
            newList.transform.SetParent(GameObject.Find("Canvas").transform);
            newList.transform.localPosition = _position;
            newList.transform.rotation = Quaternion.identity;
            particleList.Add(newList);
            return newList;
        }
        else
        {
            Debug.Log("이펙트없음");
            return null;
        }
    }
    public GameObject MakeEffect(string _name,Vector3 _position)
    {
        ResetList();
        GameObject particle = particleList.Find(item => item.name == _name && item.activeInHierarchy == false);
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = _position;
            particle.transform.rotation = Quaternion.identity;
            return particle;
        }
        GameObject _object = FindEffect(_name);
        if (_object != null)
        {
            GameObject newList = Instantiate(_object, _position, Quaternion.identity);
            newList.name = _object.name;
            particleList.Add(newList);
            return newList;
        }
        else
        {
            Debug.Log("이펙트없음");
            return null;
        }
    }
    public GameObject MakeEffect(string _name)
    {
        ResetList();
        GameObject particle = particleList.Find(item => item.name == _name && item.activeInHierarchy == false);
        if (particle != null)
        {
            particle.SetActive(true);
            particle.transform.position = Vector3.zero;
            particle.transform.rotation = Quaternion.identity;
            return particle;
        }
        GameObject _object = FindEffect(_name);
        if (_object != null)
        {
            GameObject newList = Instantiate(_object);
            newList.name = _object.name;
            particleList.Add(newList);
            return newList;
        }
        else
        {
            Debug.Log("이펙트없음");
            return null;
        }
    }
    private void ResetList()
    {
        foreach(GameObject effect in particleList)
        {
            if (!effect)
                particleList.Remove(effect);
        }
    }
    public void ResetAllList()
    {
        particleList.Clear();
    }
    public void DeleteEffect(GameObject _effect)
    {
        foreach (GameObject particle in particleList)
        {
            if (particle == _effect && particle.activeInHierarchy == true)
            {
                particle.SetActive(false);
                particle.transform.SetParent(null);
            }
        }
    }
    public static void GamePause(bool _temp)
    {
        if(_temp)
        {
            Time.timeScale = 0;
        }else
        {
            Time.timeScale = 1;
        }
    }
    public static bool IsGamePause()
    {
        if (Time.timeScale == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
