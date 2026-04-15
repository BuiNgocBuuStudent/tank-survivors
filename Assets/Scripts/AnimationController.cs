using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = PlayerControllerBase.PlayerState;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator _trackAnim;

    [SerializeField] Animator _leftExhaustAnim;
    [SerializeField] Animator _rightExhaustAnim;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerAnim(PlayerState state)
    {
        if (state == PlayerState.IDLE)
        {
            // IDLE: Track ngừng animation di chuyển
            _trackAnim.SetBool("move", false);

            _leftExhaustAnim.SetBool("accelerate", false);
            _rightExhaustAnim.SetBool("accelerate", false);
        }
        else if (state == PlayerState.MOVE)
        {
            _trackAnim.SetBool("move", true);
            _leftExhaustAnim.SetBool("accelerate", false);
            _rightExhaustAnim.SetBool("accelerate", false);
        }
        else if (state == PlayerState.ACCELERATE)
        {
            _trackAnim.SetBool("move", true);
            _leftExhaustAnim.SetBool("accelerate", true);
            _rightExhaustAnim.SetBool("accelerate", true);
        }
    }
}
