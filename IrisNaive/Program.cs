using System;

namespace IrisNaive
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] datos = System.IO.File.ReadAllLines("iris.data");
            vector[] datosIris = new vector[datos.Length];

            string clase;

            for (int i = 0; i < datos.Length; i++)
            {
                string[] valoresString = datos[i].Split(',');
                clase = valoresString[valoresString.Length - 1];

                float[] valores = new float[valoresString.Length - 1];

                for (int k = 0; k < valoresString.Length - 1; k++)
                    valores[k] = float.Parse(valoresString[k]);

                datosIris[i] = new vector(clase, valores);
            }

            Console.WriteLine("Cantidad de datos: " + datosIris.Length);
        }
    }

    class vector
    {
        public float[] datos;
        public float resultado;
        public string clase;
        public string claseRes = "null";
        public vector centroide = null;
        public int cantNodos = 0;

        public vector(string clase, float[] datos)
        {
            this.clase = clase;
            this.datos = datos;
            resultado = 0;
        }

        override
        public String ToString()
        {
            String cadena = "[";
            for (int i = 0; i < datos.Length; i++)
            {
                if (i == 0)
                    cadena += datos[i];
                else
                    cadena += ", " + datos[i];
            }

            cadena += "] Clase: " + clase + "\t\t\t";

            if (centroide != null)
            {
                cadena += "Centroide [" + claseRes + "] [";
                for (int i = 0; i < centroide.datos.Length; i++)
                {
                    if (i == 0)
                        cadena += centroide.datos[i];
                    else
                        cadena += ", " + centroide.datos[i];
                }

                cadena += "]";
            }
            else
            {
                cadena += "Cantidad de nodos: " + cantNodos + " ";
            }

            return cadena;
        }
    }

}
