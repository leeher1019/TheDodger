using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //常數
        private const int FORM_W = 700,//畫面寬度
                          FORM_H = 500,//畫面高度
                          MAX_MONSTER = 132,//怪物數量最大值
                          MONSTER_AMOUNT = MAX_MONSTER / 4,//一方平均33隻
                          DETECTIVEtoMOVE_RANGE = 30,
                          DETECTIVE_RANGE = 40,
                          FPS = 60,
                          GUARD_AMOUNT = 16,
                          FOULLINE_NUMBER = 4,
                          COUNTER = 0,
                          WARNING_AREA = 13;

        private const float PLAY_SIZE = 10,//玩家大小
                            PLAY_SPEED = 4,
                            MONSTER_SIZE = 3,//怪物大小
                            MOVE_MONSTER_SIZE = 1,//?
                            MONSTER_SPEED = 2.5f;

        public bool bStop;

        public string live_time_string;

        private Point player;
        private Point Detective_Range,
                      DetectiveToMove_Range;

        private Point[] vGuard,
                        vFoulLine;

        private Value[] vGuard_Monster_dis_R,
                        vGuard_Monster_dis_L,
                        vGuard_Monster_dis_U,
                        Max_R,
                        Max_L,
                        Max_U,
                        Max_D,
                        Max_Sum,//進到偵測範圍內的距離總和
                        Max_Sum_Op,//?
                        vGuard_Monster_dis_D;//由下方射出的子彈至守衛的距離

        private Boolean[] vMonster_R_State_Dec,
                          vMonster_L_State_Dec,
                          vMonster_U_State_Dec,
                          vMonster_D_State_Dec;

        private Value Detective_number;
              
        private Point[] vMonster_R,
                        vMonster_R_Detect;
        private Point[] vMonster_L,
                        vMonster_L_Detect;
        private Point[] vMonster_U,
                        vMonster_U_Detect;
        private Point[] vMonster_D,
                        vMonster_D_Detect;

        private Value[] vRandom_value_R,
                        vRandom_value_L,
                        vRandom_value_U,
                        vRandom_value_D;

        private Value[] vMove_X_R,
                        vMove_Y_R,
                        vMove_X_L,
                        vMove_Y_L,
                        vMove_X_U,
                        vMove_Y_U,
                        vMove_X_D,
                        vMove_Y_D;

        private Point[] vMonster_R_Path,
                        vMonster_L_Path,
                        vMonster_U_Path,
                        vMonster_D_Path;

        private KeyState keyUp,
                         keyDown,
                         keyRight,
                         keyLeft,
                         keyStop;

        private Value live_time,
                      live_time_Millisecond,
                      live_time_Second,
                      live_time_Minute;
                      
        private DateTime now1,
                         now2;
        
        private Graphics wndGraphics;   //畫布視窗
        private Graphics backGraphics;  //背景頁的畫布
        private Bitmap backbmp;          //點陣圖
        
        private Random random;

        public Form2 form2 = new Form2();
       
        public Form1()
        {          
            InitializeComponent();
        
            wndGraphics = CreateGraphics();             //建立畫布
            backbmp = new Bitmap(FORM_W, FORM_H);        //NEW一個新的點陣圖
            backGraphics = Graphics.FromImage(backbmp);  //用點陣圖畫一個背景
            
            //player座標
            player = new Point();


            //偵測範圍
            Detective_Range = new Point();
            Detective_Range.x = player.x - ( DETECTIVE_RANGE - PLAY_SIZE ); 
            Detective_Range.y = player.y - ( DETECTIVE_RANGE - PLAY_SIZE );
            //偵測移動的範圍
            DetectiveToMove_Range = new Point();
            DetectiveToMove_Range.x = player.x - (DETECTIVEtoMOVE_RANGE - PLAY_SIZE);
            DetectiveToMove_Range.y = player.y - (DETECTIVEtoMOVE_RANGE - PLAY_SIZE);

            //怪物座標的亂數
            random = new Random();
            //怪物位移亂數

            vRandom_value_R = new Value[MONSTER_AMOUNT];
            vRandom_value_L = new Value[MONSTER_AMOUNT];
            vRandom_value_U = new Value[MONSTER_AMOUNT];
            vRandom_value_D = new Value[MONSTER_AMOUNT];
            vMove_X_R = new Value[MONSTER_AMOUNT];
            vMove_Y_R = new Value[MONSTER_AMOUNT];
            vMove_X_L = new Value[MONSTER_AMOUNT];
            vMove_Y_L = new Value[MONSTER_AMOUNT];
            vMove_X_U = new Value[MONSTER_AMOUNT];
            vMove_Y_U = new Value[MONSTER_AMOUNT];
            vMove_X_D = new Value[MONSTER_AMOUNT];
            vMove_Y_D = new Value[MONSTER_AMOUNT];

            keyUp = new KeyState(Keys.Up);
            keyDown = new KeyState(Keys.Down);
            keyRight = new KeyState(Keys.Right);
            keyLeft = new KeyState(Keys.Left);
            keyStop = new KeyState(Keys.P);
            bStop = false;

            vGuard = new Point[GUARD_AMOUNT];
            vGuard_Monster_dis_R = new Value[MONSTER_AMOUNT * GUARD_AMOUNT];
            vGuard_Monster_dis_L = new Value[MONSTER_AMOUNT * GUARD_AMOUNT];
            vGuard_Monster_dis_U = new Value[MONSTER_AMOUNT * GUARD_AMOUNT];
            vGuard_Monster_dis_D = new Value[MONSTER_AMOUNT * GUARD_AMOUNT];
            Max_R = new Value[GUARD_AMOUNT];
            Max_L = new Value[GUARD_AMOUNT];
            Max_U = new Value[GUARD_AMOUNT];
            Max_D = new Value[GUARD_AMOUNT];
            Max_Sum = new Value[GUARD_AMOUNT];
            Max_Sum_Op = new Value[GUARD_AMOUNT];
            
            //有無偵測到 的布林值
            vMonster_R_State_Dec = new Boolean[MONSTER_AMOUNT];
            vMonster_L_State_Dec = new Boolean[MONSTER_AMOUNT];
            vMonster_U_State_Dec = new Boolean[MONSTER_AMOUNT];
            vMonster_D_State_Dec = new Boolean[MONSTER_AMOUNT];

            vMonster_R_Path = new Point[MONSTER_AMOUNT];
            vMonster_L_Path = new Point[MONSTER_AMOUNT];
            vMonster_U_Path = new Point[MONSTER_AMOUNT];
            vMonster_D_Path = new Point[MONSTER_AMOUNT];

            vMonster_R_Detect = new Point[MONSTER_AMOUNT];
            vMonster_L_Detect = new Point[MONSTER_AMOUNT];
            vMonster_U_Detect = new Point[MONSTER_AMOUNT];
            vMonster_D_Detect = new Point[MONSTER_AMOUNT];

            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_R_Detect[i] = new Point();
                vMonster_L_Detect[i] = new Point();
                vMonster_U_Detect[i] = new Point();
                vMonster_D_Detect[i] = new Point();
            }

            live_time = new Value();
            live_time_Millisecond = new Value();
            live_time_Second = new Value();
            live_time_Minute = new Value();
            

            now1 = new DateTime();
            now2 = new DateTime();

            for (int i = 0; i < GUARD_AMOUNT; i++)
            {
                Max_Sum[i] = new Value();
                Max_Sum_Op[i] = new Value();
            }
           
            //邊界座標
            vFoulLine = new Point[FOULLINE_NUMBER];//point [4]

            Detective_number = new Value();//偵測數量

            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_R_State_Dec[i] = new Boolean();
                vMonster_L_State_Dec[i] = new Boolean();
                vMonster_U_State_Dec[i] = new Boolean();
                vMonster_D_State_Dec[i] = new Boolean();
                vMonster_R_State_Dec[i] = false;
                vMonster_L_State_Dec[i] = false;
                vMonster_U_State_Dec[i] = false;
                vMonster_D_State_Dec[i] = false;
            }

            //monster座標
            //右側怪物           
            vMonster_R = new Point[MONSTER_AMOUNT];
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_R[i] = new Point();
                vMonster_R[i].y = random.Next(0, FORM_H);            
            }
            
            //左側怪物
            vMonster_L = new Point[MONSTER_AMOUNT];
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_L[i] = new Point();
                vMonster_L[i].y = random.Next(0, FORM_H);
            }

            //上側怪物           
            vMonster_U = new Point[MONSTER_AMOUNT];
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_U[i] = new Point();
                vMonster_U[i].x = random.Next(0, FORM_W);
                
            }

            //下側怪物           
            vMonster_D = new Point[MONSTER_AMOUNT];
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vMonster_D[i] = new Point();
                vMonster_D[i].x = random.Next(0, FORM_W);
                
            }

            //角度亂數            
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vRandom_value_R[i] = new Value();
                vRandom_value_R[i].value = random.Next(-90, 90)*(float)(Math.PI) / 180;

                vRandom_value_L[i] = new Value();
                vRandom_value_L[i].value = random.Next(-90, 90) * (float)(Math.PI) / 180;
            }

            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                vRandom_value_U[i] = new Value();
                vRandom_value_U[i].value = random.Next(0, 180) * (float)(Math.PI) / 180;

                vRandom_value_D[i] = new Value();
                vRandom_value_D[i].value = random.Next(0, 180) * (float)(Math.PI) / 180;
            }

            //資料庫的資料夾創建、TABLE創建
            using (SQLiteConnection con = Db.getConnection())
            {
                try
                {
                    con.Open();
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandText = "create table tblDodger(PlayerName TEXT, Score INT)";
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                }
            }

            form2.ShowDialog();
            
                initial();
                timer1.Interval = 1000 / FPS;
                timer1.Start();
            
        }
        /*
        private void brestart_Click(object sender, EventArgs e)     //Restart
        {
            initial();
            timer1.Start();
            brestart.Visible = false;
            label1LiveTime.Visible = false;
        }
        */
        /*
        private void restart()
        {
            if (Form3.restart == true)
            {
                Form3.restart = false;
                initial();
                timer1.Start();
            }
        }
        */
        private void initial()      //初始
        {
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                if (vMonster_D[i] != null)
                    vMonster_D[i].y = FORM_H;
                if (vMonster_U[i] != null)
                    vMonster_U[i].y = 0;
                if (vMonster_R[i] != null)
                    vMonster_R[i].x = FORM_W;
                if (vMonster_L[i] != null)
                    vMonster_L[i].x = 0;
                player.x = FORM_W / 2;
                player.y = FORM_H / 2;
                now1 = DateTime.Now;
            }
            keyUp.KeyUp(Keys.Up);
            keyDown.KeyUp(Keys.Down);
            keyRight.KeyUp(Keys.Right);
            keyLeft.KeyUp(Keys.Left);
        } 

        private void draw()
        {
            //清圖
            backGraphics.FillRectangle(Brushes.White, 0, 0, FORM_W, FORM_H);

            //player座標
            backGraphics.FillEllipse(Brushes.Blue, player.x, player.y, PLAY_SIZE * 2, PLAY_SIZE * 2);

            //偵測區域
            /*
            for (int i = 0; i < 16; i++)
            {
                backGraphics.DrawEllipse(Pens.Purple, vGuard[i].x, vGuard[i].y, PLAY_SIZE * 2, PLAY_SIZE * 2);
                backGraphics.DrawString(i + "", SystemFonts.DefaultFont, Brushes.Black, vGuard[i].x, vGuard[i].y);
            }
            */

            //backGraphics.DrawLine(Pens.Black, 0, 400, 682, 400);
            //backGraphics.DrawLine(Pens.Black, 200, 0, 200, 460);

            //monster座標
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                if (vMonster_R[i] != null)
                {
                    backGraphics.FillEllipse(Brushes.OrangeRed, vMonster_R[i].x, vMonster_R[i].y, MONSTER_SIZE * 2, MONSTER_SIZE * 2);
                }
                if (vMonster_L[i] != null)
                {
                    backGraphics.FillEllipse(Brushes.OrangeRed, vMonster_L[i].x, vMonster_L[i].y, MONSTER_SIZE * 2, MONSTER_SIZE * 2);
                }
                if (vMonster_U[i] != null)
                {
                    backGraphics.FillEllipse(Brushes.OrangeRed, vMonster_U[i].x, vMonster_U[i].y, MONSTER_SIZE * 2, MONSTER_SIZE * 2);
                }
                if (vMonster_D[i] != null)
                {
                    backGraphics.FillEllipse(Brushes.OrangeRed, vMonster_D[i].x, vMonster_D[i].y, MONSTER_SIZE * 2, MONSTER_SIZE * 2);
                }
            }
            
            /*            
            //Guard座標
            for (int i = 0; i < GUARD_AMOUNT; i++)
            {
                if (vGuard[i] != null)
                {
                    backGraphics.DrawEllipse(Pens.Gold, vGuard[i].x, vGuard[i].y, PLAY_SIZE * 2, PLAY_SIZE * 2);
                }
            }
            */
            /*
            for (int i = 0; i < 16; i++)
            {
                if (Max_Sum[i] != null)
                    backGraphics.DrawString("Sum[" + i + "]：" + Max_Sum[i].value, SystemFonts.DefaultFont, Brushes.Black, 10, 10 + (i * 12));
            }
            */
            String time = "存活時間：" + live_time_Minute.value + ":" + live_time_Second.value + "." + live_time_Millisecond.value;
            backGraphics.DrawString(time, SystemFonts.DefaultFont, Brushes.Black, 575, 2);
            /*
            String number = "偵測子彈數：" + Detective_number.value;
            backGraphics.DrawString(number, SystemFonts.DefaultFont, Brushes.Black, 10, 202);

            String min = "最小距離：" + choose_min();
            backGraphics.DrawString(min, SystemFonts.DefaultFont, Brushes.Black, 10, 214);

            String max = "最大距離：" + choose_max();
            backGraphics.DrawString(max, SystemFonts.DefaultFont, Brushes.Black, 10, 226);*/
            /*
            backGraphics.DrawEllipse(Pens.Green, DetectiveToMove_Range.x, DetectiveToMove_Range.y, DETECTIVEtoMOVE_RANGE * 2, DETECTIVEtoMOVE_RANGE * 2);
            backGraphics.DrawEllipse(Pens.Purple, Detective_Range.x, Detective_Range.y, DETECTIVE_RANGE * 2, DETECTIVE_RANGE * 2);
            */
            backGraphics.DrawRectangle(Pens.Pink, 0, 0, 682, 460);      //(682, 460)為視窗實際長寬

            wndGraphics.DrawImageUnscaled(backbmp, 0, 0);
        }

        private void shoot_Dodge_bullet()
        {
            if (Form2.bAI_show == true)
            {
                Detective_Range.x = player.x - (DETECTIVE_RANGE - PLAY_SIZE);
                Detective_Range.y = player.y - (DETECTIVE_RANGE - PLAY_SIZE);

                DetectiveToMove_Range.x = player.x - (DETECTIVEtoMOVE_RANGE - PLAY_SIZE);
                DetectiveToMove_Range.y = player.y - (DETECTIVEtoMOVE_RANGE - PLAY_SIZE);
            }
            
            monsterMove(MONSTER_AMOUNT, vMove_X_R, vMove_Y_R, vMonster_R, vRandom_value_R, -1, 1);   //1, -1影響運算時加或減  
            monsterMove(MONSTER_AMOUNT, vMove_X_L, vMove_Y_L, vMonster_L, vRandom_value_L, 1, 1);
            monsterMove(MONSTER_AMOUNT, vMove_X_U, vMove_Y_U, vMonster_U, vRandom_value_U, 1, 1);
            monsterMove(MONSTER_AMOUNT, vMove_X_D, vMove_Y_D, vMonster_D, vRandom_value_D, 1, -1);

            createMonster(MONSTER_AMOUNT, vMonster_R, vRandom_value_R, FORM_W, random.Next(0, FORM_H), -90, 90);
            createMonster(MONSTER_AMOUNT, vMonster_L, vRandom_value_L, 0, random.Next(0, FORM_H), -90, 90);
            createMonster(MONSTER_AMOUNT, vMonster_U, vRandom_value_U, random.Next(0, FORM_W), 0, 0, 180);
            createMonster(MONSTER_AMOUNT, vMonster_D, vRandom_value_D, random.Next(0, FORM_W), FORM_H, 0, 180);
            
        }

        private void onTimer(object sender, EventArgs e)
        {
            if (Form2.bPlay == true)//選擇遊玩模式
            {
                spotWatch();
                shoot_Dodge_bullet();
                if (keyUp.isDown() == true)
                {
                    player.y -= PLAY_SPEED;
                    if (player.y < 0)
                        player.y += PLAY_SPEED;
                }
                if (keyDown.isDown() == true)
                {
                    player.y += PLAY_SPEED;
                    if (player.y > 440)
                        player.y -= PLAY_SPEED;
                }
                if (keyRight.isDown() == true)
                {
                    player.x += PLAY_SPEED;
                    if (player.x > 665)
                        player.x -= PLAY_SPEED;
                }
                if (keyLeft.isDown() == true)
                {
                    player.x -= PLAY_SPEED;
                    if (player.x < 0)
                        player.x += PLAY_SPEED;
                }
                draw();
            }
            if (Form2.bAI_show == true)//選擇電腦示範模式
            {
                spotWatch();
                shoot_Dodge_bullet();                
                detective_function();
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            keyUp.KeyDown(e.KeyCode);
            keyDown.KeyDown(e.KeyCode);
            keyLeft.KeyDown(e.KeyCode);
            keyRight.KeyDown(e.KeyCode);
            keyStop.KeyDown(e.KeyCode);
            
            if (keyStop.isDown())
            {
                timer1.Enabled = !timer1.Enabled;
                bStop = true;
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            keyUp.KeyUp(e.KeyCode);
            keyDown.KeyUp(e.KeyCode);
            keyLeft.KeyUp(e.KeyCode);
            keyRight.KeyUp(e.KeyCode);
            keyStop.KeyUp(e.KeyCode);
        }

        private void detective_function()
        {
            //偵測躲子彈
            for (int i = 0; i < GUARD_AMOUNT; i++)
            {
                vGuard[i] = new Point();
            }                        

            for (int i = 0; i < FOULLINE_NUMBER; i++)
            {
                vFoulLine[i] = new Point();
            }

            vFoulLine[0].x = 0;
            vFoulLine[0].y = FORM_H / 2;
            vFoulLine[1].x = 700;
            vFoulLine[1].y = FORM_H / 2;
            vFoulLine[2].x = FORM_W / 2;
            vFoulLine[2].y = 0;
            vFoulLine[3].x = FORM_W / 2;
            vFoulLine[3].y = 500;
            
            for (int i = 0; i < GUARD_AMOUNT; i++)
            {                
                Max_Sum[i] = new Value();
                Max_Sum_Op[i] = new Value();
            }

            sixteenball();
            OutOfRange();

            int leftSideBorder = (int)PLAY_SIZE * 2;
            int rightSideBorder = (int)(682 - PLAY_SIZE * 2);   //682為畫布實際寬度
            int upSideBorder = (int)PLAY_SIZE * 2;
            int downSideBorder = (int)(460 - PLAY_SIZE * 2);    //460為畫布實際長度
            
            if (player.y < upSideBorder)
                ignore(1, 2, 3, 8, 9, 10, 11);
            else if (player.x < leftSideBorder)
                ignore(3, 4, 5, 10, 11, 12, 13);
            else if (player.x > rightSideBorder)
                ignore(0, 1, 7, 8, 9, 14, 15);
            else if (player.y > downSideBorder)
                ignore(5, 6, 7, 12, 13, 14, 15);
            else
                    sixteen_dis_sum();
            
            find_out_Max();
            predict_pathToMove();                          
        }         

        public void sixteenball()
        {
            vGuard[0].x = player.x + PLAY_SIZE * 2 * (float)Math.Sqrt(2);
            vGuard[0].y = player.y;
            vGuard[1].x = player.x + PLAY_SIZE * 2;
            vGuard[1].y = player.y - PLAY_SIZE * 2;
            vGuard[2].x = player.x;
            vGuard[2].y = player.y - PLAY_SIZE * 2 * (float)Math.Sqrt(2);
            vGuard[3].x = player.x - PLAY_SIZE * 2;
            vGuard[3].y = player.y - PLAY_SIZE * 2;
            vGuard[4].x = player.x - PLAY_SIZE * 2 * (float)Math.Sqrt(2);
            vGuard[4].y = player.y;
            vGuard[5].x = player.x - PLAY_SIZE * 2;
            vGuard[5].y = player.y + PLAY_SIZE * 2;
            vGuard[6].x = player.x;
            vGuard[6].y = player.y + PLAY_SIZE * 2 * (float)Math.Sqrt(2);
            vGuard[7].x = player.x + PLAY_SIZE * 2;
            vGuard[7].y = player.y + PLAY_SIZE * 2;
            vGuard[8].x = player.x + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[8].y = player.y - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[9].x = player.x + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[9].y = player.y - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[10].x = player.x - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[10].y = player.y - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[11].x = player.x - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[11].y = player.y - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[12].x = player.x - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[12].y = player.y + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[13].x = player.x - PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[13].y = player.y + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[14].x = player.x + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
            vGuard[14].y = player.y + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[15].x = player.x + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Cos(22.5 / 180 * Math.PI);
            vGuard[15].y = player.y + PLAY_SIZE * 2 * (float)Math.Sqrt(2) * (float)Math.Sin(22.5 / 180 * Math.PI);
        }

        public void OutOfRange()
        {
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                applyOutOfRange(i, vMonster_R, vMonster_R_State_Dec);
                applyOutOfRange(i, vMonster_L, vMonster_L_State_Dec);
                applyOutOfRange(i, vMonster_U, vMonster_U_State_Dec);
                applyOutOfRange(i, vMonster_D, vMonster_D_State_Dec);
            }
        }

        public void applyOutOfRange(int i, Point[] monster, bool[] state)
        {
            if (player.getDistance(monster[i]) > DETECTIVE_RANGE + MONSTER_SIZE)
            {
                if (state[i] == true)
                {
                    state[i] = false;
                    Detective_number.value--;
                }
            }
        }

        public void sixteen_dis_sum()
        {
            for (int i = 0; i < MONSTER_AMOUNT; i++)
            {
                applySixteenDisSum(i, vMonster_R, vMonster_R_State_Dec, vGuard_Monster_dis_R);
                applySixteenDisSum(i, vMonster_L, vMonster_L_State_Dec, vGuard_Monster_dis_L);
                applySixteenDisSum(i, vMonster_U, vMonster_U_State_Dec, vGuard_Monster_dis_U);
                applySixteenDisSum(i, vMonster_D, vMonster_D_State_Dec, vGuard_Monster_dis_D);
            }
        }

        public void applySixteenDisSum(int i, Point[] monster, bool[] state, Value[] guard)
        {
            if (player.getDistance(monster[i]) < DETECTIVE_RANGE + MONSTER_SIZE)
            {
                if (state[i] == false)
                {
                    Detective_number.value++;
                    state[i] = true;
                }
                for (int m = 0; m < GUARD_AMOUNT; m++)
                {
                    guard[m] = new Value();
                    guard[m].value = vGuard[m].getDistance(monster[i]) * (40 - player.getDistance(monster[i])) * (40 - player.getDistance(monster[i])) * (40 - player.getDistance(monster[i]));
                    Max_Sum[m].value = Max_Sum[m].value + guard[m].value;
                    Max_Sum_Op[m].value = Max_Sum[m].value;
                }
            }
        }

        public void find_out_Max()
        {
            for (int i = 0; i < 15; i++)    //取出子彈與守衛最大距離和
            {
                if (Max_Sum_Op[i + 1].value < Max_Sum_Op[i].value)
                {
                    Max_Sum_Op[i + 1].value = Max_Sum_Op[i].value;
                }
            }
        }

        public void predict_pathToMove()
        {

            for (int k = 0; k < MONSTER_AMOUNT; k++)
            {
                if (player.getDistance(vMonster_R[k]) < DETECTIVEtoMOVE_RANGE + MONSTER_SIZE)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        vMonster_R_Detect[k].x = vMonster_R[k].x;
                        vMonster_R_Detect[k].y = vMonster_R[k].y;

                        if (vMove_X_R[k] != null)
                        {
                            vMonster_R_Detect[k].x = vMonster_R_Detect[k].x - m * vMove_X_R[k].value;
                            vMonster_R_Detect[k].y = vMonster_R_Detect[k].y + m * vMove_Y_R[k].value;
                        }

                        if (player.getDistance(vMonster_R_Detect[k]) < MONSTER_SIZE + PLAY_SIZE)
                        {
                            for (int i = 0; i < GUARD_AMOUNT; i++)
                            {
                                if (Max_Sum[i].value != 0)
                                {
                                    if (Max_Sum[i].value == Max_Sum_Op[15].value)
                                    {
                                        player.moveToGuard(vGuard[i]);
                                        goto END;
                                    }
                                }
                            }
                        }
                    }
                }
                if (player.getDistance(vMonster_L[k]) < DETECTIVEtoMOVE_RANGE + MONSTER_SIZE)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        vMonster_L_Detect[k].x = vMonster_L[k].x;
                        vMonster_L_Detect[k].y = vMonster_L[k].y;

                        if (vMove_X_L[k] != null)
                        {
                            vMonster_L_Detect[k].x = vMonster_L_Detect[k].x + m * vMove_X_L[k].value;
                            vMonster_L_Detect[k].y = vMonster_L_Detect[k].y + m * vMove_Y_L[k].value;
                        }

                        if (player.getDistance(vMonster_L_Detect[k]) < MONSTER_SIZE + PLAY_SIZE)
                        {
                            for (int i = 0; i < GUARD_AMOUNT; i++)
                            {
                                if (Max_Sum[i].value != 0)
                                {
                                    if (Max_Sum[i].value == Max_Sum_Op[15].value)
                                    {
                                        player.moveToGuard(vGuard[i]);
                                        goto END;
                                    }
                                }
                            }
                        }
                    }
                }
                if (player.getDistance(vMonster_U[k]) < DETECTIVEtoMOVE_RANGE + MONSTER_SIZE)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        vMonster_U_Detect[k].x = vMonster_U[k].x;
                        vMonster_U_Detect[k].y = vMonster_U[k].y;

                        if (vMove_X_U[k] != null)
                        {
                            vMonster_U_Detect[k].x = vMonster_U_Detect[k].x + m * vMove_X_U[k].value;
                            vMonster_U_Detect[k].y = vMonster_U_Detect[k].y + m * vMove_Y_U[k].value;
                        }
                        
                        if (player.getDistance(vMonster_U_Detect[k]) < MONSTER_SIZE + PLAY_SIZE)
                        {
                            for (int i = 0; i < GUARD_AMOUNT; i++)
                            {
                                if (Max_Sum[i].value != 0)
                                {
                                    if (Max_Sum[i].value == Max_Sum_Op[15].value)
                                    {
                                        player.moveToGuard(vGuard[i]);
                                        goto END;
                                    }
                                }
                            }
                        }
                    }
                }
                if (player.getDistance(vMonster_D[k]) < DETECTIVEtoMOVE_RANGE + MONSTER_SIZE)
                {
                    for (int m = 0; m < 12; m++)
                    {
                        vMonster_D_Detect[k].x = vMonster_D[k].x;
                        vMonster_D_Detect[k].y = vMonster_D[k].y;

                        if (vMove_X_D[k] != null)
                        {
                            vMonster_D_Detect[k].x = vMonster_D_Detect[k].x + m * vMove_X_D[k].value;
                            vMonster_D_Detect[k].y = vMonster_D_Detect[k].y - m * vMove_Y_D[k].value;
                        }

                        if (player.getDistance(vMonster_D_Detect[k]) < MONSTER_SIZE + PLAY_SIZE)
                        {
                            for (int i = 0; i < GUARD_AMOUNT; i++)
                            {
                                if (Max_Sum[i].value != 0)
                                {
                                    if (Max_Sum[i].value == Max_Sum_Op[15].value)
                                    {
                                        player.moveToGuard(vGuard[i]);
                                        goto END;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        END:
            draw();
        }

        public void ignore(int a, int b, int c, int d, int e, int f, int g)    //忽略跑出邊界的守衛           
        {
            for (int m = 0; m < MONSTER_AMOUNT; m++)
            {
                for (int i = 0; i < GUARD_AMOUNT; i++)
                {
                    if (i != a && i != b && i != c && i != d && i != e && i != f && i != g)
                    {                            
                        getDis_onlyOne_sum(player, vMonster_R[m], vGuard[i], vGuard_Monster_dis_R[i], Max_Sum[i], Max_Sum_Op[i]);
                        getDis_onlyOne_sum(player, vMonster_L[m], vGuard[i], vGuard_Monster_dis_L[i], Max_Sum[i], Max_Sum_Op[i]);
                        getDis_onlyOne_sum(player, vMonster_U[m], vGuard[i], vGuard_Monster_dis_U[i], Max_Sum[i], Max_Sum_Op[i]);
                        getDis_onlyOne_sum(player, vMonster_D[m], vGuard[i], vGuard_Monster_dis_D[i], Max_Sum[i], Max_Sum_Op[i]);
                    }
                }
            }                       
        }

        public void getDis_onlyOne_sum(Point player, Point monster, Point guard,Value vG_M_which_dis, Value max_sum, Value max_sum_op)
        {
            if (player.getDistance(monster) < DETECTIVE_RANGE)
            {
                vG_M_which_dis = new Value();
                vG_M_which_dis.value = guard.getDistance(monster);
                max_sum.value = vG_M_which_dis.value + max_sum.value;
                max_sum_op.value = max_sum.value;
            }          
        }

        public string choose_min()//離最近的
        {
            bool processed = false;            
            float min = 0;
            string whichi = "";
            string total = "";
            for (int i = 0; i < 15; i++)    //取出子彈與守衛最大距離和
            {
                if (!processed)
                {
                    min = Max_Sum_Op[i].value;
                }

                processed = true;

                if (Max_Sum[i + 1].value < min)
                {                    
                    min = Max_Sum[i + 1].value;
                    whichi = i + 1 + "";
                }
            }
            total = "[" + whichi + "]" + " " + min;
            return total;
        }

        public string choose_max()//離最遠的
        {
            bool processed = false;
            float max = 0;
            string whichi = "";
            string total = "";
            for (int i = 0; i < 15; i++)    //取出子彈與守衛最大距離和
            {
                if (!processed)
                {
                    max = Max_Sum_Op[i].value;
                }

                processed = true;

                if (Max_Sum[i + 1].value > max)
                {
                    max = Max_Sum[i + 1].value;
                    whichi = i + 1 + "";
                }
            }
            total = "[" + whichi + "]" + " " + max;
            return total;
        }

        public void monsterMove(int monsAmount, Value[] moveX, Value[] moveY, Point[] monster,Value[] randomValue, int paramX, int paramY)
        {
            for (int i = 0; i < monsAmount; i++)
            {
                moveX[i] = new Value();
                moveY[i] = new Value();
                moveX[i].value = MONSTER_SPEED * (float)Math.Cos(randomValue[i].value);
                moveY[i].value = MONSTER_SPEED * (float)Math.Sin(randomValue[i].value);
                monster[i].x += moveX[i].value * paramX;
                monster[i].y += moveY[i].value * paramY;

                //判定是否碰到

                if (player.getDistance(monster[i]) <= PLAY_SIZE + MONSTER_SIZE)
                {
                    live_time_string = live_time_Minute.value + "分" + live_time_Second.value + "." + live_time_Millisecond.value + "秒";
                    timer1.Stop();
                    /*
                    brestart.Visible = true;
                    label1LiveTime.Visible = true;
                    label1LiveTime.Text = time;
                    */
                    if (Form2.bPlay)//如果遊戲結束，產生FORM3的視窗
                    {
                        Form3 form3 = new Form3();
                        form3.label3YourScore.Text = live_time_string;
                        form3.ShowDialog();
                        if (Form3.restart == true)
                        {
                            Form3.restart = false;
                            initial();
                            timer1.Start();                            
                        }
                        else if (Form3.backMenu == true)
                        {                            
                            this.Hide();
                            form2 = new Form2();
                            form2.ShowDialog();
                            if (Form2.bPlay_backMenu_choose == true)
                            {
                                Form3.backMenu = false;
                                Form2.bPlay_backMenu_choose = false;
                                this.Show();
                                initial();
                                timer1.Start();                                
                            }
                        }
                    }
                    else if (Form2.bAI_show)//如果是電腦示範模式，遊戲結束則產生FORM4
                    {
                        Form4 form4 = new Form4();
                        form4.ShowDialog();
                        if (Form4.restart == true)
                        {
                            Form4.restart = false;
                            initial();
                            timer1.Start();                            
                        }
                        else if (Form4.backMenu == true)
                        {
                            this.Hide();
                            form2 = new Form2();
                            form2.ShowDialog();
                            if (Form2.bPlay_backMenu_choose == true)
                            {
                                Form4.backMenu = false;
                                Form2.bPlay_backMenu_choose = false;
                                this.Show();
                                initial();
                                timer1.Start();                                
                            }
                        }
                    }
                }

                //判定出界                                    
                if (monster[i].x < 0)
                {
                    monster[i] = null;
                    moveX[i] = null;
                    moveY[i] = null;
                }
                else if(monster[i].x > FORM_W)
                {
                    monster[i] = null;
                    moveX[i] = null;
                    moveY[i] = null;
                }
                else if (monster[i].y > FORM_H)
                {
                    monster[i] = null;
                    moveX[i] = null;
                    moveY[i] = null;
                }
                else if (monster[i].y < 0)
                {
                    monster[i] = null;
                    moveX[i] = null;
                    moveY[i] = null;
                }
            }
        }

        public void createMonster(int monsAmount, Point[] monster, Value[] randomValue, int x, int y, int upside, int downside)
        {
            for (int i = 0; i < monsAmount; i++)
            {
                if (monster[i] == null)
                {
                    monster[i] = new Point();
                    monster[i].x = x;
                    monster[i].y = y;
                    randomValue[i] = new Value();
                    randomValue[i].value = random.Next(upside, downside) * (float)(Math.PI) / 180;
                }
            }
        }

        public void spotWatch() //計時
        {
            now2 = DateTime.Now;

            if (now2.Minute > now1.Minute)
            {
                if (now2.Second > now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second - 1 - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                }
                else if (now2.Second < now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute - 1 - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute - 1 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute - 1 - now1.Minute;
                    }
                }
                else if (now2.Second == now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute - 1 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                }
            }
            else if (now2.Minute < now1.Minute)
            {
                if (now2.Second > now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute + 60 - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second - 1 - now1.Second;
                        live_time_Minute.value = now2.Minute + 60 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute + 60 - now1.Minute;
                    }
                }
                else if (now2.Second < now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                }
                else if (now2.Second == now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute + 60 - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute + 60 - now1.Minute;
                    }
                }
            }
            else if (now2.Minute == now1.Minute)
            {
                if (now2.Second > now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second - 1 - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                }
                else if (now2.Second < now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second + 60 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                }
                else if (now2.Second == now1.Second)
                {
                    if (now2.Millisecond > now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                    else if (now2.Millisecond < now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond + 1000 - now1.Millisecond;
                        live_time_Second.value = now2.Second + 59 - now1.Second;
                        live_time_Minute.value = now2.Minute + 59 - now1.Minute;
                    }
                    else if (now2.Millisecond == now1.Millisecond)
                    {
                        live_time_Millisecond.value = now2.Millisecond - now1.Millisecond;
                        live_time_Second.value = now2.Second - now1.Second;
                        live_time_Minute.value = now2.Minute - now1.Minute;
                    }
                }
            }
        }

        /*
        public void warningDodge(int monsAmount, int guardAmount, Point player, Point[] monster, Point[] guard, Value[] max_sum)
        {
            for (int i = 0; i < monsAmount; i++)
            {
                if (player.getDistance(monster[i]) == WARNING_AREA + MONSTER_SIZE)
                {
                    float max = 0;
                    for (int k = 0; k < guardAmount; k++)
                    {
                        max_sum[k] = new Value();
                        max_sum[k].value = guard[k].getDistance(monster[i]);
                                 
                        if (max_sum[k].value > max)
                        {
                            max = max_sum[k].value;
                        }
                    }

                    for (int k = 0; k < guardAmount; k++)
                    {
                        if (guard[k].getDistance(monster[i]) == max)
                        {
                            player.moveToGuard(guard[k]);
                            break;
                        }
                    }
                }
            }
        }
        */
        

                   
    }

    //------------------------------------------------------------------------------------------------------------------------------------------//

    public class Point
    {
        public float x,y;
              
        public float getDistance(Point target)
        {
            float L = (float)Math.Sqrt((x - target.x + 7) * (x - target.x + 7) + (y - target.y + 7) * (y - target.y + 7));
            return L;
        }
        
        public float getDisPlayerToGuard(Point target)
        {
            float L = (float)Math.Sqrt((x - target.x) * (x - target.x) + (y - target.y) * (y - target.y));
            return L;
        }

        public void moveToGuard(Point Guard)
        {
            float tx, ty;
            tx = (Guard.x - x) / getDisPlayerToGuard(Guard) * 3.5f;
            ty = (Guard.y - y) / getDisPlayerToGuard(Guard) * 3.5f;

            x += tx;
            y += ty;
        }                
    }

    public class Value
    {
        public float value;
    }

    class KeyState
    {
        private Keys theKey;
        bool bDown;

        public KeyState(Keys k)
        {
            theKey = k;
            bDown = false;
        }

        public void KeyDown(Keys k)
        {
            if (theKey == k)
                bDown = true;
        }

        public void KeyUp(Keys k)
        {
            if (theKey == k)
                bDown = false;
        }

        public bool isDown()
        {
            return bDown;
        }
    }
}
