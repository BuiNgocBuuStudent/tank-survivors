using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyControllerBase : MonoBehaviour, IGetHit
{
    [SerializeField] PlayerController _player;
    Rigidbody2D _rb;

    [SerializeField] float _moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = this.GetComponent<Rigidbody2D>();

    }
    public void Init (Vector2 randomSpawnPos)
    {
       _player = GameManager.Instance.Player;
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
        quaternion.eulerAngles = new Vector3(0,0,angle);
        this.transform.rotation = quaternion;

        _rb.velocity = movement.normalized * _moveSpeed;
    }


    public void GetHit(float dmg)
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGetHit isCanGetHit = collision.gameObject.GetComponent<IGetHit>();
        if (isCanGetHit == null)
            return;
        isCanGetHit.GetHit(1);
    }
}
