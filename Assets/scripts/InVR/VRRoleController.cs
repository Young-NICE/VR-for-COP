using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;
using Valve.VR.InteractionSystem;

/**
 * Used to pick role
 */
public class VRRoleController : MonoBehaviour
{
    private String pickedRole = "policeman";

    public MapRenderer _map;
    
    public void clickPolice()
    {
        pickedRole = "policeman";
        _map.SendMessage("pickPolice");
    }
    
    public void clickFirefighter()
    {
        pickedRole = "fireman";
        _map.SendMessage("pickFireman");
    }
    
    public void clickDoctor()
    {
        pickedRole = "doctor";
        _map.SendMessage("pickDoctor");
    }
    
    
}
