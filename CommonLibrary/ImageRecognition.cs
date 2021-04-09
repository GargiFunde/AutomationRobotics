
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonLibrary
{
    public class ImageRecognition
    {
        /// <summary>
        /// Added obsolete Atrribute
        /// Vinay D.
        /// </summary>
        
        [Obsolete]
        public bool GetSetClickImage(string smallBmpPath, GetSetClick getSetClick, string setText,int timeoutInSecond=0,double accuracy= 0.2)
        {
            Rectangle rSearchImage = new Rectangle();
            rSearchImage.Width = 0;
            rSearchImage.Height = 0;
            try
            {
                Bitmap smallBmp = new Bitmap(smallBmpPath);
                //Stopwatch timer = new Stopwatch();
                //timer.Start();
                //while (timer.Elapsed.TotalSeconds < timeoutInSecond)
                //{
                    Bitmap bmGridScreen = ApplicationScreenSnapshot();

                    //  bmGridScreen.Save(@"E:\Work\TestABC.png");
                    if (rSearchImage.X == 0 && rSearchImage.Y == 0 && rSearchImage.Height == 0 && rSearchImage.Width == 0)
                    {
                        rSearchImage = searchBitmap(smallBmp, bmGridScreen, accuracy);
                    }
                //    else
                //    {
                //        break;
                //    }
                //}
                //timer.Stop();
                if((rSearchImage.Width ==0)&&(rSearchImage.Height==0))
                {
                    //Log.Logger.LogData("Image not found." , LogLevel.Error);
                    return true; //failed - need to returned true
                }
                //Log.Logger.LogData("Image found.  X : " + rSearchImage.X.ToString() + " Y:" + rSearchImage.Y, LogLevel.Debug);

                //ThreadInvoker.Instance.RunByUiThread(() =>
                //{
                    ScrapHelper scrapHelper = new ScrapHelper();
                    if (getSetClick == GetSetClick.Click)
                    {
                        scrapHelper.LeftMouseClick(rSearchImage.X + (rSearchImage.Width / 2), rSearchImage.Y + (rSearchImage.Height / 2));
                    }
                    else if (getSetClick == GetSetClick.GetSet)
                    {
                        scrapHelper.setTextInTextBox(rSearchImage.X + (rSearchImage.Width / 2), rSearchImage.Y + (rSearchImage.Height / 2), setText);
                        //Log.Logger.LogData("Set text in Image", LogLevel.Info);
                    }
                    else if (getSetClick == GetSetClick.Get)
                    {
                        //Need OCR to get text
                        //Log.Logger.LogData("Get text from Image", LogLevel.Info);
                    }
                    else if (getSetClick == GetSetClick.Set)
                    {
                        scrapHelper.setTextInTextBox(rSearchImage.X + (rSearchImage.Width / 2), rSearchImage.Y + (rSearchImage.Height / 2), setText);
                        //Log.Logger.LogData("Set text in Image", LogLevel.Info);
                    }
                //});
                return false; //success return false
            }
            catch (Exception ex)
            {
                //if (new CustomException().IsCritical(ex))
                //    throw;
                //else
                //    logger.Error(ex.StackTrace);
                Console.WriteLine(ex.Message);
                return true; //failed - need to returned true
            }
        }
        public Rectangle searchBitmap(Bitmap smallBmp, Bitmap bigBmp)
        {
            return this.SearchBitmap(smallBmp, bigBmp, 0.1, false);
        }
        public Rectangle searchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            return this.SearchBitmap(smallBmp, bigBmp, tolerance, false);
        }
        public unsafe Rectangle SearchBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance, bool fromAttachment)
        {
            Rectangle empty;
            if ((smallBmp.Width > bigBmp.Width ? true : smallBmp.Height > bigBmp.Height))
            {
                Bitmap aux = bigBmp;
                bigBmp = smallBmp;
                smallBmp = aux;
            }
            if (smallBmp.Height <= bigBmp.Height)
            {
                Rectangle location = Rectangle.Empty;
                try
                {
                    try
                    {
                        BitmapData smallData = smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        BitmapData bigData = bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        int smallStride = smallData.Stride;
                        int bigStride = bigData.Stride;
                        int bigWidth = bigBmp.Width;
                        int bigHeight = bigBmp.Height - smallBmp.Height + 1;
                        int smallWidth = smallBmp.Width * 3;
                        int smallHeight = smallBmp.Height;
                        int margin = Convert.ToInt32(255 * tolerance);
                        byte* pSmall = (byte*)((void*)smallData.Scan0);
                        byte* pBig = (byte*)((void*)bigData.Scan0);
                        int bigOffset = bigStride - bigBmp.Width * 3;
                        bool matchFound = true;
                        int y = 0;
                        while (y < bigHeight)
                        {
                            int x = 0;
                            while (x < bigWidth)
                            {
                                byte* pBigBackup = pBig;
                                byte* pSmallBackup = pSmall;
                                int i = 0;
                                while (i < smallHeight)
                                {
                                    int j = 0;
                                    matchFound = true;
                                    j = 0;
                                    while (j < smallWidth)
                                    {
                                        int inf = *pBig - margin;
                                        if ((*pBig + margin < *pSmall ? false : inf <= *pSmall))
                                        {
                                            pBig++;
                                            pSmall++;
                                            j++;
                                        }
                                        else
                                        {
                                            matchFound = false;
                                            break;
                                        }
                                    }
                                    if (matchFound)
                                    {
                                        pSmall = pSmallBackup;
                                        pBig = pBigBackup;
                                        pSmall = pSmall + smallStride * (1 + i);
                                        pBig = pBig + bigStride * (1 + i);
                                        i++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (!matchFound)
                                {
                                    pBig = pBigBackup;
                                    pSmall = pSmallBackup;
                                    pBig = pBig + 3;
                                    x++;
                                }
                                else
                                {
                                    location.X = x;
                                    location.Y = y;
                                    location.Width = smallBmp.Width;
                                    location.Height = smallBmp.Height;
                                    break;
                                }
                            }
                            if (!matchFound)
                            {
                                pBig = pBig + bigOffset;
                                y++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        bigBmp.UnlockBits(bigData);
                        smallBmp.UnlockBits(smallData);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        //this.log.Error(exception.Message);
                    }
                }
                finally
                {
                    GC.Collect(0);
                    Application.DoEvents();
                }
                empty = location;
            }
            else
            {
                empty = Rectangle.Empty;
            }
            return empty;
        }
        //Added obsolete
        [Obsolete]
        public Bitmap ApplicationScreenSnapshot()
        {
            Bitmap bitmap;
            try
            {
                try
                {
                    Thread.Sleep(1000);
                    Rectangle bounds = Screen.PrimaryScreen.Bounds;
                    int width = bounds.Width;
                    bounds = Screen.PrimaryScreen.Bounds;
                    Bitmap bitmap1 = new Bitmap(width, bounds.Height, PixelFormat.Format32bppArgb);
                    Graphics screenShot = Graphics.FromImage(bitmap1);
                    bounds = Screen.PrimaryScreen.Bounds;
                    int x = bounds.X;
                    bounds = Screen.PrimaryScreen.Bounds;
                    int y = bounds.Y;
                    bounds = Screen.PrimaryScreen.Bounds;
                    //ThreadInvoker.Instance.RunByUiThread(() =>
                    //{
                        screenShot.CopyFromScreen(x, y, 0, 0, bounds.Size, CopyPixelOperation.SourceCopy);
                    //});
                    bitmap = bitmap1;
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    if (this.IsCritical(ex))
                    {
                        throw;
                    }
                    //this.log.Error(ex.StackTrace);
                    bitmap = null;
                }
            }
            finally
            {
                GC.Collect(0);
                Application.DoEvents();
            }
            return bitmap;
        }
        /// <summary>
        /// Added obsolete attribute since  ExecutionEngineException is absolete
        /// vinay D.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        [Obsolete]
        public bool IsCritical(Exception ex)
        {
            bool flag;
            if (ex is OutOfMemoryException)
            {
                flag = true;
            }
            else if (ex is AppDomainUnloadedException)
            {
                flag = true;
            }
            else if (ex is BadImageFormatException)
            {
                flag = true;
            }
            else if (ex is CannotUnloadAppDomainException)
            {
                flag = true;
            }
            else if (ex is ExecutionEngineException)
            {
                flag = true;
            }
            else if (!(ex is InvalidProgramException))
            {
                flag = (!(ex is ThreadAbortException) ? false : true);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}
