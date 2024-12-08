using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cognitive3D;

public class ChangeScreen2 : MonoBehaviour
{
    public void ChangeScene(string defaultSceneName)
    {
        string language = PlayerPrefs.GetString("language", "default");

    
        string targetScene = defaultSceneName; 
        if (language == "Indonesia")
        {
            targetScene = "Short Movie";
        }
        else if (language == "Japan")
        {
            targetScene = "Short Movie1";
        }


        new Cognitive3D.CustomEvent("Change scene")
            .SetProperty("target scene", targetScene)
            .Send();

        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
}
