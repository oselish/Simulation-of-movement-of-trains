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
	/// Логика взаимодействия для ConfirmationOfEditingLocomotive.xaml
	/// </summary>
	public partial class ConfirmationOfEditingLocomotive : Window
	{
		string ID;
		Locomotive locomotive;
		MainWindow window;

		public ConfirmationOfEditingLocomotive(string ID, Locomotive locomotive, MainWindow window)
		{
			this.window = window;
			this.ID = ID;
			this.locomotive = locomotive;
			InitializeComponent();
		}

		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			ListOfLocomotives.Replace(ID, locomotive);
			window.locoDataGrid.ItemsSource = null;
			window.locoDataGrid.ItemsSource = ListOfLocomotives.locomotivesForDataGrid;

			window.StationDataGrid.UpdateStationDataGrid(sender, e);

			window.UpdateComboBoxes();

			Close();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
