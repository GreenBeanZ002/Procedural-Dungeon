using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private GameObject activeGamePanel;

    public void openMenu()
    {
        activeGamePanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void closeMenu()
    {
        Debug.Log("Closed menu");
        menuPanel.SetActive(false);
        activeGamePanel.SetActive(true);
    }
    public void openControls()
    {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void closeControls()
    {
        menuPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
