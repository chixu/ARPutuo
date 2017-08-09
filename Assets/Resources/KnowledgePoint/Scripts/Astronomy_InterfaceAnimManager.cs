using UnityEngine;
using System.Collections;

public class Astronomy_InterfaceAnimManager : MonoBehaviour {
    public GameObject[] childElements;
    public float[] waitTimes;
    private IEnumerator appearA;

    // Use this for initialization
    void Start()
    {
        appearA = appearAnim();
        StartAppear();
        //StartDisappear();
    }

    public void StartAppear()
    {
        if (appearA!=null)
        {
            StopCoroutine(appearA);
        }
        StartCoroutine(appearA=appearAnim());
    }

    IEnumerator appearAnim()
    {
        for(int i=0;i< childElements.Length;i++)
        {
            yield return new WaitForSeconds( waitTimes[i]);
            childElements[i].SetActive(true);
        }
        yield return null;
    }


    public void StartDisappear()
    {
        StopCoroutine(appearA);
        for(int i=0;i<childElements.Length;i++)
        {
            childElements[i].SetActive(false);
        }
    }



}
