using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SubCubesController : MonoBehaviour
{
    [SerializeField] private List<SubCube> subCubes;

    public UnityEvent<SubCube> subCubeLeftClickedEvent;
        
    private void Start()
    {
        foreach (var subCube in subCubes)
        {
            subCube.leftClicked.AddListener(OnSubCubeLeftClicked);
        }
    }

    public void FlashAll(Color colorToFlash)
    {
        Debug.Log("Flashing all sub cubes");
        foreach (var subCube in subCubes)
        {
            subCube.Flash(colorToFlash);
        }
    }

    private void OnSubCubeLeftClicked(SubCube subCube)
    {
        subCubeLeftClickedEvent?.Invoke(subCube);
    }

    private void OnDestroy()
    {
        foreach (var subCube in subCubes)
        {
            subCube.leftClicked.RemoveListener(OnSubCubeLeftClicked);
        }
    }
}