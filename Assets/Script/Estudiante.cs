using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estudiante : MonoBehaviour
{
    public string nombre;
    public string don;
    public int estatura_cm;
    public string grupo_sanguineo;
    public int notaAcceso;

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject retVal = JObject.Parse(jsonString);
        return retVal;
    }

    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }
}
