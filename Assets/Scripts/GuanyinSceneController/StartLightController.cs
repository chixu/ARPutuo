using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Guanyin{
public class StartLightController : MonoBehaviour
{
    [SerializeField] GoddessController goddessController;//观音
    [SerializeField] Material mat;
    [SerializeField] float alpha = 0;
    Color color;
	float alphaSpeed = 0.01f;
	private bool playing;

    void Awake()
    {
        color = mat.GetColor("_TintColor");
        //StartCoroutine(ShowLight());
    }

	public void Reset(){
		alpha = 0;
		mat.SetColor("_TintColor", new Color(color.r, color.g, color.b, alpha));
	}
//
//    /// <summary>
//    /// 显示
//    /// </summary>
//    /// <returns></returns>
//    public IEnumerator ShowLight()
//    {
//        yield return null;
//        alpha += 0.003f;
//        var a = Mathf.Clamp(alpha, 0, 1);
//        mat.SetColor("_TintColor", new Color(color.r, color.g, color.b, a));
//
//        if (a == 1)
//        {
//            StopCoroutine(ShowLight());
//            StopParticle();
//            StartCoroutine(HideLight());
//
//            goddessController.gameObject.SetActive(true);//移动观音
//            goddessController.DoMove();
//        }
//        else
//        {
//            StartCoroutine(ShowLight());
//        }
//    }
//
//    /// <summary>
//    /// 隐藏
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator HideLight()
//    {
//        yield return null;
//        alpha -= 0.002f;
//        var a = Mathf.Clamp(alpha, 0, 1);
//        mat.SetColor("_TintColor", new Color(color.r, color.g, color.b, a));
//
//        if (a == 0)
//        {
//            StopCoroutine(HideLight());
//        }
//        else
//        {
//            StartCoroutine(HideLight());
//        }
//    }


	public void Play(){
		alpha = 0;
		alphaSpeed = Mathf.Abs (alphaSpeed);
		mat.SetColor("_TintColor", new Color(color.r, color.g, color.b, alpha));
		playing = true;
	}


	void Update(){
		if (!playing)
			return;
		alpha += alphaSpeed;
		if (alpha > .7f)
			alphaSpeed = -alphaSpeed;
		mat.SetColor("_TintColor", new Color(color.r, color.g, color.b, alpha));
		if (alpha < 0)
			playing = false;
	}

    /// <summary>
    ///关闭粒子
    /// </summary>
    void StopParticle()
    {
        EllipsoidParticleEmitter[] ePe = GetComponentsInChildren<EllipsoidParticleEmitter>();

        for (int i = 0; i < ePe.Length; i++)
        {
            ePe[i].emit = false;
        }
    }
}
}
