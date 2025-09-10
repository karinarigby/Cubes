using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    /// <summary>
    /// a spot to hold state of the cubes to help persist changes
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "CubeTransforms", menuName = "Scriptable Objects/CubeTransforms")]
    public class CubesStateData : ScriptableObject
    {
        [field: SerializeField] public bool MouseEnabled { get; set; }
        /// <summary>
        /// Holds the positions of all the cubes for future reference
        /// </summary>
        [field:SerializeField] public List<Vector3> LastPositions { get; private set; } = new(5);
        [field: SerializeField] public List<Quaternion> LastRotations { get; private set; } = new(5);

        /// <summary>
        /// Clears the saved positions and rotations
        /// </summary>
        public void Clear()
        {
            Debug.Log("cleared");
            LastPositions.Clear();
            LastRotations.Clear();
        }

        /// <summary>
        /// Checks if Positions and Rotations are populated
        /// </summary>
        /// <returns>true if empty</returns>
        public bool TransformDataEmpty()
        {
            return (LastPositions.Count == 0 || LastRotations.Count == 0);
        }

        /// <summary>
        /// Saves the transform position and rotation to scriptable object
        /// </summary>
        /// <param name="transformPosition">the position to add to saved Positions</param>
        /// <param name="transformRotation">the rotation to add to saved Rotations</param>
        public void RecordLastPositionAndRotation(Vector3 transformPosition, Quaternion transformRotation)
        {
            LastPositions.Add(transformPosition);
            LastRotations.Add(transformRotation);
        }
    }
}

