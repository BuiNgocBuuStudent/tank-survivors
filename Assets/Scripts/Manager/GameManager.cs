using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerControllerBase _player;
    public PlayerControllerBase Player => _player;

    [SerializeField] EnemyManager _enemyManager;
    public EnemyManager EnemyManager => _enemyManager;

    [SerializeField] ExpManager _expManager;
    public ExpManager ExpManager => _expManager;

    [SerializeField] BoostManager _boostManager;
    public BoostManager BoostManager => _boostManager;


     // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }
    public void Init()
    {
        Player.Init();
        //EnemyManager.Init();
        ExpManager.Init();
        //BoostManager.Init();
    }
}
