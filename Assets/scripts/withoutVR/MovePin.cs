using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;

/**
 * This class is used to move the pin
 */
public class MovePin : MonoBehaviour
{
    [SerializeField] public Camera _referenceCamera;

    private bool _mouseOnPin = false;

    private static bool _inMovePinModel = false;

    private bool _shouldDrag = false;

    private bool _gotPin = false;


    private LatLon _originPosition;

    private LatLon _currentPosition;

    // Update is called once per frame
    void Update()
    {
        movePin();
    }

    /*
     * move pin
     */
    public void movePin()
    {
        if (_inMovePinModel)
        {
            MapRendererRaycastHit info;
            MapPin _pickedPin = null;
            if (Input.GetMouseButton(1))
            {
                _currentPosition = getPoint().Location.LatLon;
                 _pickedPin = GeneralMethod.pickPin(_referenceCamera,out _gotPin);
                if (!_shouldDrag && _gotPin)
                {
                    _shouldDrag = true;
                    _originPosition = getPoint().Location.LatLon;
                }
            }
            else
            {
                _shouldDrag = false;
            }

            if (_shouldDrag && _gotPin)
            {
                var changement = new Vector2((float) (_originPosition.LatitudeInDegrees - _currentPosition.LatitudeInDegrees),
                    (float) (_originPosition.LongitudeInDegrees-_currentPosition.LongitudeInDegrees));

                if (Math.Abs(changement.x) > 0.0f || Math.Abs(changement.y) > 0.0f)
                {
                    _originPosition = _currentPosition;
                    _pickedPin.Location = new LatLon(_pickedPin.Location.LatitudeInDegrees - changement.x,
                        _pickedPin.Location.LongitudeInDegrees - changement.y);
                }
            }
        }
    }
    
    /*
     * get the information of the hit point on the map
     */
    public MapRendererRaycastHit getPoint()
    {
        MapRendererRaycastHit info;
        var ray = _referenceCamera.ScreenPointToRay(Input.mousePosition);
        var mapRenderer = GetComponent<MapRenderer>();
        mapRenderer.Raycast(ray, out info);
        return info;
    }
    
    /*
     * Change _inMovePinModel through ChangePinModel
     */
    public static void ChangeMovePinModel(bool jd)
    {
        _inMovePinModel = jd;
    }
}
