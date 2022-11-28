using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class FloorComponent : Component
{
    IObservable<Floor> floorStream;

    List<Floor> floors = new List<Floor>();

    Vector3 defaultFloorPos = new Vector3(0, -.5f, -12);

    int floorCreateCount = 40;

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(pos =>
                {
                    if (pos.z > floors[12].transform.position.z)
                    {
                        ClearSingleFloor();

                        CreateSingleFloor();
                    }
                });

                floorStream = Observable.EveryUpdate()
                    .Where(_ => floors.Count > 0)
                    .Select(_ => floors[floors.Count - 1]
                    .transform.position.z)
                    .DistinctUntilChanged()
                    .Select(_ => floors[floors.Count - 1]);
                break;
            case GameState.STANDBY:
                CreateFloors();

                break;
            default:
                break;
        }
    }

    public void Subscribe(Action<Floor> action)
    {
        floorStream.Subscribe(action);
    }

    void CreateFloors()
    {
        ClearAllFloor();

        for (int i = 0; i < floorCreateCount; i++)
            CreateSingleFloor();
    }

    void CreateSingleFloor()
    {
        Floor floor = ObjectPool.Instance.GetObject(GetRandomFloorType()).GetComponent<Floor>();

        floor.transform.position = GetNextPosition();

        floor.Generate();

        floors.Add(floor);
    }

    void ClearSingleFloor()
    {
        floors[0].Reset();

        floors.RemoveAt(0);
    }

    void ClearAllFloor()
    {
        for (int i = 0; i < floors.Count; i++)
            floors[i].Reset();

        floors.Clear();
     }

    Vector3 GetNextPosition()
    {
        switch (floors.Count)
        {
            case 0:
                return defaultFloorPos;
            default:
                return floors[floors.Count - 1].transform.position + Vector3.forward;
        }
    }

    PoolObjectType GetRandomFloorType()
    {        
        if (floors.Count < (floorCreateCount / 2))
            return floors.Count % 2 == 0 ? PoolObjectType.Floor_Type0 : PoolObjectType.Floor_Type1;
        else
        {
            if (UnityEngine.Random.Range(0, 100) < 80)
            {
                return floors[floors.Count - 1].transform.position.z % 2 != 0 ? PoolObjectType.Floor_Type0 : PoolObjectType.Floor_Type1;
            }
            else
            {
                int random = UnityEngine.Random.Range(0, 101);

                if(random < 33)
                {
                    return PoolObjectType.Road;
                }
                else if(random < 66)
                {
                    return PoolObjectType.TrainTrack;
                }
                else
                {
                    return PoolObjectType.River;
                }
            }
        }
        
    }
}