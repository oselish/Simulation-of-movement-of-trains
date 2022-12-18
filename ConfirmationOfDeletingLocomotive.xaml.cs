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
	/// Логика взаимодействия для ConfirmationOfDeletingLocomotive.xaml
	/// </summary>
	public partial class ConfirmationOfDeletingLocomotive : Window
	{
		MainWindow window;
		public ConfirmationOfDeletingLocomotive(MainWindow window)
		{
			this.window = window;
			InitializeComponent();
		}
		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			bool successfulRemove = true;
			int countUnsuccessfulRemoves = 0;
			var selectedLocomotives = window.locoDataGrid.SelectedItems;

			foreach (LocoForDataGrid selectedLoco in selectedLocomotives)
			{
				successfulRemove = ListOfLocomotives.Remove(selectedLoco.ID, window.visualizationCanvas);
				if (!successfulRemove)
				{
					MessageBox.Show($"Локомотив с ID: {selectedLoco.ID} не удалось удалить!");
					countUnsuccessfulRemoves++;
				}
			}

			if (ListOfLocomotives.locomotivesForDataGrid.Count == 0)
			{
				window.wagonTabItem.IsEnabled = false;
				window.TrainsTabItem.IsEnabled = window.wagonTabItem.IsEnabled;
				window.scheduleTabItem.IsEnabled = window.wagonTabItem.IsEnabled;

			}
			window.WagonAddButton.IsEnabled = false;

			window.WagonRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;
			window.WagonsManyRandomButton.IsEnabled = window.WagonAddButton.IsEnabled;

			List<string> itemsForComboBox = new List<string>();

			foreach (var loco in ListOfLocomotives.locomotivesForDataGrid)
			{
				string firstStation = loco.FirstStation;
				string lastStation = loco.LastStation;
				string description = $"№{loco.RouteNum} ({firstStation} - {lastStation}) ID: {loco.ID}";
				itemsForComboBox.Add(description);
			}

			window.locoDataGrid.ItemsSource = null;
			window.locoDataGrid.ItemsSource = ListOfLocomotives.locomotivesForDataGrid;

			window.locoComboBox.ItemsSource = itemsForComboBox;
			window.trainComboBox.ItemsSource = itemsForComboBox;
			window.scheduleDataGrid.ItemsSource = null;

			window.StationDataGrid.UpdateStationDataGrid(sender, e);
			window.TrainsDataGrid.UpdateTrainsDataGrid(sender,e);



			MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);
			Close();
			if (countUnsuccessfulRemoves == 0)
				MessageBox.Show("Все выделенные локомотивы успешно удалены!");
			else
				MessageBox.Show("Не все выделенные локомотивы удалены!");

		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
