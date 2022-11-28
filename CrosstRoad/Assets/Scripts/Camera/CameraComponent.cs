using UnityEngine;
using DG.Tweening;

public class CameraComponent : Component
{

    private Camera camera;

    private Vector3 distance = new Vector3(2, 10, -5);

    public CameraComponent()
    {
        camera = Camera.main;
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(Follow);
                GameManager.Instance.GetGameBaseComponent<PlayerComponent>().Subscribe(Focus);
                break;
            case GameState.STANDBY:
                Reset();

                break;
            default:
                break;
        }
    }

    void Focus(GameObject player)
    {
        if (camera == null)
            return;

        Vector3 pos = player.transform.position;

        camera.DOOrthoSize(6, 1f);
        camera.transform.DOMove(pos + distance, 1f);
    }

    void Follow(Vector3 v3)
    {
        v3.y = 0;
        v3 += distance;

        if (camera.transform.position.z > v3.z)
            v3.z = camera.transform.position.z;

        if (Mathf.Abs(v3.x) > (v3.x >= 0 ? 8 : 4)) 
            v3.x = camera.transform.position.x;

        camera.transform.position = Vector3.Lerp(camera.transform.position, v3, Time.deltaTime * 5f);
    }

    void Reset()
    {
        camera.transform.position = distance;
        camera.orthographicSize = 7.5f;
    }
}