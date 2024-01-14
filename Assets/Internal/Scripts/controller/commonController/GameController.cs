using Unity.Netcode;

public class GameController : NetworkBehaviour
{

    public string playerName = "";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

}
