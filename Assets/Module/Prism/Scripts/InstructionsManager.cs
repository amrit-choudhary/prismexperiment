using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prism
{
    public class InstructionsManager : MonoBehaviour
    {
        public GameObject bg;
        public GameObject stopPanel;
        public List<GameObject> firstIns, secondIns;
        private bool _ifFirst = true;
        private int _index = 0;

        // Start instructions.
        public void StartIns(bool ifFirst_)
        {
            GameManager.isntance.ChangeToRefraction();
            CameraController.instance.SetCamera(2);
            bg.SetActive(true);
            stopPanel.SetActive(true);
            _ifFirst = ifFirst_;
            _index = 0;

            if (_ifFirst)
            {
                firstIns[0].SetActive(true);
            }
            else
            {
                secondIns[0].SetActive(true);
            }
        }

        // Show the next instruction screen in the instruction sequence.
        public void Next()
        {
            _index++;

            if (_index == 12)
                GameManager.isntance.ChangeToDispersion();

            if (_ifFirst)
            {
                if (_index >= firstIns.Count)
                {
                    StopIns();
                }
                else
                {
                    firstIns[_index - 1].SetActive(false);
                    firstIns[_index].SetActive(true);
                }
            }
            else
            {
                if (_index >= secondIns.Count)
                {
                    StopIns();
                }
                else
                {
                    secondIns[_index - 1].SetActive(false);
                    secondIns[_index].SetActive(true);
                }
            }
        }

        // Stop all the instructions and return to simulation.
        public void StopIns()
        {
            bg.SetActive(false);
            stopPanel.SetActive(false);

            for (int i = 0; i < firstIns.Count; i++)
            {
                firstIns[i].SetActive(false);
            }

            for (int i = 0; i < secondIns.Count; i++)
            {
                secondIns[i].SetActive(false);
            }
        }
    }
}
