using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This class is used to choose the model of your operation through a dropdown
 * Actually, DeleteModel is never used.
 */
public class ChangePinModelOrDeletePin : MonoBehaviour
{
    [SerializeField]
    public Dropdown _dropdown;
    
    private static bool _inAddModel = true;

    private static bool _inMoveModel = false;

    private static bool _allowDelete = false;
    
    public void GetMsg()
    {
        switch (_dropdown.value)
        {
            case 0:
                _inAddModel = true;
                _inMoveModel = false;
                _allowDelete = false;
                break;
            case 1:
                _inAddModel = false;
                _inMoveModel = true;
                _allowDelete = false;
                break;
            case 2 :
                _inAddModel = false;
                _inMoveModel = false;
                _allowDelete = true;
                break;
            default:
                Debug.LogError("There is no this choice.");
                break;
        }
        AddMapPin.ChangeAddPinModel(_inAddModel);
        MovePin.ChangeMovePinModel(_inMoveModel);
        DeletePin.ChangeDeletePin(_allowDelete);
    }
}
