
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(FlashAnimationController))]
[RequireComponent(typeof(ShakeAnimationController))]
public class CubeController : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public int Id { get; private set; }
    public Styles stylesSource;
    public UnityEvent<CubeController> leftClicked;
    public UnityEvent<CubeController> rightClicked;
    
    [SerializeField] MeshRenderer _ledDisplayRenderer;
    private FlashAnimationController _flashAnimationController;
    private ShakeAnimationController _shakeAnimationController;

    [field:SerializeField] public Material NeutralDisplayMaterial { get; private set; }
    [field:SerializeField] public Material PrimaryMaterial { get; private set; }
    
    
    
    public Color flashColor;
    private Color _lastColorFlashed;

    private void Awake()
    {
        _shakeAnimationController = GetComponent<ShakeAnimationController>();
        _flashAnimationController = GetComponent<FlashAnimationController>();
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
            _flashAnimationController.SetFlashSequence(colorToFlash, _ledDisplayRenderer.material );
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

    public void SetLedDisplayMaterial(Material material)
    {
        _ledDisplayRenderer.material = material;
    }

    public void ResetDisplayMaterial()
    {
        _ledDisplayRenderer.material = NeutralDisplayMaterial;
    }
}
