using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void BtnGraficar_Click(object sender, RoutedEventArgs e)
        {
            double amplitud = double.Parse(txtbox_amplitud.Text);
            double fase = double.Parse(txtbox_fase.Text);
            double frecuencia = double.Parse(txtbox_frecuencia.Text);
            double tiempoInicial = double.Parse(txtbox_tiempoInicial.Text);
            double tiempoFinal = double.Parse(txtbox_tiempoFinal.Text);
            double muestro = double.Parse(txtbox_muestreo.Text);

            SeñalSenoidal señal = new SeñalSenoidal(amplitud, fase, frecuencia);

            double periodoMuestreo = 1.0 / muestro;

            plnGrafica.Points.Clear();
            for( double i = tiempoInicial; i <= tiempoFinal; i += periodoMuestreo)
            {
                plnGrafica.Points.Add(
                    adaptarCoordenadas(i,señal.evaluar(i)));
            }
        }

        public Point adaptarCoordenadas(double x, double y)
        {
            return new Point(x * scrGrafica.Width,
                (-1 * (
                y * ((scrGrafica.Height / 2.0) -25 )) +
                (scrGrafica.Height / 2.0) ));
        }
    }
}
