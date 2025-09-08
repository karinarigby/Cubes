
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.U2D;

public class SubCube : MonoBehaviour, IPointerClickHandler
{
    public Styles stylesSource;
    public UnityEvent<SubCube> leftClicked;
    
    private MeshRenderer _renderer;
    private FlashAnimationController _flashAnimationController;
    
    [field: SerializeField] public Color flashColor;
    private Color _lastColorFlashed;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material.color = stylesSource.baseGrey;
        _flashAnimationController = GetComponent<FlashAnimationController>();
    }

    private void Start()
    {
        _flashAnimationController.SetFlashSequence(flashColor);
    }

    public void Flash(Color colorToFlash)
    {
        //start flash animation
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClicked.Invoke(this);
        }
    }
}
