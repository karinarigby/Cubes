using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tokidos
{
    [Serializable]
    [RequireComponent(typeof(SubCubesController))]
    public class CubesController : MonoBehaviour
    {
        [field: SerializeField] bool mouseEnabled {get; set;}
        public CubeController mainCube;
        public SubCubesController _subCubesController;
        
        public SavedCubesTransforms persistentCubesDataSO;

        private void OnEnable()
        {
            mouseEnabled = persistentCubesDataSO.MouseEnabled;
        }
        
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

        public void EnableMouse(bool mouseEnabled)
        {
            this.mouseEnabled = mouseEnabled;
            
            //writes it to SO so that it can be referred to at runtime
            persistentCubesDataSO.MouseEnabled = mouseEnabled;
        }

        public void SetLastTransforms()
        {
            Debug.Log("SetLastTransforms");
            if (persistentCubesDataSO.LastPositions.Count > 0)
            {
                Debug.Log("Already in snapped position");
                return;
            }
            persistentCubesDataSO.AddLastPositionAndRotation(mainCube.Index, mainCube.transform.position, mainCube.transform.rotation);
            
            foreach (var subCube in _subCubesController.SubCubes)
            {
                persistentCubesDataSO.AddLastPositionAndRotation(subCube.Index, subCube.transform.position, subCube.transform.rotation);
            }
        }

        public void ResetTransforms()
        {
            if (persistentCubesDataSO.NotSet())
            {
                Debug.Log("No cube transform data was set, no positions to revert back to");
                return;
            }
            var mainCubePosition = persistentCubesDataSO.LastPositions[mainCube.Index];
            var mainCubeRotation = persistentCubesDataSO.LastRotations[mainCube.Index];
            
            mainCube.transform.SetPositionAndRotation(mainCubePosition, mainCubeRotation);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                var  subCubePosition = persistentCubesDataSO.LastPositions[subCube.Index];
                var  subCubeRotation = persistentCubesDataSO.LastRotations[subCube.Index];
                subCube.transform.SetPositionAndRotation(subCubePosition, subCubeRotation);
            }

            persistentCubesDataSO.Clear();
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
            PlaceCubesOnXAxis();

            //Need to update the bounding boxes from resetting the rotation to zero
            Physics.SyncTransforms();
            
            //calculate total bounds of all cubes
            // |----bounds---|
            // [ ][ ][ ][ ][ ]
            var groupBounds = mainCube.Bounds.size;
            foreach (var subCube in _subCubesController.SubCubes)
            {
                groupBounds += subCube.Bounds.size;
            }

            //* start point position 
            //|------X|  (extent)
            //[ ][ ][ ][ ][ ]
            var extentsOfGroup = groupBounds / 2;
            var startPointPosition = Vector3.zero - extentsOfGroup.x*Vector3.right; 
            
            mainCube.transform.position = startPointPosition + (mainCube.GetBoundsExtentsOnXAxis());
            
            var lastPointPosition = mainCube.transform.position;
            var lastExtents = mainCube.GetBoundsExtentsOnXAxis();
            
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.position = lastPointPosition + lastExtents + (subCube.GetBoundsExtentsOnXAxis());
                lastPointPosition = subCube.transform.position;
                lastExtents = subCube.GetBoundsExtentsOnXAxis();
            }
        }

        private void PlaceCubesOnXAxis()
        {
            mainCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                // EditorUtility.SetDirty(subCube.Collider);

            }
        }
    }
}
