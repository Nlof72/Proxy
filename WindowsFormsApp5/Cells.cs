using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    public abstract class Cell
    {
        public static readonly int OPENED = 0;
        public static readonly int CLOSED = 1;

        protected int status;


        public Cell()
        {
            status = CLOSED;
        }

        public virtual void open(Button button, int mines)
        {
            status = OPENED;
        }

        public virtual int getStatus()
        {
            return status;
        }
        public abstract int getPoints();
    }

    public class Empty : Cell
    {

        public Empty(Button button, int mines) : base()
        {

            string path = @"..\..\mines" + mines + ".gif"; ;
            button.BackgroundImage = Image.FromFile(path);
        }
        public override int getPoints()
        {
            return 25; 
        }
    }

    public class Mine : Cell
    {

        public Mine(Button button, int mines) : base()
        {
            if(Form1.state == State.flag)
            {
                button.BackgroundImage = Image.FromFile(@"..\..\Image3.gif");
                return;
            }
            button.BackgroundImage = Image.FromFile(@"..\..\Image1.gif");

        }
        public override int getPoints()
        {
            return 300; 
        }
    }

    public class EmptyProxy : Cell
    {
        private Empty proxy; 

        public EmptyProxy() : base()
        {
            proxy = null;
        }


        public override void open(Button button, int mines)
        {
            if (proxy == null)
            {
                proxy = new Empty( button, mines);
            }

            proxy.open(button, mines);
        }

        public override int getStatus()
        {
            if (proxy == null)
            {
                return status;
            }
            else
            {
                return proxy.getStatus();
            }
        }

        public override int getPoints()
        {
            if (proxy == null)
            {
                return 25;
            }
            else
            {
                return proxy.getPoints();
            }
        }
    }

    public class MineProxy : Cell
    {
        private Mine proxy;

        public MineProxy() : base()
        {
            proxy = null;
        }

        public override void open(Button button, int mines)
        {
            if (proxy == null)
            {
                proxy = new Mine( button, mines);
            }

            proxy.open(button, mines);
        }


        public override int getStatus()
        {
            if (proxy == null)
            {
                return status;
            }
            else
            {
                return proxy.getStatus();
            }
        }

        public override int getPoints()
        {
            if (proxy == null)
            {
                return 300;
            }
            else
            {
                return proxy.getPoints();
            }
        }
    }
}
