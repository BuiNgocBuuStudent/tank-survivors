using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject DTO dùng để truyền data player giữa MainMenu scene và GameScene.
/// UpgradeManager ghi vào đây trước khi load PlayScene,
/// GameManager đọc ra để khởi tạo player đúng stats và skills.
/// </summary>
[CreateAssetMenu(fileName = "PlayerSessionData", menuName = "Scriptable Objects/PlayerSessionData")]
public class PlayerSessionData : ScriptableObject
{
    [Header("===== Tank =====")]
    public int selectedTankId;

    [Header("===== Stats =====")]
    public float health;
    public float energy;
    public float armor;
    public float dmgMult;

    [Header("===== Skills =====")]
    public List<string> activeSkills = new List<string>();

    /// <summary>
    /// Xóa toàn bộ data về giá trị mặc định trước khi ghi mới.
    /// Gọi trước PrepareForGame() để tránh data cũ lẫn vào.
    /// </summary>
    public void Reset()
    {
        selectedTankId = 0;
        health = 0f;
        energy = 0f;
        armor = 0f;
        dmgMult = 1f;
        activeSkills = new List<string>();
    }
}
