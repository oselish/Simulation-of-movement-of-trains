using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6OOP
{
	/// <summary>
	/// Вагон
	/// </summary>
	delegate void Message();
	public class Wagon : IInformation, IInformation1
	{
		public const int minIDLength = 10;
		public const int maxIDLength = 10;
		public const string charsForID = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

		/// <summary>
		/// Порядковый номер вагона
		/// </summary>
		private int serialNum;
		public int SerialNum { get; set; }

		/// <summary>
		/// Тип вагона
		/// </summary>
		private string name;
		public string Name { get; set; }

		/// <summary>
		/// Отображение информации о вагоне
		/// </summary>
		/// <returns></returns>
		public Wagon(int SerialNum, string Name)
		{
			this.Name = Name;
			this.SerialNum = SerialNum;
		}
		
		public Wagon()
		{
			Console.WriteLine("_____________________________________");
			Message message;
			message = ShowInfo1;
			message += ShowInfo2;
			message();
		}

		string IInformation.GetInfo()
		{
			string info = "";
			info += $"------------ Первый интерфейс ------------\nПорядковый номер вагона в поезде: {SerialNum}\nТип вагона: {Name}\n";
			return info;
		}
		string IInformation1.GetInfo()
		{
			string info = "";
			info += $"------------ Второй интерфейс ------------\nТип вагона: {Name}\nПорядковый номер вагона в поезде: {SerialNum}\n";
			return info;
		}


		public void ShowInfo1()
		{
			Console.WriteLine((this as IInformation).GetInfo());
		}
		public void ShowInfo2()
		{
			Console.WriteLine((this as IInformation1).GetInfo());
		}
		public virtual string GetInfo()
		{
			string info = "";
			info += $"Порядковый номер вагона в поезде: {SerialNum}\nТип вагона: {Name}\n";
			return info;
		}

		public Wagon DeepCopy()
		{
			Wagon copy = new Wagon();
			copy.serialNum = SerialNum;
			copy.Name      = Name;
			return copy;
		}
	}
}
