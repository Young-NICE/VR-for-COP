using System;
using Microsoft.Maps.Unity;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/**
 * This class is used to add pin in VR
 * I write this script with reference to the SteamVR official demo script "planting"
 * You can find more information in this link
 * https://valvesoftware.github.io/steamvr_unity_plugin/tutorials/SteamVR-Input.html
 */
public class VRAddPin : MonoBehaviour
{
    public SteamVR_Action_Boolean addPinAction;

    public Hand hand;

    [SerializeField]
    private MapPinLayer _mapPinLayer;

    [SerializeField] public MapRenderer _mapRenderer;

    [SerializeField]
    public MapPin _fireman;

    [SerializeField] public MapPin _policeman;
    [SerializeField] public MapPin _doctor;

    //used to mark which role will be add
    private MapPin prefabToAdd;
        
    private bool _handsOnMap;
        
    private String role = "Policeman"; 

    private void Awake()
    {
        prefabToAdd = _policeman;
    }

    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (addPinAction == null)
        {
            Debug.LogError("<b>[SteamVR Interaction]</b> No add pin action assigned", this);
            return;
        }

        addPinAction.AddOnChangeListener(OnAddActionChange, hand.handType);
    }

    private void OnDisable()
    {
        if (addPinAction != null)
            addPinAction.RemoveOnChangeListener(OnAddActionChange, hand.handType);
    }

    private void OnAddActionChange(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource, bool newValue)
    {
        if (newValue)
        {
            DoAdd();
        }
    }

    /*
     * add the pin on map
     */
    private void DoAdd()
    {
        MapRendererRaycastHit hitPoint =
            GeneralMethod.getHitPoint(hand.transform.position, Vector3.down, out _handsOnMap);

        if (_handsOnMap)
        {
            getCurrentRole();
            _handsOnMap = false;
            GeneralMethod.createPin(prefabToAdd,_mapPinLayer,hitPoint.Location.LatLon,role,false);
        }
    }

    private void getCurrentRole()
    {
        switch (roleOnMap.getPickedRole())
        {
            case "policeman":
                role = "Policeman";
                prefabToAdd = _policeman;
                break;
            case "fireman":
                role = "Fireman";
                prefabToAdd = _fireman;
                break;
            case "doctor":
                role = "Doctor";
                prefabToAdd = _doctor;
                break;
            default:
                Debug.LogError("There is no this kind of role");
                break;
        }
    }
}
