using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Setting Values")]
    [SerializeField] private string GameVersion;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI LogText;
    [SerializeField] private TextMeshProUGUI NickNameText;
    [SerializeField] private TMP_InputField LobbyInputText;
    [SerializeField] private TMP_InputField NickNameInputText;

    private string NickName;

    private void Start()
    {
        NickNameInputText.text = "Player " + Random.Range(1000, 9999);
        SetNewName();

        PhotonNetwork.GameVersion = GameVersion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() 
    {
        Log("Connected to Master server");
    }

    public void CreateRoom() 
    {
        if (LobbyInputText.text != "")
        {
            PhotonNetwork.CreateRoom(LobbyInputText.text);
        }
        else
            Log("Enter the name of the lobby");
    }

    public void JoinRoom() 
    {
        if (LobbyInputText.text != "")
        {
            PhotonNetwork.JoinRoom(LobbyInputText.text);
        }
        else
            Log("Enter the name of the lobby");
    }

    public override void OnJoinedRoom()
    {
        Log("You joined the room " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Log(newPlayer.NickName + " joined the room");
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public void SetNewName() 
    {
        NickName = NickNameInputText.text;
        NickNameText.text = NickName;
        PhotonNetwork.NickName = NickName;

        Log("Player's name is set to " + NickName);
    }
    public void Log(string text) 
    {
        LogText.text = text;
        Debug.Log(text);
    }
}
