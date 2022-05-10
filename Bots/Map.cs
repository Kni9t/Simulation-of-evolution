using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Bots
{
    public class Map
    {
        List<Bot> BotList;
        Cell[][] CellMass;

        Random R = new Random();

        int PictWigth, PictHight;

        public Map(int PictWigth, int PictHight)
        {
            BotList = new List<Bot>();

            this.PictWigth = PictWigth;
            this.PictHight = PictHight;

            int BufCountString = PictHight / Form1.HIGHTCELL;
            int BufCountColumn = PictWigth / Form1.WIDTHCELL;

            int idMySelf, idleft, idright, idtop, idbotom;

            CellMass = new Cell[BufCountString][];

            for (int i = 0; i < BufCountString; i++)
            {
                CellMass[i] = new Cell[BufCountColumn];

                for (int j = 0; j < BufCountColumn; j++)
                {
                    if ((i * BufCountColumn + j - 1) < (i * BufCountColumn)) idleft = (i * BufCountColumn + BufCountColumn - 1);
                    else idleft = (i * BufCountColumn + j - 1);

                    if ((i * BufCountColumn + j + 1) > (i * BufCountColumn + BufCountColumn-1)) idright = (i * BufCountColumn);
                    else idright = (i * BufCountColumn + j + 1);

                    if (((i - 1) * BufCountColumn + j) < 0) idtop = -1;
                    else idtop = ((i-1) * BufCountColumn + j);

                    if (((i + 1) * BufCountColumn + j) > (BufCountString * BufCountColumn)) idbotom = -1;
                    else idbotom = ((i + 1) * BufCountColumn + j);

                    idMySelf = i * BufCountColumn + j;

                    CellMass[i][j] = new Cell(Form1.WIDTHCELL * j, Form1.HIGHTCELL * i, idMySelf, idleft, idright, idtop, idbotom, i,j);
                }
            }
        }

        public void KillBots(int id)
        {
            for (int i =0; i< BotList.Count;i++)
            {
                if (BotList[i].GetIdBot() == id)
                {
                    FindCellOnId(BotList[i].GetIdLocation()).SetContent(1);
                    BotList.RemoveAt(i);
                    return;
                }
            }
        }

        public void Print(Graphics G)
        {
            foreach (Cell[] C in CellMass)
            {
                for (int i = 0; i < C.Length; i++) { C[i].Print(G); }
            }
        }

        public void UpdateLogic()
        {
            for (int i = 0; i < BotList.Count; i++)
            {
                BotList[i].Update();
            }
        }

        public int GetCountBots()
        {
            return BotList.Count;
        }

        public void AddBotToCoordinates(int x, int y)
        {
            for (int j = 0; j < CellMass.Length; j++)
            {
                for (int i = 0; i < CellMass[j].Length; i++)
                {
                    if (CellMass[j][i].CheckCoordinates(x, y) != -1)
                    {
                        if (CellMass[j][i].FreeSpace())
                        {
                            Bot bufbot = new Bot(CellMass[j][i].GetID(), BotList.Count, null);
                            CellMass[j][i].TakeBot(BotList.Count);
                            BotList.Add(bufbot);
                            return;
                        }
                        else
                            return;
                    }
                }
            }
        }

        public int[] GetGenomBots(int x, int y)
        {
            foreach (Cell[] C in CellMass)
            {
                for (int i = 0; i < C.Length; i++)
                {
                    if (C[i].CheckCoordinates(x, y) != -1)
                    {
                        if (C[i].GetIDBot() != -1)
                        {
                            for (int g= 0; g< BotList.Count;g++)
                            {
                                if (BotList[g].GetIdBot() == C[i].GetIDBot()) return BotList[g].GetGenom();
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void AddBots(Bot b)
        {
            BotList.Add(b);
            FindCellOnId(b.GetIdLocation()).TakeBot(BotList.Count);
        }
        
        public Bot FindLongLiveBots(out int MeanValue)
        {
            try
            {
                int MaxLive = 0, idbot = -1, buf = 0;

                for (int i = 0; i < BotList.Count; i++)
                {
                    buf += BotList[i].GetDuringLive();
                    if (BotList[i].GetDuringLive() > MaxLive)
                    {
                        idbot = i;
                        MaxLive = BotList[i].GetDuringLive();
                    }
                }
                if (BotList[idbot] != null)
                {
                    MeanValue = buf / BotList.Count;
                    return BotList[idbot];
                }
                else
                {
                    MeanValue = 0;
                    return null;
                }
            }
        catch
            {
                MeanValue = -1;
                return null;
            }
        }
        public Cell FindCellOnId(int id)
        {
            foreach (Cell[] C in CellMass)
            {
                for (int i = 0; i < C.Length; i++)
                {
                    if (C[i].GetID() == id) return C[i];
                }
            }
            return null;
        }
        public Cell this[int ID]
        {
            get {
                foreach (Cell[] C in CellMass)
                {
                    for (int i = 0; i < C.Length; i++)
                    {
                        if (C[i].GetID() == ID) return C[i];
                    }
                }

                return null;
            }
        }
    }
}
