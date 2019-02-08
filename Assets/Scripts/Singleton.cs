using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour,new()
{
    private static T _instance;
    public static T instance
    {
        get
        {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                }
                return _instance;
        }
    }
    virtual public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}