using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayController : MonoBehaviour
{
    public static RelayController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public async Task<string> CreateLobby(int slot)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(slot);

            string getJoinedCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData serverData = new(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
            NetworkManager.Singleton.StartHost();
            return getJoinedCode;
        }
        catch (RelayServiceException e)
        {
            LogController.instance.Log(e.Message);
            return null;
        }
    }
    public async void JoinedLobby(string lobbyCode)
    {
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(lobbyCode);

            RelayServerData serverData = new(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            LogController.instance.Log(e.Message);
        }
    }
}
