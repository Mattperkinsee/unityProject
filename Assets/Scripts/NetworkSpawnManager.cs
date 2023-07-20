using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawnManager : MonoBehaviour
{
    [SerializeField]
    private NetworkObject playerPrefab;
    [SerializeField]
    private Transform parentCanvas;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnPlayer()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var player = NetworkObject.Instantiate(playerPrefab, position: parentCanvas.position, rotation: Quaternion.identity);
            player.Spawn();
        }
    }
}
