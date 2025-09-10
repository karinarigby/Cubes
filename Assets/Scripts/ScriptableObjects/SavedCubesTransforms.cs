using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tokidos
{
    /// <summary>
    /// a spot to hold transforms of the cubes so they're not affected by runtime
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "CubeTransforms", menuName = "Scriptable Objects/CubeTransforms")]
    public class SavedCubesTransforms : ScriptableObject
    {
        [field: SerializeField] public bool MouseEnabled { get; set; }
        /// <summary>
        /// Holds the positions of all the cubes
        /// </summary>
        [field:SerializeField] public List<Vector3> LastPositions { get; private set; } = new(5);
        [field: SerializeField] public List<Quaternion> LastRotations { get; private set; } = new(5);

        public void Clear()
        {
            Debug.Log("cleared");
            LastPositions.Clear();
            LastRotations.Clear();
        }

        public bool NotSet()
        {
            return (LastPositions.Count == 0 || LastRotations.Count == 0);
        }

        public void AddLastPositionAndRotation(int index, Vector3 transformPosition, Quaternion transformRotation)
        {
           LastPositions.Add(transformPosition);
           LastRotations.Add(transformRotation);
        }
    }
}
