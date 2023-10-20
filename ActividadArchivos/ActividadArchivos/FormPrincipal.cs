using ActividadArchivos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActividadArchivos
{
    public partial class FormPrincipal : Form
    {
        Curso curso;
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string ruta = Application.StartupPath;
            string rutaBin = Path.Combine(ruta, "sistema.dato");

            FileStream fs = null;
            try
            {
                if (File.Exists(rutaBin))
                {
                    fs = new FileStream(rutaBin, FileMode.Open, FileAccess.Read);
                    BinaryFormatter bt = new BinaryFormatter();
                    curso = bt.Deserialize(fs) as Curso;
                }
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            if (curso == null)
            {
                curso = new Curso( "Matemática");
                curso.AgregarAlumno(24324232, "juan domingo");
                curso.AgregarAlumno(58233232, "fortunato perez");
            }
            curso.OrdenarAlumnos();
            PintarListado();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string ruta = Application.StartupPath;
            string rutaBin = Path.Combine(ruta, "sistema.dato");

            FileStream fs = null;
            try
            {

                fs = new FileStream(rutaBin, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter bt = new BinaryFormatter();
                bt.Serialize(fs, curso);

            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title= "Importando notas de parciales";
            openFileDialog1.Filter = "Notas de parciales|*.csv";

            openFileDialog1.InitialDirectory = Path.Combine(Application.StartupPath,"..","..","archivos");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string rutaCsv = openFileDialog1.FileName;
                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    fs = new FileStream(rutaCsv, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);

                    //leer una vez con eso descarto la cabecera
                    string linea = sr.ReadLine();

                    while (sr.EndOfStream == false)
                    {
                        linea = sr.ReadLine();
                        string[] campos = linea.Split(';');

                        int dni = Convert.ToInt32(campos[0]);
                        string nombre = campos[1];
                        double nota = Convert.ToDouble(campos[2]);

                        /**/
                       
                        curso.Importar(dni, nombre, nota);
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (fs != null)
                    {

                        if (sr != null) sr.Close();
                        fs.Close();
                    }
                }
            }
            PintarListado();
        }

        public void PintarListado()
        {
            listBox1.Items.Clear();
            for (int n = 0; n < curso.CantidadAlumno; n++)
                listBox1.Items.Add(curso.VerAlumnoIdx(n));
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Exportando listado alumnos aprobados";
            saveFileDialog1.Filter = "Fichero notas aprobados|*.csv";
            saveFileDialog1.InitialDirectory = Path.Combine(Application.StartupPath, "..", "..", "archivos");

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string rutaCsv = saveFileDialog1.FileName;
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(rutaCsv, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs);

                    //descartar cabecera
                    List<string> lineas = curso.Exportar();

                    sw.WriteLine("dni; promedio");
                    foreach (string linea in lineas)
                        sw.WriteLine(linea);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (fs != null)
                    {
                        if (sw != null) sw.Close();
                        fs.Close();
                    }
                }
            }
        }
    }
}
