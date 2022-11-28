using UnityEngine;
using DG.Tweening;

public class TrainTrack : Floor
{

    [SerializeField]
    GameObject train;

    [SerializeField]
    GameObject bell;

    [SerializeField]
    Light notifyLight;

    [SerializeField]
    PoolObjectType objectType;

    float speed = 5;

    public override void Generate()
    {
        CreateTrainLoop();
    }

    void CreateTrainLoop()
    {
        int direction = Random.Range(0, 101) > 50 ? -1 : 1;

        train.transform.position = new Vector3(40 * direction, 0,
            transform.position.z);
        train.transform.GetChild(0).localEulerAngles = new Vector3(
            0, direction == 1 ? 0 : 180, 0);

        float delayTime = Random.Range(1, 3);

        bell.transform.DOScale(Vector3.one * 1.5f, .16f)
            .SetLoops(6, LoopType.Yoyo).SetDelay(delayTime);

        notifyLight.DOIntensity(10, .16f)
            .SetLoops(6, LoopType.Yoyo).SetDelay(delayTime)
            .OnComplete(() =>
            {
                train.transform.DOLocalMoveX(.4f * -direction, speed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    CreateTrainLoop();
                });
            });
    }

    public override void Reset()
    {
        train.transform.DOKill();
        notifyLight.DOKill();
        bell.transform.DOKill();

        bell.transform.localScale = Vector3.one;
        notifyLight.intensity = 0;

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

    public override void CreateCoin()
    {

    }
}