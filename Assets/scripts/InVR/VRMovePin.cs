using System.Collections;
using System.Collections.Generic;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using UnityEngine;
using Valve.VR.InteractionSystem;

/**
 * This class is used to move pin in VR
 * 
 */
public class VRMovePin : MonoBehaviour
{
    private bool _handsOnMap = false;

    private Camera eye;
    private MapPin _pin;
    void Start()
    {
        _pin = this.GetComponent<MapPin>();
        eye = FindObjectOfType<Camera>();
    }


    
    //When the pin detached from hand, execute this method
    private void OnDetachedFromHand(Hand hand)
    {
        _handsOnMap = false;
        MapRendererRaycastHit hitPoint =
            GeneralMethod.getHitPoint(hand.transform.position, Vector3.down, out _handsOnMap);
        // Vector3 direction = new Vector3(hand.transform.position.x - eye.transform.position.x,
        //     hand.transform.position.y - eye.transform.position.y,
        //     hand.transform.position.z - eye.transform.position.z);
        // MapRendererRaycastHit hitPoint =
        //     GeneralMethod.getHitPoint(hand.transform.position, direction, out _handsOnMap);

        if (_handsOnMap)
        {
            _pin.Location = hitPoint.Location.LatLon;
        }
        else
        {
            Debug.Log(hand.name + "Destray a pin at " + hand.transform.position);
            GeneralMethod.RemovePin(_pin);
        }
    }
}
