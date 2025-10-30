using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour, IDamaged
{
    protected enum State
    {
        Move,
        Attack,
        Dead
    }

    [SerializeField] protected SoldierSO soldierSO;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;

    protected PlayerOwner playerOwner;

    protected int healthNow;
    protected State state;

    protected List<Soldier> inAttackRangeEnemyList;

    protected virtual void Awake()
    {
        healthNow = soldierSO.healthMax;

        agent.speed = soldierSO.speed;
        state = State.Move;

        inAttackRangeEnemyList = new List<Soldier>();
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Move()
    {

    }

    public virtual void Damaged(PlayerOwner DamageResource)
    {

    }

    protected virtual void Dead()
    {

    }

    public PlayerOwner GetPlayerOwner()
    {
        return playerOwner;
    }

    public void SetPlayerOwner(PlayerOwner playerOwner)
    {
        this.playerOwner = playerOwner;
    }
}
