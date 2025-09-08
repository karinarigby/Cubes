using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class MainCube : MonoBehaviour, IPointerClickHandler
{
    public Styles stylesSource;
    
    private SpriteShapeRenderer _renderer;
    private Color PrimaryColor => stylesSource.pastelRed;
    
    [SerializeField] private SubCubesController subCubesController;

    private void Awake()
    {
        _renderer = GetComponent<SpriteShapeRenderer>();
    }

    private void Start()
    {
        if (subCubesController != null)
        {
            subCubesController.subCubeLeftClickedEvent.AddListener(OnSubCubeLeftClicked);
        }
    }
    
    private void FlashSubCubes()
    {
        subCubesController.FlashAll(PrimaryColor);
    }

    private void Shake()
    {
        //start shake animation
        Debug.Log("Main Cube starting to shake");
    }

    private void OnSubCubeLeftClicked(SubCube subCube)
    {
        FlashColor(subCube.Color);
    }

    private void FlashColor(Color colorToFlash)
    {
        //start animation with flash color
        Debug.LogFormat("Main Cube starting to flash {0} color", colorToFlash);
        _renderer.color = colorToFlash;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("Left Click in the frame");
                FlashColor(PrimaryColor);
                break;
            case PointerEventData.InputButton.Right:
                Debug.Log("Right Click in the frame");
                Shake();
                FlashSubCubes();
                break;
        }
    }

    private void OnDestroy()
    {
        subCubesController.subCubeLeftClickedEvent.RemoveListener(OnSubCubeLeftClicked);
    }
}