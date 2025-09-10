using UnityEngine;

namespace Tokidos
{
    public class GridController : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private Texture2D _7By7Texture = new (7, 7);
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        
        void Start()
        {
            _7By7Texture.filterMode = FilterMode.Point;
            
            _meshRenderer.material.mainTexture = _7By7Texture;
        }

        public void SetLED(int x, int y, Color color)
        {
            _7By7Texture.SetPixel(x, y, color);
            _7By7Texture.Apply();
        }
    }
}
