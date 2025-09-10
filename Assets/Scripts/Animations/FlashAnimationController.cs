using DG.Tweening;
using UnityEngine;

/// <summary>
/// Applies a brief flash animation to the provided material
/// </summary>
public class FlashAnimationController : MonoBehaviour
{
    public Color originalColor;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private int numberOfFlashes = 3;
    
    private Sequence _flashSequence;
    
    /// <summary>
    /// Set up the flash sequence so that the targetMaterial's color is changed back and forth with targetColor
    /// </summary>
    /// <param name="targetColor">the color to flash on the material</param>
    /// <param name="targetMaterial">the material to apply the animation to</param>
    public void SetFlashSequence(Color targetColor, Material targetMaterial)
    {
        originalColor = targetMaterial.color;
        _flashSequence = DOTween.Sequence();
        _flashSequence.SetAutoKill(false);
        _flashSequence.Pause(); 

        for (var i = 0; i < numberOfFlashes; i++)
        {
            _flashSequence.Append(targetMaterial.DOColor(targetColor, flashDuration));
            _flashSequence.Append(targetMaterial.DOColor(originalColor, flashDuration));
        }
    }

    /// <summary>
    /// Restarts the flash animation
    /// </summary>
    public void DoFlashAnimation()
    {
        if (_flashSequence == null)
        {
            Debug.LogWarning("Flash Sequence not initialized yet - Call SetFlashSequence first");
            return;
        }
        _flashSequence.Restart();
    }
}
