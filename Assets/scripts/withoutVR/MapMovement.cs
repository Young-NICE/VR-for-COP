using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;

/**
 * This class is used to move the map with mouse, include pan and zoom
 */
public class MapMovement : MonoBehaviour
{
    private MapRenderer _map;
    
    private LatLon _location;

    private double _zoomlevel = 1f;
    
    public const float _initialChangement = 0.000001f;
    
    [Range(0f,20f)]
    public float moveSpeed = 0f;

    [Range(0f, 4f)] public float zoomSpeed = 0f;
    
    private MapScene _currentScene;

    [SerializeField]
    public Camera _referenceCamera;

    private bool _shouldDrag;
    private bool _mouseOnMap;
    private LatLon _originPosition;
    private LatLon _currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 4.0f;
        _map = FindObjectOfType<MapRenderer>();
        _currentScene = new MapSceneOfLocationAndZoomLevel(_map.Center,_map.ZoomLevel);

    }

    // Update is called once per frame
    void Update()
    {
        zoomMap();
        moveMapWithKeyboard();
        moveMapWithMouse();
    }
    
    /*
     * zoom in and zoom out with mouse scrollwheel
     */
    public void zoomMap()
    {
        double changement = Input.GetAxis("Mouse ScrollWheel");
        _currentScene.GetLocationAndZoomLevel(out _location,out _zoomlevel);
        if (Math.Abs(changement)>0.0f)
        {
            _zoomlevel = _zoomlevel + changement * Math.Pow(2, zoomSpeed);
            UpdateMap(_location,(float)_zoomlevel);
        }
    }
    
    /*
     * use keyboard to move map
     * directions: W A S D
     * reset: R
     */
    public void moveMapWithKeyboard()
    {
        LatLon _newLocation = _location;
        _currentScene.GetLocationAndZoomLevel(out _location,out _zoomlevel);
        if (Input.GetKey(KeyCode.W))
        {
            _newLocation = new LatLon(_location.LatitudeInDegrees + _initialChangement*Math.Pow(2,moveSpeed),
                _location.LongitudeInDegrees);
            UpdateMap(_newLocation,(float)_zoomlevel);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _newLocation = new LatLon(_location.LatitudeInDegrees - _initialChangement*Math.Pow(2,moveSpeed),
                _location.LongitudeInDegrees);
            UpdateMap(_newLocation,(float)_zoomlevel);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _newLocation = new LatLon(_location.LatitudeInDegrees,
                _location.LongitudeInDegrees - _initialChangement*Math.Pow(2,moveSpeed)*2);
            UpdateMap(_newLocation,(float)_zoomlevel);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _newLocation = new LatLon(_location.LatitudeInDegrees,
                _location.LongitudeInDegrees + _initialChangement*Math.Pow(2,moveSpeed)*2);
            UpdateMap(_newLocation, (float) _zoomlevel);
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            _newLocation = new LatLon(43.92252, 2.14711);
            UpdateMap(_newLocation,17.0f);
        }
        
    }
    
    /*
     * use mouse to move the map
     */
    void moveMapWithMouse()
    {
        // make sure _mouseOnMap == true
        getPoint();
        // click mouse and click on the map
        if (Input.GetMouseButton(0) && _mouseOnMap)
        {
            _currentPosition = getPoint().Location.LatLon;
            if (_shouldDrag == false)
            {
                _shouldDrag = true;
                _originPosition = getPoint().Location.LatLon;
            }
        }
        else
        {
            _shouldDrag = false;
        }

        if (_shouldDrag == true)
        {
            var changement = new Vector2((float) (_originPosition.LatitudeInDegrees - _currentPosition.LatitudeInDegrees),
                (float) (_originPosition.LongitudeInDegrees-_currentPosition.LongitudeInDegrees));

            if (Math.Abs(changement.x) > 0.0f || Math.Abs(changement.y) > 0.0f)
            {
                _originPosition = _currentPosition;
                /*
                 * Normally, don't need to divided by 2. But I use /2 to reduce sensitivity, because in a 3D map the hit
                 * point may not be alone. In this way can reduce the tremor in movement
                 */
                var latestlat = _location.LatitudeInDegrees + changement.x/2;
                var latestlon = _location.LongitudeInDegrees + changement.y/2;
                UpdateMap(new LatLon(latestlat,latestlon), (float)_zoomlevel);
            }
            
        }
    }
    
    /*
     * get the information of the hit point on the map
     * the ray is from camera to the mouse position
     */
    public MapRendererRaycastHit getPoint()
    {
        MapRendererRaycastHit info;
        var ray = _referenceCamera.ScreenPointToRay(Input.mousePosition);
        var mapRenderer = GetComponent<MapRenderer>();
        
        if (mapRenderer.Raycast(ray, out info))
        {
            _mouseOnMap = true;
        }
        else
        {
            _mouseOnMap = false;
        }
        return info;
    }
    
    
    
    /*
     * Update map with LatLon and zoomlevel
     * The microsoft official method to move the map is not this, but it's also a solution.
     * I use it at the beginning because I did't find another solution.
     * Microsoft uses the _map.Center method, and I use it in the VRMapMovement. 
     */
    public void UpdateMap(LatLon latLon,float zoomlevel)
    {
        _currentScene = new MapSceneOfLocationAndZoomLevel(latLon, zoomlevel);
        Animate(_currentScene);
        _currentScene.GetLocationAndZoomLevel(out _location,out _zoomlevel);
    }
    
    // Move to a new map scene
    public void Animate(MapScene mapScene)
    {
        var mapRenderer = GetComponent<MapRenderer>();
        mapRenderer.SetMapScene(mapScene,null,MapSceneAnimationKind.None,1f);
    }
    
}
