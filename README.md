## Tank Survivors

Game sinh tồn 2D top-down được phát triển bằng **Unity (C#)**. Người chơi điều khiển xe tăng chiến đấu chống lại các đợt kẻ thù ngày càng mạnh, thu thập coin để nâng cấp chỉ số và mở khóa kỹ năng.

---

## Tổng Quan

**Tank Survivors** là một game sinh tồn 2D top-down, nơi người chơi điều khiển xe tăng tự động bắn, chiến đấu chống lại các đợt kẻ thù liên tục xuất hiện. Mục tiêu là sống sót càng lâu càng tốt, thu thập coin từ kẻ thù bị tiêu diệt để nâng cấp xe tăng giữa các lượt chơi.

---

## Tính Năng Chính

### Hệ Thống Tank
- **4 xe tăng** có thể mở khóa, mỗi xe có vũ khí và skill tree riêng biệt
- Hệ thống chọn và preview tank trên MainMenu với animation chuyển đổi
- Chi phí mở khóa tăng dần

### Hệ Thống Nâng Cấp (Upgrade System)
- **4 chỉ số nâng cấp**: Health, Energy, Armor, Damage
- Mỗi stat có **10 cấp độ** với giá trị và chi phí tăng dần
- Stat được lưu **per-tank** (mỗi tank có chỉ số riêng)

### Hệ Thống Skill (5-Tier Skill Tree)
- Mỗi tank có **5 skill** mở khóa tuần tự (Tier 1 -> 5)
- Skill ảnh hưởng trực tiếp đến vũ khí: tốc độ bắn, xuyên giáp, nổ, đốt cháy, knockback...

### Hệ Thống Kẻ Thù
- **6 loại kẻ thù** với hành vi AI khác nhau:
  | Loại | Hành vi |
  |------|---------|
  | **Drone Chaser** | Đuổi theo player đơn giản |
  | **Drone Dasher** | Lao nhanh về phía player |
  | **Drone Bomber** | Nổ tung khi chết, phóng đạn radial 360° |
  | **Shooter** | Dừng lại khi trong tầm bắn, bắn đạn về phía player |
  | **Elite Twin Gunner** | Elite enemy với 2 nòng súng |
  | **Elite Artillery** | Hiển thị vòng cảnh báo -> bắn đạn pháo vào vị trí player |

- Spawn theo thời gian với `EnemySpawnEntry` cấu hình: thời điểm xuất hiện, tần suất spawn

### Hệ Thống Vũ Khí & Đạn
- **4 loại vũ khí** (1 per tank), mỗi loại có cơ chế bắn riêng
- **7 loại đạn** với hiệu ứng khác nhau: xuyên, nổ, chain, DOT, knockback
- Hệ thống `DamageMultiplier` cho skill đặc biệt (VD: Overcharge Shot x3 damage)

### Hệ Thống Hiệu Ứng (Effect System)
- **DOT (Damage Over Time)**: gây damage liên tục theo tick
- **Debuff Slow**: giảm tốc độ kẻ thù trong thời gian nhất định
- **Knockback**: đẩy lùi kẻ thù bằng velocity injection
- **Explosion**: gây damage tức thì trong bán kính
- **Burning Zone**: gây DOT cho mọi enemy trong vùng
- **Corrosive Cloud**: burning + bonus % damage

### Hệ Thống Lưu/Tải (Data Persistence)
- Tự động save/load khi chuyển scene
- Lưu trữ: coin, stat levels, skill unlocked, tank unlocked, tank đang chọn
- Hỗ trợ **mã hóa dữ liệu** (encryption) tùy chọn
- File-based storage tại `Application.persistentDataPath`

### Hệ Thống Âm Thanh
- **AudioManager** singleton với AudioMixer hỗ trợ tùy chỉnh volume riêng cho Music/SFX
- 4 bản nhạc nền (3 combat + 1 menu), 7 hiệu ứng âm thanh
- Lưu cài đặt volume qua `PlayerPrefs`

### Các Tính Năng Khác
- **Object Pooling**: tái sử dụng bullet, enemy, VFX để tối ưu bộ nhớ
- **Flash Effect**: hiệu ứng nhấp nháy trắng khi nhận damage
- **Floating Text**: hiển thị damage number bay lên
- **Tank Track**: vệt xích xe tăng khi di chuyển
- **Coin Drop**: animation coin rơi khi tiêu diệt enemy
- **HUD**: thanh HP, thanh Energy, timer, coin counter, pause/game over UI

---

## Công Nghệ Sử Dụng

| Công nghệ | Mô tả |
|------------|-------|
| **Unity 2022.3 LTS** | Game engine |
| **C#** | Ngôn ngữ lập trình |
| **TextMeshPro** | Hiển thị text chất lượng cao |
| **Unity 2D Animation** | Sprite animation |
| **AudioMixer** | Quản lý volume music/sfx |
| **ScriptableObject** | Cấu hình enemy, skill, stat (data-driven design) |
| **JSON Serialization** | Lưu/tải game data |

---

## Cài Đặt & Chạy
### Cách 1:
- Clone repo: git clone https://github.com/BuiNgocBuuStudent/Tank-Survivors.git
- Thêm project vào **Unity Hub**, nháy đúp để mở project
- Trong **Unity Editor**, mở scene **MainMenu** và nhấn **Play**
### Cách 2:
- Tải game trên itch.io: https://buingocbuustudent.itch.io/tank-survivors
- Giải nén file **Tank-Survivor.rar**
- Chạy file **Tank-Survivor.exe**
