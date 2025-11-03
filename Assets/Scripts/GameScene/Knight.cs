using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Knight : Soldier
{
    private enum State
    {
        Move,
        Attack,
        Dead
    }

    private State state;

    private const string ANIMATOR_ATTACK_0 = "Attack0";
    private const string ANIMATOR_MOVE_SPEED = "Speed";
    private const string ANIMATIR_DEAD = "Dead";

    private bool can_Attack0 = true;

    protected override void Awake()
    {
        base.Awake();

        state = State.Move;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out IDamaged damageComponent))
        {
            if (damageComponent.GetPlayerOwner() != playerOwner)
            {
                //敌对士兵
                if (inAttackRangeEnemyList.Contains(other.GetComponent<Soldier>()))
                {
                    return;
                }
                else
                {
                    if (other.GetComponent<Soldier>().IsDead())
                    {
                        inAttackRangeEnemyList.Remove(other.GetComponent<Soldier>());
                        return;
                    }
                    inAttackRangeEnemyList.Add(other.GetComponent<Soldier>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamaged damageComponent))
        {
            if (damageComponent.GetPlayerOwner() != playerOwner)
            {
                //敌对士兵
                inAttackRangeEnemyList.Remove(other.GetComponent<Soldier>());
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Move:
                Move();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Dead:
                break;
            default:
                break;
        }
    }

    protected override void Attack()
    {
        if (attackTargetTransform == null || attackTargetTransform.IsDestroyed())
        {
            state = State.Move;
            return;
        }

        if (Vector3.Distance(transform.position, attackTargetTransform.position) > soldierSO.attackRange)
        {
            state = State.Move;
            //print("there 1");
            //return;
        }

        if (can_Attack0)
        {
            //攻击
            StartCoroutine(Set_Attack0());
        }

    }

    private IEnumerator Set_Attack0()
    {
        can_Attack0 = false;

        animator.SetTrigger(ANIMATOR_ATTACK_0);

        yield return new WaitForSeconds(soldierSO.attack0_Cd);

        can_Attack0 = true;
    }

    public void Attack0()
    {
        if (attackTargetTransform.TryGetComponent(out BaseHome opposedBaseHome))
        {
            //目标是敌对基地
            opposedBaseHome.Damaged(playerOwner, soldierSO.attackDamage);
        }
        else
        {
            Vector3 judgePos = transform.position + (transform.forward * 0.5f * soldierSO.attackRange);
            float judgeRadius = soldierSO.attackRange;

            Collider[] colliders = Physics.OverlapSphere(judgePos, judgeRadius);

            List<Soldier> damageSoldierList = new List<Soldier>();

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Soldier>(out Soldier soldier))
                {
                    if (soldier.GetPlayerOwner() != playerOwner)
                    {
                        if (damageSoldierList.Contains(soldier)) continue;
                        damageSoldierList.Add(soldier);
                        soldier.Damaged(playerOwner, soldierSO.attackDamage);
                    }
                }
            }
        }
    }

    protected override void Move()
    {
        navMeshAgent.isStopped = false;
        animator.SetFloat(ANIMATOR_MOVE_SPEED, soldierSO.speed);

        if (inAttackRangeEnemyList.Count == 0)
        {
            //没有敌人在视野范围里
            attackTargetTransform = FightManager.Instance.GetOpposedBaseHome(playerOwner).transform;
            navMeshAgent.SetDestination(attackTargetTransform.GetComponent<BaseHome>().GetSoldierSpawnPos().position);
        }
        else
        {
            //有敌人在视野范围里
            for (int i = 0; i < inAttackRangeEnemyList.Count; i++)
            {
                if (inAttackRangeEnemyList[i].IsDestroyed() || inAttackRangeEnemyList[i] == null)
                {
                    inAttackRangeEnemyList.Remove(inAttackRangeEnemyList[i]);
                    continue;
                }

                attackTargetTransform = inAttackRangeEnemyList[i].transform;
                navMeshAgent.SetDestination(attackTargetTransform.position);
            }
        }

        if (Vector3.Distance(transform.position, navMeshAgent.destination) <= soldierSO.attackRange)
        {
            //进入攻击范围
            animator.SetFloat(ANIMATOR_MOVE_SPEED, 0f);

            navMeshAgent.isStopped = true;

            state = State.Attack;
        }
    }
    protected override void Dead()
    {
        navMeshAgent.isStopped = true;

        soldierBodyCollider.enabled = false;
        viewRangeCollider.enabled = false;

        animator.SetTrigger(ANIMATIR_DEAD);
        animator.applyRootMotion = true;
    }

    public override void Damaged(PlayerOwner DamageResource, float damageValue)
    {
        healthNow = Mathf.Max(0, healthNow - (int)damageValue);

        soldierHealthSlider.UpdateVisual(healthNow, soldierSO.healthMax);

        if (healthNow <= 0)
        {
            state = State.Dead;
            Dead();
        }
    }
}
