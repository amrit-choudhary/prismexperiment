using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prism
{
    public class ApplicationUI : MonoBehaviour
    {
        public void CloseMe()
        {
            Debug.Log("this function use to close the application");
            Application.Quit();
        }

        public void PlayPause()
        {
            Debug.Log("this function use to play pause the game");
        }

        public void Reset()
        {
            Debug.Log("this function use to reset the use-case");
        }

        public void GoBack()
        {

        }
    }
}


