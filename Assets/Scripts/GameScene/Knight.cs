using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Soldier
{
    private void Update()
    {
        switch (state)
        {
            case State.Move:
                break;
            case State.Attack:
                break;
            case State.Dead:
                break;
        }
    }


}
