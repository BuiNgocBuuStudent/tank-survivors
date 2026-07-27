using UnityEngine;


public class EnemyDroneDasher : EnemyControllerBase
{
    private enum State
    {
        CHASING,
        DASHING,
        COOLDOWN
    }
    [Header("-----Enemy Drone 02 Config------")]
    [SerializeField] private State _state = State.CHASING;
    private float _timer;
    private Vector2 _dashDir;
    private EnemyDroneDasherConfig _config => (EnemyDroneDasherConfig)EnemyData;

    protected override void OnInit()
    {
        _timer = _config.chaseTimeBeforeDash;
    }
    protected override void UpdateBehavior()
    {
        _timer -= Time.deltaTime;

        switch (_state)
        {
            case State.CHASING:
                if (_timer <= 0)
                {
                    _state = State.DASHING;
                    StartDash();
                }
                break;
            case State.DASHING:
                Rb.velocity = _dashDir * _config.dashSpeed;
                if (_timer <= 0)
                {
                    _state = State.COOLDOWN;
                    EndDash();
                }
                break;
            case State.COOLDOWN:
                if (_timer <= 0)
                {
                    _state = State.CHASING;
                    _timer = _config.chaseTimeBeforeDash;
                }
                break;
        }
    }

    private void StartDash()
    {
        _dashDir = (Player.transform.position - this.transform.position).normalized;
        _timer = _config.dashDuration;
        Rb.velocity = _dashDir * _config.dashSpeed;
    }

    private void EndDash()
    {
        Rb.velocity = Vector2.zero;
        _timer = _config.dashCooldown;
    }

    protected override void ChaseTarget()
    {
        if (_state == State.DASHING || _state == State.COOLDOWN) return;
        base.ChaseTarget();
    }
    // Thêm vào EnemyDrone02.cs để debug
    private void OnDrawGizmosSelected()
    {
        if (_state == State.DASHING)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, _dashDir * 2f);
        }
    }

}
