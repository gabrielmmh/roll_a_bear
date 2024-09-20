using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string WALK_FORWARD = "WalkForward";
    
    private const string WALK_BACKWARD = "WalkBackward";

    private const string RUN_FORWARD = "Run Forward";

    [SerializeField] private PlayerController playerController;
    
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(WALK_FORWARD, playerController.WalkForward());
        animator.SetBool(WALK_BACKWARD, playerController.WalkBackward());
        animator.SetBool(RUN_FORWARD, playerController.RunForward());
    }

    private void Update()
    {
        animator.SetBool(WALK_FORWARD, playerController.WalkForward());
        animator.SetBool(WALK_BACKWARD, playerController.WalkBackward());
        animator.SetBool(RUN_FORWARD, playerController.RunForward());
    }
}