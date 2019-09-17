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
            
            double tiempoInicial = double.Parse(txtbox_tiempoInicial.Text);
            double tiempoFinal = double.Parse(txtbox_tiempoFinal.Text);
            double muestro = double.Parse(txtbox_muestreo.Text);

            Señal señal;

            switch(CbTipoSeñal.SelectedIndex)
            {
                case 0: //Parabolica
                    señal = new SeñalParabolica();
                    break;
                case 1: //Senoidal
                    double amplitud =
                        double.Parse(
                    ((ConfiguracionSeñalSenoidal)(PanelConfiguracion.Children[0])).txtbox_amplitud.Text
                    );

                    double fase =
                        double.Parse(
                    ((ConfiguracionSeñalSenoidal)(PanelConfiguracion.Children[0])).txtbox_fase.Text
                    );

                    double frecuencia =
                        double.Parse(
                    ((ConfiguracionSeñalSenoidal)(PanelConfiguracion.Children[0])).txtbox_frecuencia.Text
                    );

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                case 2: //Exponencial
                    double alpha = 
                        double.Parse(
                            ((ConfiguracionSeñalExponencial)(PanelConfiguracion.Children[0])).txt_alpha.Text
                    );
                    señal = new SeñalExponencial(alpha);
                    break;
                case 3: //Audio
                    string rutaArchivo = ((ConfiguracionSeñalAudio)(PanelConfiguracion.Children[0])).txtRutaArchivo.Text;
                    señal = new SeñalAudio(rutaArchivo);
                    txtbox_tiempoInicial.Text = señal.TiempoInicial.ToString();
                    txtbox_tiempoFinal.Text = señal.TiempoFinal.ToString();
                    txtbox_muestreo.Text = señal.FrecuenciaMuestreo.ToString();
                    break;
                default:
                    señal = null;
                    break;
            }

            if(CbTipoSeñal.SelectedIndex != 3)
            {
                señal.TiempoInicial = tiempoInicial;
                señal.TiempoFinal = tiempoFinal;
                señal.FrecuenciaMuestreo = muestro;

                señal.construirSeñal();
            }

            double amplitudMaxima = señal.AmplitudMaxima;

            plnGrafica.Points.Clear();

            foreach(Muestra muestra in señal.Muestras)
            {
                plnGrafica.Points.Add(adaptarCoordenadas(muestra.X, muestra.Y, tiempoInicial, amplitudMaxima));
            }

            lblLimiteSuperior.Text = amplitudMaxima.ToString();
            lblLimiteInferior.Text = "-" + amplitudMaxima.ToString();

            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoInicial,0.0,tiempoInicial, amplitudMaxima));
            plnEjeX.Points.Add(adaptarCoordenadas(tiempoFinal, 0.0,tiempoInicial, amplitudMaxima));

            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, amplitudMaxima, tiempoInicial, amplitudMaxima));
            plnEjeY.Points.Add(adaptarCoordenadas(0.0, -amplitudMaxima, tiempoInicial, amplitudMaxima));
        }

        public Point adaptarCoordenadas(double x, double y, double tiempoInicial, double amplitudMaxima)
        {
            return new Point((x - tiempoInicial) * scrGrafica.Width,
                (-1 * (
                y * (((scrGrafica.Height / 2.0) -25) / amplitudMaxima)) +
                (scrGrafica.Height / 2.0) ));
        }

        private void CbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PanelConfiguracion.Children.Clear();
            switch(CbTipoSeñal.SelectedIndex)
            {
                case 0: //Exponencial
                    break;
                case 1: //Senoidal
                    PanelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                case 2: //Exponencial
                    PanelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;
                case 3: //Audio
                    PanelConfiguracion.Children.Add(new ConfiguracionSeñalAudio());
                    break;
                default:
                    break;
            }
        }
    }
}
