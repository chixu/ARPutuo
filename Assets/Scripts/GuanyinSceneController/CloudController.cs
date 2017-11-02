using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanyin{

enum MoveMode
{
    ToRight,//向右移
    ToLeft//向左移
}

public class CloudController : MonoBehaviour
{
//    bool isRight;
//    [SerializeField] MoveMode mode;
//    float moveX = 20;//云 endX
//
//    void Awake()
//    {
//        switch (mode)
//        {
//            case MoveMode.ToRight:
//                isRight = true;
//                break;
//            case MoveMode.ToLeft:
//                isRight = false;
//                break;
//        }
//    }
//
//    // Use this for initialization
//    void Start()
//    {
//        MoveCloud();
//    }
//
//    void Update()
//    {
//        //销毁
//        if (isRight)
//        {
//            if (transform.position.x == moveX)
//            {
//                Destroy(gameObject);
//            }
//        }
//        else
//        {
//            if (transform.position.x == -moveX)
//            {
//                Destroy(gameObject);
//            }
//        }
//    }
//
//    /// <summary>
//    /// 移动云
//    /// </summary>
//    void MoveCloud()
//    {
//        if (isRight)
//            transform.DOMove(new Vector3(moveX, transform.position.y, transform.position.z), 60).SetEase(Ease.OutQuad);
//        else
//            transform.DOMove(new Vector3(-moveX, transform.position.y, transform.position.z), 60).SetEase(Ease.OutQuad);
//    }
}
}
