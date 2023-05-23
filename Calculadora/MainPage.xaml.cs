using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Calculadora
{
    public partial class MainPage : ContentPage
    {
        private int numeroActual = 1;
        private double valor1 = 0;
        private bool poderOperar = true;
        private bool negativo = true;
        private List<double> proximosValores = new List<double>();
        private List<char> operacoes = new List<char>();
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void NumeroClicao(object sender, EventArgs e) 
        {
            Button numero = (Button)sender;
            if (Resultado.Text == "0") Resultado.Text = "";
            if(numeroActual < 0) numeroActual *= -1;

            Resultado.Text += numero.Text;
            Resultado.TextColor = Color.Black;
            if(numeroActual==1)
            { 
                if(double.TryParse(Resultado.Text,out double valor))
                {
                    Resultado.Text = valor.ToString();
                    valor1 = valor;
                }
            }
            else
            {
                int valores = valor1.ToString().Length+1;
                if(proximosValores.Count > 1)
                {
                    for(int i = 0;i < proximosValores.Count - 1; i++)
                    {
                        valores += proximosValores[i].ToString().Length + 1;
                    }
                }
                string proximoValor = Resultado.Text.Remove(0,valores);
                if(double.TryParse(proximoValor,out double valor))
                {
                    if(operacoes.Count == proximosValores.Count)
                    {
                        proximosValores[proximosValores.Count-1] = valor;
                        poderOperar = true;
                    }
                }
            }
        }

        private void OperadorClicao(object sender, EventArgs e)
        {
            Button operador = (Button)sender;
            if(valor1 !=0 && poderOperar || operador.Text.Equals("-") && negativo)
            {
                if(valor1 == 0 || Resultado.Text == "")
                {
                    Resultado.Text= "-";
                    Resultado.TextColor = Color.Black;
                    return;
                }
                if (proximosValores.Count > 0 && negativo && operador.Text.Equals("-") && proximosValores[proximosValores.Count-1]==0)
                {
                    numeroActual = -2;
                    Resultado.Text += operador.Text;
                    poderOperar = false;
                    negativo = false;
                    return;
                }
                else 
                {
                    negativo= true;
                    proximosValores.Add(0);
                    numeroActual = -2;
                    operacoes.Add(operador.Text[0]);
                    Resultado.Text += operador.Text;
                    poderOperar= false;
                }

            }

        }

        private void Calcular(object sender, EventArgs e)
        {
            if(numeroActual == 2) 
            { 
                double resultado = 0;
                int i = 0;
                foreach (char operador in operacoes)
                {
                    if (operador == 'x')
                    {
                        resultado = valor1 * proximosValores[i];
                    }
                    else if (operador == '/')
                    {
                        resultado = valor1 / proximosValores[i];
                    }
                    else if (operador == '-')
                    {
                        resultado = valor1 - proximosValores[i];
                    }
                    else if (operador == '+')
                    {
                        resultado = valor1 + proximosValores[i];
                    }

                    valor1 = resultado;

                    if (proximosValores.Count > 0 && i < proximosValores.Count - 1) i++;

                }
                if (resultado == 0) numeroActual = 1;

                operacoes.Clear();
                proximosValores.Clear();
                Resultado.Text = resultado.ToString();
            }

        }
        private void Clear(object sender, EventArgs e)
        {
            Resultado.Text = "";
            numeroActual = 1;
            valor1 = 0;
            negativo = true;
            poderOperar = true;
            proximosValores.Clear();
            operacoes.Clear();

        }


    }
}
