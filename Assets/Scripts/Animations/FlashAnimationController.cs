using System;
using DG.Tweening;
using UnityEngine;

public class FlashAnimationController : MonoBehaviour
{
    public Color flashColor;
    public Color originalColor;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int numberOfFlashes = 3;
    
    private Sequence _flashSequence;

    public void SetFlashSequence(Color targetColor, Material targetMaterial)
    {
        originalColor = targetMaterial.color;
        _flashSequence = DOTween.Sequence();
        _flashSequence.SetAutoKill(false);
        _flashSequence.Pause(); //so it doesn't start automatically

        for (int i = 0; i < numberOfFlashes; i++)
        {
            _flashSequence.Append(targetMaterial.DOColor(targetColor, flashDuration));
            _flashSequence.Append(targetMaterial.DOColor(originalColor, flashDuration));
        }
    }

    public void DoFlashAnimation(Color targetColor)
    {
        _flashSequence.Restart();
    }
}
