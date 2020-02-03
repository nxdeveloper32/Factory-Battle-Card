using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDesription;
    public TextMeshProUGUI point;
    public TextMeshProUGUI cardClass;
    public Image cardBackgroundArt;
    public Image cardArt;
    public Image cardTypeArt;
    public GameObject focusedCardUI;
    public CardSO cardSO;
    public Animator cardAnim;
    public CardType cardType;
    public GameObject cardBackSide;
    [HideInInspector]
    public bool dead = false;
    private bool cardInFocus;
    private int inputDelay = 0;
    private bool cardSelected;
    private float cardSelectZoomScale = 1.2f;
    public void SetUp()
    {
        cardBackgroundArt.sprite = cardSO.CardBackgroundArt;
        cardName.text = cardSO.cardName;
        cardDesription.text = cardSO.CardDescription;
        point.text = cardSO.Point.ToString();
        cardType = cardSO.CardType;
        cardClass.text = cardSO.Cardclass.ToString();
        cardArt.sprite = cardSO.CardArt;
        if (cardType == CardType.Heal)
        {
            cardTypeArt.sprite = cardSO.cardTypeArt[0];
        }
        else
        {
            cardTypeArt.sprite = cardSO.cardTypeArt[1];
        }
    }
    public void ShowCardDetails()
    {
        if (cardSelected)
        {
            return;
        }
        cardInFocus = true;
        StartCoroutine(CheckInput());

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && cardSelected)
        {
            MoveCard(GameManager.instance.MoveTras);
        }
    }
    public void HideCardDetails()
    {
        cardInFocus = false;
        if (inputDelay < 1)
        {
            if (GameManager.instance.activeCard != this.gameObject)
                GameManager.instance.ActivateCard(this);
        }
        CardTransform();
        inputDelay = 0;
    }

    IEnumerator CheckInput()
    {
        cardSelected = true;
        cardAnim.SetBool("isAnimating", true);
        transform.localScale = new Vector3(cardSelectZoomScale, cardSelectZoomScale, 1);
        while (inputDelay < 1)
        {
            yield return new WaitForSeconds(1f);
            if (cardInFocus)
            {
                inputDelay++;
                cardSelected = false;
            }
        }
        if (inputDelay == 1)
        {
            CardTransform();
            yield return null;
        }

    }
    void CardTransform()
    {
        if (cardInFocus)
        {
            FocusedCard focusedCard = focusedCardUI.GetComponent<FocusedCard>();
            focusedCard.cardBackgroundArt.sprite = cardSO.CardBackgroundArt;
            focusedCard.cardName.text = cardSO.cardName;
            focusedCard.cardDesription.text = cardSO.CardDescription;
            focusedCard.point.text = cardSO.Point.ToString();
            focusedCard.cardType = cardSO.CardType;
            focusedCard.cardArt.sprite = cardSO.CardArt;
            focusedCard.cardClass.text = cardSO.Cardclass.ToString();
            if (focusedCard.cardType == CardType.Heal)
            {
                focusedCard.cardTypeArt.sprite = cardSO.cardTypeArt[0];
            }
            else
            {
                focusedCard.cardTypeArt.sprite = cardSO.cardTypeArt[1];
            }
            focusedCardUI.SetActive(true);
        }
        else
        {
            if (!cardSelected)
            {
                focusedCardUI.SetActive(false);
                Reset();
            }
        }
    }

    internal void Reset()
    {
        //GetComponent<Image>().color = Color.white;
        cardAnim.SetBool("isAnimating", false);
        transform.localScale = Vector3.one;
        cardSelected = false;
        cardInFocus = false;
    }

    internal void Kill()
    {
        dead = true;
        GetComponent<RectTransform>().localScale = Vector3.zero;
        Deck.deckInstance.ShowDeck();
    }
    internal void ReCreate()
    {
        dead = false;
        GetComponent<RectTransform>().localScale = Vector3.one;
    }
    public void MoveCard(GameObject MoveTo)
    {
        GameObject overlayGO = new GameObject();
        overlayGO.AddComponent<Image>();
        overlayGO.GetComponent<Image>().sprite = cardArt.sprite;
        overlayGO.transform.SetParent(transform.GetComponentInParent<Canvas>().transform);
        overlayGO.transform.position = transform.position;
        StartCoroutine(LinearMovement(overlayGO, MoveTo));
    }
    IEnumerator LinearMovement(GameObject ObjA, GameObject ObjB)
    {
        float dis = (ObjA.transform.position - ObjB.transform.position).magnitude;

        while (dis >= 10)
        {
            dis = (ObjA.transform.position - ObjB.transform.position).magnitude;
            ObjA.transform.position = Vector3.Slerp(ObjA.transform.position, ObjB.transform.position, Time.deltaTime * 5);
            yield return null;
        }
        Destroy(ObjA);
    }
}
