using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using TMPro;

/**
 * This class is used to create pins from Json
 */
public class CreatePinFromJson : MonoBehaviour
{
    [SerializeField]
    private MapPinLayer _mapPinLayer;

    [SerializeField]
    public MapPin _fireFighterPin;

    [SerializeField] public MapPin _policeman;
    [SerializeField] public MapPin _doctor;
    [SerializeField] public MapPin _other;
    
    private string[] info = new string[4];
    // Start is called before the first frame update
    /*
     * choose the points we want from the Json, and get the information of the point
     */
    void Start()
    {
        string dataJson = File.ReadAllText(Application.dataPath + "/Data/data.json", Encoding.UTF8);
        JObject data = JObject.Parse(dataJson);
        string ts = data["node"].ToString();
        JArray node = JArray.Parse(ts);
        for (int i = 0; i < node.Count; i++)
        {
            string p0 = node[i].ToString();
            JObject nodedata = JObject.Parse(p0);
            string _geoLocalisation = nodedata["geoLocalisation"].ToString();
            if (_geoLocalisation == "")
            {
                continue;
            }
            else  // geoLocalisation not null
            {
                JObject _geoLocalisationdata = JObject.Parse(_geoLocalisation);
                string pointstring = _geoLocalisationdata["point"].ToString();
                if (pointstring == "")
                {
                    continue;
                }
                else //point not null
                {
                    JObject point = JObject.Parse(pointstring);
                    info[1] = point["latitude"].ToString();
                    info[2] = point["longitude"].ToString();
                    
                    
                    // get the name in property
                    string propertystring = nodedata["property"].ToString();
                    JArray propertyArray = JArray.Parse(propertystring);
                    for (int j = 0; j < propertyArray.Count; j++)
                    {
                        string pro = propertyArray[j].ToString();
                        JObject proobj = JObject.Parse(pro);
                        if (proobj["name"].ToString() == "name")
                        {
                            info[3] = proobj["value"].ToString();
                            break;
                        }
                    }

                    info[0] = nodedata["id"].ToString();
                    initialPin(info);


                }
            }
        }
    }

    /*
     * initialization Pin
     * Judge the role then create a pin
     */
    private void initialPin(string[] info)
    {
        string role = judgeRole(info[3]);
        double lat;
        double lon;
        Double.TryParse(info[1], out lat);
        Double.TryParse(info[2], out lon);
        LatLon location = new LatLon(lat,lon);
        if (role == "Policeman")
        {
            GeneralMethod.createPin(_policeman,_mapPinLayer,location, info[3],info[0],false);
        }
        else if (role == "Fireman")
        {
            GeneralMethod.createPin(_fireFighterPin,_mapPinLayer,location, info[3],info[0],false);
        }
        else if (role == "Doctor")
        {
            GeneralMethod.createPin(_doctor,_mapPinLayer,location, info[3],info[0],false);
        }
        else
        {
            GeneralMethod.createPin(_other,_mapPinLayer,location, info[3],info[0],false);
        }
    }

    /*
     * judge the role from info[3]
     * in this case there are 4 kind of roles: Policeman,Fireman, Doctor and Presense
     * and I treat Presense as other
     */
    private string judgeRole(string st)
    {
        switch (st.Substring(0,3))
        {
            case "Pol":
                return "Policeman";
            case "Fir":
                return "Fireman";
            case "Doc":
                return "Doctor";
            default:
                return "Other";
        }
    }
    
}
