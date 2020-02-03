using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public RPCCall MyView;
    public static GameManager instance;
    public GameObject cardPrefab;
    public GameObject playArea;
    public Transform player1HandArea;
    public Transform player2HandArea;
    public GameObject[] targetPrefab;

    public GameObject cardInFocus;
    public GameObject MoveTras;
    //[HideInInspector]
    public Card activeCard;
    public bool isSimulating = false;
    public GameObject lockInput;
    public bool myTurn;
    public List<CardSO> p1AllCardObjects;
    public List<CardSO> p2AllCardObjects;
    public int p1DeckIndex = 0;

    int player1Score = 0;
    int player2Score = 0;

    int p2DeckIndex = 0;
    int maxTurns = 40;
    int targetCount = 9;
    int handLimit = 4;
    int currTurn = 1;
    int deckSize = 15;


    public int[] RandomPoint = new int[9];
    public int[] RandomHealth = new int[9];
    public int[] RandomPrefab = new int[9];



    public GameState currState;

    public List<GameObject> onePlayerHand;
    public List<GameObject> twoPlayerHand;
    List<GameObject> oneDeck;
    List<GameObject> twoDeck;
    public List<GameObject> targets;
    public int FixedCount = 0;
    public int maxMachineHealth = 9;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    void Start()
    {

        FixedCount = 0;
        onePlayerHand = new List<GameObject>();
        twoPlayerHand = new List<GameObject>();
        oneDeck = new List<GameObject>();
        twoDeck = new List<GameObject>();
        targets = new List<GameObject>();
        currState = GameState.onePlayerTurn;

        CreatePlayer();

        SpawnHandCards();


    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnTargets();
            myTurn = true;
            Timer.instance.StartTimer(1);
        }
        else
        {
            lockInput.SetActive(true);
            myTurn = false;
            Timer.instance.StartTimer(2);
        }


    }

    public override void OnPlayerLeftRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("OnPlayerLeftRoom IsMasterClient"); // called before OnPlayerLeftRoom
            UIManager.instance.openVictoryPanel(PlayerPrefs.GetInt("player1Score"));

            //LoadArena();
        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    private void SpawnHandCards()
    {
        CardSOHolder.instance.GenerateRandomList();
        p1AllCardObjects = CardSOHolder.instance.ReturnRandomCards(deckSize);
        EventsRaiser.instance.RaiseEvent(3, CardSOHolder.instance.randomList.ToArray());

        for (int i = 0; i < handLimit; i++)
        {
            SpawnIndividualCard(1);

        }
    }
    public void GotoHome()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
    private void SpawnIndividualCard(int p)
    {
        if (p == 1)
        {
            GameObject card = Instantiate(cardPrefab, player1HandArea);
            Card c = card.GetComponent<Card>();
            c.cardSO = p1AllCardObjects[p1DeckIndex];
            c.SetUp();
            c.focusedCardUI = cardInFocus;
            onePlayerHand.Add(card);
            p1DeckIndex++;
        }
        else if (p == 2)
        {
            GameObject card = Instantiate(cardPrefab, player2HandArea);
            Card c = card.GetComponent<Card>();
            c.cardSO = p2AllCardObjects[p2DeckIndex];
            c.SetUp();
            c.cardBackSide.SetActive(true);
            c.focusedCardUI = cardInFocus;
            twoPlayerHand.Add(card);
            p2DeckIndex++;
        }
    }
    internal void SpawnOpponentCards(List<int> opponentList)
    {
        CardSOHolder.instance.randomList = opponentList;
        p2AllCardObjects = CardSOHolder.instance.ReturnRandomCards(deckSize);
        for (int i = 0; i < handLimit; i++)
        {

            SpawnIndividualCard(2);
        }

        StartGame();
    }

    void CreatePlayer()
    {
        GameObject go = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerUnit"), Vector3.zero, Quaternion.identity);
        MyView = go.GetComponent<RPCCall>();
    }
    /*void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // e.g. store this gameobject as this player's charater in Player.TagObject
        info.sender.TagObject = this.GameObject;

    }*/
    private void DrawNextCard()
    {
        if (currState == GameState.onePlayerTurn)
        {
            if (oneDeck.Count > 0)
            {
                SpawnIndividualCard(1);
                Timer.instance.StartTimer(1);
            }
        }
        else if (currState == GameState.onePlayerTurn)
        {
            if (twoDeck.Count > 0)
            {
                SpawnIndividualCard(2);
                Timer.instance.StartTimer(2);
            }
        }
    }

    private void SpawnTargets()
    {

        for (int i = 0; i < targetCount; i++)
        {
            RandomPrefab[i] = UnityEngine.Random.Range(0, targetPrefab.Length);
            RandomPoint[i] = UnityEngine.Random.Range(2, maxMachineHealth - 2);
            RandomHealth[i] = 0;


            /*GameObject target = Instantiate(targetPrefab[RandomPrefab], playArea.transform);
            target.GetComponent<TargetScript>().SetupTarget(RandomPoint, RandomHealth);
            */
        }
        createCard();
        MyView.setTarget();


    }
    public void createCard()
    {
        for (int i = 0; i < targetCount; i++)
        {
            GameObject target = Instantiate(targetPrefab[RandomPrefab[i]], playArea.transform);
            target.GetComponent<TargetScript>().SetupTarget(RandomPoint[i], RandomHealth[i]);
            targets.Add(target);

        }

    }
    void setRefTarget()
    {
        for (int i = 0; i < playArea.transform.childCount; i++)
        {
            GameObject Go = playArea.transform.GetChild(i).gameObject;
            Debug.Log(Go.name);
            this.targets.Add(Go);
        }
        /*for (int i = 0; i < targetCount; i++)
        {
            //targets.Add();
            Debug.Log(playArea.transform.gameObject.name);
        }*/

    }
    internal void ActivateCard(Card c)
    {
        if (activeCard != null)
        {
            activeCard.gameObject.GetComponent<Card>().Reset();
        }
        activeCard = c;

        HandleTargetsInteractivity(true);
    }

    public void HandleTargetsInteractivity(bool active)
    {
        if (active)
        {
            foreach (GameObject ts in targets)
            {
                if (!ts.GetComponent<TargetScript>().isHealed)
                {
                    ts.GetComponent<Button>().interactable = true;
                }
            }
        }
        else
        {
            foreach (GameObject ts in targets)
            {
                if (!ts.GetComponent<TargetScript>().isHealed)
                {
                    ts.GetComponent<Button>().interactable = false;
                }
            }
            if (activeCard != null)
            {
                activeCard.Reset();
                activeCard.Kill();
                /* if (onePlayerHand.Contains(activeCard.gameObject))
                 {
                     onePlayerHand.Remove(activeCard.gameObject);
                 }
                 else if (twoPlayerHand.Contains(activeCard.gameObject))
                 {
                     twoPlayerHand.Remove(activeCard.gameObject);
                 }*/
            }
            activeCard = null;
        }
    }
    public void ResetAllButtons()
    {
        foreach (GameObject c in onePlayerHand)
        {
            c.GetComponent<Card>().Reset();
        }
    }

    public void UpdateScore(int score)
    {
        if (myTurn)
        {
            player1Score += score;
        }
        else
        {
            player2Score += score;
        }

        UIManager.instance.UpdateScores(player1Score, player2Score);
    }
    public void ChangeTurnLockInput()
    {
        if (currTurn < maxTurns)
        {
            currState = currState == GameState.onePlayerTurn ? GameState.twoPlayerTurn : GameState.onePlayerTurn;
            currTurn++;

            myTurn = false;

            lockInput.SetActive(true);
            Timer.instance.StartTimer(2);
            MyView.UpdateTurn();
        }
        else
        {
            CheckVictory();
        }

    }

    public void CheckVictory()
    {


        currState = GameState.finished;
        if (player1Score > player2Score)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.instance.openVictoryPanel(player1Score);
            }
            else
            {
                UIManager.instance.openLoosePanel(player2Score);
            }
        }
        else if (player2Score > player1Score)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                UIManager.instance.openLoosePanel(player1Score);
            }
            else
            {
                UIManager.instance.openVictoryPanel(player2Score);
            }
        }
        else
        {
            UIManager.instance.openVictoryPanel(0);
        }



    }
    void GameOver()
    {

    }
    public enum GameState
    {
        onePlayerTurn,
        twoPlayerTurn,
        finished
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

}
