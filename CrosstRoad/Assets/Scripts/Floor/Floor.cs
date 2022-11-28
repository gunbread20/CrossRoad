using System.Collections.Generic;
using UnityEngine;

public abstract class Floor : MonoBehaviour
{

    protected List<GameObject> coins = new List<GameObject>();

    public abstract void Generate();

    public abstract void Reset();

    public abstract void CreateCoin();
}