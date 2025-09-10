using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GridDisplaysScriptableObject", menuName = "Scriptable Objects/GridDisplaysScriptableObject")]
    public class GridDisplays : ScriptableObject
    {
        public DisplayGrid displayGrid;
    }
}