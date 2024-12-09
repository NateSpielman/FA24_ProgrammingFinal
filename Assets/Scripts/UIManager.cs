using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panelOne;
    [SerializeField] private GameObject panelTwo;

    public void ResetScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Switch()
    {
        if (panelOne.active)
        {
            panelOne.SetActive(false);
            panelTwo.SetActive(true);
        }
        else
        {
            panelOne.SetActive(true);
            panelTwo.SetActive(false);
        }
    }
}
