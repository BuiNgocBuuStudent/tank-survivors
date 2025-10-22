using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerState = PlayerController.PlayerState;

public class AnimationController : MonoBehaviour
{
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
