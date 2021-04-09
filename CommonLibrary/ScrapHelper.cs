using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary
{
    public enum GetSetClick
    {
        Get = 1,
        Set = 2,
        Click = 3,
        GetSet = 4
    }
    public class ScrapHelper
    {
        public const int MOUSEEVENTF_LEFTDOWN = 2;

        public const int MOUSEEVENTF_LEFTUP = 4;

        public const int MOUSEEVENTF_RIGHTDOWN = 8;

        public const int MOUSEEVENTF_RIGHTUP = 16;

        //   private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ScrapHelper()
        {
        }

        public void LeftMouseClick(int xpos, int ypos)
        {
            try
            {
                if ((xpos != 0 ? true : ypos != 0))
                {
                    Cursor.Position = new Point(xpos, ypos);
                    Thread.Sleep(1000);
                    ScrapHelper.mouse_event(2, xpos, ypos, 0, 0);
                    Thread.Sleep(500);
                    ScrapHelper.mouse_event(4, xpos, ypos, 0, 0);
                }
                else
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                //   this.log.Error(exception.Message);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public void rightMouseClick(int xpos, int ypos)
        {
            try
            {
                Cursor.Position = new Point(xpos, ypos);
                Thread.Sleep(1000);
                ScrapHelper.mouse_event(8, xpos, ypos, 0, 0);
                Thread.Sleep(500);
                ScrapHelper.mouse_event(16, xpos, ypos, 0, 0);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void setTextInTextBox(int xpos, int ypos, string strtext)
        {
            try
            {
                Cursor.Position = new Point(xpos, ypos);
                Thread.Sleep(1000);
                ScrapHelper.mouse_event(2, 0, 0, 0, 0);
                Thread.Sleep(500);
                ScrapHelper.mouse_event(4, 0, 0, 0, 0);
                Thread.Sleep(150);
                ScrapHelper.mouse_event(2, 0, 0, 0, 0);
                Thread.Sleep(150);
                ScrapHelper.mouse_event(4, 0, 0, 0, 0);
                // Thread.Sleep(1000);
                SendKeys.SendWait(strtext);
                Thread.Sleep(1000);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
