using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;
using UnityEngine.UI;
/**
 *  This class is used to add map pin with mouse(right click)
 */
public class AddMapPin : MonoBehaviour
{
    [SerializeField] private Camera _referenceCamera;
    
    [SerializeField]
    private MapPinLayer _mapPinLayer;

    [SerializeField]
    public MapPin _fireFighterPin;

    [SerializeField] public MapPin _policeman;
    [SerializeField] public MapPin _doctor;
    
    [SerializeField] public Dropdown _dropdown;
    
    private bool _mouseOnMap;

    // if true right click to add a pin
    private static bool _inAddPinModel = true;
    
    private String role = "Policeman"; 

    // judge if it is in the add pin model
    public static void ChangeAddPinModel(bool jd)
    {
        _inAddPinModel = jd;
    }

    // get the message from the dropdown to choose the roles
    public void GetMsg()
    {
        switch (_dropdown.value)
        {
            case 0:
                role = "Policeman";
                break;
            case 1:
                role = "Fireman";
                break;
            case 2 :
                role = "Doctor";
                break;
            default:
                Debug.LogError("There is no this kind of role.");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        addPin();
    }

    
    /*
     * right click on the map to add a map pin
     */
    public void addPin()
    {
        if (_inAddPinModel && Input.GetMouseButtonUp(1))
        {
            MapRendererRaycastHit hitPoint = getPoint();
            if (role == "Policeman")
            {
                GeneralMethod.createPin(_policeman,_mapPinLayer,hitPoint.Location.LatLon,role,false);
            }
            else if (role == "Fireman")
            {
                GeneralMethod.createPin(_fireFighterPin,_mapPinLayer,hitPoint.Location.LatLon,role,false);
            }
            else if (role == "Doctor")
            {
                GeneralMethod.createPin(_doctor,_mapPinLayer,hitPoint.Location.LatLon,role,false);
            }
            else
            {
                Debug.LogError("There is no this kind of role");
            }
        }
    }
    
    /*
     * get the information of the point where your mouse on the map
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
}
