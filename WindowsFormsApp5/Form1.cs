using System;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public enum State
    {
        mine,
        flag
    }
    public partial class Form1 : Form
    {
        Cell[][] cells;
        Button[][] buttons;
        Random rand = new Random();
        int numberOfFlags;
        int score = 0;
        int numberOfMine;
        int numberOfEmpty;


        static public State state = State.mine;
        public Form1()
        {
            InitializeComponent();
            HelpButton = true;
        }

        private void Start(object sender, EventArgs e)
        {
            cells = new Cell[10][];
            buttons = new Button[10][];
            NumberOfFlags.Value = NumberOfMine.Value;
            numberOfFlags = (int)NumberOfFlags.Value;
            numberOfMine = (int)NumberOfMine.Value;

            Console.WriteLine(numberOfMine);
            for (int i = 0; i < 10; i++)
            {
                cells[i] = new Cell[10];
                buttons[i] = new Button[10];
            }
            for(int i = 0; i<100; i++)
            {
                buttons[Math.DivRem(i,10,out int r)][i % 10] = (Button)flowLayoutPanel1.Controls[i];
            }

            numberOfEmpty = 100 - numberOfMine;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if(numberOfMine > 0 && numberOfEmpty >0)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            cells[i][j] = new MineProxy();
                            numberOfMine--;
                        }
                        else
                        {
                            cells[i][j] = new EmptyProxy();
                            numberOfEmpty--;
                        }
                    }
                    else
                    {
                        if (numberOfMine > 0) { 
                            cells[i][j] = new MineProxy();
                            numberOfMine--;
                        }
                        else
                        {
                            cells[i][j] = new EmptyProxy();
                            numberOfEmpty--;
                        }

                    }

                }
            }

            for (int i = 9; i >= 1; i--)
            {
                for (int j = 9; j >= 1; j--)
                {
                    int q = rand.Next(j + 1);
                    int z = rand.Next(i + 1);
                    var tmp = cells[i][j];
                    cells[i][j] = cells[z][q];
                    cells[z][q] = tmp;
                }
            }
            Console.WriteLine(numberOfMine);
            numberOfMine = numberOfFlags;
            numberOfEmpty = 100 - numberOfMine;
            Restart();
        }

        private void OpenCell(object sender, EventArgs e)
        {
            if (cells == null) return;
            Button b = (Button)sender;
            MineProxy mineProxy = new MineProxy();
            int q;
            int i = Math.DivRem(b.TabIndex, 10, out q);
            int j = b.TabIndex % 10;
            if (state == State.flag)
            {
                if (numberOfFlags == 0) return;
                numberOfFlags--;
            }

            if (cells[i][j].getStatus() == Cell.OPENED) return;
            cells[i][j].open(buttons[i][j], CountMines(mineProxy, i, j));
            if(cells[i][j].GetType() == mineProxy.GetType())
            {
                if(state == State.flag)
                {
                    score += cells[i][j].getPoints();
                    numberOfMine--;

                    UpdateScore();
                    if (CheckWin())
                    {
                        Restart();
                        Console.WriteLine(111);
                        return;
                    }
                    return;
                }
                else
                {
                    state = State.mine;
                    for (int ii = 0; ii < 10; ii++)
                    {
                        for (int jj = 0; jj < 10; jj++)
                        {
                            cells[ii][jj].open(buttons[ii][jj], CountMines(mineProxy, ii, jj));
                            if (cells[ii][jj].GetType() == mineProxy.GetType()) { 
                                continue;
                            }
                        }
                    }
                    MessageBox.Show($"You lose, please restart\nyour score is {score} pts");
                    score = 0;
                    Restart();
                    Console.WriteLine(112221);
                    UpdateScore();
                    return;
                }
            }
            numberOfEmpty--;
            score += cells[i][j].getPoints();
            UpdateScore();
            if (CheckWin())
            {
                Restart();
                Console.WriteLine(111);
                return;
            }
        }

        int CountMines(MineProxy mineProxy, int i, int j)
        {
            int countMines = 0;
            for (int ii = -1; ii < 2; ii++)
            {
                for (int jj = -1; jj < 2; jj++)
                {

                    if (i + ii < 0) continue;
                    if (j + jj < 0) continue;
                    if (i + ii > 9) continue;
                    if (j + jj > 9) continue;
                    if (((j + jj) == j) && (i + ii == i)) continue;
                    //Console.WriteLine(111);
                    if (cells[i + ii][j + jj].GetType() == mineProxy.GetType())
                    {
                        countMines++;
                        //Console.WriteLine("_________________----------------------------________");
                    }
                }
            }
            return countMines;
        }

        void UpdateScore()
        {
            Score.Text = score + "";
            NumberOfFlags.Value = numberOfFlags;
        }

        bool CheckWin()
        {
            bool win = false; 
            if (numberOfMine == 0) { MessageBox.Show($"You won this try with score {score}"); win = true; }
            else if (numberOfEmpty == 0) { MessageBox.Show($"You won this try with score {score}"); win = true; };
            return win;
        }

        private void Flag_Click(object sender, EventArgs e)
        {
            if(state == State.mine)
            {
                state = State.flag;
                Flag.Text = "Flag";
            }
            else
            {
                state = State.mine;
                Flag.Text = "Mine";
            }
        }

        void Restart()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    buttons[i][j].BackgroundImage = null;
                    //buttons[i][j].Text = null;
                }
            }
        }
    }
}
