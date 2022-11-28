using DG.Tweening;
using UnityEngine;

public class Log : MonoBehaviour
{

    [SerializeField]
    PoolObjectType objectType;

    public bool IsEnterTrigger
    {
        get
        {
            return isEnterTrigger;
        }
        set
        {
            isEnterTrigger = value;
        }
    }

    bool isEnterTrigger = true;

    public void ReturnObject()
    {
        isEnterTrigger = true;

        gameObject.transform.DOKill();

        ObjectPool.Instance.ReturnObject(
            objectType, gameObject);
    }

}