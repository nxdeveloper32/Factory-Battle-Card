using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    // Start is called before the first frame update
    public GameObject quitPanel;
    public GameObject loadingPanel;
    public GameObject loadingAnim;
    public GameObject startBtn;
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitPanel.SetActive(true);
        }
    }
    public void openLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }
    public void openCloseQuitPanel(bool isActive)
    {
        quitPanel.SetActive(isActive);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
    public void EnablePlayBtn()
    {
        loadingAnim.SetActive(false);
        startBtn.SetActive(true);
    }
}
