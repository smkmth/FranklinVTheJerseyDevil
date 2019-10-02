﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action")]
public class EntityAction : ScriptableObject
{
    public List<EntityAction> SubActions;

}