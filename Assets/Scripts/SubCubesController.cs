using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SubCubesController : MonoBehaviour
{
    [field: SerializeField] public List<CubeController> SubCubes { get; private set; }

    public UnityEvent<CubeController> subCubeLeftClickedEvent;
        
    private void Start()
    {
        foreach (var subCube in SubCubes)
        {
            subCube.leftClicked.AddListener(OnSubCubeLeftClicked);
        }
    }

    /// <summary>
    /// Flash all sub cube faces with a color
    /// </summary>
    /// <param name="colorToFlash">The color the sub cube faces should flash to</param>
    public void FlashAllSubCubes(Color colorToFlash)
    {
        foreach (var subCube in SubCubes)
        {
            subCube.Flash(colorToFlash);
        }
    }

    private void OnSubCubeLeftClicked(CubeController cubeController)
    {
        subCubeLeftClickedEvent?.Invoke(cubeController);
    }

    private void OnDestroy()
    {
        foreach (var subCube in SubCubes)
        {
            subCube.leftClicked.RemoveListener(OnSubCubeLeftClicked);
        }
    }
}