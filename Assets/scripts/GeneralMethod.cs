using System;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using TMPro;
using UnityEngine;

/**
 * This class is some method that often used by other classes
 */
public class GeneralMethod : MonoBehaviour
{
    private static int _addid  = 0;

    /*
     * get the number for the new added pins
     */
    public static string get_addid()
    {
        _addid++;
        return Convert.ToString(_addid);
    }
    
    
    /*This is used to create a pin for add pin with id*/
    public static void createPin(MapPin role,MapPinLayer _mapPinLayer, LatLon location, string rolename, bool _defaultLayer)
    {
        
        var mapPin = Instantiate(role);
        mapPin.IsLayerSynchronized = _defaultLayer;
        mapPin.Location = location;
        var name = rolename + get_addid();
        mapPin.GetComponentInChildren<TextMeshPro>().text = name;
        _mapPinLayer.MapPins.Add(mapPin);
        mapPin.GetComponent<PinInfoForExport>().setInfo("node_"+Guid.NewGuid().ToString(),name,location);
    }

    /*This is used to create pins according to the json*/
    public static void createPin(MapPin role,MapPinLayer _mapPinLayer, LatLon location, string info, string id, bool _defaultLayer)
    {
        var mapPin = Instantiate(role);
        mapPin.IsLayerSynchronized = _defaultLayer;
        mapPin.Location = location;
        mapPin.GetComponentInChildren<TextMeshPro>().text = info;
        _mapPinLayer.MapPins.Add(mapPin);
        mapPin.GetComponent<PinInfoForExport>().setInfo(id,info,location);
    }
    
    /*
     * get the hit point by origin and direction
     * if the hitpoint is on the map _handsOnMap == true
     * else false
     */
    public static MapRendererRaycastHit getHitPoint(Vector3 origin, Vector3 direction, out bool _handsOnMap)
    {
        origin = new Vector3(origin.x,origin.y+100.0f,origin.z);
        MapRenderer _mapRenderer = FindObjectOfType<MapRenderer>();
        MapRendererRaycastHit info;
        if (_mapRenderer.Raycast(origin,direction,out info))
        {
            _handsOnMap = true;
        }
        else
        {
            _handsOnMap = false;
        }
        return info;
    }
    
    
    /*Remove Map pin*/
    public static void RemovePin(MapPin _mappin)
    {
        MapPinLayer _mapPinLayer = FindObjectOfType<MapPinLayer>();
        _mapPinLayer.MapPins.Remove(_mappin);
    }
    
    /*
     * get the mappin
     * I put all pins in the 8th layer for ray to hit the pins
     */
    public static MapPin pickPin(Camera _referenceCamera, out bool _gotPin)
    {
        Ray ray = _referenceCamera.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit ;
        int layerMask = 1 << 8;
        if (Physics.Raycast (ray, out hit, float.MaxValue,layerMask)) { 
            Transform obj = hit.collider.gameObject.transform;
            MapPin role = obj.parent.GetComponent<MapPin>();
            _gotPin = true;
            return role;
        }
        else
        {
            _gotPin = false;
            return null;
        }
    }
}
