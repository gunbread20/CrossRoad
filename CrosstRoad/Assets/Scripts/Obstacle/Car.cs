using UnityEngine;

public class Car
{

    public PoolObjectType objectType;

    public GameObject car;

    public Car(PoolObjectType objectType)
    {
        car = ObjectPool.Instance.GetObject(objectType);

        this.objectType = objectType;
    }

}