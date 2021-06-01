using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;

public class DataBase : MonoBehaviour
{
    private string nombreDB = "URI=file:Candidatos.db";
    public Animator anim;
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
    }

    public void MostrarCarta()
    {
            nombreHero.text = lista[i].nombre;
            donHero.text = lista[i].don;
            alturaHero.text = lista[i].estatura_cm.ToString() + " cm";
            notaHero.text = lista[i].notaAcceso.ToString();
    }
    public void AdmitirHeroe()
    {
        if (i < lista.Length)
        {
            AgregarAlumno(lista[i].nombre, lista[i].don, lista[i].estatura_cm, lista[i].notaAcceso, i);
            if (i >= 8) i = 8;
            else
            {
                anim.SetTrigger("aprobar");
                i++;
            }
        }
        if (i < lista.Length) MostrarCarta();

    }

    public void RechazarHeroe()
    {
        if (i < lista.Length - 1)
        {
            i++;
            anim.SetTrigger("rechazar");
            MostrarCarta();
        }
    }


}
