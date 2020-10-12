using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulationOfLife
{
    public partial class Form1 : Form
    {
        private int currentGeneration = 0;
        private Graphics graphics;
        private int resolution;
        private bool[,] field; // создание игрового поля ( таблица из клеток ), определение по типу bool
        private int rows;
        private int cols;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Start()
        {

            if (timer1.Enabled) // после нажатия кнопки, возможность регулировать параметры блокируется
                return;

            currentGeneration = 0;
            Text = $"Generation{currentGeneration}"; // Подсчет поколений

            nudResolution.Enabled = false; // Блокировка кнопок
            nudDensity.Enabled = false;

            resolution = (int)nudResolution.Value; // Присваивание значения из numeric
            rows = pictureBox1.Height / resolution; // Значение поля, деленое на разрешение ( строки )
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows]; // Создание массива ( поля ) по декартовой системе

            Random random = new Random(); // Объект, генерирующий случайные числа
            for (int x = 0; x < cols; x++) // генератор случайных чисел. Перебор всех элементов по осям
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0; // Если сгенерирован 0, то true и будет сгенерирована клетка. Чем больше density, тем больше диапазон случайности
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Создается картинка(Bitmap), соответствующая размеру boxa
            graphics = Graphics.FromImage(pictureBox1.Image); // Создается объект, в который в качестве параметра передается созданное изображение (строчка-посредник)
            timer1.Start(); // Старт таймера
        }

        private void NextGeneration() // Генерация нового поколения
        {
            graphics.Clear(Color.SeaGreen); // Заливка цвета поля

            var newField = new bool[cols, rows]; // Извлечение старых данных в новый массив, который будет генерировать новое поколение. Старый массив не изменяется

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y); // Переменная для подсчета соседей, у рассматриваемого элемента
                    var hasLife = field[x, y]; // Перемення, проверяющая, есть ли живая клетка по координатам

                    if (!hasLife && neighboursCount == 3)
                        newField[x, y] = true; // Создание клетки, если 3 соседа
                    else if (hasLife && (neighboursCount<2 || neighboursCount >3))
                        newField[x, y] = false; // Клетка умирает
                    else
                        newField[x, y] = field[x, y]; // Клетка остается жить. Изменений нет

                    if (hasLife)
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution -1, resolution -1); // Отрисовка прямоугольника. В качестве отступов - индекс массива * разрешение
                }
            }
            field = newField; // Пересоздание массива с новыми данными
            pictureBox1.Refresh();// Перерисовка игрового поля 
            Text = $"Generation{++currentGeneration}";
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

        private void Stop() // Остановка таймера по кнопке. Доступ к изменению параметров
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }


        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
                return;

            if (e.Button == MouseButtons.Left) // Управление мышкой
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMouse(x, y);
                if (validationPassed)
                    field[x, y] = true;
            }

            if (e.Button == MouseButtons.Right) // Управление мышкой
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                var validationPassed = ValidateMouse(x, y);
                if (validationPassed)
                    field[x, y] = false;
            }
        }

        private bool ValidateMouse(int x, int y) // Валидатор мыши 
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = $"Generation{currentGeneration}";
        }
    }
}
