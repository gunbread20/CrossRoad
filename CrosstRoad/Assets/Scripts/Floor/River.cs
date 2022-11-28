using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class River : Floor
{
    [SerializeField]
    PoolObjectType objectType;

    List<Log> logs = new List<Log>();

    float speed = 5;

    int direction = 1;

    public override void Generate()
    {
        direction = Random.Range(0, 101) > 50
            ? -1 : 1;

        speed = Random.Range(70, 150) * .1f;

        StartLogsLoop();
    }

    void StartLogsLoop()
    {
        logs.Add(
            ObjectPool.Instance.GetObject(
                (PoolObjectType)Random.Range(14, 17))
                .GetComponent<Log>());

        logs[logs.Count - 1].transform.position =
            new Vector3(15 * direction, 0, transform.position.z);
        logs[logs.Count - 1].transform.localEulerAngles = Vector3.zero;

        logs[logs.Count - 1].transform.DOLocalMoveX(-15 * direction, speed)
            .SetDelay(Random.Range(2, 4)).SetEase(Ease.Linear)
            .OnPlay(() =>
            {
                StartLogsLoop();
            })
            .OnComplete(() =>
            {
                logs[0].ReturnObject();

                logs.RemoveAt(0);
            });
    }

    public override void Reset()
    {
        AllReturnLogs();

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

    void AllReturnLogs()
    {
        for(int i = 0; i < logs.Count; i++)
            logs[i].ReturnObject();

        logs.Clear();
    }

    public override void CreateCoin()
    {

    }
}