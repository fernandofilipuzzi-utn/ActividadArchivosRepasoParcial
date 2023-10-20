using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActividadArchivos.Models
{
    [Serializable]
    class Alumno : IComparable
    {
        public int Dni { get; set; }
        public string Nombre { get; set; }

        double acumNotas;
        public double AcumNotas 
        {
            get 
            {
                return acumNotas;
            }
            set
            {
                acumNotas += value;
                cantNotas++;
            }
        }
        int cantNotas;
        public double Promedio {
            get {
                double promedio = 0;
                if (cantNotas > 0)
                    promedio = AcumNotas / cantNotas;
                return promedio;
            }
        }

        public Alumno(int dni)
        {
            Dni = dni;
        }

        public Alumno(int dni, string nombre)
        {
            Dni = dni;
            Nombre = nombre;
        }

        public int CompareTo(object obj)
        {
            Alumno al = obj as Alumno;
            int resultado = -1;
            if (al != null)
                resultado=Dni.CompareTo(al.Dni);
            return resultado;
        }

        public override string ToString()
        {
            return $"{Dni}; {Promedio}";
        }
    }
}
