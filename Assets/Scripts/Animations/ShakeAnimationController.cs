using UnityEngine;
using DG.Tweening;

/// <summary>
/// Applies a shake animation to the game object its attached to
/// </summary>
public class ShakeAnimationController : MonoBehaviour
{
    public float shakeDuration = 0.5f;

    [SerializeField] private Vector3 strength = new Vector3(0, 0, 1f); //only using on Z rotational axis 
    [SerializeField] int vibrato = 0;
    
    [Range(90,180)]
    [SerializeField] private float randomness = 110f; //leaving high since only using across one axis
    [SerializeField] private bool fadeOut = true;
    
    public void StartShake()
    {
        transform.DOShakeRotation(shakeDuration, strength, vibrato, randomness, fadeOut);
    }
}
