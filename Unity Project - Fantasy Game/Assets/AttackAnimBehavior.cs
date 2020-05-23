using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimBehavior : StateMachineBehaviour
{
    [System.Serializable]
    private struct Effect
    {
        public int index;
        public float time;
    }
    [System.Serializable]
    private struct Attack
    {
        public float time;
        public bool cancelEntityGravity;
    }
    [System.Serializable]
    public class AnimHoldInput
    {
        
    }
    private string[] attackNames = { "Normal Attack", "Light Attack", "Heavy Attack", "Dash Attack", "Normal Attack", "Lift Attack" };

    [Header("General Stuff")]

    public int nextComboIndex;
    public bool resetCombo;

    [Space]
    [Header("Hold Input")]
    public bool useHoldInput;
    public KeyCode holdInput = KeyCode.Mouse0;
    public bool changeMoveVector;
    [Tooltip("x, y, z, duration")]
    public Vector4 moveVector;

    [Space]
    [Header("Effects")]
    [SerializeField] private Effect[] effects;

    [Header("Sounds")]
    [SerializeField] Effect[] sounds;

    [Header("Movement")]
    public Vector3 velocity;
    public int frames = 10;

    [Header("Attack Types")]
    public bool breaker = false;
    public bool drag = false;
    public bool launch = false;

    public Vector3 launchForce;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Attacking.instance.ComboIndex = nextComboIndex;
        foreach (string s in attackNames)
        {
            animator.SetBool(s, false);
        }

        if (breaker)
            Attacking.instance.isBreakingAttack = true;
        if (breaker)
            Attacking.instance.isLiftingAttack = true;

        Attacking.instance.CanAttack = false;
        Attacking.instance.attackMove = velocity;
        Attacking.instance.moveDuration = (float)frames / 60;

        //if (stopMovement)
        //  Attacking.instance.move.CancelMovement(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*float time = stateInfo.normalizedTime;
        #region Sounds
        if (sounds.Length > 0)
            for (int i = 0; i < sounds.Length; i++)
            {
                if (Mathf.Abs(sounds[i].time - time) < 0.01f)
                    Attacking.instance.PlaySound(sounds[i].index);
            }
        #endregion

        #region Effects
        //trail
        if (time >= trailStart && time < trailStop)
            Attacking.instance.ToggleParticle(true);
        else
            Attacking.instance.ToggleParticle(false);

        //effects
        if (effects.Length > 0)
            for (int i = 0; i < effects.Length; i++)
            {
                if (Mathf.Abs(effects[i].time - time) < 0.01f)
                    Attacking.instance.PlayEffect(effects[i].index);
            }
        #endregion

        #region Attack Types
        if (Mathf.Abs(time- effectActivationTime) <= 0.01f)
        {
            if (launch)
            {
                Attacking.instance.LaunchTarget(launchForce);
                Debug.Log("Attempt Lift");
            }
        }
        #endregion

        #region Velocity
        if (velStart > 0 && velEnd > 0 && velocity.sqrMagnitude > Mathf.Epsilon)
        {
            if (velStart <= time && velEnd > time)
                Attacking.instance.SetVelocity(velocity);
        }
        #endregion*/
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (breaker)
            Attacking.instance.isBreakingAttack = false;

        Attacking.instance.StartComboTimeout();

        Attacking.instance.ToggleParticle(false);
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
