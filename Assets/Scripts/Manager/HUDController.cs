using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// HUD: toàn bộ phần UI hiển thị trong khi chơi
public class HUDController : Singleton<HUDController>, IDataPersistence
{
    [SerializeField] SlideBar _healthBar;
    [SerializeField] SlideBar _energyBar;
    [SerializeField] GameObject _pauseUI;

    [Header("-----Statistic UI------")]
    [SerializeField] CoinDrop _coinDropPrefab;
    [SerializeField] TextMeshProUGUI _coinsDropText;
    [SerializeField] TextMeshProUGUI _timerText;
    private float _elapsedTime;
    private int _coinDrop;


    [Header("-----Game Over UI------")]
    [SerializeField] GameObject _gameOverUI;
    [SerializeField] TextMeshProUGUI _lastCoinsText;
    [SerializeField] TextMeshProUGUI _survivingTimeText;

    // Giữ reference để hủy đăng ký khi player bị destroy
    private PlayerControllerBase _boundPlayer;

    protected override void Awake()
    {
        base.Awake(); 

        GameManager.OnPlayerReady += OnPlayerReady;
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60);
        _timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    private void OnDestroy()
    {
        GameManager.OnPlayerReady -= OnPlayerReady;
        UnbindFromPlayer();
    }

    // ===========================================
    // BIND / UNBIND
    // ===========================================

    /// <summary>
    /// Được gọi bởi GameManager.OnPlayerReady sau khi player init + ApplySessionData xong.
    /// Hủy bind player cũ (nếu có), bind player mới.
    /// </summary>
    private void OnPlayerReady(PlayerControllerBase player)
    {
        UnbindFromPlayer();
        BindToPlayer(player);
    }

    private void BindToPlayer(PlayerControllerBase player)
    {
        if (player == null) return;
        _boundPlayer = player;

        player.OnMaxHealthSet += SetMaxHealth;
        player.OnMaxEnergySet += SetMaxEnergy;

        player.OnHealthChanged += UpdateHealth;
        player.OnEnergyChanged += UpdateEnergy;
    }

    private void UnbindFromPlayer()
    {
        if (_boundPlayer == null) return;

        _boundPlayer.OnMaxHealthSet  -= SetMaxHealth;
        _boundPlayer.OnMaxEnergySet  -= SetMaxEnergy;
        _boundPlayer.OnHealthChanged -= UpdateHealth;
        _boundPlayer.OnEnergyChanged -= UpdateEnergy;
        _boundPlayer = null;
    }

#region SlideBar Update
    private void SetMaxHealth(float maxHealth)
    {
        if (_healthBar == null) return;
        _healthBar.SetMaxValue(maxHealth);
    }

    private void SetMaxEnergy(float maxEnergy)
    {
        if (_energyBar == null) return;
        _energyBar.SetMaxValue(maxEnergy);
    }

    private void UpdateHealth(float currentHealth)
    {
        if (_healthBar == null) return;
        _healthBar.UpdateValue(currentHealth);
    }

    private void UpdateEnergy(float currentEnergy)
    {
        if (_energyBar == null) return;
        _energyBar.UpdateValue(currentEnergy);
    }
    #endregion

    #region Coin Drop

    public void SpawnCoinDrop(Vector3 pos)
    {
        CoinDrop coinDrop = ObjectPooler.Instance.GetComp(_coinDropPrefab);
        coinDrop.transform.position = pos;
        coinDrop.gameObject.SetActive(true);
    }
    public void SetCoinDropText(int coinDrop)
    {
        _coinDrop += coinDrop;
        _coinsDropText.text = _coinDrop.ToString();
    }
    #endregion

    #region Pause/Over Game State
    public void OnPauseBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        Time.timeScale = 0f;
        _pauseUI.gameObject.SetActive(true);
    }
    public void OnHomeBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        SceneManager.LoadSceneAsync("MainMenu");
    }
    public void OnCancelBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        _pauseUI.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnGameOver()
    {
        Time.timeScale = 0f;
        _lastCoinsText.text = _coinDrop.ToString();
        _survivingTimeText.text = _timerText.text;
        _gameOverUI.gameObject.SetActive(true);
    }
    public void OnRestartBtnClicked()
    {
        AudioManager.Instance.PlaySFX(SFXType.btnClick);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("PlayScene");
        _gameOverUI.gameObject.SetActive(false);
    }
    #endregion
    public void LoadData(GameData data)
    {
        
    }

    public void SaveData(GameData data)
    {
        data.playerCoins += _coinDrop;
    }

}
