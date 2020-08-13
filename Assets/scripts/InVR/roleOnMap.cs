using UnityEngine;

/**
 * This class is used to choose role
 * When you click the role(police, fireman and doctor) in the menu in VR, it will call these method
 */
public class roleOnMap : MonoBehaviour
{
    private static string pickedRole = "policeman";

    public void pickPolice()
    {
        pickedRole = "policeman";
    }
    
    public void pickFireman()
    {
        pickedRole = "fireman";
    }
    
    public void pickDoctor()
    {
        pickedRole = "doctor";
    }

    public static string getPickedRole()
    {
        return pickedRole;
    }
    
}
