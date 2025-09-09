using System.Collections.Generic;
using UnityEngine;

namespace Tokidos
{
    /// <summary>
    /// a spot to hold transforms of the cubes so they're not affected by runtime
    /// </summary>
    [CreateAssetMenu(fileName = "CubeTransforms", menuName = "Scriptable Objects/CubeTransforms")]
    public class CubeTransforms : ScriptableObject
    {
        /// <summary>
        /// Holds the positions of all the cubes
        /// </summary>
        public Dictionary<int, Vector3> LastPositions { get; private set; } = new();
        public Dictionary<int, Quaternion> LastRotations { get; private set; } = new();

        public void Clear()
        {
            LastPositions.Clear();
            LastRotations.Clear();
        }

        public bool NotSet()
        {
            return (LastPositions.Count == 0 || LastRotations.Count == 0);
        }

        public void AddLastPositionAndRotation(int id, Vector3 transformPosition, Quaternion transformRotation)
        {
           LastPositions[id] = transformPosition;
           LastRotations[id] = transformRotation;
        }
    }
}
