using System;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.IO;
namespace Snake
{
    public partial class Form1 : Form
    {
        static Random rnd = new Random();
        static bool MovePermission, MP, Optback = true, Wall, Square = false, Continues1 = true, Continues2 = true;
        static int z = 0, k = 50, t = 150, n = 4, m = 4, p1 = 1, p2 = 1, f11 = 5, f12 = 5, f13 = 5, f21 = 5, f22 = 5, f23 = 5, rh, rw, kt1 = 0, kt2 = 0, k1 = 0, k2 = 0;
        static int[] x1 = new int[150];
        static int[] y1 = new int[150];
        static int[] x2 = new int[150];
        static int[] y2 = new int[150];
        static int w = Screen.PrimaryScreen.Bounds.Width;
        static int h = Screen.PrimaryScreen.Bounds.Height;
        static System.Timers.Timer aTimer = new System.Timers.Timer(t);
        static Color hc1, hc2, bc1, bc2, BackGroundc, Wallc, feedc;
        static WMPLib.WindowsMediaPlayer wp1 = new WMPLib.WindowsMediaPlayer();
        static WMPLib.WindowsMediaPlayer wp2 = new WMPLib.WindowsMediaPlayer();

        public Form1()
        {
            InitializeComponent();
            ///////////////////////////////////Get the current account Name
            char[] slash = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToCharArray();  //
            StringBuilder name = new StringBuilder();                                                  //
            for (int i = slash.Length - 1; i >= 0; i--)                                                //  Get the current account Name
                if (slash[i] == '\\')                                                                  //
                    for (int j = i + 1; j < slash.Length; j++)                                         // 
                        name.Append(slash[j]);                                                         //
            
            Welcom.Text = "Welcom " + name.ToString();
            MenuBox.Location = new Point((w / 2) - 160, (h / 2) - 175);
            ExitBox.Location = new Point((w / 2) - 185, (h / 2) - 70);
            OptionBox.Location = new Point((w / 2) - 275, (h / 2) - 180);
            AboutBox.Location = new Point((w / 2) - 225, (h / 2) - 135);
            RestartBox.Location = new Point((w / 2) - 125, (h / 2) - 50);
            BacktomainBox.Location = new Point((w / 2) - 185, (h / 2) - 50);
            bckgrnd.Location = new Point(0, 0);
            bckgrnd.Size = new Size(w, h);
            bc1 = Color.SandyBrown;
            hc1 = Color.GreenYellow;
            bc2 = Color.Violet;
            hc2 = Color.Purple;
            BackGroundc = Color.Green;
            Wallc = Color.SaddleBrown;
            feedc = Color.GhostWhite;
            CircleRadioButton.Checked = true;
            BC1combo.SelectedItem = "(default)";
            HC1Combo.SelectedItem = "(default)";
            BC2combo.SelectedItem = "(default)";
            HC2combo.SelectedItem = "(default)";
            Wallccombo.SelectedItem = "(default)";
            Feedccombo.SelectedItem = "(default)";
            Backgroundcombo.SelectedItem = "(default)";
            label12.Text = MusicBar.Value + "%";
            label13.Text = SoundBar.Value + "%";
            if (w <= 1200)  //check the screen size to set the pieces size
                k = 35;
            statusStrip.Location = new Point(0, ((h - k) / k) * k);
            statusStrip.Size = new Size(w, h - (((h - k) / k) * k));

            UFlowLayoutPanel.Size = new Size(w, k);
            RFlowLayoutPanel.Size = new Size(k + w - ((w / k) * k), h);
            LFlowLayoutPanel.Size = new Size(k, h);

            h = (h / k) * k;
            w = (w / k) * k;

            //StreamReader FR = new StreamReader(Application.StartupPath + "\\Save\\ScreenshotCounter.txt");
            //ScreenshotCounter = int.Parse(FR.ReadToEnd());
            //FR.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ///////////////////////////////////set the default worm position
            for (int i = 0; i <= n; i++)
            {
                x1[i] = (k * i) + 2 * k;
                y1[i] = 2 * k;
            }
            if (m >= 3)
                for (int i = 0; i <= m; i++)
                {
                    x2[i] = (k * i) + +2 * k;
                    y2[i] = ((h / 2 + k) / k) * k;
                }
            ///////////////////////////////////Play background music
            if (File.Exists(Application.StartupPath + "\\Sounds\\Music.mp3"))
            {
                wp1.URL = Application.StartupPath + "\\Sounds\\Music.mp3";
                wp1.settings.volume = MusicBar.Value;
                wp1.controls.play();
            }

            ///////////////////////////////////a infinity loop "every t Second"
            aTimer.Elapsed += new ElapsedEventHandler(Main);
            aTimer.Enabled = true;
        }


        private void Main(object source, ElapsedEventArgs e)
        {
            aTimer.Interval = t;

            toolStripStatusLabel2.Text = "    |    Speed: " + (15000 / t) + "%";

            ///////////////////////////////////Repeat the music if the ends
            if (!wp1.status.StartsWith("Playing") && File.Exists(Application.StartupPath + "\\Sounds\\Music.mp3"))
            {
                wp1.URL = Application.StartupPath + "\\Sounds\\Music.mp3";
                wp1.settings.volume = MusicBar.Value;
                wp1.controls.play();
            }

            ///////////////////////////////////decreas width if there are walls
            if (Wall)
                w = Screen.PrimaryScreen.Bounds.Width - k;
            else
                w = Screen.PrimaryScreen.Bounds.Width;
            w = (w / k) * k;


            if (MP) ///////////////////////////////////MP = Move permission
            {
                Cursor.Position = new Point(0,0);
                Moving();
                ///////////////////////////////////Feed ate checker1
                if (x1[n] == rw && y1[n] == rh || (rh == 0 && rw == 0))
                {
                    kt1++;
                    n++;
                    x1[n] = x1[n - 1];
                    y1[n] = y1[n - 1];
                    Feed();
                    if (File.Exists(Application.StartupPath + "\\Sounds\\Eat1.mp3"))
                    {
                        wp2.URL = Application.StartupPath + "\\Sounds\\Eat1.mp3";
                        wp2.controls.play();
                    }
                }
                ///////////////////////////////////Feed ate checker2
                if ((x2[m] == rw && y2[m] == rh || (rh == 0 && rw == 0)) && m >= 3)
                {
                    kt2++;
                    m++;
                    x2[m] = x2[m - 1];
                    y2[m] = y2[m - 1];
                    Feed();
                    if (File.Exists(Application.StartupPath + "\\Sounds\\Eat2.mp3"))
                    {
                        wp2.URL = Application.StartupPath + "\\Sounds\\Eat2.mp3";
                        wp2.controls.play();
                    }
                }
            }
            if (m <= 3)
                toolStripStatusLabel1.Text = " Score:  " + (n + 1) + "     Dies:  " + k1 + "     Total Score:  " + kt1;
            else
            {
                if (w <= 1200)
                    toolStripStatusLabel1.Text = " Score 1:  " + (n + 1) + "     Dies 1:  " + k1 + "     Total Score 1:  " + kt1 + "\n Score 2:  " + (m + 1) + "     Dies 2:  " + k2 + "     Total Score 2:  " + kt2;
                else
                    toolStripStatusLabel1.Text = " Score 1:  " + (n + 1) + "     Dies 1:  " + k1 + "     Total Score 1:  " + kt1 + "        |        Score 2:  " + (m + 1) + "     Dies 2:  " + k2 + "     Total Score 2:  " + kt2;
            }
            Draw();
        }



        private void Moving()
        {
            Graphics body = this.CreateGraphics();

            ///////////////////////////////////earase the last piece
            if (!Square)
            {
                body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x1[0], y1[0], k, k));
                body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x1[0] + x1[1]) / 2, (y1[0] + y1[1]) / 2, k, k));
                body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x2[0], y2[0], k, k));
                body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x2[0] + x2[1]) / 2, (y2[0] + y2[1]) / 2, k, k));
            }
            else
            {
                body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x1[0], y1[0], k, k));
                body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x1[0] + x1[1]) / 2, (y1[0] + y1[1]) / 2, k, k));
                body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x2[0], y2[0], k, k));
                body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x2[0] + x2[1]) / 2, (y2[0] + y2[1]) / 2, k, k));
            }

            ///////////////////////////////////Swap pieces
            for (int i = 1; i <= n; i++)
            {
                x1[i - 1] = x1[i];
                y1[i - 1] = y1[i];
            }
            if (m >= 3)
                for (int i = 1; i <= m; i++)
                {
                    x2[i - 1] = x2[i];
                    y2[i - 1] = y2[i];
                }

            ///////////////////////////////////Move direction check
            switch (f11)
            {
                case 0:
                    x1[n] -= k;
                    break;
                case 1:
                    x1[n] += k;
                    break;
                case 2:
                    y1[n] -= k;
                    break;
                case 3:
                    y1[n] += k;
                    break;
                default:
                    x1[n] += k;
                    break;
            }
            if (m >= 3)
                switch (f21)
                {
                    case 0:
                        x2[m] -= k;
                        break;
                    case 1:
                        x2[m] += k;
                        break;
                    case 2:
                        y2[m] -= k;
                        break;
                    case 3:
                        y2[m] += k;
                        break;
                    default:
                        x2[m] += k;
                        break;
                }


            ///////////////////////////////////Read key buffer
            if (f12 != 5)
            {
                f11 = f12;
                if (f13 != 5)
                {
                    f12 = f13;
                    f13 = 5;
                }
                else f12 = 5;
            }

            if (f22 != 5)
            {
                f21 = f22;
                if (f23 != 5)
                {
                    f22 = f23;
                    f23 = 5;
                }
                else f22 = 5;
            }

            ///////////////////////////////////Check accidents
            for (int i = 0; i < n; i++)
                if (x1[n] == x1[i] && y1[n] == y1[i])
                    nDie();

            if (m >= 3)
            {
                for (int i = 0; i < m; i++)
                    if (x2[m] == x2[i] && y2[m] == y2[i])
                        mDie();

                for (int i = 0; i <= n; i++)
                    if (x2[m] == x1[i] && y2[m] == y1[i])
                        mDie();

                for (int i = 0; i <= m; i++)
                    if (x1[n] == x2[i] && y1[n] == y2[i])
                        nDie();

                for (int i = 0; i <= n; i++)
                    for (int j = 0; j <= m; j++)
                        if (x2[j] == x1[i] && y2[j] == y1[i])
                        {
                            nDie();
                            mDie();
                        }
            }


            ///////////////////////////////////Cross OR Crash!!!?
            if (Wall)
            {
                if ((y1[n] > (h - (2 * k))) || (y1[n] < k) || (x1[n] > (w - k)) || (x1[n] < k))
                    nDie();

                if (m >= 3 && ((y2[m] > (h - (2 * k))) || (y2[m] < k) || (x2[m] > (w - k)) || (x2[m] < k)))
                    mDie();
            }
            else
            {
                if (y1[n] > (h - (2 * k))) y1[n] = 0;
                if (y1[n] < 0) y1[n] = h - 2 * k;
                if (x1[n] > (w - k)) x1[n] = 0;
                if (x1[n] < 0) x1[n] = w - k;

                if (m >= 3)
                {
                    if (y2[m] > (h - (2 * k))) y2[m] = 0;
                    if (y2[m] < 0) y2[m] = h - (2 * k);
                    if (x2[m] > (w - k)) x2[m] = 0;
                    if (x2[m] < 0) x2[m] = w - k;
                }
            }

        }

        private void nDie()
        {
            Graphics body = this.CreateGraphics();
            if (File.Exists(Application.StartupPath + "\\Sounds\\Crash.mp3"))
            {
                wp2.URL = Application.StartupPath + "\\Sounds\\Crash.mp3";
                wp2.controls.play();
            }

            if (!Square)
                for (int i = 0; i <= n; i++)
                {
                    body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x1[i], y1[i], k, k));
                    body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x1[i] + x1[i + 1]) / 2, (y1[i] + y1[i + 1]) / 2, k, k));
                }
            else
                for (int i = 0; i <= n; i++)
                {
                    body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x1[i], y1[i], k, k));
                    body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x1[i] + x1[i + 1]) / 2, (y1[i] + y1[i + 1]) / 2, k, k));
                }

            //HighScore[50] = (n + 1).ToString() + "\n";
            //for (int i = 0; i < 50; i++)
            //    HighScore[i] = HighScore[i + 1];
            

            k1++;
            n = 4;
            f11 = 1;
            f12 = 5;
            f13 = 5;
            p1 = 1;
            MP = false;
            for (int i = 0; i <= n; i++)
            {
                x1[i] = (k * i) + (2 * k);
                y1[i] = 2 * k;
            }
        }

        private void mDie()
        {
            Graphics body = this.CreateGraphics();
            if (File.Exists(Application.StartupPath + "\\Sounds\\Crash.mp3"))
            {
                wp2.URL = Application.StartupPath + "\\Sounds\\Crash.mp3";
                wp2.controls.play();
            }

            if (!Square)
                for (int i = 0; i <= m; i++)
                {
                    body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x2[i], y2[i], k, k));
                    body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x2[i] + x2[i + 1]) / 2, (y2[i] + y2[i + 1]) / 2, k, k));
                }
            else
                for (int i = 0; i <= m; i++)
                {
                    body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(x2[i], y2[i], k, k));
                    body.FillRectangle(new System.Drawing.SolidBrush(BackGroundc), new Rectangle((x2[i] + x2[i + 1]) / 2, (y2[i] + y2[i + 1]) / 2, k, k));
                }

            k2++;
            m = 4;
            f21 = 1;
            f22 = 5;
            f23 = 5;
            p2 = 1;
            MP = false;
            for (int i = 0; i <= m; i++)
            {
                x2[i] = (k * i) + (2 * k);
                y2[i] = ((h / 2 + k) / k) * k;
            }
        }


        private void Draw()
        {
            Graphics body = this.CreateGraphics();

            if (!Square)
            {
                for (int i = 0; i < n; i++)
                {
                    body.FillEllipse(new System.Drawing.SolidBrush(bc1), new Rectangle(x1[i], y1[i], k, k));                                    //Player1 Body
                    if (Continues1 && Math.Abs(x1[i] - x1[i + 1]) < 2 * k && Math.Abs(y1[i] - y1[i + 1]) < 2 * k)
                        body.FillEllipse(new System.Drawing.SolidBrush(bc1), new Rectangle((x1[i] + x1[i + 1]) / 2, (y1[i] + y1[i + 1]) / 2, k, k));       //Player1 Body
                }

                if (m >= 3)
                    for (int i = 0; i < m; i++)
                    {
                        body.FillEllipse(new System.Drawing.SolidBrush(bc2), new Rectangle(x2[i], y2[i], k, k));                                              //Player2 Body
                        if (Continues2 && Math.Abs(x2[i] - x2[i + 1]) < 2 * k && Math.Abs(y2[i] - y2[2 + 1]) < 2 * k)
                            body.FillEllipse(new System.Drawing.SolidBrush(bc2), new Rectangle((x2[i] + x2[i + 1]) / 2, (y2[i] + y2[i + 1]) / 2, k, k));      //Player2 Body
                    }

                body.FillEllipse(new System.Drawing.SolidBrush(hc1), new Rectangle(x1[n], y1[n], k, k));           //Player1 Head
                if (m >= 3)
                    body.FillEllipse(new System.Drawing.SolidBrush(hc2), new Rectangle(x2[m], y2[m], k, k));       //Player2 Head

                body.FillEllipse(new System.Drawing.SolidBrush(feedc), new Rectangle(rw, rh, k, k));               //Feed
            }
            else
            {
                for (int i = 0; i < n; i++)
                    body.FillRectangle(new System.Drawing.SolidBrush(bc1), new Rectangle(x1[i], y1[i], k, k));       //Player1 Body
                if (m >= 3)
                    for (int i = 0; i < m; i++)
                        body.FillRectangle(new System.Drawing.SolidBrush(bc2), new Rectangle(x2[i], y2[i], k, k));   //Player2 Body

                body.FillRectangle(new System.Drawing.SolidBrush(hc1), new Rectangle(x1[n], y1[n], k, k));           //Player1 Head
                if (m >= 3)
                    body.FillRectangle(new System.Drawing.SolidBrush(hc2), new Rectangle(x2[m], y2[m], k, k));       //Player2 Head

                body.FillRectangle(new System.Drawing.SolidBrush(feedc), new Rectangle(rw, rh, k, k));               //Feed
            }

        }


        private void Feed()
        {
            Graphics body = this.CreateGraphics();
            body.FillEllipse(new System.Drawing.SolidBrush(BackGroundc), new Rectangle(rw, rh, k, k));
        a:
            rw = (rnd.Next(0, w / k) * k);
            rh = (rnd.Next(0, (h - k) / k) * k);


            ///////////////////////////////////Feed on the body!!!?
            for (int i = 0; i <= n; i++)
                if (rw == x1[i] && rh == y1[i])
                    goto a;

            if (m >= 3)
                for (int i = 0; i <= m; i++)
                    if (rw == x2[i] && rh == y2[i])
                        goto a;

            if (Wall && (rw <= k + 10 || rw >= (w - k + 10) || rh <= k + 10 || rh >= (h - k + 10)))  //Feed on the wall!!?
                goto a;
        }



        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (MovePermission)
            {
                if ((e.KeyCode == Keys.Left) && p1 != 1)
                    p1 = 0;
                if ((e.KeyCode == Keys.Right) && p1 != 0)
                    p1 = 1;
                if ((e.KeyCode == Keys.Up) && p1 != 3)
                    p1 = 2;
                if ((e.KeyCode == Keys.Down) && p1 != 2)
                    p1 = 3;

                if (e.KeyCode == Keys.A && p2 != 1)
                    p2 = 0;
                if (e.KeyCode == Keys.D && p2 != 0)
                    p2 = 1;
                if (e.KeyCode == Keys.W && p2 != 3)
                    p2 = 2;
                if (e.KeyCode == Keys.S && p2 != 2)
                    p2 = 3;

                if (f12 == 5) f12 = p1;
                else f13 = p1;

                if (m >= 3)
                    if (f22 == 5) f22 = p2;
                    else f23 = p2;


                if (e.KeyCode == Keys.Subtract && t <= 3500)
                    t += 10;
                if (e.KeyCode == Keys.Add && t >= 20)
                    t -= 10;
                if (e.KeyCode == Keys.Multiply)
                    t = 150;

                if (e.KeyCode == Keys.Left
                    || e.KeyCode == Keys.A
                    || e.KeyCode == Keys.Right
                    || e.KeyCode == Keys.D
                    || e.KeyCode == Keys.Up
                    || e.KeyCode == Keys.W
                    || e.KeyCode == Keys.Down
                    || e.KeyCode == Keys.S)
                    MP = true;
            }

            if (e.KeyCode == Keys.F10)
            {
                if (!File.Exists(Application.StartupPath + "\\ScreenShots\\"))
                    Directory.CreateDirectory(Application.StartupPath + "\\ScreenShots\\");
                int i = 1;
                while (File.Exists(Application.StartupPath + "\\ScreenShots\\Screenshot" + i + ".png"))
                    i++;
                var bmpScreenshot = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var gfxScreenshot = System.Drawing.Graphics.FromImage(bmpScreenshot);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                bmpScreenshot.Save(Application.StartupPath + "\\ScreenShots\\Screenshot" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }

            if (e.KeyCode == Keys.Escape)
            {
                if (MovePermission)
                    PauseMenu(true);
                else if (MenuBox.Visible)
                    PauseMenu(false);
                if (ExitBox.Visible)
                {
                    ExitBox.Visible = false;
                    ExitBox.Enabled = false;
                    if (!StartPanel.Visible)
                        PauseMenu(true);
                }
                if (OptionBox.Visible)
                {
                    OptionBox.Visible = false;
                    OptionBox.Enabled = false;
                    if (Optback)
                    {
                        StartPanel.Visible = true;
                        StartPanel.Enabled = true;
                    }
                    else
                    {
                        MenuBox.Visible = true;
                        MenuBox.Enabled = true;
                    }
                }
            }

        }


        private void PauseMenu(bool o)
        {
            bckgrnd.Visible = o;
            bckgrnd.Enabled = o;
            MenuBox.Visible = o;
            MenuBox.Enabled = o;
            MovePermission = !o;
            if (o)
            {
                MP = !o;
                MenuBox.Select();
            }
        }


        private void Resume_Click(object sender, EventArgs e)
        {
            PauseMenu(false);
        }
        private void Restart_Click(object sender, EventArgs e)
        {
            MenuBox.Enabled = false;
            RestartBox.Visible = true;
            RestartBox.Enabled = true;
        }


        private void RNo_Click(object sender, EventArgs e)
        {
            RestartBox.Visible = false;
            RestartBox.Enabled = false;
            MenuBox.Enabled = true;
        }

        private void RYes_Click(object sender, EventArgs e)
        {
            Feed();
            t = 150;
            k1 = 0;
            kt1 = 0;
            n = 4;
            p1 = 1;
            f11 = 1;
            f12 = 5;
            f13 = 5;
            for (int i = 0; i <= n; i++)
            {
                x1[i] = (k * i) + (2 * k);
                y1[i] = 2 * k;
            }
            if (m >= 3)
            {
                for (int i = 0; i <= m; i++)
                {
                    x2[i] = (k * i) + (2 * k);
                    y2[i] = ((h / 2 + k) / k) * k;
                }
                m = 4;
                p2 = 1;
                f21 = 1;
                f22 = 5;
                f23 = 5;
                kt2 = 0;
                k2 = 0;
            }
            else m = 0;

            RestartBox.Visible = false;
            RestartBox.Enabled = false;
            PauseMenu(false);
        }



        private void Option_Click(object sender, EventArgs e)
        {
            OptionBox.Enabled = true;
            OptionBox.Visible = true;
            MenuBox.Visible = false;
            MenuBox.Enabled = false;
            Optback = false;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            ExitBox.Visible = true;
            ExitBox.Enabled = true;
            MenuBox.Visible = false;
            MenuBox.Enabled = false;
        }


        private void No_Click(object sender, EventArgs e)
        {
            ExitBox.Visible = false;
            ExitBox.Enabled = false;
            if (!StartPanel.Visible)
                PauseMenu(true);
            else
                StartPanel.Enabled = true;
        }


        private void Yes_Click(object sender, EventArgs e)
        {
            //StreamWriter FW = new StreamWriter(Application.StartupPath + "\\Save\\ScreenshotCounter.txt");
            //FW.Write(ScreenshotCounter);
            //FW.Close();

            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OptionBox.Visible = false;
            OptionBox.Enabled = false;
            if (Optback)
            {
                StartPanel.Visible = true;
                StartPanel.Enabled = true;
            }
            else
            {
                MenuBox.Visible = true;
                MenuBox.Enabled = true;
            }
        }

        private void SExit_Click(object sender, EventArgs e)
        {
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            AboutBox.Visible = false;
            AboutBox.Enabled = false;
            ExitBox.Visible = !ExitBox.Visible;
            ExitBox.Enabled = !ExitBox.Enabled;
        }

        private void SOption_Click(object sender, EventArgs e)
        {
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            ExitBox.Visible = false;
            ExitBox.Enabled = false;
            StartPanel.Visible = false;
            StartPanel.Enabled = false;
            AboutBox.Visible = false;
            AboutBox.Enabled = false;
            OptionBox.Enabled = true;
            OptionBox.Visible = true;
            Optback = true;
        }


        private void OnePlayer_Click(object sender, EventArgs e)
        {
            m = 0;
            f21 = 5;
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            OptionBox.Visible = false;
            ExitBox.Visible = false;
            ExitBox.Enabled = false;
            AboutBox.Visible = false;
            AboutBox.Enabled = false;
            OnePlayerPanel.Visible = !OnePlayerPanel.Visible;
            OnePlayerPanel.Enabled = !OnePlayerPanel.Enabled;
        }


        private void TwoPlayer_Click(object sender, EventArgs e)
        {
            m = 4;
            f21 = 1;
            OptionBox.Visible = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            ExitBox.Visible = false;
            ExitBox.Enabled = false;
            AboutBox.Visible = false;
            AboutBox.Enabled = false;
            TwoPlayerPanel.Visible = !TwoPlayerPanel.Visible;
            TwoPlayerPanel.Enabled = !TwoPlayerPanel.Enabled;
        }

        private void backToMainMenu_Click(object sender, EventArgs e)
        {
            BacktomainBox.Visible = true;
            BacktomainBox.Enabled = true;
            MenuBox.Enabled = false;
        }


        private void BNo_Click(object sender, EventArgs e)
        {
            BacktomainBox.Visible = false;
            BacktomainBox.Enabled = false;
            MenuBox.Enabled = true;
        }

        private void BYes_Click(object sender, EventArgs e)
        {
            Feed();
            t = 150;
            k1 = 0;
            k2 = 0;
            kt1 = 0;
            kt2 = 0;
            n = 4;
            m = 4;
            p1 = 1;
            f11 = 1;
            f12 = 5;
            f13 = 5;
            p2 = 1;
            f21 = 1;
            f22 = 5;
            f23 = 5;
            for (int i = 0; i <= n; i++)
            {
                x1[i] = (k * i) + (2 * k);
                y1[i] = 2 * k;
            }

            for (int i = 0; i <= m; i++)
            {
                x2[i] = (k * i) + (2 * k);
                y2[i] = ((h / 2 + k) / k) * k;
            }
            BacktomainBox.Visible = false;
            BacktomainBox.Enabled = false;
            MenuBox.Visible = false;
            MenuBox.Enabled = false;
            StartPanel.Visible = true;
            StartPanel.Enabled = true;

        }


        private void Classic1_Click(object sender, EventArgs e)
        {
            Wall = true;
            DrawWall(Wall);
            StartPanel.Visible = false;
            StartPanel.Enabled = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            PauseMenu(false);
        }

        private void Classic2_Click(object sender, EventArgs e)
        {
            Wall = true;
            DrawWall(Wall);
            StartPanel.Visible = false;
            StartPanel.Enabled = false;
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            PauseMenu(false);
        }

        private void Unlimited1_Click(object sender, EventArgs e)
        {
            Wall = false;
            DrawWall(Wall);
            StartPanel.Visible = false;
            StartPanel.Enabled = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            PauseMenu(false);
        }



        private void Unlimited2_Click(object sender, EventArgs e)
        {
            Wall = false;
            DrawWall(Wall);
            StartPanel.Visible = false;
            StartPanel.Enabled = false;
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            PauseMenu(false);
        }
        private void DrawWall(bool Wall)
        {
            Feed();
            UFlowLayoutPanel.Visible = Wall;
            UFlowLayoutPanel.Enabled = Wall;
            LFlowLayoutPanel.Visible = Wall;
            LFlowLayoutPanel.Enabled = Wall;
            RFlowLayoutPanel.Visible = Wall;
            RFlowLayoutPanel.Enabled = Wall;
        }

        private void About_Click(object sender, EventArgs e)
        {
            TwoPlayerPanel.Visible = false;
            TwoPlayerPanel.Enabled = false;
            OnePlayerPanel.Visible = false;
            OnePlayerPanel.Enabled = false;
            ExitBox.Visible = false;
            ExitBox.Enabled = false;
            OptionBox.Enabled = false;
            OptionBox.Visible = false;
            AboutBox.Visible = !AboutBox.Visible;
            AboutBox.Enabled = !AboutBox.Enabled;

        }

        private void HC1combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HC1Combo.SelectedItem == "(default)")
                hc1 = Color.GreenYellow;
            if (HC1Combo.SelectedItem == "Red")
                hc1 = Color.Red;
            if (HC1Combo.SelectedItem == "Yellow")
                hc1 = Color.Yellow;
            if (HC1Combo.SelectedItem == "Brown")
                hc1 = Color.Brown;
            if (HC1Combo.SelectedItem == "Black")
                hc1 = Color.Black;
            if (HC1Combo.SelectedItem == "Blue")
                hc1 = Color.Blue;
            if (HC1Combo.SelectedItem == "Green")
                hc1 = Color.Green;
            ColorSameAsBackgroundCheck();
        }

        private void BC1combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BC1combo.SelectedItem == "(default)")
                bc1 = Color.SandyBrown;
            if (BC1combo.SelectedItem == "Red")
                bc1 = Color.Red;
            if (BC1combo.SelectedItem == "Yellow")
                bc1 = Color.Yellow;
            if (BC1combo.SelectedItem == "Brown")
                bc1 = Color.Brown;
            if (BC1combo.SelectedItem == "Black")
                bc1 = Color.Black;
            if (BC1combo.SelectedItem == "Blue")
                bc1 = Color.Blue;
            if (BC1combo.SelectedItem == "Green")
                bc1 = Color.Green;
            ColorSameAsBackgroundCheck();
        }

        private void HC2combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HC2combo.SelectedItem == "(default)")
                hc2 = Color.Purple;
            if (HC2combo.SelectedItem == "Red")
                hc2 = Color.Red;
            if (HC2combo.SelectedItem == "Yellow")
                hc2 = Color.Yellow;
            if (HC2combo.SelectedItem == "Brown")
                hc2 = Color.Brown;
            if (HC2combo.SelectedItem == "Black")
                hc2 = Color.Black;
            if (HC2combo.SelectedItem == "Blue")
                hc2 = Color.Blue;
            if (HC2combo.SelectedItem == "Green")
                hc2 = Color.Green;
            ColorSameAsBackgroundCheck();
        }

        private void BC2combo_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (BC2combo.SelectedItem == "(default)")
                bc2 = Color.Violet;
            if (BC2combo.SelectedItem == "Red")
                bc2 = Color.Red;
            if (BC2combo.SelectedItem == "Yellow")
                bc2 = Color.Yellow;
            if (BC2combo.SelectedItem == "Brown")
                bc2 = Color.Brown;
            if (BC2combo.SelectedItem == "Black")
                bc2 = Color.Black;
            if (BC2combo.SelectedItem == "Blue")
                bc2 = Color.Blue;
            if (BC2combo.SelectedItem == "Green")
                bc2 = Color.Green;
            ColorSameAsBackgroundCheck();
        }

        private void Backgroundcombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Backgroundcombo.SelectedItem == "(default)")
                BackGroundc = Color.Green;
            if (Backgroundcombo.SelectedItem == "Red")
                BackGroundc = Color.Red;
            if (Backgroundcombo.SelectedItem == "Yellow")
                BackGroundc = Color.Yellow;
            if (Backgroundcombo.SelectedItem == "Brown")
                BackGroundc = Color.Brown;
            if (Backgroundcombo.SelectedItem == "Black")
                BackGroundc = Color.Black;
            if (Backgroundcombo.SelectedItem == "Blue")
                BackGroundc = Color.Blue;
            if (Backgroundcombo.SelectedItem == "Green")
                BackGroundc = Color.Green;

            this.ForeColor = BackGroundc;
            this.BackColor = BackGroundc;
            ColorSameAsBackgroundCheck();
        }

        private void Feedccombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Feedccombo.SelectedItem == "(default)")
                feedc = Color.GhostWhite;
            if (Feedccombo.SelectedItem == "Red")
                feedc = Color.Red;
            if (Feedccombo.SelectedItem == "Yellow")
                feedc = Color.Yellow;
            if (Feedccombo.SelectedItem == "Brown")
                feedc = Color.Brown;
            if (Feedccombo.SelectedItem == "Black")
                feedc = Color.Black;
            if (Feedccombo.SelectedItem == "Blue")
                feedc = Color.Blue;
            if (Feedccombo.SelectedItem == "Green")
                feedc = Color.Green;
            ColorSameAsBackgroundCheck();
        }

        private void Wallccombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Wallccombo.SelectedItem == "(default)")
                Wallc = Color.SaddleBrown;
            if (Wallccombo.SelectedItem == "Red")
                Wallc = Color.Red;
            if (Wallccombo.SelectedItem == "Yellow")
                Wallc = Color.Yellow;
            if (Wallccombo.SelectedItem == "Brown")
                Wallc = Color.Brown;
            if (Wallccombo.SelectedItem == "Black")
                Wallc = Color.Black;
            if (Wallccombo.SelectedItem == "Blue")
                Wallc = Color.Blue;
            if (Wallccombo.SelectedItem == "Green")
                Wallc = Color.Green;

            UFlowLayoutPanel.ForeColor = Wallc;
            UFlowLayoutPanel.BackColor = Wallc;
            RFlowLayoutPanel.ForeColor = Wallc;
            RFlowLayoutPanel.BackColor = Wallc;
            LFlowLayoutPanel.ForeColor = Wallc;
            LFlowLayoutPanel.BackColor = Wallc;
            ColorSameAsBackgroundCheck();
        }

        void ColorSameAsBackgroundCheck()
        {
            if (HC1Combo.SelectedItem == Backgroundcombo.SelectedItem && HC1Combo.SelectedItem != "(default)" ||
                BC1combo.SelectedItem == Backgroundcombo.SelectedItem && BC1combo.SelectedItem != "(default)" ||
                HC2combo.SelectedItem == Backgroundcombo.SelectedItem && HC2combo.SelectedItem != "(default)" ||
                BC2combo.SelectedItem == Backgroundcombo.SelectedItem && BC2combo.SelectedItem != "(default)" ||
                Feedccombo.SelectedItem == Backgroundcombo.SelectedItem && Feedccombo.SelectedItem != "(default)" ||
                Wallccombo.SelectedItem == Backgroundcombo.SelectedItem && Wallccombo.SelectedItem != "(default)")
                Label11.Text = "\"NOTICE: Dont same any of colors with background\"";
            else
                Label11.Text = "";
        }

        private void MusicBar_Scroll(object sender, EventArgs e)
        {
            wp1.settings.volume = MusicBar.Value;
            label12.Text = MusicBar.Value + "%";
        }

        private void SoundBar_Scroll(object sender, EventArgs e)
        {
            wp2.settings.volume = SoundBar.Value;
            label13.Text = SoundBar.Value + "%";
            switch (rnd.Next(0, 3))
            {
                case 0:
                    wp2.URL = Application.StartupPath + "\\Sounds\\Eat1.mp3";
                    break;
                case 1:
                    wp2.URL = Application.StartupPath + "\\Sounds\\Eat2.mp3";
                    break;
                case 2:
                    wp2.URL = Application.StartupPath + "\\Sounds\\Eat3.mp3";
                    break;
                case 3:
                    wp2.URL = Application.StartupPath + "\\Sounds\\Crash.mp3";
                    break;
                default:
                    wp2.URL = Application.StartupPath + "\\Sounds\\Eat1.mp3";
                    break;
            }
            wp2.controls.play();
        }

        private void SquareRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SquareRadioButton.Checked)
                Square = true;
            ContinuesBody1.Enabled = !Square;
            ContinuesBody2.Enabled = !Square;
        }

        private void CircleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CircleRadioButton.Checked)
                Square = false;
            ContinuesBody1.Enabled = !Square;
            ContinuesBody2.Enabled = !Square;
        }

        private void ContinuesBody1_CheckedChanged(object sender, EventArgs e)
        {
            Continues1 = ContinuesBody1.Checked;
        }

        private void ContinuesBody2_CheckedChanged(object sender, EventArgs e)
        {
            Continues2 = ContinuesBody2.Checked;
        }


    }
}