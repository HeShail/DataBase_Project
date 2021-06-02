using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase que contiene toda la información requerida para la matriculación en la Academia.
public class Estudiante : MonoBehaviour
{
    public string nombre;
    public string don;
    public int estatura_cm;
    public string grupo_sanguineo;
    public int notaAcceso;

    //Método que transforma primero el objeto al formato JSON y acto seguido lo devuelve en formato JObject.
    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject retVal = JObject.Parse(jsonString);
        return retVal;
    }

    //Método que sobreescribe los objetos en formato JSON.
    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }
}
