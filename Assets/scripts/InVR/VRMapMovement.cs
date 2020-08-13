using System;
using System.Numerics;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/**
 * This class is used to move the map in VR, include pan, zoom and rotation
 */
public class VRMapMovement : MonoBehaviour
{
    [SerializeField]
    public MapRenderer _map;
    
    public Hand _currentHand;

    public Hand _anotherHand;

    [Range(0,10)]
    public float _zoomSpeed = 1.0f;

    [SerializeField]
    public bool _notlefthand;

    private bool _handOnMap = false;

    private LatLon _originLocation;

    private LatLon _currentLocation;

    private bool _shouldDrag = false;

    private bool _shouldZoom = false;

    private Vector2 _originHandTransformPosition;
    
    private Vector2 _currentHandTransformPosition;

    private bool _inRotate = false;

    private bool _inPan = true;

    private bool _inZoom = false;
    
    private double _currentDistance;

    private double _originDistance;

    private BigInteger i = 0;
    private void Update()
    {
        if (SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].state)
        {
            panMap();
        }

        if (SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].state)
        {
            zoomMap();
        }
        if (SteamVR_Actions.MySet.MyRorateMap[_currentHand.handType].state)
        {
            rotateMap();
        }
    }
    
    // pan the map
    private void panMap()
    {
        GeneralMethod.getHitPoint(_currentHand.transform.position, Vector3.down, out _handOnMap);
        /*
         * The first if and !SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].lastStateDown is very important
         * Without them the map cannot move smoothly.
         */
        if (SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].stateDown)
        {
            
        }
        else if (_handOnMap && SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].state && !SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].lastStateDown)
        {
            
            _currentLocation = GeneralMethod.getHitPoint(_currentHand.transform.position, Vector3.down, out _handOnMap).Location.LatLon;
            if (_shouldDrag == false)
            {
                _shouldDrag = true;
                _originLocation = GeneralMethod.getHitPoint(_currentHand.transform.position, Vector3.down, out _handOnMap).Location.LatLon;
            }
        }
        else
        {
            _shouldDrag = false;
        }
        
        if (_shouldDrag == true)
        {
            var changement = new Vector2((float) (_originLocation.LatitudeInDegrees - _currentLocation.LatitudeInDegrees),
                (float) (_originLocation.LongitudeInDegrees-_currentLocation.LongitudeInDegrees));

            if (((Math.Abs(changement.x) > 0.000001f) || Math.Abs(changement.y) > 0.000001f) &&
                (Math.Abs(changement.x)<10.0f && Math.Abs(changement.y)<10.0f))
            {
                _originLocation = _currentLocation;
                /*
                 * Normally, don't need to divided by 2. But I use /2 to reduce sensitivity, because in a 3D map the hit
                 * point may not be alone. In this way can reduce the tremor in movement
                 */
                _map.Center = new LatLon(_map.Center.LatitudeInDegrees + changement.x/2f,
                    _map.Center.LongitudeInDegrees + changement.y/2f);

            }
            
        }

        if (SteamVR_Actions.MySet.MyMoveMap[_currentHand.handType].stateUp)
        {
            _originLocation = LatLon.Origin;
            _currentLocation = LatLon.Origin;
        }
    }
    

    // zoom map
    private void zoomMap()
    {
        if (SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].stateDown)
        {
            
        }
        else if (SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].state && !SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].lastStateDown)
        {
            if (SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].stateUp)
            {
                _shouldZoom = false;
                return;
            }
            _currentDistance = CalculateDistance(new Vector2(_currentHand.transform.position.x,_currentHand.transform.position.z),
                new Vector2(_anotherHand.transform.position.x,_anotherHand.transform.position.z));

            if (_shouldZoom == false)
            {
                _shouldZoom = true;
                _originDistance = _currentDistance;
            }
        }
        else
        {
            _shouldZoom = false;
        }

        if (_shouldZoom == true)
        {
            var changement = _currentDistance - _originDistance;
            if (Math.Abs(changement) > 0.0f && Math.Abs(changement) < 8.0f)
            {
                _map.ZoomLevel = (float) (_map.ZoomLevel + 0.11 * changement/_zoomSpeed);
            }
        }
    }

    private void rotateMap()
    {
        Vector2 _mapCenter = new Vector2(_map.transform.position.x, _map.transform.position.z);
        if (SteamVR_Actions.MySet.MyZoomMap[_currentHand.handType].stateDown)
        {
            
        }
        else if (SteamVR_Actions.MySet.MyRorateMap[_currentHand.handType].state && !SteamVR_Actions.MySet.MyRorateMap[_currentHand.handType].lastStateDown)
        {
            _currentHandTransformPosition = new Vector2(_currentHand.transform.position.x,_currentHand.transform.position.z);
            if (_shouldDrag == false)
            {
                _shouldDrag = true;
                _originHandTransformPosition = _currentHandTransformPosition;
            }
        }
        else
        {
            _shouldDrag = false;
        }
        
        if (_shouldDrag == true)
        {
            //calculate angle with Law of cosines
            var a = CalculateDistance(_mapCenter, _originHandTransformPosition);
            var b = CalculateDistance(_mapCenter, _currentHandTransformPosition);
            var c = CalculateDistance(_originHandTransformPosition, _currentHandTransformPosition);
            var cosC = (a * a + b * b - c * c) / (2 * a * b);
            var rorateAngle = (Math.Acos(cosC) / Math.PI * 180 )/10.0f;
                
            if (rorateAngle > 0f)
            {
                var dir = 1;
                if (!_notlefthand)
                {
                    dir = -1;
                }
                if (_currentHandTransformPosition.y > _originHandTransformPosition.y)
                {
                    _map.transform.Rotate(new Vector3(0,(float) (-rorateAngle)/2*dir,0));
                }
                else
                {
                    _map.transform.Rotate(new Vector3(0,(float) rorateAngle/2*dir,0));
                }
            }
            
        }
    }

    // Calculate distance between two points
    private double CalculateDistance(Vector2 p1, Vector2 p2)
    {
        return Math.Sqrt(Math.Pow((p1.x - p2.x), 2) + Math.Pow((p1.y - p2.y), 2));
    }
}
