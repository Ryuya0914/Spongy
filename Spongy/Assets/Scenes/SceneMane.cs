using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMane : MonoBehaviour
{
    [SerializeField] string nextSceneName;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Invoke("SceneLoad", 1f);
        }
    }

    void SceneLoad() {
        SceneManager.LoadScene(nextSceneName);
    }
}
