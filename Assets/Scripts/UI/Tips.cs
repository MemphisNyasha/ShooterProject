using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Tips : MonoBehaviour
{
    public TextMeshProUGUI Text;
    [Range(0f, 10f)]
    public float TimeToShow = 8f;
    [Range(0f, 5f)]
    public float FadeTime = 1.5f;
    [Range(0f, 500f)]
    public float MoveDistance = 100f;

    private CanvasGroup canvasGroup;
    private float yPos;

    public static Tips Instance { get; private set; }

    private void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();

        yPos = this.transform.position.y;

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    
    public void ShowTip(string text)
    {
        Text.text = text;

        StopAllCoroutines();
        StartCoroutine(ShowUI());
    }

    private IEnumerator ShowUI()
    {
        canvasGroup.DOFade(1, FadeTime);
        this.transform.position = new Vector3(this.transform.position.x, yPos - MoveDistance, this.transform.position.z);

        float endValue = yPos;
        this.transform.DOMoveY(endValue, FadeTime);

        yield return new WaitForSeconds(TimeToShow);

        canvasGroup.DOFade(0, FadeTime);

        endValue = yPos + MoveDistance;
        this.transform.DOMoveY(endValue, FadeTime);
    }
}
