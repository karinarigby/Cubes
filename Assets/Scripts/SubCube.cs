
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

public class SubCube : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<SubCube> leftClicked;
    
    private SpriteShapeRenderer _renderer;
    public Color Color { get; private set; }
    
    private void Awake()
    {
        _renderer = GetComponent<SpriteShapeRenderer>();
        Color = _renderer.color;
    }


    public void Flash(Color colorToFlash)
    {
        //start flash animation
        Debug.LogFormat("SubCube Flashing color {0}", colorToFlash);
        _renderer.color = colorToFlash;
        
        //return to regular colors
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClicked.Invoke(this);
        }
    }
}
