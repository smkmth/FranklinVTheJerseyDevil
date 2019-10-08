using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{


    public static StatusEffects statusEffects;

    public int BasePoisionDamage;   //value taken away from health each turn
    public float BaseChanceToStun; //value between 0-100 - hitting above that value stops you attacking

    public void Start()
    {
        if (statusEffects == null)
        {
            statusEffects = this;  
        }
        else
        {
            Destroy(this);
        }
    }
    public void ApplyStatus(Status status, EntityContainer container)
    {
        container.CanAttack = true;
        switch (status)
        {
            case Status.Buffed:
                break;
            case Status.Poisoned:
                container.Health -= BasePoisionDamage;
                Debug.Log(container.name + " is poisioned for " + BasePoisionDamage + "damage");
                break;
            case Status.Stunned:
                float val = Random.Range(0, 100);
                if (val > BaseChanceToStun)
                {
                    container.CanAttack = false;
                }
                break;
        }
    }
}
