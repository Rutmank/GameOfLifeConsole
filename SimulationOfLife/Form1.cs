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
        private Graphics graphics;
        private int resolution;
        private Engine gameEngine;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Start()
        {

            if (timer1.Enabled) // после нажатия кнопки, возможность регулировать параметры блокируется
                return;

            nudResolution.Enabled = false; // Блокировка кнопок
            nudDensity.Enabled = false;

            resolution = (int)nudResolution.Value; // Присваивание значения из numeric

            gameEngine = new Engine
            (
                rows: pictureBox1.Height / resolution, // Значение поля, деленое на разрешение ( строки )
                cols: pictureBox1.Width / resolution, // :  Присваивание. Именнованный параметр 
                density: (int)nudDensity.Minimum + (int)nudDensity.Maximum - (int)nudDensity.Value // Исправление количества клеток. 
            );

            Text = $"Generation{gameEngine.CurrentGeneration}"; // Подсчет поколений

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // Создается картинка(Bitmap), соответствующая размеру boxa
            graphics = Graphics.FromImage(pictureBox1.Image); // Создается объект, в который в качестве параметра передается созданное изображение (строчка-посредник)
            timer1.Start(); // Старт таймера
        }

        private void DrawNextGeneration() // Генерация нового поколения
        {
            graphics.Clear(Color.SeaGreen); // Заливка цвета поля

            var field = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if(field[x,y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1); // Отрисовка прямоугольника. В качестве отступов - индекс массива * разреш
                }
            }
            pictureBox1.Refresh();// Перерисовка игрового поля 
            Text = $"Generation{gameEngine.CurrentGeneration}"; // отображение счетчика поколений
            gameEngine.NextGeneration(); //  Генерация следующего поколения
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
            DrawNextGeneration();
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

            if (e.Button == MouseButtons.Left) // Управление мышкой. Добавление клетки
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
            }

            if (e.Button == MouseButtons.Right) // Управление мышкой. Удаление клетки
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.DelCell(x, y);
            }
        }


        


    }
}
