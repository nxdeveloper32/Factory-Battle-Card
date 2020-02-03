using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class TargetScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI currHealthText;
    public GameObject machineHealthArt;
    public GameObject machineDamageArt;
    public bool isHealed = false;

    int currHealth;
    int pointsOffered;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HandleClick);

    }
    public void SetupTarget(int pointOffer, int healthPoint)
    {
        pointsOffered = pointOffer;
        currHealth = healthPoint;

        pointsText.text = pointsOffered.ToString();
        currHealthText.text = currHealth.ToString();
    }

    public void UpdateDamageLevel(int damage)
    {
        if (!GameManager.instance.isSimulating)
        {
            int[] data = new int[2];

            data[0] = GameManager.instance.onePlayerHand.IndexOf(GameManager.instance.activeCard.gameObject);

            data[1] = GameManager.instance.targets.IndexOf(this.gameObject);



            EventsRaiser.instance.RaiseEvent(1, data);
        }

        currHealth += damage;
        Debug.LogError(currHealth);
        currHealthText.text = currHealth.ToString();
        if (currHealth >= pointsOffered)
        {
            currHealthText.gameObject.SetActive(false);
            isHealed = true;
            transform.GetChild(0).gameObject.SetActive(false);
            machineHealthArt.SetActive(true);
            machineDamageArt.SetActive(false);
            HealTarget();
        }
        else if (currHealth <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            machineHealthArt.SetActive(false);
            machineDamageArt.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            machineHealthArt.SetActive(false);
            machineDamageArt.SetActive(false);
        }

        if (GameManager.instance.isSimulating)
        {
            EventsRaiser.instance.RaiseEvent(2);
        }

    }

    void HealTarget()
    {
        GameManager.instance.FixedCount++;
        if (GameManager.instance.FixedCount == GameManager.instance.maxMachineHealth)
        {
            GameManager.instance.CheckVictory();

        }
        GetComponent<Button>().interactable = false;
        GameManager.instance.UpdateScore(pointsOffered);
    }

    public void HandleClick()
    {
        GameManager.instance.activeCard.GetComponent<Card>().MoveCard(gameObject);

        StartCoroutine(DelayedEffect());

        if (!GameManager.instance.isSimulating)
        {
            Timer.instance.turnPlayed = true;
        }

    }
    IEnumerator DelayedEffect()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.instance.activeCard.GetComponent<Card>().cardType == CardType.Heal)
        {
            UpdateDamageLevel(GameManager.instance.activeCard.GetComponent<Card>().cardSO.Point);
        }
        else
        {
            UpdateDamageLevel(-GameManager.instance.activeCard.GetComponent<Card>().cardSO.Point);
        }
        if (!GameManager.instance.isSimulating)
        {
            GameManager.instance.HandleTargetsInteractivity(false);
        }
    }
}
