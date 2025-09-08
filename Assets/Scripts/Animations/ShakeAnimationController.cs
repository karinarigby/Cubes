using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeAnimationController : MonoBehaviour
{
    public float shakeDuration = 0.5f;

    [SerializeField] private Vector3 strength = new Vector3(0, 0, 1f); //only using on Z rotational axis 
    [SerializeField] int vibrato = 0;
    [SerializeField] private float randomness = 110f; //leaving high since only using across one axis
    [SerializeField] private bool fadeOut = true;
    
    // Since iterating quickly is valued at your studio, instead of creating manually I'd opt for 
    // using this trusted open source library
    public void StartShake()
    {
        transform.DOShakeRotation(shakeDuration, strength, vibrato, randomness, fadeOut);
    }
}
