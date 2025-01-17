﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6OOP
{

	static class Train
	{
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

        // Получение локомотива из поезда
        public static Locomotive GetLocomotive(this Wagon[] train)
		{
            return train[0] as Locomotive;
		}

        // Получение вагона из поезда
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

        // Создание поезда
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

        
        // Генерация рандомного поезда
        public static Passenger[] CreateRandom(Locomotive locomotive)
        {
            Random random = new Random();

            //Шанс добавления в состав - тип пассажирского вагона
            var passengerWagonTypes = new Dictionary<int, string>()
            {
                
                //Текущ. ключ = пред. кдюч - пред. шанс
                {   1, Passenger.WagonTypes[0]  }, // Вагон-бар   Шанс добавления - 1%  
                {   3, Passenger.WagonTypes[1]  }, // Вагон-зал   Шанс добавления - 2%  
                {   8, Passenger.WagonTypes[2]  }, // Купе "Люкс" Шанс добавления - 5%  
                {  23, Passenger.WagonTypes[3]  }, // Купе СВ     Шанс добавления - 15% 
                {  53, Passenger.WagonTypes[4]  }, // Купе        Шанс добавления - 30% 
                { 100, Passenger.WagonTypes[5]  }  // Плацкарт    Шанс добавления - 47% 
            };

            //Класс вагона - пассажировместимость вагона
            var wagonCapacity = new Dictionary<string, int>()
            {
                { Passenger.WagonTypes[0], 52 }, //Вагон-бар   
                { Passenger.WagonTypes[1], 64 }, //Вагон-зал   
                { Passenger.WagonTypes[2], 10 }, //Купе "Люкс" 
                { Passenger.WagonTypes[3], 20 }, //Купе СВ     
                { Passenger.WagonTypes[4], 36 }, //Купе        
                { Passenger.WagonTypes[5], 54 }, //Плацкарт    
                { Passenger.WagonTypes[6], 48 }  //Вагон-ресторан
            };

            //Рандомный тип локомотива
            int trainLength = random.Next(40, locomotive.MaxWagonsAmount);   //длина состава от 40 до макс. для локомотива
            Passenger[] result = new Passenger[trainLength];

            int typeChance;

            //Добавление вагонов к локомотиву
            string wagonType;
            int wagonRestarauntNum = trainLength / 2;
            for (int i = 0; i < trainLength;i++)
            {
                //Генерация ID
                //Random random = new Random();
                string ID;
            generateID:
                ID = "";

                for (int j = 0; j < random.Next(Wagon.minIDLength, Wagon.maxIDLength); j++)
                {
                    string chars = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
                    ID += chars[random.Next(0, chars.Length - 1)];
                }

                if (!ListOfWagons.IDisUnique(ID))
                    goto generateID;

                if (i != wagonRestarauntNum)
                {
                    typeChance = random.Next(1, 100);
                    wagonType = Passenger.ChooseRandomType(passengerWagonTypes, typeChance);
                    Passenger passengerWagon = new Passenger(wagonType, locomotive, ID);
                    passengerWagon.SerialNum = i;
					passengerWagon.Type = Passenger.ChooseRandomType(passengerWagonTypes, typeChance);
                    result[i] = passengerWagon;
                }
                //Добавление вагона-ресторана в середину поезда
                else
				{
                    wagonType = Passenger.WagonTypes[6];// Вагон-ресторан;
                    Passenger wagonRestaraunt = new Passenger(wagonType,locomotive, ID);
                    wagonRestaraunt.SerialNum = i;
                    result[wagonRestarauntNum] = wagonRestaraunt;
                }
                    
            }

            return result;
        }
    }
}