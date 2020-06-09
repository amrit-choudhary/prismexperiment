using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Prism
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        private void Awake()
        {
            instance = this;
        }

        public Transform cameraParentTrans;
        public Transform cameraTrans;
        public Transform prismTrans;
        public Transform[] cameraTargets;
        public float moveDuration;
        public float zoomFactor;
        public float minZoomDistance;
        public float maxZoomDistance;
        public Vector3 mouseDrag;
        private Vector3 _preMouseDrag;
        private bool _isDragging = false;
        private Quaternion _initQuarter;
        private Quaternion _targetQuarter;
        public float dragFactor;
        public float slerpFactor;

        void Start()
        {
            DOTween.Init();
        }

        // Set camera to predefined positions in scene.
        public void SetCamera(int i)
        {
            cameraTrans.DOMove(cameraTargets[i].position, moveDuration);
            cameraTrans.DORotateQuaternion(cameraTargets[i].rotation, moveDuration);
        }
    }
}
