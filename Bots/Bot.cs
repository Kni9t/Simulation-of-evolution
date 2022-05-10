using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bots
{
    public class Bot
    {
        const int ENERGYFROMDEAD = 50; // Колличество энергии за поедание трупика бота
        const int ENERGYFROMHUNT = 75; // Колличество энергии за поедание бота
        const int GENOMSIZE = 64; // Размер генома
        const int MAXENERGY = 250; // Максимальное колличество энергии
        const int ENERGYFORSEGMENTATION = 65; // Колличество энергии необходимое на одно деление


        int IDLocation, IdBot, Energy = 30, Register = 0, DuringLive = 0;
        Brush Color = Brushes.Green;
        int[] Genom;

        /* Коды генома
         0-3 - перемещение (0 - вверх, 1 - вправо, 2 - вниз, 3 - влево)
         4-7 - осмотрется (4 - вверх, 5 - вправо, 6 - вниз, 7 - влево)
         8-11 - схватить (8 - вверх, 9 - вправо, 10 - вниз, 11 - влево)
         12 - фотосинтез
         13-16 - попытатся оставить потомка (13 - вверх, 14 - вправо, 15 - вниз, 16 - влево)
             */

        public Bot(int IDLocation, int IdBot, int[] BufGenom)
        {
            this.IdBot = IdBot;
            this.IDLocation = IDLocation;
            Genom = new int[GENOMSIZE];

            if (BufGenom == null)
            {
                for (int i = 0; i < Genom.Length; i++)
                {
                    Genom[i] = 12;
                }
            }
            else
            {
                for (int i = 0; i < BufGenom.Length; i++)
                {
                    Genom[i] = BufGenom[i];
                }
            }
        }

        public void ChangeID(int IDLocation)
        {
            this.IDLocation = IDLocation;
        }

        public int GetDuringLive()
        {
            return DuringLive;
        }

        public int GetIdBot()
        {
            return IdBot;
        }

        public int GetIdLocation()
        {
            return IDLocation;
        }

        void Photosynthesis()
        {
            UpdateEnergy((Form1.MIGHTLIGHT - (Form1.CellMap[IDLocation].GetNumberString()/3)) * Form1.ENERGYFORMLIGHT);
        }

        void UpdateEnergy(int count)
        {
            Energy += count;

            if (Energy > MAXENERGY)
            {
                Energy = MAXENERGY;
                if (Form1.CellMap[IDLocation].GetIdFreeSpace() != -1)
                    Segmentation(Form1.R.Next(0, 4));
                else
                    Form1.CellMap.KillBots(IdBot);
            }

            if (Energy < 0) Form1.CellMap.KillBots(IdBot);
        }

        int[] GetMutationGenom()
        {
            int[] buf = new int[GENOMSIZE];

            for (int i = 0; i < Genom.Length; i++)
            {
                buf[i] = Genom[i];
            }

            if (Form1.R.Next(0,101) <= 25) buf[Form1.R.Next(0, GENOMSIZE)] = Form1.R.Next(0, GENOMSIZE);

            return buf;
        }

        void Segmentation(int direction)
        {
            switch (direction)
            {
                case 0:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap.AddBots(new Bot(Form1.CellMap[IDLocation].GetIDList()[0], Form1.CellMap.GetCountBots(), GetMutationGenom()));
                        UpdateEnergy(-ENERGYFORSEGMENTATION);
                    }
                    break;

                case 1:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap.AddBots(new Bot(Form1.CellMap[IDLocation].GetIDList()[1], Form1.CellMap.GetCountBots(), GetMutationGenom()));
                        UpdateEnergy(-ENERGYFORSEGMENTATION);
                    }
                    break;

                case 2:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap.AddBots(new Bot(Form1.CellMap[IDLocation].GetIDList()[2], Form1.CellMap.GetCountBots(), GetMutationGenom()));
                        UpdateEnergy(-ENERGYFORSEGMENTATION);
                    }
                    break;

                case 3:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap.AddBots(new Bot(Form1.CellMap[IDLocation].GetIDList()[3], Form1.CellMap.GetCountBots(), GetMutationGenom()));
                        UpdateEnergy(-ENERGYFORSEGMENTATION);
                    }
                    break;
            }
        }

        public void Move(int direction)
        {
            // Direction - направление 0 - вверх, 1 - право, 2 - низ, 3- лево

            switch (direction)
            {
                case 0:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[0]].TakeBot(Form1.CellMap[IDLocation].GiveBot());
                        IDLocation = Form1.CellMap[IDLocation].GetIDList()[0];
                        Energy -= Form1.ENERGYLOSTMOVE;
                    }
                    break;

                case 1:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[1]].TakeBot(Form1.CellMap[IDLocation].GiveBot());
                        IDLocation = Form1.CellMap[IDLocation].GetIDList()[1];
                        Energy -= Form1.ENERGYLOSTMOVE;
                    }
                    break;

                case 2:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[2]].TakeBot(Form1.CellMap[IDLocation].GiveBot());
                        IDLocation = Form1.CellMap[IDLocation].GetIDList()[2];
                        Energy -= Form1.ENERGYLOSTMOVE;
                    }
                    break;

                case 3:
                    if (Form1.CellMap[IDLocation].CheckFreeSpace(direction))
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[3]].TakeBot(Form1.CellMap[IDLocation].GiveBot());
                        IDLocation = Form1.CellMap[IDLocation].GetIDList()[3];
                        Energy -= Form1.ENERGYLOSTMOVE;
                    }
                    break;
            }
        }

        void UpdateRegister(int count)
        {
            Register += count;
            if (Register > (GENOMSIZE-1)) Register -= GENOMSIZE;
        }

        int CountrolRegister(int value)
        {
            if (value > (GENOMSIZE-1)) value -= GENOMSIZE;
            return value;
        }

        public int[] GetGenom()
        {
            return Genom;
        }

        public void Update()
        {
            /* Коды генома
         0-3 - перемещение (0 - вверх, 1 - вправо, 2 - вниз, 3 - влево)
         4-7 - осмотрется (4 - вверх, 5 - вправо, 6 - вниз, 7 - влево)
         8-11 - схватить (8 - вверх, 9 - вправо, 10 - вниз, 11 - влево)
         12 - фотосинтез
         13-16 - попытатся оставить потомка (13 - вверх, 14 - вправо, 15 - вниз, 16 - влево)
         17-20 - схватить + переместится (17 - вверх, 18 - вправо, 19 - вниз, 20 - влево)
             */

            // -2 - Резервированное значение для несуществующий ячейки -1 - Ячейка пуста, 0 - Содержит бота, 1 - Cодержит труп бота

            switch (Genom[Register])
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Move(Genom[Register]);
                    UpdateRegister(Genom[Register] + Genom[CountrolRegister(Register + 1)]);
                    break;

                case 4:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 0) UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 1) UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    break;
                case 5:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 0) UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 1) UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    break;
                case 6:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 0) UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 1) UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    break;
                case 7:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 0) UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 4) == 1) UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    break;

                case 8:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;
                case 9:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;
                case 10:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;
                case 11:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == -1) UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 8) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 8]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;

                case 12:
                    Photosynthesis();
                    UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    break;

                case 13:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 13) == -1)
                    {
                        Segmentation(Genom[Register] - 13);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }
                    break;
                case 14:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 13) == -1)
                    {
                        Segmentation(Genom[Register] - 13);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }
                    break;
                case 15:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 13) == -1)
                    {
                        Segmentation(Genom[Register] - 13);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }
                    break;
                case 16:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 13) == -1)
                    {
                        Segmentation(Genom[Register] - 13);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }
                    break;

                case 17:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == -1)
                    {
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;

                case 18:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == -1)
                    {
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;

                case 19:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == -1)
                    {
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;

                case 20:
                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == -1)
                    {
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 1)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 0)
                    {
                        Form1.CellMap.KillBots(Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].GetIdContent());
                        UpdateEnergy(ENERGYFROMHUNT);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 2)]);
                    }

                    if (Form1.CellMap[IDLocation].CheckContent(Genom[Register] - 17) == 1)
                    {
                        Form1.CellMap[Form1.CellMap[IDLocation].GetIDList()[Genom[Register] - 17]].SetContent(-1);
                        UpdateEnergy(ENERGYFROMDEAD);
                        Move(Genom[Register] - 17);
                        UpdateRegister(Genom[CountrolRegister(Register + 3)]);
                    }
                    break;


                default:
                    UpdateRegister(Genom[Register]);
                    break;
            }

            DuringLive++;
            UpdateEnergy (-Form1.ENERGYLOST);
        }
    }
}
