using DG.Tweening;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEditor;

public class PlayerComponent : Component
{

    GameObject player;

    IObservable<Vector3> moveStream;

    IObservable<GameObject> overStream;

    IObservable<Collider> triggerStream;

    public PlayerComponent()
    {
        moveStream = Observable.EveryUpdate()
            .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
            .Select(_ => player.transform.position);
    }

    public void Subscribe(Action<Vector3> action)
    {
        moveStream.Subscribe(action);
    }

    public void Subscribe(Action<GameObject> action)
    {
        overStream.Subscribe(action);
    }

    public void Subscribe(Action<Collider> action)
    {
        triggerStream.Subscribe(action);
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                player = ObjectPool.Instance.GetObject(PoolObjectType.Player);

                overStream = player.transform.GetChild(0)
                    .OnBecameInvisibleAsObservable()
                    .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
                    .Select(_ => player);

                triggerStream = player.OnTriggerEnterAsObservable();

                triggerStream
                    .Subscribe(collider =>
                        {
                            if (GameManager.Instance.STATE != GameState.RUNNING)
                                return;

                            switch (collider.tag)
                            {
                                case "Car":
                                    GameManager.Instance.UpdateState(GameState.OVER);

                                    player.transform.DOScaleY(.1f, .16f);
                                    break;
                                case "Train":
                                    GameManager.Instance.UpdateState(GameState.OVER);

                                    player.transform.DOScaleY(.1f, .16f);
                                    break;
                                default:
                                    break;
                            }
                        });

                GameManager.Instance.GetGameBaseComponent<InputComponent>().Subscribe(Move);

                break;
            case GameState.STANDBY:
                Reset();
                break;
            default:
                break;
        }
    }

    void Reset()
    {
        player.transform.SetParent(
            ObjectPool.Instance.transform, true);

        player.transform.position = Vector3.zero;
        player.transform.localEulerAngles = Vector3.zero;
        player.transform.localScale = Vector3.one;
    }

    void Move(Direction direction)
    {
        if (direction == Direction.None) return;

        player.transform.DOScaleY(.8f, .08f).OnComplete(() =>
        {
            player.transform.DOScaleY(1, .08f);

            Vector3 pos = player.transform.position;

            RaycastHit hit;

        if (Physics.Raycast(player.transform.position + Vector3.up,
            GetConvertedDirectionV3(direction)
            + Vector3.down, out hit, Mathf.Sqrt(2.5f)
                , ~(1 << LayerMask.NameToLayer(
                    "Ignore Raycast"))))
            {
                Vector3 movePos;
                Vector3 scale;

                switch (hit.collider.tag)
                {
                    case "Car":
                        movePos = hit.normal;
                        movePos.z *= .5f;

                        scale = Vector3.one;
                        scale.z *= Math.Abs(scale.z) > 0 ? .25f : 1;

                        player.transform.DOKill();

                        player.transform.SetParent(hit.collider.transform, true);

                        player.transform.DOScale(scale, .16f);

                        player.transform.DOLocalMove(movePos, .16f);

                        player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);

                        GameManager.Instance.UpdateState(GameState.OVER);
                        return;
                    case "Train":
                        movePos = hit.point;

                        scale = new Vector3(1, 1, .25f);

                        player.transform.DOKill();

                        player.transform.SetParent(
                            hit.collider.transform, true);

                        player.transform.DOScale(scale, .16f);

                        player.transform.DOMove(movePos, .16f);

                        player.transform.DOLocalRotate(
                            GetConvertedRotateV3(direction), .32f,
                            RotateMode.Fast);

                        GameManager.Instance.UpdateState(GameState.OVER);
                        return;
                    case "Log":
                        player.transform.DOKill();

                        player.transform.SetParent(
                            hit.collider.transform, true);

                        player.transform
                        .DOLocalJump(
                            new Vector3(0, .3f, 0),
                            1, 1, .32f);
                        player.transform.DOScaleY(1, .32f);

                        player.transform.DOLocalRotate(
                                 GetConvertedRotateV3(direction), .32f,
                                 RotateMode.Fast);
                        return;
                    case "Water":
                        GameManager.Instance.UpdateState(GameState.OVER);

                        switch (player.transform.parent.tag)
                        {
                            case "Log":
                                player.transform.DOLocalJump(GetConvertedDirectionV3(direction), 1, 1, .32f).SetEase(Ease.Linear).OnComplete(() =>
                                {
                                    player.transform.DOMoveY(player.transform.position.y - 2, .19f).SetEase(Ease.Linear);
                                });

                                player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);
                                break;
                            default:
                                player.transform.DOJump(GetConvertedDirectionV3(direction), 1, 1, .32f).SetRelative().SetEase(Ease.Linear).OnComplete(() =>
                                {
                                    player.transform.DOMoveY(player.transform.position.y - 2, .19f).SetEase(Ease.Linear);
                                });

                                player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);
                                break;
                        }
                        return;
                    default:
                        break;
                }
            }
            else
            {
                switch (player.transform.parent.tag)
                {
                    case "Log":
                        player.transform.SetParent(ObjectPool.Instance.transform, true);

                        Vector3 movePos = player.transform.position;
                        movePos += GetConvertedDirectionV3(direction);

                        movePos = new Vector3((float)Math.Round(movePos.x, MidpointRounding.AwayFromZero), 0, (float)Math.Round(movePos.z, MidpointRounding.AwayFromZero));

                        player.transform.DOKill();

                        player.transform.DOJump(movePos, 1, 1, .32f);
                        player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);
                        return;
                    default:
                        pos += GetConvertedDirectionV3(direction);
                        break;
                }
            }

            player.transform.DOJump(pos, 1, 1, .32f).OnComplete(() =>
             {
                 player.transform.position = new Vector3((float)Math.Round(player.transform.position.x, 1), 0, (float)Math.Round(player.transform.position.z, 1));
             });

            player.transform.DOLocalRotate(GetConvertedRotateV3(direction), .32f, RotateMode.Fast);
        });
    }

    Vector3 GetConvertedDirectionV3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.forward;
            case Direction.Back:
                return Vector3.back;
            case Direction.Left:
                return Vector3.left;
            case Direction.Right:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    Vector3 GetConvertedRotateV3(Direction direction)
    {
        switch (direction)
        {
            case Direction.Forward:
                return Vector3.zero;
            case Direction.Back:
                return Vector3.down * 180;
            case Direction.Left:
                return Vector3.down * 90;
            case Direction.Right:
                return Vector3.up * 90;
            default:
                return Vector3.zero;
        }
    }
}