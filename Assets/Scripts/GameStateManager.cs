using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameStateManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform player;
    State currentState;
    NavMeshAgent agent;
    Animator Animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        currentState = new Idle(this.gameObject,agent,Animator,player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState=currentState.Process();
    }
}
    public class State
    {
        public enum STATE { IDLE, CHASE, PATROL, ATTACK, DEATH };
        public enum EVENTS { ENTER, UPDATE, EXIT };

        public STATE stateName;
        public EVENTS eventStage;
        public GameObject npc;
        public NavMeshAgent agent;
        public Animator animator;

        public Transform playerPosition;
        public State nextState;
        float visualDistance, visualSpeed, shootingDistance;

        public State(GameObject _npc, NavMeshAgent agent, Animator animator, Transform _playerPosition)
        {
            this.npc = _npc;
            this.agent = agent;
            this.animator = animator;
            this.playerPosition = _playerPosition;
            eventStage = EVENTS.ENTER;
        }
        public virtual void EnterMethod()
        {
            eventStage = EVENTS.UPDATE;
        }

        public virtual void UpdateMethod()
        {
            eventStage = EVENTS.UPDATE;
        }

        public virtual void ExitMethod()
        {
            eventStage = EVENTS.EXIT;
        }

        public State Process()
        {
            if (eventStage == EVENTS.ENTER)
            {
                EnterMethod();
            }

            if (eventStage == EVENTS.UPDATE)
            {
                UpdateMethod();
            }

            if (eventStage == EVENTS.EXIT)
            {
                ExitMethod();
                return nextState;
            }
            return this;
        }


    }


public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent agent, Animator animator, Transform _playerPosition) : base(_npc, agent, animator, _playerPosition)
    {
        stateName = STATE.IDLE;
    }
    public override void EnterMethod()
    {
        animator.SetTrigger("isIdle");
        base.EnterMethod();
    }

    public override void UpdateMethod()
    {
        if (Random.Range(0, 100) < 5)
        {
            nextState = new patroll(npc,agent,animator,playerPosition);
            eventStage = EVENTS.EXIT;
        }
        base.UpdateMethod();
    }

    public override void ExitMethod()
    {
        animator.ResetTrigger("isIdle");
        base.ExitMethod();
    }
}

public class patroll:State
{
    int currentIndex = -1;

    public patroll(GameObject _npc, NavMeshAgent agent, Animator animator, Transform _playerPosition) : base(_npc, agent, animator, _playerPosition)
    {
        stateName = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void EnterMethod()
    {
        animator.SetTrigger("isWalking");
        currentIndex = 0;
        base.EnterMethod();
    }

    public override void UpdateMethod()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex > GameController.Instance.Checkpoints.Count)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameController.Instance.Checkpoints[currentIndex].transform.position);
        }
            
        base.UpdateMethod();
    }

    public override void ExitMethod()
    {
        animator.ResetTrigger("isIdle");
        base.ExitMethod();
    }
}
