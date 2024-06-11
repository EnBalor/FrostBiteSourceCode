using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, Interactable
{
    public string GetInteractPrompt()
    {
        string str = "문\n오두막에 들어갑니다.";
        return str;
    }

    public void OnInteract()
    {
        SceneManager.LoadScene("HutScene");
    }
}
