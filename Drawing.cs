using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab6OOP
{
	public class Point
	{
		public double X;
		public double Y;
		public Point(double x, double y) { X = x; Y = y; }
		public void SetPoint(double x, double y) { X = x; Y = y; }
		public Point GetPoint() { return new Point(X, Y); }

	}

	public class CustomColor
	{

		public static SolidColorBrush lineColor = Brushes.Gray;
		public static SolidColorBrush textColor = Brushes.DarkBlue;
		public static SolidColorBrush whiteColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
		public static List<SolidColorBrush> occupiedColors = new List<SolidColorBrush>();

		public static SolidColorBrush RandomColor()
		{
			Random random = new Random();
		generateColor:
			byte R = (byte)random.Next(192);
			byte G = (byte)random.Next(192);
			byte B = (byte)random.Next(192);

			SolidColorBrush result = new SolidColorBrush(Color.FromRgb(R, G, B));
			foreach (var color in occupiedColors)
				if (result == color)
					goto generateColor;
			occupiedColors.Add(result);
			return result;
		}
	}

}

