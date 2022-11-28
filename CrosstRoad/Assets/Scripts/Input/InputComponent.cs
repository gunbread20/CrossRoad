using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public abstract class InputComponent : Component
{
    protected IObservable<Direction> clickStream;

    public InputComponent()
    {
        clickStream = Observable.EveryUpdate()
        .Where(_ => Input.anyKeyDown)
        .ThrottleFirst(TimeSpan.FromMilliseconds(250))
        .Where(_ => GameManager.Instance.STATE == GameState.RUNNING)
        .Select(_ => GetDirection(Input.inputString));
    }

    public abstract Direction GetDirection(string keycode);

    public void UpdateState(GameState state) { }

    public void Subscribe(Action<Direction> action) { clickStream.Subscribe(action); }
}