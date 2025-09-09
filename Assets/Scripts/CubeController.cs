
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FlashAnimationController))]
[RequireComponent(typeof(ShakeAnimationController))]
public class CubeController : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public int Id { get; private set; }
    public Styles stylesSource;
    public UnityEvent<CubeController> leftClicked;
    public UnityEvent<CubeController> rightClicked;
    
    [SerializeField] MeshRenderer _renderer;
    private FlashAnimationController _flashAnimationController;
    private ShakeAnimationController _shakeAnimationController;
    
    [field: SerializeField] public Color flashColor;
    private Color _lastColorFlashed;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _shakeAnimationController = GetComponent<ShakeAnimationController>();
        _flashAnimationController = GetComponent<FlashAnimationController>();
    }

    public void SetBaseColor(Color color)
    {
        _renderer.material.color = color;
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
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                leftClicked.Invoke(this);
                break;
            case PointerEventData.InputButton.Right:
                rightClicked.Invoke(this);
                break;
        }
    }
    
    public void Shake()
    {
        _shakeAnimationController.StartShake();
    }
}
