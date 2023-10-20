using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActividadArchivos.Models
{
    [Serializable]
    class Curso
    {
        public string Nombre { get; set; }
        List<Alumno> alumnos = new List<Alumno>();
        public int CantidadAlumno { get { return alumnos.Count; } }

        public void Importar(int dni, string nombre, double nota)
        {

            Alumno al = VerAlumno(dni);
            if (al != null)
            {
                al.AcumNotas = nota;
            }
            else
            {
                al = new Alumno { Dni = dni, Nombre = nombre, AcumNotas = nota };
                alumnos.Add(al);
                OrdenarAlumnos();
            }
        }

        public List<string> Exportar()
        {
            List<string> lista = new List<string>();
            foreach (Alumno al in alumnos)
            {
                if (al.Promedio >= 60)
                {
                    lista.Add(al.ToString());
                }
            }
            return lista;
        }

        public Alumno VerAlumno(int dni)
        {
            //alumnos.Sort();
            int idx=alumnos.BinarySearch(new Alumno { Dni = dni });
            if (idx > 0)
                return alumnos[idx];
            return null;
        }

        public Alumno VerAlumnoIdx(int n)
        {
            return alumnos[n];
        }

        public void OrdenarAlumnos()
        {
            alumnos.Sort();
        }

        public void AgregarAlumno(int dni, string nombre)
        {
            Alumno nuevo = new Alumno() { Dni = dni, Nombre = nombre };
            int idx = alumnos.BinarySearch(nuevo);

            if (idx < 0)
            {
                alumnos.Add(nuevo);
                OrdenarAlumnos();
            }
            else
                throw new Exception($"el alumno existe: {dni}");
        }
    }
}
