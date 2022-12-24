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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace lab6OOP
{
    public enum Engines
	{
        directCurrent, alternatingCurrent
	}

    [Serializable]
    public class Locomotive : Wagon, IComparable
    {
        //Типы электропоездов
        public static string[] types = { "Постоянного тока", "Переменного тока" };


        // Маршрут
        public Station[] Route;
		
        // Макс скорость локомотива
		private double maxSpeed;
		public double MaxSpeed { get; set; }
		
        // Текущвая скорость локомотива
		private double speed = 0;
        public double Speed { get; set; } = 0;
		
        // Максимальное кол-во вагонов
		private int maxWagonsAmount;
		public int MaxWagonsAmount { get; set; }
		
        // Тип двигателя
		private string engineType;
		public string EngineType { get; set; }

		// Время запаздывания в минутах
		private int lateTime = 0;
        public int LateTime { get; set; } = 0;
		
        // Номер маршрута
		private int routeNum;
		public int RouteNum { get; set; }

		// Текущая станция
		private Station currentStation;
		public Station CurrentStation { get; set; }
		
        // Первая станция
		private Station firstStation;
        public Station FirstStation { get; set; }

        // Конечная станция
        private Station lastStation;
		public Station LastStation { get; set; }

        // Следующая станция
        private Station nextStation;
        public Station NextStation { get; set; }

        // Тип локомотива - шанс добавления в состав
        public static Dictionary<int, Engines> locomotiveTypes = new Dictionary<int, Engines>()
        {
			//Текущ. ключ = пред. ключ - пред. шанс
            {  40, Engines.alternatingCurrent }, //Шанс добавления - 40%
            { 100, Engines.directCurrent      }  //Шанс добавления - 60%
        };

        // Происшествие на путях, в поезде и т.п.
        private string incident;
		public string Incident
        {
			get
			{
                if (incident == null)
                    return "Без происшествий";
                else return incident;
			}
            set
            {
                incident = value;
            }
        }

        public DateTime timeOfStart { get; set; } = MainWindow.TIME;

        // Получение характеристик по типу двигателя
        private void SetStats(string EngineType)
		{
            if (EngineType == types[(int)Engines.directCurrent])
            {
                MaxSpeed = 75;
                MaxWagonsAmount = 48;
            }
            else if (EngineType == types[(int)Engines.alternatingCurrent])
            {
                MaxSpeed = 100;
                MaxWagonsAmount = 115;
            }
            else
            {
                throw new Exception("Неверный тип локомотива!");
            }
        }

        public double TimeOfStop { get; set; }


        // ID
        public string ID { get; set; }

        public double x { get; set; }
        public double y { get; set; }

        [NonSerialized]
        [XmlIgnore]
        public Ellipse point;

        public static int pointRadius = 3;
        public static int borderThickness = 2;

		[NonSerialized]
		[XmlIgnore]
		public SolidColorBrush pointColor;

        public bool OnTheWay { get; set; } = false;
		
		// Копирование локомотива
		public Locomotive DeepCopy()
        {
            Locomotive copyingLocomotive = new Locomotive();
            copyingLocomotive.MaxSpeed        = MaxSpeed;
            copyingLocomotive.MaxWagonsAmount = MaxWagonsAmount;
            copyingLocomotive.CurrentStation  = CurrentStation;
            copyingLocomotive.NextStation     = NextStation;
            copyingLocomotive.EngineType      = EngineType;
            copyingLocomotive.RouteNum        = RouteNum;
            copyingLocomotive.Incident        = Incident;
            copyingLocomotive.SerialNum       = SerialNum;
            return copyingLocomotive;
        }

        public List<Passenger> wagons = new List<Passenger>();//пассажирские вагоны поезда

        // Доступные типы локомотивов:
        // Постоянного тока, переменного тока
        public Locomotive(string EngineType, int SerialNum = 0, string Name = "Локомотив") : base(SerialNum, Name)
		{
            this.EngineType = EngineType;
            if (EngineType == types[(int)Engines.directCurrent])
            {
                MaxSpeed = 150;
                MaxWagonsAmount = 48;
            }
            else if (EngineType == types[(int)Engines.alternatingCurrent])
            {
                MaxSpeed = 200;
                MaxWagonsAmount = 115;
            }
            else
            {
                Console.WriteLine("Неверный тип локомотива!");
            }
        }


        public Locomotive(int MaxSpeed, int RouteNum, int SerialNum, int MaxWagonsAmount, string EngineType, string Incident, string Name) : base(SerialNum, Name)
        {
            this.MaxSpeed = MaxSpeed;
            this.RouteNum = RouteNum;
            this.MaxWagonsAmount = MaxWagonsAmount;
            this.EngineType = EngineType;
            this.Incident = Incident;
        }

        public void CreatePoint(Canvas visualizationCanvas, SolidColorBrush color)
		{
            pointColor = color;
            point = new Ellipse();
            point.Width = pointRadius * 2 + borderThickness * 2;
            point.Height = point.Width;
            point.Fill = pointColor;
            point.Stroke = CustomColor.whiteColor;
            point.StrokeThickness = borderThickness;
            point.Margin = new Thickness(x - pointRadius - borderThickness, y - pointRadius - borderThickness, 0, 0);
            visualizationCanvas.Children.Add(point);
            SetPosition(FirstStation.x, FirstStation.y);

        }
        public Locomotive(string ID, string EngineType, int RouteNum, Canvas visualizationCanvas, SolidColorBrush color)
        {
            this.EngineType = EngineType;
            SetStats(EngineType);
            this.RouteNum = RouteNum;
            this.ID = ID;
            this.Route = Station.GetRoute(RouteNum);
            this.FirstStation = Route[0];
            this.CurrentStation = this.FirstStation;
            this.LastStation = Route[Route.Length - 1];
            this.SerialNum = 0;
            this.Speed = 0;
            this.LateTime = 0;
            this.pointColor = CustomColor.RandomColor();
            CreatePoint(visualizationCanvas, color);
        }

        public void SetPosition(double x, double y)
		{
            this.x = x;
            this.y = y;
            point.Margin = new Thickness(x - pointRadius - borderThickness, y - pointRadius - borderThickness, 0, 0);
        }

        public Station GetNextStation()
		{
            for (int i = 0; i < Route.Length; i++)
			{
                if (Route[i].Name == CurrentStation.Name)
                {
                    if (i+1 < Route.Length)
                        return Route[i+1];
				}
			}
            return null;//конечная
		}

        Locomotive() { }
        ~Locomotive() { }


        // Получение информации о локомотиве
        public override string GetInfo()
		{
            string info = "";
            info += base.GetInfo();
            info += $"Маршрут №{RouteNum}\n";
            info += (Route[0] == null) ?   $"Начальная станция: < не указано >\n" : $"Начальная станция: {Route[0]}\n";
            info += (CurrentStation == null) ? $"Текущая станция: < не указано >\n"   : $"Текущая станция: {CurrentStation}\n";
            info += (Route[Route.Length - 1] == null) ?    $"Конечная станция: < не указано >\n"  : $"Конечная станция: {Route[Route.Length - 1]}\n";
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

        private static Engines ChooseRandomEngineType(Dictionary<int, Engines> dict, int chance)
        {
            Engines choosenType = 0;
            foreach (var Type in dict)
            {
                if (chance <= Type.Key)
                {
                    choosenType = Type.Value;
                    break;
                }
            }
            return choosenType;
        }

        private static string GenerateID()
        {
            Random random = new Random();
            string ID;
        generateID:
            ID = "";

            for (int i = 0; i < random.Next(minIDLength, maxIDLength); i++)
            {
                ID += charsForID[random.Next(0, charsForID.Length - 1)];
            }

            if (!ListOfLocomotives.IDisUnique(ID))
                goto generateID;
            return ID;
        }

        public static Locomotive CreateRandom(Canvas visualizationCanvas)
		{
            Random random = new Random();
            int typeChance = random.Next(1, 100);
            string ID = GenerateID();

            string engineType = types[(int)ChooseRandomEngineType(locomotiveTypes, typeChance)]; //выбор случайного типа локомотива

            int routeNum = random.Next(1, Station.routesAmount);
            return new Locomotive(ID, engineType, routeNum, visualizationCanvas, CustomColor.RandomColor());
        }

        public void RandomIncident(double timeOfstep)
		{
            Dictionary<int, string> incidentDict = new Dictionary<int, string>()
            {
                { 1, "Пожар в вагоне"     },
                { 2, "Буйный пассажир"    },
                { 3, "Нет электроэнергии" }
            };

            Dictionary<int, double> timeOfStop = new Dictionary<int, double>()
            {
                { 1, 2  },
                { 2, 1  },
                { 3, 3 }
            };

            Random random = new Random();
            int chance = random.Next(1, 100);
            if (chance <= incidentDict.Count)
            {
                incident = incidentDict[chance];
                lateTime += (int)(timeOfStop[chance] * 60 / timeOfstep);
            }
		}		
	}
}