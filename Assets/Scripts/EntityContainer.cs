using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityContainer : MonoBehaviour
{
    public int health;
    public bool playerParty;

    public EntityStats stats;

    private BattleDisplayer battleDisplayer;

    public void Start()
    {
        battleDisplayer = GameObject.Find("BattleManager").GetComponent<BattleDisplayer>();
    }

    public void TakeTurn()
    {
        battleDisplayer.DisplayOptions(stats.entityActions, this);
    }

    public void SelectOption(int optionIndex)
    {
        if(stats.entityActions[optionIndex].SubActions.Count == 0)
        {
            PerformAction();
        }
        else
        {

        }
    }
}
