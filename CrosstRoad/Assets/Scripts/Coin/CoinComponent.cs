using UnityEngine;

public class CoinComponent : Component
{
    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<FloorComponent>()
                    .Subscribe(CreateCoin);

                break;
        }
    }

    void CreateCoin(Floor floor)
    {
        floor.CreateCoin();
    }
}