using UnityEngine;
using System;


[Serializable]
public class Craftingngridient
{
    [SerializeField] private ItemDefinition item;
    [SerializeField] private int quanity = 1;

    public ItemDefinition Item => item;
    public int Quanity => quanity;
}
