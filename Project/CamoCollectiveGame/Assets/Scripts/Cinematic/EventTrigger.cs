using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : StateMachineBehaviour
{
    [SerializeField]
    private GameEvent m_onStart;
    [SerializeField]
    private GameEvent m_onUpdate;
    [SerializeField]
    private float m_frame;
    [SerializeField]
    private GameEvent m_onComplete;
    private float m_currentFrame;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Invoke onstart at start of animation
        if (m_onStart)
            m_onStart.Invoke();
        m_currentFrame = 0;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Invoke onUpdate on specified time
        if (m_onUpdate)
        {
            if (m_currentFrame >= m_frame)
                m_onUpdate.Invoke();
        }
        m_currentFrame += Time.deltaTime;
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //invoke onComplete when animation state exits
        if (m_onComplete)
            m_onComplete.Invoke();
    }
}
