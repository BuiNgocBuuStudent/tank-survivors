using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyControllerBase : MonoBehaviour, IGetHit
{
    private PlayerControllerBase _player;
    private EnemyManager _enemyManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] FlashEffect _flashEffect;
    [SerializeField] EnemyDataBase _enemyDataBase;

    [SerializeField] float _currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init(Vector2 randomSpawnPos)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        _flashEffect = this.GetComponentInChildren<FlashEffect>();
        _player = GameManager.Instance.Player;
        _enemyManager = GameManager.Instance.EnemyManager;

        _currentHealth = _enemyDataBase.intialHealth;
        this.transform.position = randomSpawnPos;
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        ChaseTarget();
    }

    private void ChaseTarget()
    {

        Vector2 movement = _player.transform.position - this.transform.position;

        float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90;
        Quaternion quaternion = this.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        this.transform.rotation = quaternion;

        _rb.velocity = movement.normalized * _enemyDataBase.moveSpeed;
    }

    public void GetHit(float dmg)
    {
        if (gameObject.activeSelf)
            _flashEffect.Flash();

        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            _flashEffect.ResetMaterial();
            _enemyManager.SpawnExpGem(this.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;
        isCanGetHit.GetHit(_enemyDataBase.damage);
    }
}
