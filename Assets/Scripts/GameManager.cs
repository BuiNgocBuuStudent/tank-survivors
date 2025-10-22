using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController _player;
    public PlayerController Player => _player;

    // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }
    public void Init()
    {
        Player.Init();
    }
    // Update is called once per frame
    void Update()
    {

    }

   
}
