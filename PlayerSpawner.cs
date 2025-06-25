using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
            player.transform.rotation = spawnPoint.transform.rotation;

            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPoint.transform.position);
            }
        }
        else
        {
            Debug.LogError("Player or SpawnPoint not found in the scene!");
        }
    }
}
