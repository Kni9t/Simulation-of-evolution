using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Bots
{
    public class Cell
    {
        int x, y, idMySelf, idleft, idright, idtop, idbotom, IdBot = -1, NumberString, NumberColumn;
        int idContent = -1; // -2 - Резервированное значение для несуществующий ячейки -1 - Ячейка пуста, 0 - Содержит бота, 1 - Cодержит труп бота

        public Cell(int x, int y, int idMySelf, int idleft, int idright, int idtop, int idbotom, int NumberString, int NumberColumn)
        {
            this.x = x;
            this.y = y;
            this.idMySelf = idMySelf;
            this.idleft = idleft;
            this.idright = idright;
            this.idtop = idtop;
            this.idbotom = idbotom;
            this.NumberString = NumberString;
            this.NumberColumn = NumberColumn;
        }

        public int GiveBot()
        {
            int buf = IdBot;
            idContent = -1;
            IdBot = -1;
            return buf;
        }

        public void TakeBot(int IdBot)
        {
            this.IdBot = IdBot;
            idContent = 0;
        }

        public int GetID()
        {
            return idMySelf;
        }

        public int GetIdContent()
        {
            if (idContent == 0) return IdBot;
            else return idContent;
        }

        public void SetContent(int id)
        {
            idContent = id;
        }

        public int[] GetIDList()
        {
            return new int[] { idtop, idright, idbotom, idleft };
        }

        public void Print(Graphics G)
        {
            if ((Form1.MIGHTLIGHT- NumberString/3) >= 0) G.FillRectangle(new SolidBrush(Color.FromArgb((Form1.MIGHTLIGHT - (NumberString / 3)) * 15, Color.Yellow)), x, y, Form1.WIDTHCELL, Form1.HIGHTCELL);

            if (idContent == 1) G.FillRectangle(Brushes.Gray, x, y, Form1.WIDTHCELL, Form1.HIGHTCELL);
            if (idContent == 0) G.FillRectangle(Brushes.Green, x, y, Form1.WIDTHCELL, Form1.HIGHTCELL);

            G.DrawRectangle(Pens.Black, x, y, Form1.WIDTHCELL, Form1.HIGHTCELL);
        }

        public bool FreeSpace()
        {
            if (idContent == -1) return true;
            else return false;
        }

        public int CheckCoordinates(int X, int Y)
        {
            if (((X > x) && (Y > y)) && ((X < x + Form1.WIDTHCELL) && (Y < y + Form1.HIGHTCELL))) return idMySelf;
            else return -1;
        }

        public int[] GetCoordinates()
        {
            return new int[2] { x, y };
        }

        public bool CheckFreeSpace(int direction)
        {
            switch (direction)
            {
                case 0:
                    if ((Form1.CellMap[idtop] != null) && (Form1.CellMap[idtop].FreeSpace())) return true;
                    break;
                case 1:
                    if ((Form1.CellMap[idright] != null) && (Form1.CellMap[idright].FreeSpace())) return true;
                    break;
                case 2:
                    if ((Form1.CellMap[idbotom] != null) && (Form1.CellMap[idbotom].FreeSpace())) return true;
                    break;
                case 3:
                    if ((Form1.CellMap[idleft] != null) && (Form1.CellMap[idleft].FreeSpace())) return true;
                    break;
            }
            return false;
        }

        public int GetIdFreeSpace()
        {
            if ((Form1.CellMap[idtop] != null) && (Form1.CellMap[idtop].FreeSpace())) return idtop;
            if ((Form1.CellMap[idright] != null) && (Form1.CellMap[idright].FreeSpace())) return idright;
            if ((Form1.CellMap[idbotom] != null) && (Form1.CellMap[idbotom].FreeSpace())) return idbotom;
            if ((Form1.CellMap[idleft] != null) && (Form1.CellMap[idleft].FreeSpace())) return idleft;
            return -1;
        }

        public int CheckContent(int direction)
        {
            switch (direction)
            {
                case 0:
                    if(Form1.CellMap[idtop] != null) return Form1.CellMap[idtop].GetIdContent();
                    break;
                case 1:
                    if (Form1.CellMap[idright] != null) return Form1.CellMap[idright].GetIdContent();
                    break;
                case 2:
                    if (Form1.CellMap[idbotom] != null) return Form1.CellMap[idbotom].GetIdContent();
                    break;
                case 3:
                    if (Form1.CellMap[idleft] != null) return Form1.CellMap[idleft].GetIdContent();
                    break;
            }
            return -2;
        }

        public int GetNumberString()
        {
            return NumberString;
        }

        public int GetIDBot()
        {
            if (idContent == 0) return IdBot;
            else return -1;
        }
    }
}
