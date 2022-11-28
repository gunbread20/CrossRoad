using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField]
    internal GameState screenState = GameState.STANDBY;
    [SerializeField]
    protected CanvasGroup canvasGroup;

    bool isFirst = true;

    public virtual void UpdateScreenStatus(bool open)
    {
        if (isFirst && open)
            OnStart();

        canvasGroup.alpha = open ? 1 : 0;
        canvasGroup.interactable = open;
        canvasGroup.blocksRaycasts = open;
    }

    protected virtual void OnStart()
    {
        isFirst = false;
    }

    public virtual void Init()
    {

    }
}