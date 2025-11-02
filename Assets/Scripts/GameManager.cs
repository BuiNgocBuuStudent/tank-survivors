using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerController _player;
    public PlayerController Player => _player;

    [SerializeField] EnemyManager _enemyManager;
    public EnemyManager EnemyManager => _enemyManager;

    [SerializeField] ExpManager _expManager;
    public ExpManager ExpManager => _expManager;
    // Start is called before the first frame update
    void Start()
    {
        this.Init();
    }
    public void Init()
    {
        Player.Init();
        EnemyManager.Init();
        ExpManager.Init();
    }
    // Update is called once per frame
    void Update()
    {

    }

   
}
