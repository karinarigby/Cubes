using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tokidos
{
    [Serializable]
    [RequireComponent(typeof(SubCubesController))]
    public class CubesController : MonoBehaviour
    {
        [field: SerializeField] bool MouseEnabled {get; set;}
        public CubeController mainCube;
        public SubCubesController subCubesController;
        
        public CubesStateData persistentCubesDataSO;

        private void OnEnable()
        {
            MouseEnabled = persistentCubesDataSO.MouseEnabled;
        }
        
        private void Awake()
        {
            subCubesController = GetComponent<SubCubesController>();
        }

        private void Start()
        {
            subCubesController.subCubeLeftClickedEvent.AddListener(OnSubCubeLeftClicked);
            mainCube.rightClicked.AddListener(OnMainCubeRightClicked);
            mainCube.leftClicked.AddListener(OnMainCubeLeftClicked);
        }

        public void EnableMouse(bool mouseEnabled)
        {
            MouseEnabled = mouseEnabled;
            
            //writes it to Scriptable object so that it can be persisted
            persistentCubesDataSO.MouseEnabled = mouseEnabled;
        }

        /// <summary>
        /// Write the positions and rotations of the cubes to disk for later retrieval
        /// </summary>
        public void RecordTransforms()
        {
            Debug.Log("Recording the Cube transforms");
            if (persistentCubesDataSO.LastPositions.Count > 0)
            {
                Debug.Log("Already in snapped position");
                return;
            }
            persistentCubesDataSO.RecordLastPositionAndRotation(mainCube.transform.position, mainCube.transform.rotation);
            
            foreach (var subCube in subCubesController.SubCubes)
            {
                persistentCubesDataSO.RecordLastPositionAndRotation(subCube.transform.position, subCube.transform.rotation);
            }
        }

        /// <summary>
        /// Revert the positions and rotations of the cubes to original state 
        /// </summary>
        public void ResetTransforms()
        {
            if (persistentCubesDataSO.TransformDataEmpty())
            {
                Debug.Log("No cube transform data was set, no positions to revert back to");
                return;
            }
            var mainCubePosition = persistentCubesDataSO.LastPositions[mainCube.Index];
            var mainCubeRotation = persistentCubesDataSO.LastRotations[mainCube.Index];
            mainCube.transform.SetPositionAndRotation(mainCubePosition, mainCubeRotation);

            foreach (var subCube in subCubesController.SubCubes)
            {
                var  subCubePosition = persistentCubesDataSO.LastPositions[subCube.Index];
                var  subCubeRotation = persistentCubesDataSO.LastRotations[subCube.Index];
                subCube.transform.SetPositionAndRotation(subCubePosition, subCubeRotation);
            }
            persistentCubesDataSO.Clear();
        }

        /// <summary>
        /// Reset cube faces to their original material
        /// </summary>
        public void ResetCubeMaterials()
        {
            mainCube.ResetDisplayMaterial();
            foreach (var subCube in subCubesController.SubCubes)
            {
                subCube.ResetDisplayMaterial();
            }
        }

        /// <summary>
        /// Set all cube face materials to targeted material
        /// </summary>
        /// <param name="material">the material to apply to cube face</param>
        public void SetCubesDisplayMaterial(Material material)
        {
            mainCube.SetLedDisplayMaterial(material);
            foreach (var subCube in subCubesController.SubCubes)
            {
                subCube.SetLedDisplayMaterial(material);
            }
        }

        /// <summary>
        /// Aligns all cubes side by side on X Axis
        /// </summary>
        public void SnapAllToXAxis()
        {
            //get on same axis
            PlaceCubesOnXAxis();

            //Need to update the bounding boxes from resetting the rotation to zero
            Physics.SyncTransforms();
            
            //This could have probably been more easily achieved with transform.Translate but the math still works so
            //am leaving it for now.
            
            //calculate total bounds of all cubes
            // calculate starting point to place first cube(leftmost point along x axis)
            // starting point = total group bounds extents
            // next object center point placement = last cube center point placement + last bound extents + current bound extents
            
            // |----bounds---|
            // [ ][ ][ ][ ][ ]
            var groupBounds = mainCube.Bounds.size;
            foreach (var subCube in subCubesController.SubCubes)
            {
                groupBounds += subCube.Bounds.size;
            }
            
            var extentsOfGroup = groupBounds / 2;
            var leftmostStartingCoordinate = Vector3.zero - extentsOfGroup.x*Vector3.right; 
            
            mainCube.transform.position = leftmostStartingCoordinate + (mainCube.GetBoundsExtentsOnXAxis());
            
            var lastPointPosition = mainCube.transform.position;
            var lastExtents = mainCube.GetBoundsExtentsOnXAxis();
            
            foreach (var subCube in subCubesController.SubCubes)
            {
                subCube.transform.position = lastPointPosition + lastExtents + (subCube.GetBoundsExtentsOnXAxis());
                lastPointPosition = subCube.transform.position;
                lastExtents = subCube.GetBoundsExtentsOnXAxis();
            }
        }

        private void FlashSubCubes()
        {
            subCubesController.FlashAllSubCubes(mainCube.flashColor);
        }

        private void OnMainCubeLeftClicked(CubeController cubeController)
        {
            if (MouseEnabled)
            {
                mainCube.Flash(mainCube.flashColor);
            }
        }

        private void OnMainCubeRightClicked(CubeController cubeController)
        {
            if (!MouseEnabled)
            {
                return;
            }
            mainCube.Shake();
            FlashSubCubes();
        }

        private void OnSubCubeLeftClicked(CubeController cubeController)
        {
            if (MouseEnabled)
            {
                mainCube.Flash(cubeController.flashColor);
            }
        }

        private void PlaceCubesOnXAxis()
        {
            mainCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            foreach (var subCube in subCubesController.SubCubes)
            {
                subCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }
        }

        private void OnDestroy()
        {
            subCubesController.subCubeLeftClickedEvent.RemoveListener(OnSubCubeLeftClicked);
            mainCube.rightClicked.RemoveListener(OnMainCubeRightClicked);
            mainCube.leftClicked.RemoveListener(OnMainCubeLeftClicked);
        }
    }
}
