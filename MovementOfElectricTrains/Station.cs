using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Trains;

namespace MovementOfElectricTrains
{
    /// <summary>
    /// Ж/Д станция
    /// </summary>
    public class Station
    {
        /// <summary>
        /// Порядковый номер станции
        /// </summary>
        private byte serialNum;
		public byte SerialNum { get; set; }

		/// <summary>
		/// Поезд на станции
		/// </summary>
		private Locomotive[] stoppedLocomotives = null;
		public Locomotive[] StoppedLocomotives { get; set; }

		/// <summary>
		/// Название станции
		/// </summary>
		private string name;
		public string Name { get; set; }

		/// <summary>
		/// Генерация маршрута
		/// </summary>
		/// <param Name="stations"></param>
		/// <param Name="namesOfStations"></param>
		public static void GenerateRoute(Station[] stations, string[] namesOfStations)
        {
            for (byte i = 0; i < stations.Length; i++)
                stations[i] = new Station(namesOfStations[i], i);
        }

        /// <summary>
        /// Отображение маршрута
        /// </summary>
        /// <param Name="stations"></param>
        public static void ShowRoute(Station[] stations)
        {
            for (int i = 0; i < stations.Length; i++)
                Console.WriteLine($"{stations[i].SerialNum + 1}. {stations[i].Name}");
        }
        public Station(string Name, byte SerialNum)
        {
            this.Name = Name;
            this.SerialNum = SerialNum;
        }

        /// <summary>
        /// Получение информации о станции
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
		{
            string info = "";
            string stoppedLocomotivesNums = ""; //номера маршрутов остановленныпоездов на станции
            int lastNum = StoppedLocomotives.Length - 1;
            for (int i = 0; i < lastNum; i++)
			{
                stoppedLocomotivesNums += StoppedLocomotives[i].SerialNum.ToString() + ", ";
			}
            stoppedLocomotivesNums += StoppedLocomotives[lastNum].SerialNum.ToString();
            info += $"Название станции: {Name}\nПорядковый номер станции: {SerialNum}\nПоезда на станции: {stoppedLocomotivesNums}\n";
            return info;
		}
    }
}
