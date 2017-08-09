using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgePointController : MonoBehaviour {

    public GameObject mPointer;
    public GameObject mFrame;
    public UnityEngine.UI.Text mTitle;
    public UnityEngine.UI.Text mContent;

    private Coroutine tooltipRoutine;

    // Use this for initialization
    void Start () {

    }
	
    public void OnDisplayTootip()
    {

        tooltipRoutine = StartCoroutine(showTooltip());
       
    }

    public void SetDisplayTooltip(string _title, string _content)
    {

        mTitle.text = _title;
        mContent.text = _content;
    }

    public IEnumerator showTooltip()
    {
        mPointer.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        mFrame.SetActive(true);
    }

    public void OnHideTooltip()
    {
        if(tooltipRoutine != null)
            StopCoroutine(tooltipRoutine);

        mPointer.SetActive(false);
        mFrame.SetActive(false);
    }
}
