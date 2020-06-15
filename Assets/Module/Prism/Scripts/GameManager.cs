using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

namespace Prism
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager isntance;

        private void Awake()
        {
            isntance = this;
        }

        public List<GameObject> prisms;
        public Transform torchStandTrans;
        public Transform swivelTrans;
        public Transform incidentPosition;
        public float minY, maxY;
        public float moveDuration;
        public LightSource lightSource;
        public CameraController cameraController;
        public GameObject leftPanel;
        public GameObject rightPanel;
        public GameObject mainMenuPanel;
        public static float prismAngleMultiplier = 1.0f;
        public float prismAngleMultiplierFactor = 1.0f;
        public List<TransmissionProperty> prismProperties;
        public float minIndex, maxIndex;
        public static bool ifDrawNormals = false;

        public GameObject videoPanel;
        public VideoPlayer videoPlayer;
        public VideoClip clip1, clip2;

        public TextMeshProUGUI refractiveIndexText;
        public GameObject water;
        public GameObject boardCanvasGO;

        public GameObject tabGameObject;

        public TextMeshProUGUI n1Board;
        private string _currentRefractiveIndex = "1.25";
        private float _currentRefractiveIndexFloat = 1.25f;
        public TextMeshProUGUI n2Board;
        private string _environmentIndex = "1.0";
        private float _environmentIndexFloat = 1.0f;
        public TextMeshProUGUI aBoard;
        private string _prismAngle = "60";
        private int _prismAngleInt = 60;
        public TextMeshProUGUI iBoard;
        [HideInInspector] public string _iBoard = "0";
        public TextMeshProUGUI r1Board;
        [HideInInspector] public string _r1Board = "0";
        public TextMeshProUGUI r2Board;
        [HideInInspector] public string _r2Board = "0";
        public TextMeshProUGUI eBoard;
        [HideInInspector] public string _eBoard = "0";
        public TextMeshProUGUI dBoard;
        [HideInInspector] public string _dBoard = "0";
        public static Vector3 rayI = Vector3.one, rayR1 = Vector3.one, rayR2 = Vector3.one,
            rayN1 = Vector3.one, rayN2 = Vector3.one;

        public Sprite unselectedSprite, selectedSprite;

        public List<Image> topButtonImages;
        public List<Image> bottomButtonImages;

        public InstructionsManager instructionsManager;
        public GameObject instructionButton;
        private bool _ifRefraction = true;

        public TextMeshProUGUI playPauseBtnText;
        public Toggle waterToggle;

        public Vector3 test1, test2, test3, test4;

        public MouseMovement mouseMovement;

        private void Start()
        {
            SelectTopButtons(0);
            SelectBottomButtons(2);
            ChangePrism(1);
        }

        private void Update()
        {
            float i = Mathf.Deg2Rad * (180 - Vector3.Angle(rayN1, rayI));
            float iDeg = i * Mathf.Rad2Deg;
            float factor1 = _environmentIndexFloat / _currentRefractiveIndexFloat;
            float factor2 = _currentRefractiveIndexFloat / _environmentIndexFloat;
            float r1 = Mathf.Asin(factor1 * Mathf.Sin(i)) * Mathf.Rad2Deg;
            float r2 = _prismAngleInt - r1;
            float e = Mathf.Asin(factor2 * Mathf.Sin(r2 * Mathf.Deg2Rad)) * Mathf.Rad2Deg;

            n1Board.text = _environmentIndex;
            n2Board.text = _currentRefractiveIndex;
            aBoard.text = _prismAngleInt.ToString() + "°";

            iBoard.text = iDeg.ToString("00.00") + "°";
            r1Board.text = r1.ToString("00.00") + "°";
            r2Board.text = r2.ToString("00.00") + "°";
            eBoard.text = e.ToString("00.00") + "°";
            dBoard.text = (iDeg + e - _prismAngleInt).ToString("00.00");

            //iBoard.text = (180 - Vector3.Angle(rayN1, rayI)).ToString("00.00") + "°";
            //r1Board.text = (180 - Vector3.Angle(rayN1, rayR1)).ToString("00.0") + "°";
            //r2Board.text = (_prismAngleInt - (180 - Vector3.Angle(rayN1, rayR1))).ToString("00.0") + "°";
            //eBoard.text = (Vector3.Angle(rayN2, rayR2)).ToString("00") + "°";
            //dBoard.text = Vector3.Angle(rayI, rayR2).ToString("00") + "°";
        }

        // Change between available prism options.
        public void ChangePrism(int index_)
        {
            for (int i = 0; i < prisms.Count; i++)
            {
                prisms[i].SetActive(false);
            }

            prisms[index_].SetActive(true);

            if (index_ == 0) _prismAngleInt = 30;
            if (index_ == 1) _prismAngleInt = 60;
            if (index_ == 2) _prismAngleInt = 75;

            prismAngleMultiplier = 1.0f + prismAngleMultiplierFactor * index_;
        }

        // Remove all the prisms.
        public void RemoveAllPrisms()
        {
            for (int i = 0; i < prisms.Count; i++)
            {
                prisms[i].SetActive(false);
            }

            prismAngleMultiplier = 1.0f;
        }

        // Move the light source vertically.
        public void ChangeSwivel(float sliderValue_)
        {
            torchStandTrans.DOLocalMoveY(minY + (maxY - minY) * sliderValue_, moveDuration).OnUpdate(StandMoveUpdate);
        }

        public void ChangeZoom(float sliderValue_)
        {
            float f = (sliderValue_ - 0.5f) * 2.0f;

            if (f < -0.1f || f > 0.1f) mouseMovement.zoomFromUI = f;
            else mouseMovement.zoomFromUI = 0.0f;
        }

        // Change refractive index of the prism.
        public void ChangeRefractiveIndex(float sliderValue_)
        {
            for (int i = 0; i < prismProperties.Count; i++)
            {
                prismProperties[i].index = minIndex + (maxIndex - minIndex) * sliderValue_;
            }

            _currentRefractiveIndex = (1.2f + (1.3f - 1.2f) * sliderValue_).ToString("0.00");
            _currentRefractiveIndexFloat = (1.2f + (1.3f - 1.2f) * sliderValue_);
            refractiveIndexText.text = _currentRefractiveIndex;
        }

        private void StandMoveUpdate()
        {
            swivelTrans.right = (incidentPosition.position - swivelTrans.position).normalized;
        }

        public void ShowNormal(bool ifOn_)
        {
            ifDrawNormals = ifOn_;
        }

        public void ShowWater(bool isOn_)
        {
            water.SetActive(isOn_);
            waterToggle.isOn = isOn_;

            if (isOn_)
            {
                lightSource.environmentIndex = lightSource.waterIndex;
                _environmentIndex = "1.2";
                _environmentIndexFloat = 1.2f;
            }
            else
            {
                lightSource.environmentIndex = lightSource.airIndex;
                _environmentIndex = "1.00";
                _environmentIndexFloat = 1.0f;
            }
        }

        public void ShowBoard(bool isOn_)
        {
            boardCanvasGO.SetActive(isOn_);
        }

        // Change simulation to the refraction experiment.
        public void ChangeToRefraction()
        {
            lightSource.ChangeColor(5);
            videoPanel.SetActive(false);
            tabGameObject.SetActive(true);
            _ifRefraction = true;
            instructionButton.SetActive(true);
            ShowWater(false);
        }

        // Change simulation to the dispersion experiment.
        public void ChangeToDispersion()
        {
            lightSource.ChangeColor(0);
            videoPanel.SetActive(false);
            tabGameObject.SetActive(false);
            _ifRefraction = false;
            instructionButton.SetActive(true);
            ShowWater(false);
        }

        // Go to simulation screen.
        public void GoToSimulation()
        {
            mainMenuPanel.SetActive(false);
            cameraController.SetCamera(2);
        }

        public void GoToMainMenu()
        {
            mainMenuPanel.SetActive(true);
        }

        // Show the refraction video.
        public void ShowVideoBtn1()
        {
            ShowVideo(clip1);
            videoPanel.SetActive(true);
            instructionButton.SetActive(true);
        }

        // Show the dispersion video.
        public void ShowVideoBtn2()
        {
            ShowVideo(clip2);
            videoPanel.SetActive(true);
            instructionButton.SetActive(true);
        }

        public void ShowVideo(VideoClip clip_)
        {
            videoPlayer.clip = clip_;
            videoPlayer.Play();
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
        }

        public void PlayPauseVideo()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                playPauseBtnText.text = "Play";
            }
            else
            {
                videoPlayer.Play();
                playPauseBtnText.text = "Pause";
            }
        }

        public void RestartVideo()
        {
            videoPlayer.Stop();
            videoPlayer.Play();
            playPauseBtnText.text = "Pause";
        }

        public void CloseVideo()
        {
            videoPlayer.Stop();
            videoPanel.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        // Select between top options: 
        // 0: Refraction video, 1: Refraction simulation
        // 2: Dispersion video, 3: Dispersion simulation
        public void SelectTopButtons(int index_)
        {
            for (int i = 0; i < topButtonImages.Count; i++)
            {
                topButtonImages[i].sprite = unselectedSprite;
            }

            topButtonImages[index_].sprite = selectedSprite;
        }

        // Select between bottom buttons, selects prisms.
        public void SelectBottomButtons(int index_)
        {
            for (int i = 0; i < bottomButtonImages.Count; i++)
            {
                bottomButtonImages[i].sprite = unselectedSprite;
            }

            bottomButtonImages[index_].sprite = selectedSprite;
        }

        // Start showing instructions.
        public void ShowInstructions()
        {
            instructionsManager.StartIns(true);
        }
    }
}
