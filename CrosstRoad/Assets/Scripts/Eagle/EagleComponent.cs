using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EagleComponent : Component
{
    GameObject eagle;

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().
                    Subscribe(Kill);
                break;
        }
    }

    void Kill(GameObject player)
    {
        Debug.Log("Kill");

        GameManager.Instance.UpdateState(GameState.OVER);

        eagle = ObjectPool.Instance.GetObject(PoolObjectType.Eagle);

        eagle.transform.position =
            player.transform.position + (Vector3.up * 2)
            + (Vector3.forward * 25);

        eagle.transform.DOMoveZ(player.transform.position.z, .75f)
            .SetEase(Ease.Linear).OnComplete(() =>
            {
                player.transform.SetParent(eagle.transform, true);

                eagle.transform.DOMoveZ(player.transform.position.z - 25, .75f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Eagle, eagle);
                });
            });
    }
}

