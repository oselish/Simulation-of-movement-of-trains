using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementOfElectricTrains
{
	/// <summary>
	/// Пассажирский вагон
	/// </summary>
	public class Passenger : Wagon
	{
		/// <summary>
		/// Вместимость вагона
		/// </summary>
		private short capacity;
		public short Capacity { get; set; }

		/// <summary>
		/// Класс пассажирского вагона (плацкарт, купе, СВ и т.п.)
		/// </summary>
		private string type;
		public string Type { get; set; }

		/// <summary>
		/// Поезд к которому прицеплен вагон
		/// </summary>
		private Locomotive loco;
		public Locomotive Loco { get; set; }

		/// <summary>
		/// Получение информации о пассажирском вагоне
		/// </summary>
		/// <returns></returns>

		public Passenger(short Capacity, string Type, Locomotive Loco, short SerialNum, string Name) : base(SerialNum, Name)
		{
			this.Capacity = Capacity;
			this.Type = Type;
			this.loco = Loco;
		}
		public override string GetInfo()
		{
			string info = "";
			info += base.GetInfo();
			info += $"Класс вагона: {Type}\nВместимость вагона: {Capacity}\n";
			return info;
		}
	}
}
