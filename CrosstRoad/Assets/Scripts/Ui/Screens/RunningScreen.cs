using UnityEngine;
using UnityEngine.UI;

public class RunningScreen : UIScreen
{
    [SerializeField]
    Button startButton;

    [SerializeField]
    Text bestScore;

    [SerializeField]
    Text score;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.OVER));
    }

    public override void UpdateScreenStatus(bool open)
    {
        base.UpdateScreenStatus(open);
    }

    public override void Init()
    {
        GameManager.Instance.GetGameBaseComponent<ScoreComponent>()
            .Subscribe(data => {
                this.score.text = data.score.ToString();
                this.bestScore.text = "BEST " + data.bestScore;
        });
    }
}