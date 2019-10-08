using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Status
{ 
    Poisoned,
    Buffed,
    Stunned
}
[CreateAssetMenu(menuName = "Entity")]
public class EntityStats : ScriptableObject
{

    public EntityAction[] entityActions;


    public Sprite sprite;
    public int MaxHealth;
    public int BaseDamage;


}
