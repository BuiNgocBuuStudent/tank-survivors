using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyControllerBase _enemyPrefab;
    [SerializeField] Gem _gemPrefab;

    float _enemySpawnRate;
    float _offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init()
    {
        _enemySpawnRate = 1f;
        _offset = 2f;
        StartCoroutine(SpawnEnemy());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(_enemySpawnRate);

            EnemyControllerBase enemy = ObjectPooler.Instance.GetComp(_enemyPrefab);
            Vector2 randomPos = this.GetRandomSpawnPos();
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
        Vector2 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height)); // góc trên cùng phải
        Vector2 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); // góc dưới cùng trái

        Vector2 randomPos = Vector2.zero;

        // Chọn ngẫu nhiên 1 trong 4 cạnh
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Trên
                randomPos.x = Random.Range(screenBottomLeft.x, screenTopRight.x);
                randomPos.y = screenTopRight.y + _offset;
                break;
            case 1: // Dưới
                randomPos.x = Random.Range(screenBottomLeft.x, screenTopRight.x);
                randomPos.y = screenBottomLeft.y - _offset;
                break;
            case 2: // Trái
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
