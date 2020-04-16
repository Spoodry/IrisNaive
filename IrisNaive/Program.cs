using System;
using System.Collections.Generic;

namespace IrisNaive
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] datos = System.IO.File.ReadAllLines("iris-entrenamiento.data");
            List<Especie> especies = new List<Especie>();

            string clase;

            for (int i = 0; i < datos.Length; i++)
            {
                string[] valoresString = datos[i].Split(',');
                clase = valoresString[valoresString.Length - 1];

                double[] valores = new double[valoresString.Length - 1];

                for (int k = 0; k < valoresString.Length - 1; k++)
                    valores[k] = double.Parse(valoresString[k]);

                Boolean especieEncontrada = false;
                int posicionEspecie = 0;

                for(int k = 0; k < especies.Count; k++)
                {
                    if(especies[k].clase.Equals(clase))
                    {
                        especieEncontrada = true;
                        posicionEspecie = k;
                    }
                }

                if(especieEncontrada)
                {
                    especies[posicionEspecie].agregarMuestra(valores);
                } else
                {
                    especies.Add(new Especie(clase, valores));
                }

            }

            for(int i = 0; i < especies.Count; i++)
            {
                especies[i].calculoGaussiano();
                Console.WriteLine(especies[i]);
            }

            datos = System.IO.File.ReadAllLines("iris-prueba.data");

            for (int i = 0; i < datos.Length; i++)
            {
                string[] valoresString = datos[i].Split(',');
                string clasePertenece = valoresString[valoresString.Length - 1];

                double[] valores = new double[valoresString.Length - 1];

                for (int k = 0; k < valoresString.Length - 1; k++)
                    valores[k] = double.Parse(valoresString[k]);

                double evidencia = 0;
                double pEspecie = 1 / (double)especies.Count;
                for (int k = 0; k < especies.Count; k++)
                {
                    double pLS = (1 / Math.Sqrt(especies.Count * Math.PI * especies[k].varianzaLargoSepalo)) * ((Math.Pow(valores[0] - especies[k].mediaLargoSepalo,2) * -1) / especies.Count * especies[k].varianzaLargoSepalo);
                    double pAS = (1 / Math.Sqrt(especies.Count * Math.PI * especies[k].varianzaAnchoSepalo)) * ((Math.Pow(valores[1] - especies[k].mediaAnchoSepalo, 2) * -1) / especies.Count * especies[k].varianzaAnchoSepalo);
                    double pLP = (1 / Math.Sqrt(especies.Count * Math.PI * especies[k].varianzaLargoPetalo)) * ((Math.Pow(valores[2] - especies[k].mediaLargoPetalo, 2) * -1) / especies.Count * especies[k].varianzaLargoPetalo);
                    double pAP = (1 / Math.Sqrt(especies.Count * Math.PI * especies[k].varianzaAnchoPetalo)) * ((Math.Pow(valores[3] - especies[k].mediaAnchoPetalo, 2) * -1) / especies.Count * especies[k].varianzaAnchoPetalo);

                    especies[k].anteriorXprobabilidad = pEspecie * pLS * pAS * pLP * pAP;

                    evidencia += especies[k].anteriorXprobabilidad;

                }

                double mayor = 0;
                int posicionMayor = 0;
                for(int k = 0; k < especies.Count; k++)
                {
                    double posteriori = especies[k].anteriorXprobabilidad / evidencia;
                    if (k == 0)
                    {
                        mayor = posteriori;
                        posicionMayor = 0;
                    } else
                    {
                        if(posteriori < mayor)
                        {
                            mayor = posteriori;
                            posicionMayor = k;
                        }
                    }

                }

                Console.WriteLine(datos[i] + " Clase clasificada: " + especies[posicionMayor].clase);

            }

        }
    }

    class Especie
    {
        public string clase;

        public double mediaLargoSepalo;
        public double varianzaLargoSepalo;

        public double mediaAnchoSepalo;
        public double varianzaAnchoSepalo;

        public double mediaLargoPetalo;
        public double varianzaLargoPetalo;

        public double mediaAnchoPetalo;
        public double varianzaAnchoPetalo;

        public double anteriorXprobabilidad;

        List<double> largoSepalo;
        List<double> anchoSepalo;
        List<double> largoPetalo;
        List<double> anchoPetalo;

        public Especie(string clase, double[] datos)
        {
            this.clase = clase;
            mediaLargoSepalo = 0;
            varianzaLargoSepalo = 0;

            mediaAnchoSepalo = 0;
            varianzaAnchoSepalo = 0;

            mediaLargoPetalo = 0;
            varianzaLargoPetalo = 0;

            mediaAnchoPetalo = 0;
            varianzaAnchoPetalo = 0;

            anteriorXprobabilidad = 0;

            largoSepalo = new List<double>();
            anchoSepalo = new List<double>();
            largoPetalo = new List<double>();
            anchoPetalo = new List<double>();

            agregarMuestra(datos);

        }

        public void agregarMuestra(double[] datos)
        {
            largoSepalo.Add(datos[0]);
            anchoSepalo.Add(datos[1]);
            largoPetalo.Add(datos[2]);
            anchoPetalo.Add(datos[3]);
        }

        public void calculoGaussiano()
        {
            mediaLargoSepalo = obtenerMedia(largoSepalo);
            varianzaLargoSepalo = obtenerVarianza(largoSepalo, mediaLargoSepalo);

            mediaAnchoSepalo = obtenerMedia(anchoSepalo);
            varianzaAnchoSepalo = obtenerVarianza(anchoSepalo, mediaAnchoSepalo);

            mediaLargoPetalo = obtenerMedia(largoPetalo);
            varianzaLargoPetalo = obtenerVarianza(largoPetalo, mediaLargoPetalo);

            mediaAnchoPetalo = obtenerMedia(anchoPetalo);
            varianzaAnchoPetalo = obtenerVarianza(anchoPetalo, mediaAnchoPetalo);
        }

        public double obtenerMedia(List<double> datos)
        {
            double suma = 0;
            for (int i = 0; i < datos.Count; i++)
            {
                suma += datos[i];
            }
            double media = suma / datos.Count;

            return media;
        }

        public double obtenerVarianza(List<double> datos, double media)
        {
            double suma = 0;

            for(int i = 0; i < datos.Count; i++)
            {
                suma += Math.Pow((datos[i] - media), 2);
            }

            double varianza = suma / (datos.Count - 1);

            return varianza;
        }

        override
        public String ToString()
        {
            /*public double mediaLargoSepalo;
            public double varianzaLargoSepalo;

            public double mediaAnchoSepalo;
            public double varianzaAnchoSepalo;

            public double mediaLargoPetalo;
            public double varianzaLargoPetalo;

            public double mediaAnchoPetalo;
            public double varianzaAnchoPetalo;*/

            String cadena = "[mLS = " + mediaLargoSepalo + ", vLS = " + varianzaLargoSepalo + ", mAS = " + mediaAnchoSepalo + ", vAS = " + varianzaAnchoSepalo + ", mLP = " + mediaLargoPetalo + ", vLP = " + varianzaLargoPetalo + ", mAP = " + mediaAnchoPetalo + ", vAP = " + varianzaAnchoPetalo + "] Clase: " + clase + "\n";

            return cadena;
        }
    }

}
