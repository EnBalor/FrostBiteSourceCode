using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public AudioClip titleBGM;

    private void Start()
    {
        AudioManager.instance.PlayBGM(titleBGM);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
        AudioManager.instance.StopBGM();
    }
}
