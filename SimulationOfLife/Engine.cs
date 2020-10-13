using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationOfLife
{
    public class Engine 
    {
        public uint CurrentGeneration { get; private set; } // Возможность получать информацию из интерфейса, однако изменять ее не нужно, по-этому лучше private set
        private bool[,] field; // Массив - поле
        private readonly int rows; // Присваивает значения в конструкторе, которые сразу становятся константами // Строки
        private readonly int cols; // Колонки
        
        public Engine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows]; // Создание массива ( поля ) по декартовой системе
            Random random = new Random(); // Объект, генерирующий случайные числа
            for (int x = 0; x < cols; x++) // генератор случайных чисел. Перебор всех элементов по осям
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0; // Если сгенерирован 0, то true и будет сгенерирована клетка. Чем больше density, тем больше диапазон случайности
                }
            }
        }

        public void NextGeneration() // Генерация нового поколения и перенос его в CurrentGeneration
        {
            var newField = new bool[cols, rows]; // Извлечение старых данных в новый массив, который будет генерировать новое поколение. Старый массив не изменяется

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y); // Переменная для подсчета соседей, у рассматриваемого элемента
                    var hasLife = field[x, y]; // Перемення, проверяющая, есть ли живая клетка по координатам

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true; // Создание клетки, если 3 соседа
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                        newField[x, y] = false; // Клетка умирает
                    else
                        newField[x, y] = field[x, y]; // Клетка остается жить. Изменений нет
                }
            }
            field = newField; // Пересоздание массива с новыми данными
            CurrentGeneration++;
        }

        public bool[,] GetCurrentGeneration() // Реализация нового поколения
        {
            var result = new bool[cols, rows]; // Извлечение старых данных в новый массив, который будет генерировать новое поколение. Старый массив не изменяется
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++) // подсчет соседей для обеих координат
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols; // Круглость планеты
                    var row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y; // Проверка самого себя
                    var hasLife = field[col, row]; // Если клетка жива, ее значение помещается в массив

                    if (hasLife && !isSelfChecking) // Если есть жизнь, но это не сама клетка, значит это живой сосед
                        count++;
                }
            }
            return count;
        }

        private bool ValidateCell(int x, int y) // Валидатор позиции клетки, чтобы не выходила за границу поля
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell(int x, int y, bool state) // Обновление клетки для мыши
        {
            if (ValidateCell(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y) // Добавление клетки
        {
            UpdateCell(x, y, state: true);
        }

        public void DelCell(int x, int y) // Удаление клетки
        {
            UpdateCell(x, y, state: false);
        }
    }
}
