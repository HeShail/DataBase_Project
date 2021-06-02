using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;

// Clase encargada de monitorizar el sistema de admisión del programa y añadirlos a la base de datos (SQLite)
public class DataBase : MonoBehaviour
{
    private string nombreDB = "URI=file:Candidatos.db";
    public Animator anim;
    public AudioSource firma;
    public AudioClip pasarPagina;
    public AudioClip firmar;
    public GameObject botonMostrar;
    public TextMeshProUGUI nombreHero;
    public TextMeshProUGUI donHero;
    public TextMeshProUGUI alturaHero;
    public TextMeshProUGUI notaHero;
    public TextMeshProUGUI admitidos;
    public Estudiante[] lista;
    private int i;

    void Start()
    {
        i = 0;
        DespertarDB();
        lista = GetComponent<SavegameManager>().estudiantes;
        MostrarCarta();
    }

    //En esta función se construye la base de datos de nombre candidatos con los tipos Nombre, Quirk, Estatura_cm y Nota_Acceso
    void DespertarDB()
    {

        using (var connection = new SqliteConnection(nombreDB))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS candidatos (Nombre VARCHAR(20), Quirk VARCHAR(15), Estatura_cm INT, Nota_Acceso INT )";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    //Método empleado para matricular un solicitante al curso e incorporarlo a la base de datos.
    public void AgregarAlumno(string nombre, string poder, int estatura, int calificacion, int i)
    {
        if (i < lista.Length-1)
        {
            using (var connection = new SqliteConnection(nombreDB))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO candidatos (Nombre, Quirk, Estatura_cm, Nota_Acceso) VALUES ('" + nombre + "', '" + poder + "', '" + estatura + "', '" + calificacion + "');";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

        }

    }
    
    //Función encargada de mostrar los alumnos matriculados en el curso. Despertable tras juzgar a todos los solicitantes.
    public void MostrarAlumnos()
    {
        using (var connection = new SqliteConnection(nombreDB))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM candidatos;";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        admitidos.text += reader["nombre"] + "\n";
                    }
                }
                connection.Close();
            }
        }
        botonMostrar.SetActive(false);
    }

    /*Método que muestra los datos de cada solicitante en pantalla, 
     * siguiendo el array lista que hereda directamente del array estudiantes de la clase SavegameManager. 
     */
    public void MostrarCarta()
    {
            nombreHero.text = lista[i].nombre;
            donHero.text = lista[i].don;
            alturaHero.text = lista[i].estatura_cm.ToString() + " cm";
            notaHero.text = lista[i].notaAcceso.ToString();
    }

    /*Método que incorpora la función AgregarAlumno con los parámetros del individuo i del array lista. 
     * También se encarga de suceder a los siguientes estudiantes en la sesión y de evaluar si se ha
     * finiquitado el ejercicio de admisiones, en orden de despertar el botón que muestra la lista de admitidos en pantalla.
     */
    public void AdmitirHeroe()
    {
        if (i < lista.Length)
        {
            AgregarAlumno(lista[i].nombre, lista[i].don, lista[i].estatura_cm, lista[i].notaAcceso, i);
            if (i >= lista.Length-1) i = lista.Length - 1;
            else
            {
                anim.SetTrigger("aprobar");
                firma.PlayOneShot(firmar);
                i++;
                if (i == lista.Length-1) botonMostrar.SetActive(true);
            }
        }
        if (i < lista.Length) MostrarCarta();

    }

    //Función empleada para rechazar la entrada del solicitante a la Academia UA y pasar al siguiente.
    public void RechazarHeroe()
    {
        if (i < lista.Length - 1)
        {
            i++;
            firma.PlayOneShot(pasarPagina);
            anim.SetTrigger("rechazar");
            MostrarCarta();
            if (i == lista.Length - 1) botonMostrar.SetActive(true);
        }
    }


}
