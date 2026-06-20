using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("=== Gem ===")]
    [SerializeField] Gem _gemPrefab;

    [Header("=== Enemy Spawn Config ===")]
    [SerializeField] float _offset;

    [SerializeField] List<EnemySpawnEntry> _enemyTypes;

    public void Init()
    {
        foreach (var entry in _enemyTypes)
        {
            StartCoroutine(SpawnEnemyType(entry));
        }
    }

    private IEnumerator SpawnEnemyType(EnemySpawnEntry entry)
    {
        yield return new WaitForSeconds(entry.unlockAtMinute);

        while (true)
        {
            yield return new WaitForSeconds(entry.spawnInterval);

            EnemyControllerBase enemy = ObjectPooler.Instance.GetComp(entry.prefab);
            Vector2 randomPos = GetRandomSpawnPos();
            enemy.Init(randomPos);
            enemy.gameObject.SetActive(true);
        }
    }

    public void SpawnExpGem(Vector2 enemyDiePos)
    {
        Gem gem = ObjectPooler.Instance.GetComp(_gemPrefab);
        gem.transform.position = enemyDiePos;
        gem.gameObject.SetActive(true);
    }

    private Vector2 GetRandomSpawnPos()
    {
        Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));

        Vector2 randomPos = Vector2.zero;

        // Chọn ngẫu nhiên 1 trong 2 cạnh (trái/phải)
        int side = Random.Range(0, 2);

        switch (side)
        {
            case 1: // Trái
                randomPos.x = screenBottomLeft.x - _offset;
                randomPos.y = Random.Range(screenBottomLeft.y, screenTopRight.y);
                break;
            default: // Phải
                randomPos.x = screenTopRight.x + _offset;
                randomPos.y = Random.Range(screenBottomLeft.y, screenTopRight.y);
                break;
        }

        return randomPos;
    }
}

[System.Serializable]
public struct EnemySpawnEntry
{
    [Tooltip("Prefab của enemy loại này")]
    public EnemyControllerBase prefab;

    [Tooltip("Giây giữa mỗi lần spawn (spawn rate)")]
    public float spawnInterval;

    [Tooltip("Phút bắt đầu spawn loại enemy này (0 = ngay từ đầu)")]
    public float unlockAtMinute;
}
