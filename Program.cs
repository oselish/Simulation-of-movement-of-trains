using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Trains;
using Stations;

namespace МоделированиеДвиженияЭлектропоездов
{
    static class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("<-=-=-=-=-=-= Маршрут =-=-=-=-=-=->\n");
            string[] namesOfStations = { "Оренбург", "Орск", "Самара", "Воронеж", "Москва" }; //имена станций
            int amountOfStations = namesOfStations.Length;   //кол-во станций

            Station[] route = new Station[amountOfStations]; //маршрут поезда

            Station.GenerateRoute(route, namesOfStations);
            Station.ShowRoute(route);
            
            Locomotive loco2 = new Locomotive("Электровоз");
            Locomotive loco1 = new Locomotive();
            loco1 = loco2.DeepCopy();

            Wagon[] train = Train.CreateRandom();

			Console.WriteLine("\n<-=-=-=-=-=-= Сравнения локомотивов =-=-=-=-=-=->\n");

            if (loco1 == loco2)
				Console.WriteLine("Локомотивы равны");
            else
				Console.WriteLine("Локомотивы не равны");

			Console.WriteLine("\n<-=-=-=-=-=-= Информация о поезде =-=-=-=-=-=->\n");

            //Получение информации о локомотиве в поезде
            var loco3 = (Locomotive)train[0];
            loco3.RouteNum = 55;

			Console.WriteLine(train.GetInfo());

        }
    }
}
