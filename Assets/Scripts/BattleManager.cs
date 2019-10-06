using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattleState
{
    PartyWon,
    PartyLost,
    BattleContinues
}
public class BattleManager : MonoBehaviour
{
    private BattleDisplayer battleDisplayer;

    public List<EntityContainer> combatants;


    public List<EntityStats> players;
    public List<EntityStats> enemeies;

    public int currentCombatant;

    public int playerPartyCount;
    public int enemyPartyCount;

    public int deadPlayers;
    public int deadEnemies;
    public BattleState currentBattleState;

    public void Start()
    {
        battleDisplayer = GetComponent<BattleDisplayer>();
        StartBattle();

    }

    public void StartBattle()
    {
        currentBattleState = BattleState.BattleContinues;
        //find who we are fighting
        for (int i = 0; i < playerPartyCount; i++)
        {
            combatants[i].Init(players[i], true);
        }
        int j = 0;
        for (int i = playerPartyCount; i < (enemyPartyCount + playerPartyCount); i++)
        {
            combatants[i].Init(enemeies[j], false);
            j++;
        }

        enemyPartyCount = enemeies.Count;
        playerPartyCount = players.Count;
        battleDisplayer.PopulateMenu();



        //find out who goes first
        //  currentCombatant = Random.Range(0, combatants.Count);
        currentCombatant = 0;
        //call take turn with that guy
        TakeTurn();


    }

   
    public void TakeTurn()
    {
        Debug.Log("Its " + combatants[currentCombatant].name + " Turn! ");
        
        if (combatants[currentCombatant].isControlled)
        {

            battleDisplayer.PopulateActions(combatants[currentCombatant].stats);
            battleDisplayer.awaitingInput = true;
            return;
        }
        else
        {
            //pick a move from move list 
            int target = Random.Range((0), playerPartyCount);
            int sanity = 0;
            while (combatants[target].isDead)
            {
                sanity++;
                if (sanity > 1000)
                {
                    return;
                }
                target = Random.Range((0), playerPartyCount);

            }
            int action = Random.Range(0, (combatants[currentCombatant].stats.entityActions.Length -1));

            //perform that move on a target      
            PerformAction(combatants[currentCombatant].stats.entityActions[action], combatants[target]);
        }
        
    }

    public void TakePlayerTurn(int selectedActionIndex, int selectedTargetIndex)
    {
        PerformAction(combatants[currentCombatant].stats.entityActions[selectedActionIndex], combatants[selectedTargetIndex]);
        

    }


    public void PerformAction(EntityAction action, EntityContainer target)
    {
        Debug.Log(combatants[currentCombatant].name + " performed " + action.name + " on " + target.name);
        combatants[currentCombatant].PlayAnimation(action, UpdateBattle);
        target.currentStatus = action.statusEffect;
        target.Health -= action.damage;


    }

    public void UpdateBattle()
    {
        for (int i = 0; i < playerPartyCount + enemyPartyCount; i++)
        {
            if (!combatants[i].isDead)
            {

                if (combatants[i].Health <= 0)
                {
                    combatants[i].isDead = true;
                    if (combatants[i].isControlled)
                    {
                        deadPlayers++;
                    }
                    else
                    {
                        deadEnemies++;
                    }
                }
            }
        }
        for(int i =0; i< enemyPartyCount + playerPartyCount; i++)
        {
            
            combatants[i].UpdateText();
        }
        if (deadEnemies >= enemyPartyCount)
        {
            currentBattleState = BattleState.PartyWon;


        }
        else if (deadPlayers >= playerPartyCount)
        {
            currentBattleState = BattleState.PartyLost;
        }
        else
        {
            currentBattleState = BattleState.BattleContinues;

        }


        //if enemies / players are still alive
        if (currentBattleState == BattleState.BattleContinues)
        {
            //find the next entity to take turn 
            currentCombatant++;
            if (currentCombatant >= (playerPartyCount + enemyPartyCount))
            {
                currentCombatant = 0;

            }
            if (combatants[currentCombatant].isDead)
            {
                currentCombatant++;
            }
            TakeTurn();
        }
        else
        {
            EndCombat();
        }
    }

    public void EndCombat()
    {
        switch (currentBattleState)
        {
            case BattleState.PartyLost:
                Debug.Log("Failure!");
                break;
            case BattleState.PartyWon:
                Debug.Log("Victory!");
                break;
        }
    }


}
