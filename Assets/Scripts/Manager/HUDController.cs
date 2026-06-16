using UnityEngine;

public class HUDController : MonoBehaviour // HUD: toàn bộ phần UI hiển thị trong khi chơi
{
    [SerializeField] SlideBar _healthBar;
    [SerializeField] SlideBar _energyBar;

    // Giữ reference để hủy đăng ký khi player bị destroy
    private PlayerControllerBase _boundPlayer;

    private void Awake()
    {
        // Đăng ký vào static event — an toàn dù GameManager chưa tồn tại
        GameManager.OnPlayerReady += OnPlayerReady;
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
}
