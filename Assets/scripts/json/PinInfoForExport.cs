using Microsoft.Geospatial;
using UnityEngine;

/**
 * This class is the variables that need to be export in a json.
 */
public class PinInfoForExport : MonoBehaviour
{
    public string id;

    public string name;

    public LatLon location;
    
    public void setInfo(string id,string name, LatLon location)
    {
        this.id = id;
        this.name = name;
        this.location = location;
    }

    public string getId()
    {
        return id;
    }
    public string getName()
    {
        return name;
    }

    public LatLon getLocation()
    {
        return location;
    }
    
}
