using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject victoryPanel;
    public GameObject LoosePanel;
    public GameObject QuitPanel;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerScore;
    public Text onePlayerScore;
    public Text twoPlayerScore;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateScores(int p1Score, int p2Score)
    {
        onePlayerScore.text = p1Score.ToString();
        twoPlayerScore.text = p2Score.ToString();
    }
    public void openVictoryPanel(int score)
    {
        playerScore.text = score.ToString();
        victoryPanel.SetActive(true);
    }
    public void openLoosePanel(int score)
    {
        playerScore.text = score.ToString();
        LoosePanel.SetActive(true);
    }
}
