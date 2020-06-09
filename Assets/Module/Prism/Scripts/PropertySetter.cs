using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prism
{
    // Class to set required parameters and reset it when experiment ends.
    public class PropertySetter : MonoBehaviour
    {
        private void OnEnable()
        {
            Physics.queriesHitBackfaces = true;
        }

        private void OnDisable()
        {
            Physics.queriesHitBackfaces = false;
        }
    }
}
