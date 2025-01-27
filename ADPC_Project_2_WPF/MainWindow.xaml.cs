using ADPC_Project_2_WPF.Models;
using ADPC_Project_2_WPF.Services;
using LiveCharts.Wpf;
using LiveCharts;
using MongoDB.Driver;
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

namespace ADPC_Project_2_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MongoService _mongoService;
        private List<GeneExpressionWithClinical> _geneExpressions;
        private Dictionary<string, List<string>> _cancerToPatientsMap;

        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var actionChoiceWindow = new ActionChoiceWindow();
            actionChoiceWindow.ShowDialog();

            _mongoService = new MongoService();

            _cancerToPatientsMap = new Dictionary<string, List<string>>();

            SeriesCollection = new SeriesCollection();
            Labels = new List<string>();

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                _geneExpressions = await _mongoService.GetGeneExpressionsAsync();

                // Обновление данных на UI потоке
                Dispatcher.Invoke(() =>
                {
                    _cancerToPatientsMap = _geneExpressions
                        .GroupBy(g => g.CancerCohort)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.PatientId).Distinct().ToList()
                        );

                    CancerTypeDropdown.ItemsSource = _cancerToPatientsMap.Keys;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void CancerTypeDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PatientDropdown.ItemsSource = null;

            var selectedCancerType = CancerTypeDropdown.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedCancerType))
                return;

            if (_cancerToPatientsMap.ContainsKey(selectedCancerType))
            {
                PatientDropdown.ItemsSource = _cancerToPatientsMap[selectedCancerType];
            }
        }

        private void PatientDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPatient = PatientDropdown.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedPatient))
                return;

            UpdateChart(selectedPatient);
        }

        private void UpdateChart(string patientId)
        {
            SeriesCollection.Clear();
            Labels.Clear();

            var patientData = _geneExpressions.FirstOrDefault(g => g.PatientId == patientId);

            if (patientData == null)
            {
                MessageBox.Show("No data available for the selected patient.");
                return;
            }

            Labels = patientData.GeneValues.Keys.ToList();
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Expression Level",
                Values = new ChartValues<double>(patientData.GeneValues.Values)
            });

            chart.AxisX[0].Labels = Labels;
            chart.Series = SeriesCollection;
        }
    }
}
