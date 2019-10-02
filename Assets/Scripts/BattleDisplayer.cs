using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
[System.Serializable]
public class SelectOption : UnityEvent<int>
{
}
public class BattleDisplayer : MonoBehaviour
{


    public int currentlySelectedAction;
    public Transform actionPanel;

    public TextMeshProUGUI[] actionText;

    public SelectOption SelectAction;

    public void DisplayOptions(EntityAction[] optionsToDisplay, EntityContainer container)
    {
        for(int i = 0; i < optionsToDisplay.Length; i++)
        {
            actionText[i].text = optionsToDisplay[i].name;
        }
        currentlySelectedAction = 0;
        SelectAction.AddListener(container.SelectOption);
        Refresh();
    }


    public void ChangeAction(int actionChange)
    {
        actionText[currentlySelectedAction].color = Color.black;

        currentlySelectedAction += actionChange;
        currentlySelectedAction = Mathf.Abs(currentlySelectedAction % actionText.Length);



        Refresh();
    }

    public void Refresh()
    {
        actionText[currentlySelectedAction].color = Color.red;

    }

    public void Update()
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
            SelectAction.Invoke(currentlySelectedAction);
        }
    }
}
