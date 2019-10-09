using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI battleOverMessage;

    public void Start()
    {
        battleDisplayer = GetComponent<BattleDisplayer>();
        StartBattle();

    }

    public void StartBattle()
    {
        enemyPartyCount = enemeies.Count;
        playerPartyCount = players.Count;
        currentBattleState = BattleState.BattleContinues;
        //find who we are fighting
        for (int i = 0; i < playerPartyCount; i++)
        {
            combatants[i].Init(players[i], true);
        }
        int j = 0;
        for (int i = playerPartyCount; i < (enemyPartyCount + playerPartyCount); i++)
        {
            if (combatants[i].stats)
            {

                combatants[i].Init(enemeies[j], false);
                j++;
            }
        }

        battleDisplayer.PopulateMenu();



        //find out who goes first
        //  currentCombatant = Random.Range(0, combatants.Count);
        currentCombatant = 0;
        battleDisplayer.HighlightCurrentTurn(combatants[currentCombatant]);

        //call take turn with that guy
        TakeTurn();


    }

   
    public void TakeTurn()
    {
        Debug.Log("Its " + combatants[currentCombatant].name + " Turn! ");
        
        if (combatants[currentCombatant].isControlled && combatants[currentCombatant].CanAttack)
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
        
        foreach(Status status in action.statusEffects)
        {
            target.currentStatus.Add(status);
        }

        target.Health -= action.damage;


    }
 
    public void UpdateBattle()
    {
        for (int i = 0; i < playerPartyCount + enemyPartyCount; i++)
        {
            combatants[i].UpdateText();

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
                UpdateRound();
                currentCombatant = 0;

            }
            if (combatants[currentCombatant].isDead)
            {
                currentCombatant++;
            }
            battleDisplayer.HighlightCurrentTurn(combatants[currentCombatant]);

            TakeTurn();
        }
        else
        {
            EndCombat();
        }
    }

    public void UpdateRound()
    {
        for (int i = 0; i < playerPartyCount + enemyPartyCount; i++)
        {
            if (!combatants[i].isDead)
            {
                combatants[i].ApplyStatusEffects();
                combatants[i].UpdateText();

            }
        }
        UpdateBattle();
    }

    public void EndCombat()
    {
        switch (currentBattleState)
        {
            case BattleState.PartyLost:
                battleOverMessage.gameObject.SetActive(true);
                battleOverMessage.text = "YOU DONE LOST";
                Debug.Log("Failure!");
                break;
            case BattleState.PartyWon:
                battleOverMessage.gameObject.SetActive(true);
                battleOverMessage.text = "YOU DONE WON";

                Debug.Log("Victory!");
                break;
        }
    }


}
