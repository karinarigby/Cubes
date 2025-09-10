using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tokidos.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GridDisplaysScriptableObject", menuName = "Scriptable Objects/GridDisplaysScriptableObject")]
    public class GridDisplays : ScriptableObject
    {
        public DisplayGrid displayGrid;
    }
}