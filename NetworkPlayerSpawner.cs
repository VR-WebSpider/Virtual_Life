using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint; 

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}
