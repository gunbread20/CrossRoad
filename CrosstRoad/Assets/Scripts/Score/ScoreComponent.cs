using UnityEngine;
using UniRx;
using System;

public class ScoreComponent : Component
{

    readonly string bestScoreKey = "bestScoreKey";

    IObservable<ScoreData> scoreStream;

    int bestScore = 0;

    int score = 0;

    int condition = 0;

    public ScoreComponent()
    {
        if (!PlayerPrefs.HasKey(bestScoreKey))
        {
            PlayerPrefs.SetInt(bestScoreKey, 0);
        }
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>()
                    .Subscribe(UpdateScoreByPlayerTrigger);
                
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>()
                    .Subscribe(UpdateScore);

                GameManager.Instance.GetGameBaseComponent<InputComponent>()
                    .Subscribe(UpdateScore);

                scoreStream = Observable.EveryUpdate()
                    .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
                    .Select(_ => score)
                    .DistinctUntilChanged()
                    .Select(_ => new ScoreData(score, bestScore));
                break;
            case GameState.STANDBY:
                score = 0;

                condition = 0;
                break;
        }
    }

    void UpdateScoreByPlayerTrigger(Collider col)
    {
        switch (col.tag)
        {
            case "Log":
                if(col.transform.parent.parent.GetComponent<Log>()
                    .IsEnterTrigger)
                score += 5;

                col.transform.parent.parent.GetComponent<Log>()
                    .IsEnterTrigger = false;
                break;
        }
    }

    void UpdateScore(Vector3 pos)
    {
        if(condition < pos.z)
        {
            // Condtion - 0
            // Player Position - 0
            // Input W -> Player Move (0, 0, 1)
            // .1432(value) -> .323543 -> .56542342
            score++;

            condition++;

            int posCeil = (int)Mathf.Ceil(pos.z);

            if (posCeil % 10 == 0) score += posCeil;

            if (bestScore < score)
            {
                PlayerPrefs.SetInt(bestScoreKey, score);

                bestScore = score;
            }
        }
    }

    void UpdateScore(Direction direction)
    {
        switch (direction)
        {
            case Direction.Back:
                score--;
                break;
        }
    } 

    public void Subscribe(Action<ScoreData> action)
    {
        scoreStream.Subscribe(action);
    }
}