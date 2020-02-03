using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Image onePlayerTimer;
    public Image twoPlayerTimer;
    [HideInInspector]
    public bool turnPlayed = false;

    int timeLimit = 30;
    int currActiveTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        /*onePlayerTimer.maxValue = timeLimit;
        onePlayerTimer.minValue = 0;
        twoPlayerTimer.maxValue = timeLimit;
        twoPlayerTimer.minValue = 0;
*/
        currActiveTime = timeLimit;
    }

    public void StartTimer(int p)
    {
        turnPlayed = false;
        if (p == 1)
        {
            twoPlayerTimer.transform.parent.gameObject.SetActive(false);
            onePlayerTimer.transform.parent.gameObject.SetActive(true);
            twoPlayerTimer.fillAmount = 1;
            StopAllCoroutines();
            StartCoroutine(DecreaseTimer(onePlayerTimer));
        }
        else
        {
            onePlayerTimer.transform.parent.gameObject.SetActive(false);
            twoPlayerTimer.transform.parent.gameObject.SetActive(true);
            onePlayerTimer.fillAmount = 1;
            StopAllCoroutines();
            StartCoroutine(DecreaseTimer(twoPlayerTimer));
        }
    }

    IEnumerator DecreaseTimer(Image s)
    {
        currActiveTime = timeLimit;
        while (!turnPlayed && currActiveTime > 0)
        {
            yield return new WaitForSeconds(1);
            currActiveTime--;

            s.fillAmount = (float)currActiveTime / timeLimit;
        }
        if (currActiveTime <= 0)
        {
            GameManager.instance.ResetAllButtons();
            GameManager.instance.ChangeTurnLockInput();

        }

    }
}
