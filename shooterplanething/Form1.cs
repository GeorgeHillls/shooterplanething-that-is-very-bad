using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace shooterplanething
{
    public partial class Form1 : Form
    {
        //sound things
        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootMedia;
        WindowsMediaPlayer explosion;

        //picture box tings
        PictureBox[] enemM;
        int enemMspeed; 

        PictureBox[] stars;
        int bgSpeed;
        int Pspeed;

        PictureBox[] munitions;
        int Mspeed;

        PictureBox[] enemies;
        int Espeed;

        Random rnd;

        int score;
        int level;
        int diff; //difficulty
        bool pause;
        bool GameIsOver;

        public Form1()
        {
            InitializeComponent();
        }

        //basically most things
        private void Form1_Load(object sender, EventArgs e)
        {
            //set game stuff
            pause = false;
            GameIsOver = false;
            score = 0;
            level = 1;
            diff = 9;

            //set speeds
            bgSpeed = 4;
            Pspeed = 5;
            Mspeed = 20;
            Espeed = 4;
            enemMspeed = 4;
            
            munitions = new PictureBox[3];

            //load image ting innit for the bad people
            Image E1 = Image.FromFile("assets\\E1.png");
            Image E2 = Image.FromFile("assets\\E2.png");
            Image E3 = Image.FromFile("assets\\E3.png");
            Image B1 = Image.FromFile("assets\\Boss1.png");
            Image B2 = Image.FromFile("assets\\Boss1.png");

            enemies = new PictureBox[10];

            //initialise enemies picutre boxesssssssssss
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle = BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50, -50);
            }

            //assign images to each enemy?
            enemies[0].Image = B1;
            enemies[1].Image = E2;
            enemies[2].Image = E3;
            enemies[3].Image = E3;
            enemies[4].Image = E1;
            enemies[5].Image = E3;
            enemies[6].Image = E2;
            enemies[7].Image = E3;
            enemies[8].Image = E2;
            enemies[9].Image = B2;





            //load munition image
            Image munition = Image.FromFile(@"assets\munition.png");

            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i] = new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode = PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            //crfeatn wmp
            gameMedia = new WindowsMediaPlayer();
            shootMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //looad all songs
            gameMedia.URL = "songs\\GameSong.mp3";
            shootMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            //setup song settings
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 1;
            shootMedia.settings.volume = 1;
            explosion.settings.volume = 6;


            
            stars = new PictureBox[15];
            rnd = new Random();

            //star stuff

            for (int i = 0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i % 2 == 1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;

                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);
            }
            // enemy munition
            enemM = new PictureBox[10];

            for (int i = 0; i < enemM.Length; i++)
            {
                enemM[i] = new PictureBox();
                enemM[i].Size = new Size(2, 25);
                enemM[i].Visible = false;
                enemM[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemM[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemM[i]); 
            }
            
            
            gameMedia.controls.play();


        }

        // stars moving and background
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length / 2; i++)
            {
                stars[i].Top += bgSpeed;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += bgSpeed -2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        //idek
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        //left
        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left > 10)
            {
                Player.Left -= Pspeed;
            }
        }
        //right
        private void RightMoverTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Right < 580)
            {
                Player.Left += Pspeed;
            }
        }
        //down
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += Pspeed;
            }
        }
        //up
        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top > 10 )
            {
                Player.Top -= Pspeed;
            }
        }

        // key stuff for movement and pause etc
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoverTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }
            

        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoverTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if (!GameIsOver)
                {
                    if (pause)
                    {
                        StartTimers();
                        label1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 - 120, 150);
                        label1.Text = "PAUSED BUDDY";
                        label1.Visible = true;
                        gameMedia.controls.pause();
                        StopTimers();
                        pause = true;
                    }
                }
            }
            
        }

        //move bullets timer
        private void MMT_Tick(object sender, EventArgs e)
        {
            shootMedia.controls.play();
            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top > 0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= Mspeed;

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location = new Point(Player.Location.X + 20, Player.Location.Y - i * 30);

                }
            }
        }

        //move enemy timer
        private void MET_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, Espeed);
        }

        //enemy move stuff
        private void MoveEnemies(PictureBox[] array, int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;

                if (array[i].Top > this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        //collision stuff
        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds)
                    || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.controls.play();
                    score += 1;
                    scorelbl.Text = (score < 10) ? "0" + score.ToString() : score.ToString();

                    if (score % 30 == 0)
                    {
                        level += 1;
                        levellbl.Text = (level < 10) ? "0" + level.ToString() : level.ToString();

                        if (Espeed <= 10 && enemMspeed <=10 && diff >= 0)
                        {
                            diff--;
                            Espeed++;
                            enemMspeed++;

                        }

                        if (level == 10)
                        {
                            GameOver("Nice work person");
                        }
                    }
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("");
                }
            }
        }
        //gameover stuff
        private void GameOver(String str)
        {
            label1.Text = str;
            label1.Location = new Point(120, 120);
            label1.Text = str.Trim();
            label1.Visible = true;
            GameIsOver = true;
            pause = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;


            gameMedia.controls.stop();
            StopTimers();
        }
        //stoptimers
        private void StopTimers()
        {
            MoverBigTimer.Stop(); //do this
            MET.Stop();
            MMT.Stop(); //may need to change
            EMT.Stop();
        }

        //start timers
        private void StartTimers()
        {
            MoverBigTimer.Start(); //do this
            MET.Start();
            MMT.Start();
            EMT.Start();
        }

        //enemy munition timer stuff
        private void EMT_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < (enemM.Length - diff); i++)
            {
                if (enemM[i].Top < this.Height) 
                {
                    enemM[i].Visible = true;
                    enemM[i].Top += enemMspeed;

                    CollisionWithEnemM();
                }
                else
                {
                    enemM[i].Visible = false;
                    int x = rnd.Next(0, 10);
                    enemM[i].Location = new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        //player hits enemy munition stuff
        private void CollisionWithEnemM()
        {
            for (int i = 0; i < enemM.Length; i++)
            {
                if (enemM[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemM[i].Visible = false;
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible = false;
                    GameOver("Game Over L");
                }
            }
        }

        //label
        private void label1_Click(object sender, EventArgs e)
        {
             

        }

        //replay button
        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }

        //exit button
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void levellbl_Click(object sender, EventArgs e)
        {

        }
    }
}
