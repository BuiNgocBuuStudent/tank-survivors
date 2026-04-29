using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>, IDataPersistence
{
    // ===========================================
    // CONFIG — Kéo thả ScriptableObject vào Inspector
    // ===========================================

    [Header("===== Stat Upgrade Configs =====")]
    [SerializeField] StatUpgradeConfig[] _statConfigs;

    [Header("===== Skill Upgrade Configs =====")]
    [SerializeField] SkillUpgradeConfig[] _skillConfigs;

    [Header("===== Tank Unlock =====")]
    [Tooltip("Chi phí mở khóa mỗi tank. Index 0 = Tank01 (0 = miễn phí)")]
    [SerializeField] int[] _tankUnlockCosts = { 0, 500, 1200, 2000 };

    [Header("===== References =====")]
    [SerializeField] PlayerControllerBase _player;

    // ===========================================
    // RUNTIME STATE — Được lưu/load qua GameData
    // ===========================================


    private Dictionary<string, int> _statLevels = new Dictionary<string, int>();

    // Danh sách index của skill đã unlock (index trong mảng _skillConfigs)
    private List<int> _unlockedSkills = new List<int>();

    // Danh sách index tank đã unlock (0=Tank01, 1=Tank02, ...)
    private List<int> _unlockedTanksId = new List<int>();

    // Coin hiện tại của player
    [SerializeField] int _playerCoins;

    // Tank được chọn để CHƠI (saved to disk)
    [SerializeField] int _selectedTankId;

    // Tank đang xem trên UI (tạm thời, không save)
    private int _previewTankId;

    // ===========================================
    // EVENTS — UI sẽ đăng ký lắng nghe để tự cập nhật
    // ===========================================

    // Phát khi stat/skill/tank thay đổi → UI refresh
    public event Action OnUpgradeChanged;

    // Phát khi coin thay đổi → cập nhật text coin
    public event Action<int> OnCoinsChanged;

    // ===========================================
    // PROPERTIES — Cho UI và script khác đọc giá trị
    // ===========================================

    public int PlayCoins => _playerCoins;
    public int SelectedTankId => _selectedTankId;

    public int PreviewTankId => _previewTankId;
    public StatUpgradeConfig[] StatConfigs => _statConfigs;
    public SkillUpgradeConfig[] SkillConfigs => _skillConfigs;

    // ===========================================
    // INIT
    // ===========================================

    void Start()
    {
    }

    // ===========================================
    // COIN MANAGEMENT
    // ===========================================

    /// <summary>
    /// Thêm coin cho player. Gọi khi giết enemy, hoàn thành wave, v.v.
    /// VD: UpgradeManager.Instance.AddCoins(10);
    /// (Lưu ý: UpgradeManager chưa dùng Singleton<T>, nên truy cập qua 
    /// GameManager hoặc tự thêm static Instance nếu cần)
    /// </summary>
    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Coin must positive!!!");
            return;
        }       
        _playerCoins += amount;
        OnCoinsChanged?.Invoke(_playerCoins);
    }
    /// <summary>
    /// Trừ coin nội bộ. Return false nếu không đủ.
    /// Private vì chỉ gọi bên trong class, không cho bên ngoài trừ tự do.
    /// </summary>
    private bool SpendCoins(int amount)
    {
        if (amount < 0 || _playerCoins < amount)
            return false;
        _playerCoins -= amount;
        OnCoinsChanged?.Invoke(_playerCoins);
        return true;
    }

    // =====================================================
    //   STAT UPGRADE — Health, Energy, Armor, Damage
    // =====================================================

    #region Stat Upgrade

    /// <summary>
    /// Tạo key per-tank: "Tank0_Health", "Tank1_Armor", ...
    /// Tank mới unlock sẽ không có key → GetStatLevel trả về 0 (base).
    /// </summary>
    private string GetStatKey(string statName)
    {
        return $"Tank{_previewTankId}_{statName}";
    }

    /// <summary>
    /// Lấy level hiện tại của 1 stat cho tank đang chọn (0 = base, 10 = max).
    /// </summary>
    public int GetStatLevel(string statName)
    {
        string key = GetStatKey(statName);
        return _statLevels.TryGetValue(key, out int level) ? level : 0;
    }

    /// <summary>
    /// Lấy giá trị thực tế của stat dựa trên level hiện tại.
    /// VD: GetStatValue("Health") khi level=3 → trả về values[3] = 86
    /// </summary>
    public float GetStatValue(string statName)
    {
        StatUpgradeConfig config = FindStatConfig(statName);
        if (config == null)
        {
            Debug.LogWarning($"[UpgradeManager] Không tìm thấy config: {statName}");
            return 0f;
        }

        int level = GetStatLevel(statName);
        if (level >= config.values.Length)
            level = config.values.Length - 1;
        return config.values[level];
    }

    /// <summary>
    /// Lấy chi phí để nâng cấp lên level tiếp theo.
    /// Return -1 nếu đã max → UI dùng để hiển thị "MAX" thay vì giá coin.
    /// </summary>
    public int GetStatUpgradeCosts(string statName)
    {
        StatUpgradeConfig config = FindStatConfig(statName);
        if (config == null) return -1;

        int level = GetStatLevel(statName);
        if (level >= config.maxLevel) return -1; // Đã max
        return config.costs[level];
    }

    /// <summary>
    /// Check stat đã max level chưa (cho tank đang chọn).
    /// </summary>
    public bool IsStatMaxLevel(string statName)
    {
        StatUpgradeConfig config = FindStatConfig(statName);
        if (config == null) return true;
        return GetStatLevel(statName) >= config.maxLevel;
    }

    /// <summary>
    /// ★ HÀM CHÍNH: Thử nâng cấp 1 stat. Return true nếu thành công.
    /// 
    /// Flow:
    ///   1. Tìm config → 2. Check max level → 3. Check coin → 
    ///   4. Trừ coin → 5. Tăng level → 6. Apply vào player → 7. Fire event
    /// </summary>
    public bool UpgradeStat(string statName)
    {
        StatUpgradeConfig config = FindStatConfig(statName);
        if (config == null)
        {
            Debug.LogWarning($"[UpgradeManager] Không tìm thấy config: {statName}");
            return false;
        }

        int currentLevel = GetStatLevel(statName);
        if (IsStatMaxLevel(statName))
        {
            Debug.Log($"[UpgradeManager] {statName} đã max level");
            return false;
        }

        int cost = config.costs[currentLevel];
        if (!SpendCoins(cost))
        {
            Debug.Log($"[UpgradeManager] Thiếu coin! Cần {cost}, có {_playerCoins}");
            return false;
        }

        string key = GetStatKey(statName);
        _statLevels[key] = currentLevel + 1;
        Debug.Log($"[UpgradeManager] Tank{_selectedTankId}.{statName} → Lv.{currentLevel + 1} (-{cost} coin)");


        ApplyAllStats();

        OnUpgradeChanged?.Invoke();

        return true;
    }

    #endregion

    // =====================================================
    //   TANK UNLOCK — Mở khóa xe tăng mới
    // =====================================================

    #region Tank Unlock

    /// <summary>
    /// Tank đã unlock chưa? Tank01 (index 0) luôn = true.
    /// </summary>
    public bool IsTankUnlocked(int tankIndex)
    {
        if (tankIndex == 0) return true;
        return _unlockedTanksId.Contains(tankIndex);
    }

    /// <summary>
    /// Preview stats của 1 tank bất kỳ (kể cả chưa unlock).
    /// Dùng khi user dùng mũi tên ◄► xem tank khác trên MainMenu.
    /// Stat UI sẽ refresh hiển thị stat của tank đang preview.
    /// </summary>
    public void PreviewTank(int tankIndex)
    {
        _previewTankId = tankIndex;
        OnUpgradeChanged?.Invoke();
    }

    /// <summary>
    /// Chọn tank để CHƠI (chỉ tank đã unlocked).
    /// Khác PreviewTank: hàm này yêu cầu tank phải unlocked.
    /// </summary>
    public bool SetSelectedTankId(int tankIndex)
    {
        if (!IsTankUnlocked(tankIndex))
            return false;

        _selectedTankId = tankIndex;
        _previewTankId = tankIndex;
        OnUpgradeChanged?.Invoke();
        return true;
    }
    /// <summary>
    /// Chi phí unlock tank. Return 0 nếu đã owned, -1 nếu index sai.
    /// </summary>
    public int GetTankUnlockCost(int tankIndex)
    {
        if (IsTankUnlocked(tankIndex)) return 0;
        if (tankIndex < 0 || tankIndex >= _tankUnlockCosts.Length) return -1;
        return _tankUnlockCosts[tankIndex];
    }

    /// <summary>
    /// ★ Thử mở khóa tank. Return true nếu thành công.
    /// </summary>
    public bool UnlockTank(int tankIndex)
    {
        if (IsTankUnlocked(tankIndex))
        {
            Debug.Log($"[UpgradeManager] Tank {tankIndex} đã unlock rồi");
            return false;
        }

        int cost = GetTankUnlockCost(tankIndex);
        if (cost < 0) return false;

        if (!SpendCoins(cost))
        {
            Debug.Log($"[UpgradeManager] Thiếu coin unlock Tank {tankIndex}. Cần {cost}");
            return false;
        }

        _unlockedTanksId.Add(tankIndex);
        Debug.Log($"[UpgradeManager] Unlock Tank {tankIndex}! (-{cost} coin)");

        OnUpgradeChanged?.Invoke();
        return true;
    }

    #endregion

    // =====================================================
    //   SKILL UNLOCK — Mở khóa kỹ năng
    // =====================================================

    #region Skill Unlock

    /// <summary>
    /// Skill đã unlock chưa? (theo index trong mảng _skillConfigs)
    /// </summary>
    public bool IsSkillUnlocked(int skillIndex)
    {
        return _unlockedSkills.Contains(skillIndex);
    }

    /// <summary>
    /// Lấy danh sách skill của 1 tank, sắp xếp theo tier (1→5).
    /// VD: GetSkillsForTank("Tank01") → 5 skills cho xe 01
    /// </summary>
    public List<SkillUpgradeConfig> GetSkillsForTank(string tankId)
    {
        return _skillConfigs
            .Where(s => s.tankId == tankId)
            .OrderBy(s => s.tier)
            .ToList();
    }

    /// <summary>
    /// Tier cao nhất đã unlock của 1 tank.
    /// VD: Tank01 unlock tier 1,2 → return 2. Chưa unlock → return 0.
    /// </summary>
    public int GetHighestUnlockedTier(string tankId)
    {
        int highest = 0;
        for (int i = 0; i < _skillConfigs.Length; i++)
        {
            if (_skillConfigs[i].tankId == tankId
                && IsSkillUnlocked(i)
                && _skillConfigs[i].tier > highest)
            {
                highest = _skillConfigs[i].tier;
            }
        }
        return highest;
    }

    /// <summary>
    /// Tìm index trong _skillConfigs từ 1 SkillUpgradeConfig reference.
    /// UI sẽ dùng: int idx = GetSkillIndex(mySkillConfig);
    ///             TryUnlockSkill(idx);
    /// </summary>
    public int GetSkillIndex(SkillUpgradeConfig skill)
    {
        for (int i = 0; i < _skillConfigs.Length; i++)
        {
            if (_skillConfigs[i] == skill) return i;
        }
        return -1;
    }

    /// <summary>
    /// ★ HÀM CHÍNH: Thử unlock 1 skill. Return true nếu thành công.
    /// 
    /// Flow:
    ///   1. Validate → 2. Check đã unlock → 3. Check tier trước → 
    ///   4. Check tank unlocked → 5. Check coin → 6. Mark unlock → 7. Fire event
    /// </summary>
    public bool TryUnlockSkill(int skillIndex)
    {
        // 1. Validate
        if (skillIndex < 0 || skillIndex >= _skillConfigs.Length)
        {
            Debug.LogWarning($"[UpgradeManager] Skill index {skillIndex} không hợp lệ");
            return false;
        }

        SkillUpgradeConfig skill = _skillConfigs[skillIndex];

        // 2. Đã unlock rồi?
        if (IsSkillUnlocked(skillIndex))
        {
            Debug.Log($"[UpgradeManager] '{skill.skillName}' đã unlock");
            return false;
        }

        // 3. Tier trước đã unlock chưa? (phải mở tuần tự 1→2→3→4→5)
        int highestTier = GetHighestUnlockedTier(skill.tankId);
        if (skill.tier > highestTier + 1)
        {
            Debug.Log($"[UpgradeManager] Cần unlock tier {highestTier + 1} " +
                       $"trước khi unlock '{skill.skillName}' (tier {skill.tier})");
            return false;
        }

        // 4. Tank đã unlock chưa?
        //    Chuyển "Tank01" → index 0, "Tank02" → index 1, ...
        int tankIndex = int.Parse(skill.tankId.Replace("Tank", "")) - 1;
        if (!IsTankUnlocked(tankIndex))
        {
            Debug.Log($"[UpgradeManager] Cần unlock {skill.tankId} trước");
            return false;
        }

        // 5. Trừ coin
        if (!SpendCoins(skill.cost))
        {
            Debug.Log($"[UpgradeManager] Thiếu coin! '{skill.skillName}' cần {skill.cost}");
            return false;
        }

        // 6. Mark unlocked
        _unlockedSkills.Add(skillIndex);
        Debug.Log($"[UpgradeManager] Unlock '{skill.skillName}' (Tier {skill.tier})! (-{skill.cost} coin)");

        // 7. Thông báo UI
        OnUpgradeChanged?.Invoke();

        return true;
    }

    #endregion

    #region Apply Stats

    /// <summary>
    /// Đọc tất cả stat levels → lấy giá trị → ghi vào Player.
    /// Gọi khi: upgrade stat, load game, bắt đầu chơi.
    /// </summary>
    private void ApplyAllStats()
    {
        if (_player == null) return;

        float health  = GetStatValue("Health");
        float energy  = GetStatValue("Energy");
        float armor   = GetStatValue("Armor");
        float dmgMult = GetStatValue("Damage");

        _player.ApplyStatUpgrades(health, energy, armor, dmgMult);
    }

    #endregion

    // =====================================================
    //   SAVE / LOAD — Tích hợp với DataPersistenceManager
    // =====================================================

    #region Data Persistence

    public void LoadData(GameData data)
    {
        // Load coin
        _playerCoins = data.playerCoins;
        _selectedTankId = data.selectedTankId;
        _previewTankId = _selectedTankId;
        // Load stat levels (Dictionary)
        _statLevels.Clear();
        if (data.statLevels != null)
        {
            foreach (var kvp in data.statLevels)
                _statLevels[kvp.Key] = kvp.Value;
        }

        // Load unlocked skills (List<int>)
        _unlockedSkills = data.unlockedSkills != null
            ? new List<int>(data.unlockedSkills)
            : new List<int>();

        // Load unlocked tanks (List<int>)
        _unlockedTanksId = data.unlockedTanks != null
            ? new List<int>(data.unlockedTanks)
            : new List<int>();

        // Áp dụng stats ngay sau khi load
        ApplyAllStats();

        Debug.Log($"[UpgradeManager] Loaded: {_playerCoins} coins, " +
                  $"{_statLevels.Count} stats, {_unlockedSkills.Count} skills, " +
                  $"{_unlockedTanksId.Count} tanks");

    }

    public void SaveData(GameData data)
    {
        data.selectedTankId = _selectedTankId;
        data.playerCoins = _playerCoins;
        data.statLevels = new Dictionary<string, int>(_statLevels);
        data.unlockedSkills = new List<int>(_unlockedSkills);
        data.unlockedTanks = new List<int>(_unlockedTanksId);
    }

    #endregion

    // =====================================================
    //   HELPER — Tìm config
    // =====================================================

    /// <summary>
    /// Tìm StatUpgradeConfig theo tên. Return null nếu không tìm thấy.
    /// </summary>
    private StatUpgradeConfig FindStatConfig(string statName)
    {
        foreach (var config in _statConfigs)
        {
            if (config.statName == statName)
                return config;
        }
        return null;
    }
}
