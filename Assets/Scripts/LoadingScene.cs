using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    public GameObject loadingDoorLeft;
    public GameObject loadingDoorRight;

    // Use this for initialization
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        //EffectManager.instance.ResetAllList();
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        AudioManager.instance.PauseBGM();
        StartCoroutine(LoadScene());
    }
    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            if (op.progress >= 0.9f)
            {
                loadingDoorLeft.transform.rotation = Quaternion.Euler(0.0f, 70.0f, 0.0f);
                loadingDoorRight.transform.rotation = Quaternion.Euler(0.0f, -70.0f, 0.0f);
                AudioManager.instance.PlayEffect("doorSound");
                yield return new WaitForSeconds(0.3f);
                
                while (loadingDoorLeft.transform.position.z > -8.8f)
                {
                    loadingDoorLeft.transform.position += Vector3.back * Time.deltaTime * 10.0f;
                    loadingDoorRight.transform.position += Vector3.back * Time.deltaTime * 10.0f;
                    yield return null;
                }
                break;
            }
            else
            {
            }
            loadingDoorLeft.transform.rotation = Quaternion.Euler(0.0f, op.progress*33.3f, 0.0f);
            loadingDoorRight.transform.rotation = Quaternion.Euler(0.0f, op.progress * -33.3f, 0.0f);
            yield return null;
        }
        op.allowSceneActivation = true;
        EffectManager.instance.ResetAllList();

    }
}