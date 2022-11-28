using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Road : Floor
{

    [SerializeField]
    PoolObjectType objectType;

    List<Car> cars = new List<Car>();

    int direction = 1;

    float speed = 5;

    public override void Generate()
    {
        direction = Random.Range(0, 101) > 50 ? -1 : 1;

        speed = Random.Range(40, 80) * .1f;

        CreateCarLoop();
    }

    void CreateCarLoop()
    {
        PoolObjectType carType = (PoolObjectType)Random.Range(7, 11);

        cars.Add(new Car(carType));

        cars[cars.Count - 1].car.transform.position = new Vector3(17 * direction, 0, transform.position.z);
        cars[cars.Count - 1].car.transform.GetChild(0).localEulerAngles = new Vector3(0, direction == 1 ? 0 : 180, 0);

        cars[cars.Count - 1].car.transform.DOLocalMoveX(-17 * direction, speed).SetDelay(Random.Range(10, 40) * .1f).SetEase(Ease.Linear)
            .OnPlay(() =>
            {
                CreateCarLoop();
            })
            .OnComplete(() =>
            {
                ObjectPool.Instance.ReturnObject(cars[0].objectType, cars[0].car);

                cars.RemoveAt(0);
            });
    }

    void AllReturnCars()
    {
        for (int i = 0; i < cars.Count; i++)
        {
            cars[i].car.transform.DOKill();

            ObjectPool.Instance.ReturnObject(cars[i].objectType, cars[i].car);
        }

        cars.Clear();
    }

    public override void Reset()
    {
        AllReturnCars(); 

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

    public override void CreateCoin()
    {

    }
}