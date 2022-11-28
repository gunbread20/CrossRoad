using UnityEngine;
using UnityEngine.UI;

public class StandbyScreen : UIScreen
{
    [SerializeField]
    Button startButton;

    [SerializeField]
    Button shopButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.RUNNING));

        shopButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.SHOP));
    }

    public override void UpdateScreenStatus(bool open)
    {
        base.UpdateScreenStatus(open);
    }
}