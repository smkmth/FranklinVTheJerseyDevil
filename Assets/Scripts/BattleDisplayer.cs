using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class SelectOption : UnityEvent<int,int>
{}
public enum ActionWindows
{
    Action,
    Target,
    Confirm
}
public class BattleDisplayer : MonoBehaviour
{

    public ActionWindows currentlyFocuedWindow;
    public bool awaitingInput;
    private BattleManager battleManager;
    public int currentlySelectedAction;
    public int currentlySelectedTarget;

    public GameObject textPrefab;
    public Transform actionPanel;
    public Transform enemyPanel;
    public Transform playerPanel;

    private List<TextMeshProUGUI> actionText;
    private List<TextMeshProUGUI> targetText;
    private List<TextMeshProUGUI> playerText;

    public SelectOption SelectAction = new SelectOption();

    public void Start()
    {
        battleManager = GetComponent<BattleManager>();
        SelectAction.AddListener(battleManager.TakePlayerTurn);
        playerText = new List<TextMeshProUGUI>();
        targetText = new List<TextMeshProUGUI>();
        actionText = new List<TextMeshProUGUI>();
    }

    public void PopulateMenu()
    {
        for (int i = 0; i < battleManager.playerPartyCount + battleManager.enemyPartyCount; i++)
        {
            if (!battleManager.combatants[i].isControlled)
            {
                TextMeshProUGUI textMesh = Instantiate(textPrefab, enemyPanel).GetComponent<TextMeshProUGUI>();
                textMesh.text = battleManager.combatants[i].name + " " + battleManager.combatants[i].Health;
                battleManager.combatants[i].label = textMesh;
                targetText.Add(textMesh);
                
            }
            else
            {
                TextMeshProUGUI textMesh = Instantiate(textPrefab, playerPanel).GetComponent<TextMeshProUGUI>();
                textMesh.text = battleManager.combatants[i].name + " " + battleManager.combatants[i].Health;
                battleManager.combatants[i].label = textMesh;
                playerText.Add(textMesh);
            }
        }
    }
  
    public void PopulateActions(EntityStats stats)
    {
        foreach (Transform child in actionPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
        actionText.Clear();
        for (int i = 0; i < stats.entityActions.Length; i++)
        {
            TextMeshProUGUI textMesh = Instantiate(textPrefab, actionPanel).GetComponent<TextMeshProUGUI>();
            textMesh.text = stats.entityActions[i].name;
            actionText.Add(textMesh);
        }

    }
    public void ChangeAction(int actionChange)
    {
        switch (currentlyFocuedWindow)
        {
            case ActionWindows.Action:
                actionText[currentlySelectedAction].color = Color.black;
                currentlySelectedAction += actionChange;
                if (currentlySelectedAction < 0)
                {
                    currentlySelectedAction = (actionText.Count - 1);
                }
                else
                {
                    currentlySelectedAction = currentlySelectedAction % actionText.Count;
                }
                actionText[currentlySelectedAction].color = Color.red;
                break;
            case ActionWindows.Target:
                targetText[currentlySelectedTarget].color = Color.black;
                currentlySelectedTarget += actionChange;
                if (currentlySelectedTarget < 0)
                {
                    currentlySelectedTarget = (targetText.Count - 1);
                }
                else
                {
                    currentlySelectedTarget = currentlySelectedTarget % targetText.Count;
                }
                targetText[currentlySelectedTarget].color = Color.red;
                break;

        }

    }
    
    public void ResetView()
    {
        currentlyFocuedWindow = ActionWindows.Action;
        currentlySelectedAction = 0;
        currentlySelectedTarget = 0;
        foreach(TextMeshProUGUI text in targetText)
        {
            text.color = Color.black;
        }
        foreach (TextMeshProUGUI text in actionText)
        {
            text.color = Color.black;
        }
    }

    public void Update()
    {
        if (awaitingInput)
        {

            if (Input.GetButtonDown("Down"))
            {
                ChangeAction(1);
            }
            if (Input.GetButtonDown("Up"))
            {
                ChangeAction(-1);
            }
            if (Input.GetButtonDown("Select"))
            {
                switch (currentlyFocuedWindow)
                {
                    case ActionWindows.Action:
                        currentlyFocuedWindow = ActionWindows.Target;
                        ChangeAction(0);
                        break;
                    case ActionWindows.Target:
                        currentlyFocuedWindow = ActionWindows.Confirm;
                        ChangeAction(0);
                        break;
                    case ActionWindows.Confirm:
                        awaitingInput = false;
                        ChangeAction(0);
                        SelectAction.Invoke(currentlySelectedAction, currentlySelectedTarget + battleManager.playerPartyCount);
                        ResetView();
                        break;
                }
            }
        }
    }
}
