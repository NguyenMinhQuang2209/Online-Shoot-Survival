using Unity.Collections;
using Unity.Netcode;

public class GameController : NetworkBehaviour
{
    public static GameController instance;

    public NetworkVariable<int> gameMode = new NetworkVariable<int>();

    public enum GameMode
    {
        Lobby,
        SelectWeapon,
        Play,
        Die,
        Ready
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            gameMode.Value = (int)GameMode.Lobby;
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public bool CanMove()
    {
        return gameMode.Value == (int)GameMode.SelectWeapon || gameMode.Value == (int)GameMode.Play;
    }
    public bool CanDie()
    {
        return gameMode.Value == (int)GameMode.Play;
    }
    public bool CanShoot()
    {
        return gameMode.Value == (int)GameMode.Play;
    }
    public void ChangeCurrentMode(GameMode newMode)
    {
        if (IsServer)
        {
            gameMode.Value = (int)newMode;
        }
    }
}
