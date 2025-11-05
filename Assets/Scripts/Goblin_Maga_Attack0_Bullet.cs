using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin_Maga_Attack0_Bullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private float attackDamage;
    private PlayerOwner playerOwner;

    private Transform targetTransform;

    private void Update()
    {
        if (targetTransform == null || targetTransform.IsDestroyed())
        {
            DestorySelf();
            return;
        }

        transform.LookAt(targetTransform.position + targetTransform.up);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true) return;

        if (other.transform != targetTransform) return;

        if (other.TryGetComponent(out IDamaged damageComponent))
        {
            damageComponent.Damaged(playerOwner, attackDamage);

            DestorySelf();
        }
    }

    public void Initialize(PlayerOwner playerOwner, float attackDamage, Transform targetTransform)
    {
        this.playerOwner = playerOwner;
        this.attackDamage = attackDamage;
        this.targetTransform = targetTransform;
    }

    private void DestorySelf()
    {
        Destroy(gameObject);
    }
}
