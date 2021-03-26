using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CanvasGroup))]
public class MainUI : MonoBehaviour
{
    private PlayerInput playerInput;
    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (playerInput.Quit())
        {
            Show();
        }
    }

    public void Show()
    {
        if (isOpen)
        {
            Time.timeScale = 1;
            canvasGroup.DOFade(0, 0.5f);
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.DOFade(1, 0.5f).OnComplete(() => Time.timeScale = 0);
            canvasGroup.interactable = true;
        }

        isOpen = !isOpen;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
