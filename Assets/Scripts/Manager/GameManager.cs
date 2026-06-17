using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] PlayerControllerBase _player;
    public PlayerControllerBase Player => _player;
    [SerializeField] List<PlayerControllerBase> _playerPrefabs;

    [SerializeField] EnemyManager _enemyManager;
    public EnemyManager EnemyManager => _enemyManager;

    [SerializeField] ExpManager _expManager;
    public ExpManager ExpManager => _expManager;

    [SerializeField] BoostManager _boostManager;
    public BoostManager BoostManager => _boostManager;

    [Header("===== Session Data (Scene Transfer) =====")]
    [Tooltip("Cùng ScriptableObject asset với UpgradeManager — đã được ghi bởi PrepareForGame()")]
    [SerializeField] PlayerSessionData _sessionData;

    public static event Action<PlayerControllerBase> OnPlayerReady;
    void Start()
    {
        this.Init();
    }

    public void Init()
    {
        //EnemyManager.Init();
        ExpManager.Init();
        //BoostManager.Init();

        if (_sessionData == null)
        {
            Debug.LogError("[GameManager] _sessionData chưa được gán! Kéo PlayerSessionData asset vào Inspector.");
            return;
        }

        // 1. Instantiate đúng tank đã chọn
        int tankId = _sessionData.selectedTankId;
        _player = Instantiate(_playerPrefabs[tankId], this.transform.position, Quaternion.identity);

        // 2. Đăng ký load data cho player
        DataPersistenceManager.Instance.RegisterAndLoad(_player);

        // 3. Init trước: resolve _rb, _gun, SlideBar references
        _player.Init();

        // 4. Apply sau: ghi đè stats + sync bar + apply skills
        _player.ApplySessionData(_sessionData);

        OnPlayerReady?.Invoke(_player);

        _player.gameObject.SetActive(true);
    }
}
