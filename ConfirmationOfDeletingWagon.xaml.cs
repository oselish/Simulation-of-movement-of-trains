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

 

namespace lab6OOP
{
	/// <summary>
	/// Логика взаимодействия для ConfirmationOfDeletingWagon.xaml
	/// </summary>
	public partial class ConfirmationOfDeletingWagon : Window
	{
		MainWindow window;
		Locomotive locomotive;

		public ConfirmationOfDeletingWagon(MainWindow window)
		{
			locomotive = MainWindow.FindLocomotive(window.locoComboBox.SelectedItem);
			this.window = window;
			InitializeComponent();
		}

		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			bool successfulRemove = true;
			int countUnsuccessfulRemoves = 0;

			var selectedWagons = window.wagonDataGrid.SelectedItems;
			foreach (WagonForDataGrid selectedWagon in selectedWagons)
			{
				successfulRemove = ListOfWagons.Remove(selectedWagon.ID, locomotive);
				if (!successfulRemove)
				{
					MessageBox.Show($"{selectedWagon.Type} с ID: {selectedWagon.ID} не удалось удалить!");
					countUnsuccessfulRemoves++;
				}
			}
			ListOfWagons.FixUpIndexes(locomotive);

			window.wagonDataGrid.ItemsSource = null;
			window.wagonDataGrid.ItemsSource = ListOfWagons.wagonsForDataGrid;

			window.TrainsDataGrid.UpdateTrainsDataGrid(sender,e);
			MainWindow.Visualize(window.visualizationCanvas, window.DateTimeLabel, MainWindow.TIME);
			Close();
			if (countUnsuccessfulRemoves == 0)
				MessageBox.Show("Все выделенные вагоны успешно удалены!");
			else
				MessageBox.Show("Не все выделенные вагоны удалены!");
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
