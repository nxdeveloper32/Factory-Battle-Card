using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class StartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int MultiplayerSceneIndex;
    private int playerCount = 2;
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room ");
        //SceneManager.LoadScene(MultiplayerSceneIndex);
        //StartGame();
        /* Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
         if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
         {
             Debug.Log("We load the 'Room for 1' ");


             // #Critical
             // Load the Room Level.
             if (lockLevel)
                 PhotonNetwork.LoadLevel("Room for 1");
         }*/
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.PlayerList.Length == playerCount)
        {
            StartGame();
        }
    }
    // Start is called before the first frame update
    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(MultiplayerSceneIndex);
        }
    }
}
