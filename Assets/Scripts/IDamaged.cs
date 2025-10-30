using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamaged
{
    public void Damaged(PlayerOwner damageResource);

    public PlayerOwner GetPlayerOwner();
}
