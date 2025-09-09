using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tokidos
{
    [RequireComponent(typeof(SubCubesController))]
    public class CubesController : MonoBehaviour
    {
        public bool mouseEnabled;
        public CubeController mainCubeController;
        public SubCubesController _subCubesController;

        /// <summary>
        /// Holds the positions of all the cubes
        /// </summary>
        public Dictionary<int, Vector3> CubesLastPositions { get; private set; } = new();
        public Dictionary<int, Quaternion> CubesLastRotations { get; private set; } = new();
        
        /// <summary>
        /// Holds last saved colors of all cubes
        /// </summary>
        public Dictionary<int, Material> CubesMaterials { get; private set; } = new();
        private void Awake()
        {
            _subCubesController = GetComponent<SubCubesController>();
        }

        private void Start()
        {
            _subCubesController.subCubeLeftClickedEvent.AddListener(OnSubCubeLeftClicked);
            mainCubeController.rightClicked.AddListener(OnMainCubeRightClicked);
            mainCubeController.leftClicked.AddListener(OnMainCubeLeftClicked);
        }

        public void SetLastTransforms()
        {
            CubesLastPositions.Clear();
            CubesLastRotations.Clear();
            
            CubesLastPositions.Add(mainCubeController.Id, mainCubeController.transform.position);
            CubesLastRotations.Add(mainCubeController.Id, mainCubeController.transform.rotation);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                CubesLastPositions.Add(subCube.Id, subCube.transform.position);
                CubesLastRotations.Add(subCube.Id, subCube.transform.rotation);
            }
        }

        public void ResetTransforms()
        {
            if (CubesLastPositions.Count == 0 || CubesLastRotations.Count == 0)
            {
                Debug.Log("Nothing to reset to");
                return;
            }
            
            var mainCubePosition = CubesLastPositions[mainCubeController.Id];
            var mainCubeRotation = CubesLastRotations[mainCubeController.Id];
            mainCubeController.transform.SetPositionAndRotation(mainCubePosition, mainCubeRotation);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                var  subCubePosition = CubesLastPositions[subCube.Id];
                var  subCubeRotation = CubesLastRotations[subCube.Id];
                subCube.transform.SetPositionAndRotation(subCubePosition, subCubeRotation);
            }
        }

        /// <summary>
        /// Reset cubes to their original colors
        /// </summary>
        public void ResetCubeMaterials()
        {
            mainCubeController.ResetDisplayMaterial();
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.ResetDisplayMaterial();
            }
        }
        public void SetCubesDisplayMaterial(Material material)
        {
            mainCubeController.SetLedDisplayMaterial(material);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.SetLedDisplayMaterial(material);
            }
        }
        
        public void FlashAllCubes(Color flashColor)
        {
            mainCubeController.Flash(flashColor);
            _subCubesController.FlashAllSubCubes(flashColor);
        }

        private void FlashSubCubes()
        {
            _subCubesController.FlashAllSubCubes(mainCubeController.flashColor);
        }

        private void OnMainCubeLeftClicked(CubeController cubeController)
        {
            if (mouseEnabled)
            {
                mainCubeController.Flash(mainCubeController.flashColor);
            }
        }

        private void OnMainCubeRightClicked(CubeController cubeController)
        {
            if (!mouseEnabled)
            {
                return;
            }
            mainCubeController.Shake();
            FlashSubCubes();
        }

        private void OnSubCubeLeftClicked(CubeController cubeController)
        {
            if (mouseEnabled)
            {
                mainCubeController.Flash(cubeController.flashColor);
            }
        }

        private void OnDestroy()
        {
            _subCubesController.subCubeLeftClickedEvent.RemoveListener(OnSubCubeLeftClicked);
            mainCubeController.rightClicked.RemoveListener(OnMainCubeRightClicked);
            mainCubeController.leftClicked.RemoveListener(OnMainCubeLeftClicked);
        }

        public void SnapAllToXAxis()
        {
            
            
            //get on same axis
            mainCubeController.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                // EditorUtility.SetDirty(subCube.Collider);

            }
       
            Physics.SyncTransforms();
            
            //calculate bounds
            var totalBounds = mainCubeController.Bounds.size;
            foreach (var subCube in _subCubesController.SubCubes)
            {
                totalBounds += subCube.Bounds.size;
            }

            var extents = totalBounds / 2;
            var startPointPosition = Vector3.zero - extents.x*Vector3.right;
            
            mainCubeController.transform.position = startPointPosition + (mainCubeController.Bounds.extents.x*Vector3.right);
            
            var lastPointPosition = mainCubeController.transform.position;
            var lastExtents = (mainCubeController.Bounds.extents.x*Vector3.right);
            
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.position = lastPointPosition + lastExtents + (subCube.Bounds.extents.x*Vector3.right);
                lastPointPosition = subCube.transform.position;
                lastExtents = subCube.Bounds.extents.x*Vector3.right;
            }
        }
    }
}
