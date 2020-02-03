using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Deck : MonoBehaviour
{
    public static Deck deckInstance;
    public Sprite backArt;
    public Animator myAnim;
    private void Awake()
    {
        deckInstance = this;
    }
    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {

    }
    public void ShowDeck()
    {
        myAnim.SetBool("isDeckOpen", true);
        CreateCardFromDeck();
    }

    void CreateCardFromDeck()
    {
        Debug.Log("CreateCard from deck called");
        GameObject currentCard = ReturnDeadCard();
        Debug.Log(currentCard.name);
        if (currentCard != null)
        {
            GameObject overlayGO = new GameObject();
            overlayGO.AddComponent<Image>();
            overlayGO.GetComponent<Image>().sprite = backArt;
            overlayGO.transform.SetParent(transform.GetComponentInParent<Canvas>().transform);
            overlayGO.transform.position = transform.position;
            overlayGO.transform.localScale = new Vector3(1, 1.5f, 0);
            StartCoroutine(LinearMovement(overlayGO, currentCard));
        }
    }
    private GameObject ReturnDeadCard()
    {

        foreach (GameObject card in GameManager.instance.onePlayerHand)
        {
            if (card.GetComponent<Card>().dead)
            {
                return card;
            }
        }

        return null;
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
        myAnim.SetBool("isDeckOpen", false);
        Card card = ObjB.GetComponent<Card>();
        card.cardSO = GameManager.instance.p1AllCardObjects[GameManager.instance.p1DeckIndex];
        card.SetUp();
        card.ReCreate();
    }

}
