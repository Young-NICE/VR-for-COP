using System.Collections;
using System.IO;
using System.Text;
using Microsoft.Maps.Unity;
using Newtonsoft.Json;
using UnityEngine;

namespace json
{
    /**
     * This class is used to export Json
     */
    public class ExportJson : MonoBehaviour
    {
        /*
         * This method will be called when the exportJson button is active.
         * Find the active pins in the list of GameObject map's Children object.
         * Get the information(id,name,location) and write them in exportjson.jso
         */
        public void click()
        {
            ArrayList _mylist = new ArrayList();
            JsonStructure nodes = new JsonStructure();
            var list = GetComponentsInChildren<MapPin>();
            foreach (var pin in list)
            {
                OutputData outputData = new OutputData();
                outputData.id = pin.GetComponent<PinInfoForExport>().getId();
                outputData.name = pin.GetComponent<PinInfoForExport>().getName();
                outputData.location = pin.GetComponent<PinInfoForExport>().getLocation();
                //var data = JsonConvert.SerializeObject(outputData);
                _mylist.Add(outputData);
            }

            nodes.nodes = _mylist;
            var finaljson = JsonConvert.SerializeObject(nodes);

            /*
             * write the json to file exportjson.json in Data
             */
            FileStream fs = new FileStream(Application.dataPath + "/Data/exportjson.json", FileMode.Create);
            byte[] bytes = new UTF8Encoding().GetBytes(finaljson);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}