using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6OOP
{
	/// <summary>
	/// Пассажирский вагон
	/// </summary>
     
	public class Passenger : Wagon
	{
		public static string[] WagonTypes = { "Вагон-бар", "Вагон-зал", "Купе \"Люкс\"", "Купе СВ", "Купе", "Плацкарт", "Вагон-ресторан" };

        //Шанс добавления в состав - тип пассажирского вагона
        private static Dictionary<int, string> passengerWagonTypes = new Dictionary<int, string>()
        {
            
            //Текущ. ключ = пред. кдюч - пред. шанс
            {   1, WagonTypes[0]  }, // Вагон-бар   Шанс добавления - 1%  
            {   3, WagonTypes[1]  }, // Вагон-зал   Шанс добавления - 2%  
            {   8, WagonTypes[2]  }, // Купе "Люкс" Шанс добавления - 5%  
            {  23, WagonTypes[3]  }, // Купе СВ     Шанс добавления - 15% 
            {  53, WagonTypes[4]  }, // Купе        Шанс добавления - 30% 
            { 100, WagonTypes[5]  }  // Плацкарт    Шанс добавления - 47% 
        };

        //Класс вагона - пассажировместимость вагона
        private static Dictionary<string, int> wagonCapacity = new Dictionary<string, int>()
        {
            { WagonTypes[0], 52 }, //Вагон-бар   
            { WagonTypes[1], 64 }, //Вагон-зал   
            { WagonTypes[2], 10 }, //Купе "Люкс" 
            { WagonTypes[3], 20 }, //Купе СВ     
            { WagonTypes[4], 36 }, //Купе        
            { WagonTypes[5], 54 }, //Плацкарт    
            { WagonTypes[6], 48 }  //Вагон-ресторан
        };

        /// <summary>
        /// Вместимость вагона
        /// </summary>
        private int capacity;
		public int Capacity { get; set; }

		/// <summary>
		/// Класс пассажирского вагона (плацкарт, купе, СВ и т.п.)
		/// </summary>
		private string type;
		public string Type { get; set; }

		/// <summary>
		/// Поезд к которому прицеплен вагон
		/// </summary>
		private Locomotive locomotive;
		public Locomotive Loco { get; set; }

		/// <summary>
		/// Получение информации о пассажирском вагоне
		/// </summary>
		/// <returns></returns>

		public Passenger(int Capacity, string Type, Locomotive Loco, int SerialNum, string Name) : base(SerialNum, Name)
		{
			this.Type = Type;
			this.Capacity = Capacity;
			this.locomotive = Loco;
		}

		public Passenger(string wagonType, Locomotive locomotive, string ID)
		{
			var wagonCapacity = new Dictionary<string, int>()
			{
				{ WagonTypes[0], 52 }, //Вагон-бар   
                { WagonTypes[1], 64 }, //Вагон-зал   
                { WagonTypes[2], 10 }, //Купе "Люкс" 
                { WagonTypes[3], 20 }, //Купе СВ     
                { WagonTypes[4], 36 }, //Купе        
                { WagonTypes[5], 54 }, //Плацкарт    
                { WagonTypes[6], 48 }  //Вагон-ресторан
            };

			this.Type = wagonType;
			this.Capacity = wagonCapacity[wagonType];
			this.Name = "Пассажирский вагон";
			this.locomotive = locomotive;
			this.SerialNum = locomotive.wagons.Count;
			this.ID = ID;
			//locomotive.wagons.Add(this);
		}

        ~Passenger() { }
        Passenger() { }
		public string ID { get; set; }

		public override string GetInfo()
		{
			string info = "";
			info += base.GetInfo();
			info += $"Класс вагона: {Type}\nВместимость вагона: {Capacity}\n";
			return info;
		}

        public static string GenerateID()
		{
            Random random = new Random();
            string ID;
        generateID:
            ID = "";

            for (int i = 0; i < random.Next(minIDLength, maxIDLength); i++)
                ID += charsForID[random.Next(0, charsForID.Length - 1)];

            if (!ListOfWagons.IDisUnique(ID))
                goto generateID;
            return ID;
        }

		public static Passenger CreateRandom(Locomotive locomotive)
		{
			var random = new Random();
            int typeChance = random.Next(1, 100);
            string wagonType = ChooseRandomType(passengerWagonTypes, typeChance);

            string ID = GenerateID();

			return new Passenger(wagonType, locomotive, ID);
		}

        public static string ChooseRandomType(Dictionary<int, string> dict, int chance)
        {
            string choosenType = "";
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
    }
}
