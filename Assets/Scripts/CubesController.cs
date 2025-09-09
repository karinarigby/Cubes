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
        [FormerlySerializedAs("mainCubeController")] public CubeController mainCube;
        public SubCubesController _subCubesController;
        
        [field: SerializeField] private CubeTransforms _cubeTransformData;
        
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
            mainCube.rightClicked.AddListener(OnMainCubeRightClicked);
            mainCube.leftClicked.AddListener(OnMainCubeLeftClicked);
        }

        public void SetLastTransforms()
        {
            _cubeTransformData.Clear();
            _cubeTransformData.AddLastPositionAndRotation(mainCube.Id, mainCube.transform.position, mainCube.transform.rotation);
            
            foreach (var subCube in _subCubesController.SubCubes)
            {
                _cubeTransformData.AddLastPositionAndRotation(subCube.Id, subCube.transform.position, subCube.transform.rotation);
            }
        }

        public void ResetTransforms()
        {
            if (_cubeTransformData.NotSet())
            {
                Debug.Log("No cube transform data was set, no positions to revert back to");
                return;
            }
            var mainCubePosition = _cubeTransformData.LastPositions[mainCube.Id];
            var mainCubeRotation = _cubeTransformData.LastRotations[mainCube.Id];
            mainCube.transform.SetPositionAndRotation(mainCubePosition, mainCubeRotation);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                var  subCubePosition = _cubeTransformData.LastPositions[subCube.Id];
                var  subCubeRotation = _cubeTransformData.LastRotations[subCube.Id];
                subCube.transform.SetPositionAndRotation(subCubePosition, subCubeRotation);
            }
        }

        /// <summary>
        /// Reset cubes to their original colors
        /// </summary>
        public void ResetCubeMaterials()
        {
            mainCube.ResetDisplayMaterial();
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.ResetDisplayMaterial();
            }
        }
        public void SetCubesDisplayMaterial(Material material)
        {
            mainCube.SetLedDisplayMaterial(material);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.SetLedDisplayMaterial(material);
            }
        }
        
        public void FlashAllCubes(Color flashColor)
        {
            mainCube.Flash(flashColor);
            _subCubesController.FlashAllSubCubes(flashColor);
        }

        private void FlashSubCubes()
        {
            _subCubesController.FlashAllSubCubes(mainCube.flashColor);
        }

        private void OnMainCubeLeftClicked(CubeController cubeController)
        {
            if (mouseEnabled)
            {
                mainCube.Flash(mainCube.flashColor);
            }
        }

        private void OnMainCubeRightClicked(CubeController cubeController)
        {
            if (!mouseEnabled)
            {
                return;
            }
            mainCube.Shake();
            FlashSubCubes();
        }

        private void OnSubCubeLeftClicked(CubeController cubeController)
        {
            if (mouseEnabled)
            {
                mainCube.Flash(cubeController.flashColor);
            }
        }

        private void OnDestroy()
        {
            _subCubesController.subCubeLeftClickedEvent.RemoveListener(OnSubCubeLeftClicked);
            mainCube.rightClicked.RemoveListener(OnMainCubeRightClicked);
            mainCube.leftClicked.RemoveListener(OnMainCubeLeftClicked);
        }

        public void SnapAllToXAxis()
        {
            
            
            //get on same axis
            mainCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                // EditorUtility.SetDirty(subCube.Collider);

            }
       
            Physics.SyncTransforms();
            
            //calculate bounds
            var totalBounds = mainCube.Bounds.size;
            foreach (var subCube in _subCubesController.SubCubes)
            {
                totalBounds += subCube.Bounds.size;
            }

            var extents = totalBounds / 2;
            var startPointPosition = Vector3.zero - extents.x*Vector3.right;
            
            mainCube.transform.position = startPointPosition + (mainCube.Bounds.extents.x*Vector3.right);
            
            var lastPointPosition = mainCube.transform.position;
            var lastExtents = (mainCube.Bounds.extents.x*Vector3.right);
            
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.position = lastPointPosition + lastExtents + (subCube.Bounds.extents.x*Vector3.right);
                lastPointPosition = subCube.transform.position;
                lastExtents = subCube.Bounds.extents.x*Vector3.right;
            }
        }
    }
}
