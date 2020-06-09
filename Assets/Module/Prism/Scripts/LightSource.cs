using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prism
{
    [System.Serializable]
    public class LightColor
    {
        public string name;
        public Color color;
        public float multiplier;

        public LightColor()
        {
            name = "Red";
            color = Color.red;
            multiplier = 1.3f;
        }
    }

    // Class for light simulation and drawing light rays.
    public class LightSource : MonoBehaviour
    {
        public Transform origin;
        public LineRenderer lineRenderer;
        public int maxBounceCount = 5;
        private int _bounceCount = 0;
        public int maxDistance = 10;
        public float width = 0.1f;
        private List<LightSegment> _lightSegments = new List<LightSegment>();
        private Vector3[] _drawPoints;
        public float environmentIndex = 1.0f;
        public float airIndex = 1.0f;
        public float waterIndex = 1.0f;
        private RaycastHit _hit;
        private RaycastHit _topHit;
        private Vector3 _normal;
        private TransmissionProperty _property;
        private Vector3 _direction;
        private float _index;
        public float _sphereCastDistance = 0.2f;
        private float _adjustedCastDistance = 1.0f;
        public float _sphereCastRadius = 0.01f;
        private Collider[] _overlapSpheres;

        public List<LightColor> lightColors;
        private LightColor currentColor;
        public List<LineRenderer> lineRenderers;
        public List<Material> lineMaterials;
        public LineRenderer normalLR, normalLR2, incidentLR;
        public float normalWidth;
        public Color normalColor, incidentColor;
        private Vector3[] _normaPoints = new Vector3[2];
        private Vector3[] _normaPoints2 = new Vector3[2];
        private Vector3[] _incidentPoints = new Vector3[2];
        public float normalDrawLength, incidentDrawLength;
        public float refractiveEmissionIntensity, dispersionEmissionIntensity;

        private bool _ifDrawingWhite = true;
        private int _savedIndex = 0;

        void Start()
        {
            SetLineRenderers();

            currentColor = lightColors[5];
            _savedIndex = 5;
            _ifDrawingWhite = false;
        }

        void Update()
        {
            LightUpdate();
        }

        public void LightUpdate()
        {
            if (_ifDrawingWhite)
            {
                for (int i = 1; i < 8; i++)
                {
                    currentColor = lightColors[i];
                    CastLight();
                    DrawLightWhite(i);
                }
            }
            else
            {
                CastLight();
                DrawLight(_savedIndex);
            }
        }

        // Runs the light travel simulation.
        public void CastLight()
        {
            _lightSegments.Clear();
            _bounceCount = 0;

            LightSegment first = new LightSegment(origin.position, origin.forward, environmentIndex, currentColor);
            _lightSegments.Add(first);

            CastSegment(first);
        }

        // Draw light rays using line renderer based on light cast result.
        public void DrawLight(int index_)
        {
            for (int i = 0; i < lineRenderers.Count; i++)
            {
                lineRenderers[i].positionCount = 0;
            }

            _drawPoints = new Vector3[_lightSegments.Count + 1];
            _drawPoints[0] = _lightSegments[0].start;

            for (int i = 0; i < _lightSegments.Count; i++)
            {
                _drawPoints[i + 1] = _lightSegments[i].end;
            }

            lineRenderers[index_].positionCount = _lightSegments.Count + 1;
            lineRenderers[index_].SetPositions(_drawPoints);

            if (GameManager.ifDrawNormals && _lightSegments.Count > 1)
            {
                _normaPoints[0] = _lightSegments[0].end - _lightSegments[0].normal * normalDrawLength;
                _normaPoints[1] = _lightSegments[0].end + _lightSegments[0].normal * normalDrawLength;

                _normaPoints2[0] = _lightSegments[1].end - _lightSegments[1].normal * normalDrawLength;
                _normaPoints2[1] = _lightSegments[1].end + _lightSegments[1].normal * normalDrawLength;

                _incidentPoints[0] = _lightSegments[0].end;
                _incidentPoints[1] = _lightSegments[0].end + _lightSegments[0].direction * incidentDrawLength;

                normalLR.positionCount = 2;
                normalLR.SetPositions(_normaPoints);

                normalLR2.positionCount = 2;
                normalLR2.SetPositions(_normaPoints2);

                incidentLR.positionCount = 2;
                incidentLR.SetPositions(_incidentPoints);
            }
            else
            {
                normalLR.positionCount = 0;
                normalLR2.positionCount = 0;
                incidentLR.positionCount = 0;
            }

            if (_lightSegments.Count > 2)
            {
                GameManager.rayI = _lightSegments[0].direction.normalized;
                GameManager.rayR1 = _lightSegments[1].direction.normalized;
                GameManager.rayR2 = _lightSegments[2].direction.normalized;
                GameManager.rayN1 = _lightSegments[0].normal.normalized;
                GameManager.rayN2 = _lightSegments[1].normal.normalized;
            }
            else
            {
                GameManager.rayI = Vector3.one;
                GameManager.rayR1 = Vector3.one;
                GameManager.rayR2 = Vector3.one;
                GameManager.rayN1 = Vector3.one;
                GameManager.rayN2 = Vector3.one;
            }
        }

        // Draw white light as a combination of 7 light rays. Similar to DrawLight().
        public void DrawLightWhite(int index_)
        {
            _drawPoints = new Vector3[_lightSegments.Count + 1];
            _drawPoints[0] = _lightSegments[0].start;

            for (int i = 0; i < _lightSegments.Count; i++)
            {
                _drawPoints[i + 1] = _lightSegments[i].end;
            }

            lineRenderers[index_].positionCount = _lightSegments.Count + 1;
            lineRenderers[index_].SetPositions(_drawPoints);

            if (GameManager.ifDrawNormals && _lightSegments.Count > 1)
            {
                _normaPoints[0] = _lightSegments[0].end - _lightSegments[0].normal * normalDrawLength;
                _normaPoints[1] = _lightSegments[0].end + _lightSegments[0].normal * normalDrawLength;

                _normaPoints2[0] = _lightSegments[1].end - _lightSegments[1].normal * normalDrawLength;
                _normaPoints2[1] = _lightSegments[1].end + _lightSegments[1].normal * normalDrawLength;

                _incidentPoints[0] = _lightSegments[0].end;
                _incidentPoints[1] = _lightSegments[0].end + _lightSegments[0].direction * incidentDrawLength;

                normalLR.positionCount = 2;
                normalLR.SetPositions(_normaPoints);

                normalLR2.positionCount = 2;
                normalLR2.SetPositions(_normaPoints2);

                incidentLR.positionCount = 2;
                incidentLR.SetPositions(_incidentPoints);
            }
            else
            {
                normalLR.positionCount = 0;
                normalLR2.positionCount = 0;
                incidentLR.positionCount = 0;
            }

            if (_lightSegments.Count > 2)
            {
                GameManager.rayI = _lightSegments[0].direction.normalized;
                GameManager.rayR1 = _lightSegments[1].direction.normalized;
                GameManager.rayR2 = _lightSegments[2].direction.normalized;
                GameManager.rayN1 = _lightSegments[0].normal.normalized;
                GameManager.rayN2 = _lightSegments[1].normal.normalized;
            }
            else
            {
                GameManager.rayI = Vector3.one;
                GameManager.rayR1 = Vector3.one;
                GameManager.rayR2 = Vector3.one;
                GameManager.rayN1 = Vector3.one;
                GameManager.rayN2 = Vector3.one;
            }
        }

        // Simulates light's interaction with materials. When light strikes an opaque or transparent medium,
        // will calculate the refracted ray and cast a new light segment.
        private void CastSegment(LightSegment segment_)
        {
            if (segment_ == null) return;

            _bounceCount++;
            if (_bounceCount > maxBounceCount) return;

            _adjustedCastDistance = _sphereCastDistance * GameManager.prismAngleMultiplier;

            if (Physics.Raycast(segment_.start + segment_.direction * _adjustedCastDistance, segment_.direction, out _hit, maxDistance))
            {
                segment_.end = _hit.point;
                segment_.normal = _hit.normal;
                _normal = _hit.normal;
                _property = _hit.collider.GetComponent<TransmissionProperty>();

                if (_property.transmission == 0)
                {
                    if (_property.reflectance == 1)
                    {
                        _direction = Vector3.Reflect(segment_.direction, -_normal);
                        _index = environmentIndex;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    bool ifGoingInMedium = false;

                    if (Physics.Raycast(segment_.end + segment_.direction * _adjustedCastDistance + new Vector3(0, 100, 0), Vector3.down, out _topHit, 200))
                    {
                        _index = _topHit.collider.GetComponent<TransmissionProperty>().index;
                        ifGoingInMedium = true;
                    }
                    else
                    {
                        _index = environmentIndex;
                        _normal = -_normal;
                        ifGoingInMedium = false;
                    }

                    float adjustedIndex = segment_.index * segment_.color.multiplier;

                    if (!ifGoingInMedium)
                        adjustedIndex = segment_.index * 1 / segment_.color.multiplier;

                    float ratio = adjustedIndex / _index;

                    _direction = ratio * Vector3.Cross(_normal, Vector3.Cross(-_normal, segment_.direction)) - _normal *
                        Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(_normal, segment_.direction) * (ratio * ratio),
                        Vector3.Cross(_normal, segment_.direction)));

                    _direction.Normalize();
                }

                LightSegment newSegment = new LightSegment(segment_.end, _direction, _index, currentColor);
                _lightSegments.Add(newSegment);
                CastSegment(newSegment);
            }
            else
            {
                segment_.end = segment_.start + segment_.direction * maxDistance;
                return;
            }
        }

        // Line renderer's initialization.
        private void SetLineRenderers()
        {
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            for (int i = 0; i < 8; i++)
            {
                lineRenderers[i].widthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0.0f, width),
                new Keyframe(1.0f, width)}
                );

                gradient = new Gradient();

                colorKey = new GradientColorKey[2];
                colorKey[0].color = lightColors[i].color;
                colorKey[0].time = 0.0f;
                colorKey[1].color = lightColors[i].color;
                colorKey[1].time = 1.0f;

                alphaKey = new GradientAlphaKey[2];
                alphaKey[0].alpha = 1.0f;
                alphaKey[0].time = 0.0f;
                alphaKey[1].alpha = 1.0f;
                alphaKey[1].time = 1.0f;
                gradient.SetKeys(colorKey, alphaKey);

                lineRenderers[i].colorGradient = gradient;
            }

            normalLR.widthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0.0f, normalWidth),
                new Keyframe(1.0f, normalWidth)}
            );

            normalLR2.widthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0.0f, normalWidth),
                new Keyframe(1.0f, normalWidth)}
            );

            incidentLR.widthCurve = new AnimationCurve(new Keyframe[]{
                new Keyframe(0.0f, normalWidth),
                new Keyframe(1.0f, normalWidth)}
            );

            gradient = new Gradient();

            colorKey = new GradientColorKey[2];
            colorKey[0].color = normalColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = normalColor;
            colorKey[1].time = 1.0f;

            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            normalLR.colorGradient = gradient;
            normalLR2.colorGradient = gradient;

            gradient = new Gradient();

            colorKey = new GradientColorKey[2];
            colorKey[0].color = incidentColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = incidentColor;
            colorKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);

            incidentLR.colorGradient = gradient;
        }

        // Pick different light color in refraction experiment.
        public void ChangeColor(int index_)
        {
            _savedIndex = index_;
            currentColor = lightColors[index_];

            if (index_ == 0)
            {
                _ifDrawingWhite = true;
                SetEmissions(dispersionEmissionIntensity);
            }
            else
            {
                _ifDrawingWhite = false;
                SetEmissions(refractiveEmissionIntensity);
            }
        }

        // Set line renderer's emission based on color.
        public void SetEmissions(float value_)
        {
            for (int i = 0; i < lineMaterials.Count; i++)
            {
                lineMaterials[i].SetColor("_EmissionColor", lightColors[i].color * value_);
            }
        }
    }

    // Class for a single light segment as it travels between mediums.
    public class LightSegment
    {
        public Vector3 start;
        public Vector3 direction;
        public Vector3 end;
        public float index;
        public LightColor color;
        public Vector3 normal;

        public LightSegment()
        {
            start = Vector3.zero;
            end = Vector3.zero;
            index = 1.0f;
            color = new LightColor();
        }

        public LightSegment(Vector3 start_)
        {
            start = start_;
            end = Vector3.zero;
            index = 1.0f;
            color = new LightColor();
        }

        public LightSegment(Vector3 start_, Vector3 direction_, float index_, LightColor lightColor_)
        {
            start = start_;
            direction = direction_;
            index = index_;
            color = lightColor_;
        }
    }
}
