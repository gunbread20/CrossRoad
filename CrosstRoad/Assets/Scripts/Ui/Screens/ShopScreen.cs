using UnityEngine;
using UnityEngine.UI;

public class ShopScreen : UIScreen
{
    [SerializeField]
    Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.STANDBY));
    }

    public override void UpdateScreenStatus(bool open)
    {
        base.UpdateScreenStatus(open);
    }
}