using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAnimBehavior : StateMachineBehaviour
{
    enum AnimType
    {
        none, idle, attack, special, block
    }
    [SerializeField]
    AnimType animType;

    public float parryWindowTime = 0.25f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var rigTest = FindObjectOfType<FirstPersonRigTest>();
        switch (animType)
        {
            case AnimType.idle:
                rigTest.canAttack = true;
                rigTest.blocking = false;
                break;
            case AnimType.attack:
            case AnimType.special:
                rigTest.canAttack = false;
                break;
            case AnimType.block:
                rigTest.StartParry(parryWindowTime);
                rigTest.blocking = true;
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    // 
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //FindObjectOfType<FirstPersonRigTest>().DisableHitbox();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
