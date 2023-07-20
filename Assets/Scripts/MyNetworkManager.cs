using Unity.Netcode;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] // Add this to show in inspector
    private NetworkObject playerPrefab;
    [SerializeField] // Add this to show in inspector
    private Transform parentCanvas;

    private void Start() 
    {
        Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer)
        {
            SpawnPlayerServerRpc(clientId);
        }
    }

    [ServerRpc]
    private void SpawnPlayerServerRpc(ulong clientId)
    {
        var player = Instantiate(playerPrefab, parentCanvas);
        player.SpawnWithOwnership(clientId);
    }
}
