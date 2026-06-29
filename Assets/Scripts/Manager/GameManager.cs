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
        EnemyManager.Init();
        ExpManager.Init();
        //BoostManager.Init();

        if (_sessionData == null)
        {
            Debug.LogError("[GameManager] _sessionData chưa được gán! Kéo PlayerSessionData asset vào Inspector.");
            return;
        }

        int tankId = _sessionData.selectedTankId;
        _player = Instantiate(_playerPrefabs[tankId], this.transform.position, Quaternion.identity);

        DataPersistenceManager.Instance.RegisterAndLoad(_player);

        _player.Init();

        // QUAN TRỌNG: OnPlayerReady phải được gọi TRƯỚC ApplySessionData.
        // HUDController lắng nghe OnPlayerReady để BindToPlayer (subscribe vào
        // player events). Nếu ApplySessionData chạy trước, các events
        // OnMaxHealthSet / OnMaxEnergySet / OnHealthChanged sẽ fire khi HUD
        // chưa subscribe → HUD miss toàn bộ data mới → bar không cập nhật.
        OnPlayerReady?.Invoke(_player);

        _player.ApplySessionData(_sessionData);

        _player.gameObject.SetActive(true);
    }
    private void Update()
    {
        Vector3 followPos = _player.transform.position;
        followPos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = followPos;
    }
}
