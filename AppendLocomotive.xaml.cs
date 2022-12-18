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
using System.Windows.Shapes;

 

namespace lab6OOP
{
	/// <summary>
	/// Логика взаимодействия для AppendLocomotive.xaml
	/// </summary>
	public partial class AppendLocomotive : Window
	{
		MainWindow window;
		public AppendLocomotive(MainWindow window)
		{
			InitializeComponent();
			this.window = window;
			//Добавление значений в комбобокс "номер маршрута"
			for (int i = 1; i <= Station.routesAmount; i++)
			{
				var tempRoute = Station.GetRoute(i);
				string routeNum = $"№{i} ({tempRoute[0].Name} - {tempRoute[tempRoute.Length - 1].Name})";
				this.routeNumComboBox.Items.Add(routeNum);
			}
			//Добавление значений в комбобокс "тип электропоезда"
			for (int i = 0; i < Locomotive.types.Length; i++)
			{
				locoEngineTypeComboBox.Items.Add(Locomotive.types[i]);
			}
		}
		//Преобразование описания маршрута в номер маршрута
		private int RouteDescriptionToInt(string description)
		{
			string result = "";
			foreach (var i in description)
			{
				if (i - '0' >= 0 && i - '0' <= 9)
					result += i;
			}
			return Convert.ToInt32(result);
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			if (routeNumComboBox.SelectedItem == null && locoEngineTypeComboBox.SelectedItem == null && locoIDTextBox.Text == "")
				MessageBox.Show("Не выбрано ни одно свойство!");
			else if (routeNumComboBox.SelectedItem == null)
				MessageBox.Show("Выберите номер маршрута");
			else if (locoEngineTypeComboBox.SelectedItem == null)
				MessageBox.Show("Выберите тип электропоезда");
			else if (locoIDTextBox.Text == "")
				MessageBox.Show("Введите ID локомотива");
			else
			{
				string ID = locoIDTextBox.Text;

				if (ListOfLocomotives.IDisUnique(ID))
				{
					var engineType = locoEngineTypeComboBox.SelectedItem.ToString();
					int routeNum = RouteDescriptionToInt(routeNumComboBox.SelectedItem.ToString());

					ListOfLocomotives.Insert(new Locomotive(ID, engineType, routeNum, window.visualizationCanvas));
					
					window.locoDataGrid.ItemsSource = null;
					window.locoDataGrid.ItemsSource = ListOfLocomotives.locomotivesForDataGrid;

					window.UpdateComboBoxes();

					window.wagonTabItem.IsEnabled = true;
					window.TrainsTabItem.IsEnabled = window.wagonTabItem.IsEnabled;
					window.scheduleTabItem.IsEnabled = window.wagonTabItem.IsEnabled;
					window.WagonAddButton.IsEnabled = false;
					window.WagonRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;
					window.WagonsManyRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;


					window.StationDataGrid.UpdateStationDataGrid(sender, e);
					MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);
					Close();
				}
				else
				{
					MessageBox.Show("Локомотив с таким уникальным номером уже существует");
				}
			}
		}
	}
}
