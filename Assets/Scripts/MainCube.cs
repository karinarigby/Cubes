using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class MainCube : MonoBehaviour, IPointerClickHandler
{
    public Styles stylesSource;
    
    private MeshRenderer _renderer;
    private ShakeAnimationController _shakeAnimationController;
    private FlashAnimationController  _flashAnimationController;
    private Color PrimaryColor => stylesSource.pastelRed;
    
    private SubCubesController _subCubesController;
    private Color _lastColorFlashed;


    private void Awake()
    {
        _subCubesController = GetComponent<SubCubesController>();
        _renderer = GetComponent<MeshRenderer>();
        _shakeAnimationController = GetComponent<ShakeAnimationController>();
        _flashAnimationController = GetComponent<FlashAnimationController>();
        _renderer.material.color = stylesSource.baseGrey;
    }

    private void Start()
    {
        _subCubesController.subCubeLeftClickedEvent.AddListener(OnSubCubeLeftClicked);
        _flashAnimationController.SetFlashSequence(PrimaryColor);
    }
    
    private void FlashSubCubes()
    {
        _subCubesController.FlashAll(PrimaryColor);
    }

    private void Shake()
    {
        _shakeAnimationController.StartShake();
    }

    private void OnSubCubeLeftClicked(SubCube subCube)
    {
        FlashColor(subCube.flashColor);
    }

    private void FlashColor(Color colorToFlash)
    {
        if (_flashAnimationController == null)
        {
            return;
        }
        if (colorToFlash != _lastColorFlashed)
        {
            _flashAnimationController.SetFlashSequence(colorToFlash);
        }
        _flashAnimationController.DoFlashAnimation(colorToFlash);
        _lastColorFlashed = colorToFlash;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                FlashColor(PrimaryColor);
                break;
            case PointerEventData.InputButton.Right:
                Shake();
                FlashSubCubes();
                break;
        }
    }

    private void OnDestroy()
    {
        _subCubesController.subCubeLeftClickedEvent.RemoveListener(OnSubCubeLeftClicked);
    }
}