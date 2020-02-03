using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RPCCall : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void setTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTargetVariables", RpcTarget.Others, GameManager.instance.RandomPrefab, GameManager.instance.RandomPoint, GameManager.instance.RandomHealth);
        }
    }
    public void UpdateTurn()
    {

        photonView.RPC("SetTurn", RpcTarget.Others);

    }
    [PunRPC]
    void SetTargetVariables(int[] index, int[] temp, int[] number)
    {
        for (int i = 0; i < 9; i++)
        {
            GameManager.instance.RandomPrefab[i] = index[i];
            GameManager.instance.RandomPoint[i] = temp[i];
            GameManager.instance.RandomHealth[i] = number[i];
        }
        GameManager.instance.createCard();
    }
    [PunRPC]
    void SetTurn()
    {
        GameManager.instance.myTurn = true;
        GameManager.instance.isSimulating = false;
        Timer.instance.StartTimer(1);
        GameManager.instance.currState = PhotonNetwork.IsMasterClient ? GameManager.GameState.onePlayerTurn : GameManager.GameState.twoPlayerTurn;
        GameManager.instance.lockInput.SetActive(false);
    }
}
