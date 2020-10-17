using SimulationOfLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.CursorVisible = false; // Убрать отображение курсора
            Console.SetCursorPosition(0, 0); // Установка расположения курсора

            var engine = new Engine // Создание объекта класса, и установка параметров
            (
                rows: 175,
                cols: 610,
                density: 2
            );

            while (true) // Отрисовка поколений клеток, а также метод для генерации нового поколения
            {
                Console.Title = engine.CurrentGeneration.ToString(); // Информация о текущем поколении в шапке

                var field = engine.GetCurrentGeneration(); // Получение информации об поле с клетками

                for (int y = 0; y < field.GetLength(1); y++) // Перебор массива по вертикали. Перебор по первому измерению
                {
                    var str = new char[field.GetLength(0)]; // Для каждой строки создается массив на столько элементов, сколько в строке

                    for (int x = 0; x < field.GetLength(0); x++) // Перебор всех колонок конкретной строки
                    {
                        if (field[x, y]) // Если по искомым координатам true, то рисуется символ
                            str[x] = '#';
                        else
                            str[x] = ' ';
                    }
                    Console.WriteLine(str);
                }
                Console.SetCursorPosition(0, 0);
                engine.NextGeneration(); // Расчет следующего поколения клеток
            }
        }
    }
}
