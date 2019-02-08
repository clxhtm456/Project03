using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveListManager : Singleton<SaveListManager> {
    public SaveData playerdata;
    //public int recentSave;
    public enum SaveState
    {
        NEWPLAY
    };
    public void ResetSaveAll()
    {
        for (int i = 0; i < 10; i++)
            DeleteSaveList(i);
    }
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void writeStringToFile(string str, string filename)
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(str);

        sw.Close();
        file.Close();
#endif
    }


    private string readStringFromFile(string filename)//, int lineIndex )
    {
#if !WEB_BUILD
        string path = pathForDocumentsFile(filename);

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadLine();

            sr.Close();
            file.Close();

            return str;
        }

        else
        {
            return null;
        }
#else
return null;
#endif
    }

    // 파일쓰고 읽는넘보다 이놈이 핵심이죠
    private string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }

        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
    public void GameSave(int _name)
    {
        try
        {
            BinaryFormatter _binary_formatter = new BinaryFormatter();

            //각 플랫폼별 폴더 지정하는건 http://blog.naver.com/nameisljk/110157303742  <-- 요 블로그에 잘 설명이 되있음
            //폴더와 파일이름 을 넣음 .

            FileStream _filestream = File.Create(Application.persistentDataPath + "/" + _name + ".dat");

            //내가 저장할 리스트를 넣음.

            _binary_formatter.Serialize(_filestream, playerdata);

            _filestream.Close();
            Debug.Log("파일저장 세이브" + _name);
        }
        catch
        {
            Debug.Log("파일 저장 실패");
        }
    }
    public void AutoGameSave()
    {
        try
        {
            BinaryFormatter _binary_formatter = new BinaryFormatter();

            //각 플랫폼별 폴더 지정하는건 http://blog.naver.com/nameisljk/110157303742  <-- 요 블로그에 잘 설명이 되있음
            //폴더와 파일이름 을 넣음 .

            FileStream _filestream = File.Create(Application.persistentDataPath + "/" + 3 + ".dat");

            //내가 저장할 리스트를 넣음.

            _binary_formatter.Serialize(_filestream, playerdata);

            _filestream.Close();
            Debug.Log("파일저장 세이브" + playerdata.index);
        }
        catch
        {
            Debug.Log("파일 저장 실패");
        }
    }
    public int LoadingSaveList()
    {
        int i = 0;
        while (File.Exists(pathForDocumentsFile(Application.persistentDataPath + "/" + i + ".dat")))
        {
            i++;
        }
        return i;
    }
    public void DeleteSaveList(int _name)
    {
        try
        {
            //내가 파일을 만든 파일 혹은 불러올 파일이 있는지 없는지를 체크함.
            bool _fhile_check = File.Exists(pathForDocumentsFile(Application.persistentDataPath + "/" + _name + ".dat"));
            if (_fhile_check == false)
                return;

            BinaryFormatter _binary_formatter = new BinaryFormatter();

            //파일을 불러옴.  FileMode - Open 설정. 
            File.Delete((Application.persistentDataPath + "/" + _name + ".dat"));
            Debug.Log("세이브삭제" + _name);
        }
        catch
        {
            Debug.Log("세이브삭제 실패");
        }
    }
    public bool GameLoad(int _name)
    {
        try
        {
            //내가 파일을 만든 파일 혹은 불러올 파일이 있는지 없는지를 체크함.
            bool _fhile_check = File.Exists(pathForDocumentsFile(Application.persistentDataPath + "/" + _name + ".dat"));
            if (_fhile_check == false)
            {
                Debug.Log("세이브 파일 없음");
                return false;
            }

            BinaryFormatter _binary_formatter = new BinaryFormatter();

            //파일을 불러옴.  FileMode - Open 설정. 
            FileStream _filestream = File.Open((Application.persistentDataPath + "/" + _name + ".dat"), FileMode.Open);

            //불러온 파일을 변환. 
            //내가 저장할 리스트 변수에 형변환해서 넣어줌.
            playerdata = (SaveData)_binary_formatter.Deserialize(_filestream);

            _filestream.Close();
            Debug.Log("파일로드 세이브" + playerdata.index);
            return true;
        }
        catch
        {
            Debug.Log("불러오기 실패");
            return false;
        }
    }

    

    public void NewGame(int _val)
    {
        playerdata.index = _val;
        SceneManager.LoadScene("MainScene");
        //LoadingScene.LoadScene("MainScene");
    }
    public void StartGame(int _val)
    {
        if (GameLoad(_val))
        {
            SceneManager.LoadScene("MainScene");
            //LoadingScene.LoadScene("MainScene");
        }
        else
            Debug.Log("불러올수없음");
    }
}
