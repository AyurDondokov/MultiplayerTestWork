using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GamePhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    private void Start()
    {
        Vector3 pos = new Vector3(Random.Range(0, 5), Random.Range(0, 5));
        PhotonNetwork.Instantiate(playerPrefab.name, pos, Quaternion.identity);
    }
}
