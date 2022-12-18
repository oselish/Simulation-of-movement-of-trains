using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementOfElectricTrains
{

	static class Train
	{
        /// <summary>
        /// Получение информации о поезде
        /// </summary>
        /// <param Name="train"></param>
        /// <returns></returns>
        public static string GetInfo(this Wagon[] train)
        {
            string info = "";
            var loco = train[0] as Locomotive;
            info += loco.GetInfo();

            for (int i = 1; i < train.Length; i++)
            {
                var passenger = train[i] as Passenger;
                info += passenger.GetInfo() + "\n";
            }
            return info;
        }

        /// <summary>
        /// Получение локомотива из поезда
        /// </summary>
        /// <param Name="train"></param>
        /// <returns></returns>
        public static Locomotive GetLocomotive(this Wagon[] train)
		{
            return train[0] as Locomotive;
		}

        /// <summary>
        /// Получение вагона из поезда
        /// </summary>
        /// <param Name="train"></param>
        /// <returns></returns>
        public static Passenger GetWagon(this Wagon[] train, int wagonSerialNum)
        {
            if (wagonSerialNum < 0)
				Console.WriteLine($"Порядковый номер вагона не может быть меньше нуля. Значение: {wagonSerialNum}");
            else if (wagonSerialNum > train.Length)
				Console.WriteLine($"Порядковый номер вагона не может быть больше кол-ва вагонов в поезде. Значение: {wagonSerialNum}");
            else
               return (Passenger)train[wagonSerialNum];
            return null;
        }

        /// <summary>
        /// Создание поезда
        /// </summary>
        /// <param Name="loco"></param>
        /// <param Name="wagons"></param>
        /// <param Name="randomGeneration"></param>
        /// <returns></returns>
        public static Wagon[] Create(Locomotive loco, Wagon[] wagons)
        {

            int trainLength = wagons.GetLength(0) + 1;
            Wagon[] train = new Wagon[trainLength]; //поезд - массив вагонов
            train[0] = loco;                        //первый вагон - локомотив

            //Добавление вагонов локомотиву
            for (int i = 1; i < trainLength; i++)
            {
                train[i] = wagons[i - 1];
            }
            return train;
        }

        /// <summary>
        /// Выбор случайного типа вагона или локомотива
        /// </summary>
        /// <param Name="dict"></param>
        /// <returns></returns>
        private static string ChooseRandomType(Dictionary<int, string> dict, int chance)
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

        /// <summary>
        /// Генерация рандомного поезда.<br/><br/>
        /// Указываются самостоятельно:<br/>
        /// 1. Начальная станция<br/>
        /// 2. Текущая станция<br/>
        /// 3. Конечная станция<br/>
        /// 4. Происшествие<br/>
        /// 5. Время опаздывания<br/>
        /// 6. Номер маршрута<br/>
        /// 7. Текущая скорость
        /// </summary>
        /// <returns></returns>
        public static Wagon[] CreateRandom()
        {
            Random random = new Random();

            //Шанс добавления в состав - тип пассажирского вагона
            var passengerWagonTypes = new Dictionary<int, string>()
            {
                //Текущ. ключ = пред. кдюч - пред. шанс
                {   1, "Вагон-бар"      }, //Шанс добавления - 1%  
                {   3, "Вагон-зал"      }, //Шанс добавления - 2%  
                {   8, "Купе \"Люкс\""  }, //Шанс добавления - 5%  
                {  23, "Купе СВ"        }, //Шанс добавления - 15% 
                {  53, "Купе"           }, //Шанс добавления - 30% 
                { 100, "Плацкарт"       }  //Шанс добавления - 47% 
            };
            //    {  22, "Вагон-ресторан" }, //Шанс добавления - 10% 

            

            //Класс вагона - пассажировместимость вагона
            var wagonCapacity = new Dictionary<string, int>()
            {
                {"Вагон-бар",      52 },
                {"Вагон-зал",      64 },
                {"Купе \"Люкс\"",  10 },
                {"Купе СВ",        20 },
                {"Купе",           36 },
                {"Плацкарт",       54 },
                {"Вагон-ресторан", 48 }
            };

            //Рандомный тип локомотива
            int typeChance = random.Next(1, 100);
            string locoType = ChooseRandomType(Locomotive.locomotiveTypes, typeChance); //выбор случайного типа локомотива
            Locomotive loco = new Locomotive(locoType);                      //инициализация локомотива
            int trainLength = random.Next(40, loco.MaxWagonsAmount);           //длина состава от 6 до макс. для локомотива

            Wagon[] train = new Wagon[trainLength];                          //поезд - массив вагонов
            train[0] = loco;                                                 //первый вагон - локомотив


            //Добавление вагонов к локомотиву
            string wagonType;
            int wagonRestarauntNum = trainLength / 2;
            for (int i = 1; i < trainLength;i++)
            {
                if (i != wagonRestarauntNum)
                {
                    typeChance = random.Next(1, 100);
                    wagonType = ChooseRandomType(passengerWagonTypes, typeChance);
                    Passenger passengerWagon = new Passenger
                    (
                        Capacity:  (short)wagonCapacity[wagonType],
                        Type: wagonType,
                        Loco: loco,
                        SerialNum: (short)i,
                        Name: "Пассажирский вагон"

                    );
                    passengerWagon.Type = ChooseRandomType(passengerWagonTypes, typeChance);
                    train[i] = passengerWagon;
                }
                //Добавление вагона-ресторана в середину поезда
                else
				{
                    wagonType = "Вагон-ресторан";
                    Passenger wagonRestaraunt = new Passenger
                    (
                        Capacity: (short)wagonCapacity[wagonType],
                        Type: wagonType,
                        Loco: loco,
                        SerialNum: (short)wagonRestarauntNum,
                        Name: "Пассажирский вагон"
                    );
                    train[wagonRestarauntNum] = wagonRestaraunt;
                }
                    
            }

            return train;
        }
    }
}