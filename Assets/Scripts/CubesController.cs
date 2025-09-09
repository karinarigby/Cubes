using System;
using System.Collections.Generic;
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
        public Dictionary<int, Transform> CubesTransformsSnapshot { get; private set; } = new();
        
        /// <summary>
        /// Holds last saved colors of all cubes
        /// </summary>
        public Dictionary<int, Color> CubesColorsSnapshot { get; private set; } = new();
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

        public void DisableMouseClicks()
        {
            
        }

        public void SetColorsSnapshot()
        {
            CubesColorsSnapshot.Clear();
            CubesColorsSnapshot.Add(mainCubeController.Id, mainCubeController.flashColor);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                CubesColorsSnapshot.Add(subCube.Id, subCube.flashColor);
            }
        }

        public void SetLastTransforms()
        {
            CubesTransformsSnapshot.Clear();
            CubesTransformsSnapshot.Add(mainCubeController.Id, mainCubeController.transform);

            foreach (var subCube in _subCubesController.SubCubes)
            {
                CubesTransformsSnapshot.Add(subCube.Id, subCube.transform);
            }
        }

        public void ApplyColorSnapshot()
        {
            mainCubeController.flashColor = CubesColorsSnapshot[mainCubeController.Id];
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.flashColor = CubesColorsSnapshot[subCube.Id];
            }
        }

        public void ApplyTransformSnapshot()
        {
            var mainTransform = CubesTransformsSnapshot[mainCubeController.Id];
            mainCubeController.transform.SetPositionAndRotation(mainTransform.position, mainTransform.rotation);
            mainCubeController.transform.localScale = mainTransform.localScale;

            foreach (var subCube in _subCubesController.SubCubes)
            {
                var  subCubeTransform = CubesTransformsSnapshot[subCube.Id];
                subCubeTransform.SetPositionAndRotation(subCubeTransform.position, subCubeTransform.rotation);
                subCubeTransform.localScale = subCubeTransform.localScale;
            }
        }

        public void SetCubesBaseColor(Color color)
        {
            mainCubeController.SetBaseColor(color);
            foreach (var subCube in _subCubesController.SubCubes)
            {
                subCube.SetBaseColor(color);
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
    }
}
