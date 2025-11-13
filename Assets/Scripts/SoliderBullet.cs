using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoliderBullet : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected float attackDamage;
    protected PlayerOwner playerOwner;

    protected Transform targetTransform;

    protected virtual void Update()
    {
        if (targetTransform == null || targetTransform.IsDestroyed())
        {
            DestorySelf();
            return;
        }

        transform.LookAt(targetTransform.position + targetTransform.up);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true) return;

        if (other.transform != targetTransform) return;

        if (other.TryGetComponent(out IDamaged damageComponent))
        {
            damageComponent.Damaged(playerOwner, attackDamage);

            DestorySelf();
        }
    }

    public virtual void Initialize(PlayerOwner playerOwner, float attackDamage, Transform targetTransform)
    {
        this.playerOwner = playerOwner;
        this.attackDamage = attackDamage;
        this.targetTransform = targetTransform;
    }

    protected virtual void DestorySelf()
    {
        Destroy(gameObject);
    }
}
