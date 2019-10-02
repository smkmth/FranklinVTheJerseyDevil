using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleDisplayer battleDisplayer;

    public EntityStats[] globalEnemeyEntites;
    public EntityStats[] globalPlayerEntites;
    public int maxCombatants;
    public int currentCombatant;

    public List<EntityContainer> enemyParty;
    public List<EntityContainer> playerParty;
    public List<EntityContainer> combatants;

    public void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        battleDisplayer = GetComponent<BattleDisplayer>();
        int i = 0;
        foreach(EntityContainer container in playerParty)
        {
            container.stats = globalPlayerEntites[i];
            combatants.Add(container);
            i++;
        }
        i = 0;

        int currentCombatant = Random.Range(0, combatants.Count);

        combatants[currentCombatant].TakeTurn();
         



    }

  
}
