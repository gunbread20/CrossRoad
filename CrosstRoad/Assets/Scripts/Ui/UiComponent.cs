using System.Collections.Generic;
using UnityEngine;

public class UiComponent : Component
{

    List<UIScreen> screens = new List<UIScreen>();

    public UiComponent()
    {
        this.screens.Add(GameObject.Find("StandbyScreen").GetComponent<UIScreen>());
        this.screens.Add(GameObject.Find("ShopScreen").GetComponent<UIScreen>());
        this.screens.Add(GameObject.Find("RunningScreen").GetComponent<UIScreen>());
        this.screens.Add(GameObject.Find("OverScreen").GetComponent<UIScreen>());
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                InitAllScreens();

                CloseAllScreens();
                break;
            default:
                ActiveScreen(state);
                break;
        }
    }

    void ActiveScreen(GameState type)
    {
        CloseAllScreens();

        GetScreen(type).UpdateScreenStatus(true);
    }

    void CloseAllScreens()
    {
        foreach (var screen in screens)
        {
            screen.UpdateScreenStatus(false);
        }
    }

    void InitAllScreens()
    {
        foreach (var screen in screens)
        {
            screen.Init();
        }
    }

    UIScreen GetScreen(GameState screenState)
    {
        return screens.Find(el => el.screenState == screenState);
    }

}