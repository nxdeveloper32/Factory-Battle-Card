using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsRaiser : MonoBehaviour
{
    public static EventsRaiser instance;

    void Start()
    {
        instance = this;
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData p)
    {
        if (p.Code == 1)
        {

            int[] data = (int[])p.CustomData;

            GameManager.instance.activeCard = GameManager.instance.twoPlayerHand[data[0]].GetComponent<Card>();


            GameManager.instance.isSimulating = true;
            GameManager.instance.targets[data[1]].GetComponent<TargetScript>().HandleClick();
        }
        else if (p.Code == 2)
        {
            GameManager.instance.ChangeTurnLockInput();
        }
        else if (p.Code == 3)
        {
            List<int> opponentList = ((int[])p.CustomData).ToList();
            GameManager.instance.SpawnOpponentCards(opponentList);
        }
    }

    public void RaiseEvent(byte evCode)
    {
        System.Object content = null;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }

    public void RaiseEvent(byte evCode, int[] content)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        SendOptions sendOptions = new SendOptions { Reliability = true };
        PhotonNetwork.RaiseEvent(evCode, content, raiseEventOptions, sendOptions);
    }



}
