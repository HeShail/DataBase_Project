using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class DataBase : MonoBehaviour
{
    private string nombreDB = "URI=file:Candidatos.db";

    void Start()
    {
        DespertarDB();
        AñadirAlumno("Izuku Midoriya", "One for All", 167, 9);
        MostrarAlumnos();
    }


    void Update()
    {
        
    }

    void DespertarDB()
    {

        using (var connection = new SqliteConnection(nombreDB))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS candidatos (nombre VARCHAR(20), quirk VARCHAR(15), estatura_cm INT, notaAcceso INT )";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void AñadirAlumno(string nombre, string poder, int estatura, int calificacion)
    {
        using (var connection = new SqliteConnection(nombreDB))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO candidatos (nombre, quirk, estatura_cm, notaAcceso) VALUES ('" + nombre + "', '" + poder + "', '" + estatura + "', '" + calificacion + "');";
                command.ExecuteNonQuery();
            }
            connection.Close();
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
                        Debug.Log("Nombre: " + reader["nombre"] + " Quirk: " + reader["quirk"] + " Estatura: " + reader["estatura_cm"] + " Nota de acceso: " + reader["notaAcceso"]);
                    }
                }
                connection.Close();
            }
        }
    }
}
