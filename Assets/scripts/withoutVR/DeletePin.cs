using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;

/**
 * This class is used to delete pin at the beginning.
 * But I don't use it anywhere in the project, because I find it that when you move
 * the pin out of the map, the pin will be delete.
 */
public class DeletePin : MonoBehaviour
{
    [SerializeField]
    public Camera _referenceCamera;
    
    public static bool _allowDelete;

    private bool _gotPin = false;

    private void Update()
    {
        PinDelete();
    }

    private void PinDelete()
    {
        if (_allowDelete && Input.GetMouseButton(1))
        {
            MapPin _pickedPin = GeneralMethod.pickPin(_referenceCamera, out _gotPin);
            if (_gotPin)
            {
                GeneralMethod.RemovePin(_pickedPin);
            }
        }
    }

    public static void ChangeDeletePin(bool jd)
    {
        _allowDelete = jd;
    }
}
