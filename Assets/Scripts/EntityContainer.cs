using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EntityContainer : MonoBehaviour
{
    public int Health;
    public EntityStats stats;
    public Status currentStatus;
    public bool isDead;
    public bool isControlled;
    public TextMeshProUGUI label;
    public SpriteRenderer image;

    public void UpdateText()
    {
        label.text = name + " " + Health;
    }
    public void Init(EntityStats _stats, bool _isControlled)
    {
        isControlled = _isControlled;

        stats = _stats;
        Health = stats.MaxHealth;
        name = stats.name;

        image = GetComponent<SpriteRenderer>();
        image.sprite = stats.sprite;
    }

    public void PlayAnimation(EntityAction action, System.Action _finishedAnimation)
    {

        StartCoroutine(DoLittleAction(_finishedAnimation));
    }

    public IEnumerator DoLittleAction(System.Action _finishedAnimation)
    {
        Debug.Log("ANIMATION");
        yield return new WaitForSeconds(1);
        _finishedAnimation.Invoke();
        Debug.Log("FINISHED");
        

    }

}
