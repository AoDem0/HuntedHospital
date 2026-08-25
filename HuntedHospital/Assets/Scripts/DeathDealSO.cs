using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deal
{
    public DealType dealType;
    public int dealAmount;
}
[CreateAssetMenu(fileName = "DeathDeals", menuName = "SO/DeathDeals")]
public class DeathDealSO : ScriptableObject
{
    public List<Deal> Deals;
    public int SoulPrice;
}
