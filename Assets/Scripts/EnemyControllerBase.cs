using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControllerBase : MonoBehaviour, IGetHit
{
    private PlayerController _player;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] protected GameObject _gemPrefab;

    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _dmg, _initialHealth, _currentHealth;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init(Vector2 randomSpawnPos)
    {
        if (_rb == null)
            _rb = this.GetComponent<Rigidbody2D>();
        _player = GameManager.Instance.Player;

        _currentHealth = _initialHealth;
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

        _rb.velocity = movement.normalized * _moveSpeed;
    }


    public void GetHit(float dmg)
    {
        _currentHealth -= dmg;
        Debug.Log(_currentHealth);
        if(_currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            SpawnGem();
        }
    }

    protected void SpawnGem()
    {
        GameObject gem = ObjectPooler.Instance.GetObject(_gemPrefab);
        gem.transform.position = this.transform.position;
        gem.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;
        isCanGetHit.GetHit(_dmg);
    }
}
