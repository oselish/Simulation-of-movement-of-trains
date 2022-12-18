using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Stations;

namespace MovementOfElectricTrains
{
    /// <summary>
    /// Тягач состава
    /// </summary>
    public class Locomotive : Wagon, IComparable
    {
        /// <summary>
        /// Типы электропоездов
        /// </summary>
        private static string directCurrent = "Постоянного тока";
        private static string alternatingCurrent = "Переменного тока";


        /// <summary>
        /// Макс скорость локомотива
        /// </summary>
        private short maxSpeed;
		public short MaxSpeed { get; set; }

		/// <summary>
		/// Текущвая скорость локомотива
		/// </summary>
		private short speed = 0;
		public short Speed { get; set; }

		/// <summary>
		/// Максимальное кол-во вагонов
		/// </summary>
		private short maxWagonsAmount;
		public short MaxWagonsAmount { get; set; }

		/// <summary>
		/// Тип двигателя
		/// Паровоз, электровоз, тепловоз
		/// </summary>
		private string engineType;
		public string EngineType { get; set; }

		/// <summary>
		/// Время запаздывания в минутах
		/// </summary>
		private int lateTime = 0;
		public int LateTime { get; set; }

		/// <summary>
		/// Номер маршрута
		/// </summary>
		private short routeNum;
		public short RouteNum { get; set; }

		/// <summary>
		/// Текущая станция
		/// </summary>
		private Station currentStation;
		public Station CurrentStation { get; set; }

		/// <summary>
		/// Первая станция
		/// </summary>
		private Station firstStation;
        public Station FirstStation { get; set; }

		
        /// <summary>
		/// Конечная станция
		/// </summary>
		private Station lastStation;
		public Station LastStation { get; set; }

        /// <summary>
        /// Тип локомотива - шанс добавления в состав
        /// </summary>
        public static Dictionary<int, string> locomotiveTypes = new Dictionary<int, string>()
        {
            //Текущ. ключ = пред. ключ - пред. шанс
            {  40, alternatingCurrent }, //Шанс добавления - 40%
            { 100, directCurrent      }  //Шанс добавления - 60%
        };

        /// <summary>
        /// Происшествие на путях, в поезде и т.п.
        /// </summary>
        private string incident;
		public string Incident { get; set; }

		/// <summary>
		/// Копирование локомотива
		/// </summary>
		/// <param Name="copyingLocomotive"></param>
		public Locomotive DeepCopy()
        {
            Locomotive copyingLocomotive = new Locomotive();
            copyingLocomotive.MaxSpeed        = MaxSpeed;
            copyingLocomotive.MaxWagonsAmount = MaxWagonsAmount;
            copyingLocomotive.EngineType      = EngineType;
            copyingLocomotive.RouteNum        = RouteNum;
            copyingLocomotive.FirstStation    = FirstStation;
            copyingLocomotive.CurrentStation  = CurrentStation;
            copyingLocomotive.LastStation     = LastStation;
            copyingLocomotive.Incident        = Incident;
            copyingLocomotive.SerialNum       = SerialNum;
            return copyingLocomotive;
        }

        /// <summary>
        /// Доступные типы локомотивов:
        /// Постоянного тока, переменного тока
        /// </summary>
        /// <param Name="EngineType"></param>
        public Locomotive(string EngineType, short SerialNum = 0, string Name = "Локомотив") : base(SerialNum, Name)
		{
            this.EngineType = EngineType;
            if (EngineType == directCurrent)
            {
                MaxSpeed = 150;
                MaxWagonsAmount = 48;
            }
            else if (EngineType == alternatingCurrent)
            {
                MaxSpeed = 200;
                MaxWagonsAmount = 115;
            }
            else
            {
                Console.WriteLine("Неверный тип локомотива!");
            }
        }


        public Locomotive(short MaxSpeed, short RouteNum, short SerialNum, short MaxWagonsAmount, string EngineType, Station FirstStation,
            Station CurrentStation, Station LastStation, string Incident, string Name) : base(SerialNum, Name)
        {
            this.MaxSpeed = MaxSpeed;
            this.RouteNum = RouteNum;
            this.MaxWagonsAmount = MaxWagonsAmount;
            this.EngineType = EngineType;
            this.FirstStation = FirstStation;
            this.CurrentStation = CurrentStation;
            this.LastStation = LastStation;
            this.Incident = Incident;
        }


        public Locomotive(short SerialNum = 0, string Name = "Локомотив") : base(SerialNum, Name)
        {

        }

        /// <summary>
        /// Сравнение локомотивов
        /// </summary>
        /// <param Name="firstLocomotive"></param>
        /// <param Name="secondLocomotive"></param>
        /// <returns></returns>
        public static bool Equals(Locomotive firstLocomotive, Locomotive secondLocomotive)
		{
            bool stationCompare         = firstLocomotive.CurrentStation  == secondLocomotive.CurrentStation;
            bool maxSpeedCompare        = firstLocomotive.MaxSpeed        == secondLocomotive.MaxSpeed;
            bool speedCompare           = firstLocomotive.Speed           == secondLocomotive.Speed;
            bool maxWagonsAmountCompare = firstLocomotive.MaxWagonsAmount == secondLocomotive.MaxWagonsAmount;
            bool engineTypeCompare      = firstLocomotive.EngineType      == secondLocomotive.EngineType;
            bool lateTimeCompare        = firstLocomotive.LateTime        == secondLocomotive.LateTime;
            bool routeNumCompare        = firstLocomotive.RouteNum        == secondLocomotive.RouteNum;
            bool currentStationCompare  = firstLocomotive.CurrentStation  == secondLocomotive.CurrentStation;
            bool firstStationCompare    = firstLocomotive.FirstStation    == secondLocomotive.FirstStation;
            bool lastStationCompare     = firstLocomotive.LastStation     == secondLocomotive.LastStation;
            bool incidentCompare        = firstLocomotive.Incident        == secondLocomotive.Incident;

            return stationCompare && maxSpeedCompare && speedCompare && maxWagonsAmountCompare && engineTypeCompare &&
                   lateTimeCompare && routeNumCompare && currentStationCompare && firstStationCompare && lastStationCompare && incidentCompare;
		}

        /// <summary>
        /// Переопределение оператора ==
        /// </summary>
        /// <param Name="firstLocomotive"></param>
        /// <param Name="secondLocomotive"></param>
        /// <returns></returns>
        public static bool operator == (Locomotive firstLocomotive, Locomotive secondLocomotive)
        {
            return Equals(firstLocomotive, secondLocomotive);
        }

        /// <summary>
        /// Переопределение оператора !=
        /// </summary>
        /// <param Name="firstLocomotive"></param>
        /// <param Name="secondLocomotive"></param>
        /// <returns></returns>
        public static bool operator != (Locomotive firstLocomotive, Locomotive secondLocomotive)
        {
            return !Equals(firstLocomotive, secondLocomotive);
        }

        /// <summary>
        /// Получение информации о локомотиве
        /// </summary>
        /// <returns></returns>
        public override string GetInfo()
		{
            string info = "";
            info += base.GetInfo();
            info += $"Маршрут №{RouteNum}\n";
            info += (FirstStation == null) ?   $"Начальная станция: < не указано >\n" : $"Начальная станция: {FirstStation}\n";
            info += (CurrentStation == null) ? $"Текущая станция: < не указано >\n"   : $"Текущая станция: {CurrentStation}\n";
            info += (LastStation == null) ?    $"Конечная станция: < не указано >\n"  : $"Конечная станция: {LastStation}\n";
            info += $"Тип локомотива: {EngineType}\n";
            info += (LateTime == 0) ? "" : $"Время запаздывания: {LateTime}\n";
            info += $"Макс. кол-во вагонов: {MaxWagonsAmount}\nМакс. скорость: {MaxSpeed} км/ч\nТекущая скорость: {Speed}\n\n";
            return info;
		}

		public int CompareTo(object obj)
		{
			if (obj == null) return 1;

            Locomotive loco = obj as Locomotive;

            if (loco != null)
                return this.CompareTo(loco);
            else
                throw new ArgumentException("Object is not a Locomotive");
		}
	}
}