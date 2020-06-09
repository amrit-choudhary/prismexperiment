using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prism
{
    public class MouseCursor : MonoBehaviour
    {

        // public Texture2D normalCursor;
        // public Texture2D scrollCursor;
        // public Texture2D penCursoer;
        // public Texture2D selectableCursor;

        Vector2 hotspot = Vector2.zero;
        CursorMode curMode = CursorMode.Auto;

        public static bool obstacle;

        void Start()
        {
            // used to set cursor in game.
            //Cursor.SetCursor(normalCursor, hotspot, curMode);
        }

        void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIObject())
                {
                    obstacle = true;
                }
                else
                {
                    obstacle = false;
                }
            }
        }

        bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

    }
}