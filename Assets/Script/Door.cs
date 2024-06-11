using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, Interactable
{
    public string GetInteractPrompt()
    {
        string str = "��\n���θ��� ���ϴ�.";
        return str;
    }

    public void OnInteract()
    {
        SceneManager.LoadScene("HutScene");
    }
}
