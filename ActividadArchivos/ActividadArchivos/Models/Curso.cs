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

        public Curso(string nombre)
        {
            Nombre = nombre;
        }

        public void Importar(int dni, string nombre, double nota)
        {

            Alumno al = VerAlumno(dni);
            if (al != null)
            {
                //actualizo
                al.AcumNotas = nota;
            }
            else
            {
                //sino agrego
                Alumno nuevo=AgregarAlumno(dni,nombre);
                nuevo.AcumNotas = nota;
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
            Alumno buscado = null;
            int idx=alumnos.BinarySearch(new Alumno( dni));
            if (idx >= 0)
                buscado= alumnos[idx];
            return buscado;
        }

        public Alumno VerAlumnoIdx(int n)
        {
            Alumno buscado = null;
            if(n>=0 && n<CantidadAlumno)
                buscado =alumnos[n];
            return buscado;
        }

        public void OrdenarAlumnos()
        {
            alumnos.Sort();
        }

        public Alumno AgregarAlumno(int dni, string nombre)
        {
            Alumno nuevo = new Alumno( dni,  nombre);
            int idx = alumnos.BinarySearch(nuevo);

            if (idx < 0)
            {
                //este me exige ordenar cada vez que agrego
                //alumnos.Add(nuevo);
                //OrdenarAlumnos();

                //este inserta de forma ordenada
                //vean que devuelve el binarysearch en la doc oficial
                //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.binarysearch?view=net-7.0
                alumnos.Insert(~idx, nuevo);
            }
            else
                throw new Exception($"el alumno existe: {dni}");

            return nuevo;
        }
    }
}
