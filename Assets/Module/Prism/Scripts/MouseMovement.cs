using UnityEngine;

namespace Prism
{
    public class MouseMovement : MonoBehaviour
    {
        public Transform target;

        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;
        public float distance;
        public float distanceMin = .5f;
        public float distanceMax = 15f;
        public float yMinLimit = 0f;
        public float yMaxLimit = 90f;
        public float zoomFromUI = 0.0f;

        Vector3 negDistance;

        float x = 0.0f;
        float y = 0.0f;

        Vector3 clickedMousePosition, leavedMousePosition;

        public float minZoomV = 20f;
        public float maxZoomV = 70f;

        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;
            distance = Mathf.Abs(Camera.main.transform.position.z);
            negDistance = new Vector3(0f, 0f, -distance);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }

        void LateUpdate()
        {

            if (Input.GetMouseButtonDown(0))
            {
                clickedMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !MouseCursor.obstacle)
            {

                leavedMousePosition = Input.mousePosition;

                // if distance is more than 0.1 then mouse is dragged.
                if (Vector3.Distance(clickedMousePosition, leavedMousePosition) > 0.1f)
                {
                    // CameraSwitchBellJar.instance.objectLables.SetActive(false);
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);

                    Quaternion rotation = Quaternion.Euler(y, x, 0);

                    Vector3 position = target.position + rotation * new Vector3(negDistance.x, 0.0f, negDistance.z);

                    transform.rotation = rotation;
                    transform.position = position;

                    float angleLimit = Mathf.Lerp(-40f, 40, Mathf.InverseLerp(0f, 40f, (transform.localEulerAngles.y)));
                    //Debug.Log(angleNeedle);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0) // back
            {
                float value = Camera.main.fieldOfView;
                value++;
                float v = Mathf.Clamp(value, minZoomV, maxZoomV);
                //Debug.Log("value : " + v);
                Camera.main.fieldOfView = v;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0) // forward
            {
                float value = Camera.main.fieldOfView;
                value--;
                float v = Mathf.Clamp(value, minZoomV, maxZoomV);
                Camera.main.fieldOfView = v;
            }

            if (zoomFromUI < 0) // back
            {
                float value = Camera.main.fieldOfView;
                value++;
                float v = Mathf.Clamp(value, minZoomV, maxZoomV);
                //Debug.Log("value : " + v);
                Camera.main.fieldOfView = v;
            }

            if (zoomFromUI > 0) // forward
            {
                float value = Camera.main.fieldOfView;
                value--;
                float v = Mathf.Clamp(value, minZoomV, maxZoomV);
                Camera.main.fieldOfView = v;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

    }
}