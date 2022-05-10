using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bots
{
    public partial class Form1 : Form
    {
        public const int WIDTHCELL = 20, HIGHTCELL = 20; // 40
        public const int ENERGYLOST = 1; // Кол-во потери энергии за тик
        public const int ENERGYLOSTMOVE = 5; // Кол-во потери энергии за перемещение
        public const int ENERGYFORMLIGHT = 5; // Коэффициент получения энергии
        public const int MIGHTLIGHT = 6; // Мощность света, каждые 3 уровня, уменьшается
        public Brush BotColor = Brushes.Green; // Цвет ботов

        Bitmap BitMap;
        Graphics G;
        public static Random R = new Random();
        public static Map CellMap;

        bool infocheck = false;
        int time = 0;

        void UpdateText()
        {
            label2.Text = "Текущее ускорение: " + timer1.Interval + "\n" + "Прошло ед. времени: " + time/100 + "," + time%100;
            label1.Text = "Колличество ботов: " + CellMap.GetCountBots() + "\n" + "Размеры поля: " + pictureBox1.Width / WIDTHCELL + " на " + pictureBox1.Height / HIGHTCELL + "\n" + "Колличество клеток: " + ((pictureBox1.Height / Form1.HIGHTCELL) * (pictureBox1.Width / WIDTHCELL));
        }

        void UpdateGraphics()
        {
            BitMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            G = Graphics.FromImage(BitMap);

            CellMap.Print(G);

            pictureBox1.Image = BitMap;

            GC.Collect();
        }

        void UpdateLogic()
        {
            CellMap.UpdateLogic();
        }

        void UpdateMaxLive()
        {
            int buf;
            Bot B;

            if (CellMap.FindLongLiveBots(out buf) != null)
            {
                B = CellMap.FindLongLiveBots(out buf);

                label3.Text = ("Макс. время жизни: " + B.GetDuringLive() / 100 + "," + B.GetDuringLive() % 100 + "\n" + "Среднее значение: " + buf / 100 + "," + buf % 100);
            }
        }
        
        public Form1()
        {
            InitializeComponent();
            CellMap = new Map(pictureBox1.Width, pictureBox1.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            UpdateLogic();
            UpdateGraphics();
            UpdateText();
            UpdateMaxLive();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (infocheck == false) CellMap.AddBotToCoordinates(e.X, e.Y);
            if (infocheck)
            {
                int[] buf = CellMap.GetGenomBots(e.X, e.Y);
                listBox1.Items.Clear();
                if(buf != null)
                for (int i = 0; i < buf.Length; i++)
                {
                    listBox1.Items.Add(i+") " + buf[i]);
                }
            }

            UpdateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Enabled = false;
                button1.Text = "Возобновить";
            }
            else
            {
                timer1.Enabled = true;
                button1.Text = "Остановить";
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           if(timer1.Interval > 100) timer1.Interval = timer1.Interval - 100;
            UpdateText();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (timer1.Interval < 1000) timer1.Interval = timer1.Interval + 100;
            UpdateText();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (infocheck)
            {
                infocheck = false;
                button4.Text = "Режим выбора генома (Выкл)";
            }
            else
            {
                infocheck = true;
                button4.Text = "Режим выбора генома (Вкл)";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateGraphics();
            UpdateText();
            label1.Text = "Колличество ботов: " + CellMap.GetCountBots() + "\n" + "Размеры поля: " + pictureBox1.Width / WIDTHCELL + " на " + pictureBox1.Height / HIGHTCELL + "\n" + "Колличество клеток: " + ((pictureBox1.Height / Form1.HIGHTCELL) * (pictureBox1.Width / WIDTHCELL));
        }

    }
}
