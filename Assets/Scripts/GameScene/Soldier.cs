using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour, IDamaged
{

    [SerializeField] protected SoldierSO soldierSO;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected Animator animator;

    [SerializeField] protected CapsuleCollider soldierBodyCollider;
    [SerializeField] protected SphereCollider viewRangeCollider;

    [SerializeField] protected PlayerOwner playerOwner;

    [SerializeField] protected SoldierHealthSlider soldierHealthSlider;

    protected int healthNow;

    protected List<Soldier> inAttackRangeEnemyList;

    protected Transform attackTargetTransform;

    protected bool isDead;

    protected virtual void Awake()
    {
        healthNow = soldierSO.healthMax;

        soldierHealthSlider.UpdateVisual(healthNow, soldierSO.healthMax);

        navMeshAgent.speed = soldierSO.speed;

        inAttackRangeEnemyList = new List<Soldier>();

        isDead = false;
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Move()
    {

    }

    public virtual void Damaged(PlayerOwner DamageResource, float damageValue)
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

    public virtual void DestroySelf()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
