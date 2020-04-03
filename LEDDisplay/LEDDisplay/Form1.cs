using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEDDisplay
{
    public partial class Form1 : Form
    {

        //窗口自适应中间变量
        private float X;//窗体宽度
        private float Y;//窗体高度 

        //段选位选相关变量
        private int LED_Place = 1;              //位选
        private int[] LED_Segment = new int[8]; //段选

        //配色方案相关变量
        Color StdLightColorB = Color.FromArgb(0, 200, 234);
        Color StdLightColorG = Color.FromArgb(100, 255, 0);
        Color StdLightColorY = Color.FromArgb(255, 200, 0);
        Color StdLightColorR = Color.FromArgb(255, 20, 20);

        Color StdDarkColorD = Color.FromArgb(32, 42, 52);
        Color StdDarkColorB = Color.FromArgb(62, 72, 82);
        Color StdDarkColorW = Color.FromArgb(220, 220, 220);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            setSegment_State(LED_Place);

            this.Resize += new EventHandler(Form1_ResizeEnd);//窗体调整大小时引发事件
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法
        }

    
        /// <summary>
        /// 自适应窗体
        /// </summary>
        /// <param name="cons"></param>
        private void setTag(Control cons)   //遍历修改窗口中空间的大小
        {
            //遍历窗体中的控件
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            float newx = (this.Width) / X; //窗体宽度缩放比例
            float newy = this.Height / Y;//窗体高度缩放比例
            setControls(newx, newy, this);//随窗体改变控件大小
            //this.Text = this.Width.ToString() + " " + this.Height.ToString();//窗体标题栏文本
        }

        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                float a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                con.Width = (int)a;//宽度
                a = Convert.ToSingle(mytag[1]) * newy;//高度
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;//左边距离
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * (newx + newy) / 2;//字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }
        }
        /// <summary>
        /// 位控制组
        /// </summary>
        /// <param name="place"></param>
        private void setSegment_State(int place)
        {
            //位网络组
            Label[] LabPnum = { LabS1num, LabS2num, LabS3num, LabS4num, };//位网络标号
            Label[] LabNHR = { LabNHR1, LabNHR2, LabNHR3, LabNHR4, };//位网络行
            Label[] LabNHC = { LabNHC1, LabNHC2, LabNHC3, LabNHC4, };//位网络列

            Label[] LabS = { LabS1, LabS2, LabS3, LabS4, };//位标选中

            Label[] LabNP = { LabNP1, LabNP2, LabNP3, LabNP4, };//位网络标号2

            for (int i = 0; i < 4; i++)
            {
                if (place == (i + 1))//当前位选被激活
                {
                    LabPnum[i].BackColor = StdLightColorB;//位网络标号
                    LabNHR[i].BackColor = StdLightColorY;//位网络行
                    LabNHC[i].BackColor = StdLightColorY;//位网络列
                    LabS[i].BackColor = StdDarkColorD;//位标选中
                    LabNP[i].ForeColor = StdLightColorY;//位网络标号2
                }
                else//  当前位选未被激活
                {
                    LabPnum[i].BackColor = StdDarkColorB;//位网络标号
                    LabNHR[i].BackColor = StdDarkColorB;//位网络行
                    LabNHC[i].BackColor = StdDarkColorB;//位网络列
                    LabS[i].BackColor = StdDarkColorB;//位标选中
                    LabNP[i].ForeColor = StdDarkColorW;//位网络标号2
                }

                LabNP[i].BackColor = StdDarkColorD;
            }

            LabPNum.Text = LED_Place.ToString();
        }

        private void LabS1num_Click(object sender, EventArgs e)
        {
            //执行激活位1的网络与标号
            LED_Place = 1;
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }

        private void LabS2num_Click(object sender, EventArgs e)
        {
            //执行激活位2的网络与标号
            LED_Place = 2;
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }

        private void label53_Click(object sender, EventArgs e)
        {
            //执行激活位3的网络与标号
            LED_Place = 3;
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }

        private void label62_Click(object sender, EventArgs e)
        {
            //执行激活位4的网络与标号
            LED_Place = 4;
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }
        /// <summary>
        /// 段控制组
        /// </summary>
        /// <param name="place"></param>
        /// <param name="seg"></param>
        private void setSegment_State(int place, int[] seg)//位段操作方法
        {
            Dictionary<string, string> BintoHex = new Dictionary<string, string>();

            BintoHex.Add("0000", "0"); BintoHex.Add("0001", "1"); BintoHex.Add("0010", "2"); BintoHex.Add("0011", "3");
            BintoHex.Add("0100", "4"); BintoHex.Add("0101", "5"); BintoHex.Add("0110", "6"); BintoHex.Add("0111", "7");
            BintoHex.Add("1000", "8"); BintoHex.Add("1001", "9"); BintoHex.Add("1010", "A"); BintoHex.Add("1011", "B");
            BintoHex.Add("1100", "C"); BintoHex.Add("1101", "D"); BintoHex.Add("1110", "E"); BintoHex.Add("1111", "F");

            //段网络组
            Label[] LabSnum = { LabSAnum, LabSBnum, LabSCnum, LabSDnum, LabSEnum, LabSFnum, LabSGnum, LabSHnum, };//左侧段标号
            Label[] LabNLR = { LabNLRA, LabNLRB, LabNLRC, LabNLRD, LabNLRE, LabNLRF, LabNLRG, LabNLRH, };//左侧段网络行
            Label[] LabNLC = { LabNLCA, LabNLCB, LabNLCC, LabNLCD, LabNLCE, LabNLCF, LabNLCG, LabNLCH, };//左侧段网络列

            Label[] LabSP = { LabSPA, LabSPB, LabSPC, LabSPD, LabSPE, LabSPF, LabSPG, LabSPH,};//左侧网络标号

            Label[] LabDisR = { LabDisRA, LabDisRB, LabDisRC, LabDisRD, LabDisRE, LabDisRF, LabDisRG, LabDisRH, };//右侧段显示
            PictureBox[] LEDpic = { LEDpicA, LEDpicB, LEDpicC, LEDpicD, LEDpicE, LEDpicF, LEDpicG, LEDpicH, };     //右侧LED显示

            Label[,] LabDis = {
                { LabDis1A, LabDis1B, LabDis1C, LabDis1D, LabDis1E, LabDis1F, LabDis1G, LabDis1H, },//四位数码管
                { LabDis2A, LabDis2B, LabDis2C, LabDis2D, LabDis2E, LabDis2F, LabDis2G, LabDis2H, },
                { LabDis3A, LabDis3B, LabDis3C, LabDis3D, LabDis3E, LabDis3F, LabDis3G, LabDis3H, },
                { LabDis4A, LabDis4B, LabDis4C, LabDis4D, LabDis4E, LabDis4F, LabDis4G, LabDis4H, },
            };

            Label[] LabHnum = { LabHnumA, LabHnumB, LabHnumC, LabHnumD, LabHnumE, LabHnumF, LabHnumG, LabHnumH, };//二进制显示

            Label[] LabSnumL = { LabSAnumL, LabSBnumL, LabSCnumL, LabSDnumL, LabSEnumL, LabSFnumL, LabSGnumL, LabSHnumL,};//LED段标记

            //使能与失能 网络，标号和其他标记
            for(int i = 0; i < 8; i++)
            {
                if (seg[i] != 0)//当前段标识被使能
                {
                    LabSnum[i].BackColor = StdLightColorB;//左侧段标号
                    LabNLR[i].BackColor = StdLightColorG;//左侧段网络行
                    LabNLC[i].BackColor = StdLightColorG;//左侧段网络列

                    LabSP[i].ForeColor = StdLightColorG;//左侧网络标号2

                    LabDisR[i].BackColor = StdLightColorR;//右侧段显示

                    LEDpic[i].Image = LEDDisplay.Properties.Resources.DR;//右侧LED显示

                    LabSnumL[i].ForeColor = StdLightColorR;//LED段标记

                    LabHnum[i].Text = "0";//二进制显示
                    LabHnum[i].ForeColor = StdLightColorG;//二进制显示颜色
                }
                else//当前段标识被失能
                {
                    LabSnum[i].BackColor = StdDarkColorB;//左侧段标号
                    LabNLR[i].BackColor = StdDarkColorB;//左侧段网络行
                    LabNLC[i].BackColor = StdDarkColorB;//左侧段网络列

                    LabSP[i].ForeColor = StdDarkColorW;//左侧网络标号2

                    LabDisR[i].BackColor = StdDarkColorW;//右侧段显示

                    LEDpic[i].Image = LEDDisplay.Properties.Resources.DB;//右侧LED显示
                    
                    LabSnumL[i].ForeColor = StdDarkColorW;//LED段标记

                    LabHnum[i].Text = "1";//二进制显示
                    LabHnum[i].ForeColor = StdLightColorY;//二进制显示颜色
                }
                LabSP[i].BackColor = StdDarkColorD;//左侧网络标号2
                LabHnum[i].BackColor = StdDarkColorD;//二进制显示颜色
                LabSnumL[i].BackColor = StdDarkColorD;//LED段标记
            }
            
            //使能与失能 四位数码管 其中的段码

            for (int i = 0; i < 4; i++) //位环段
            {
                for (int j = 0;j < 8;j++)//段循环
                {
                    //当前位被激活且当前段被激活
                    if ((LED_Place == (i + 1)) && (LED_Segment[j] != 0))
                    {
                        LabDis[i, j].BackColor = StdLightColorR;
                    }
                    else
                    {
                        LabDis[i, j].BackColor = StdDarkColorW;
                    }
                }
            }
            
            //显示二进制
            LabBin.Text = "";
            for (int i = 0; i < 8; i++)
            {
                LabBin.Text += LabHnum[7 - i].Text;
            }

            //显示十六进制
            LabHex.Text = BintoHex[LabHnum[7].Text + LabHnum[6].Text + LabHnum[5].Text + LabHnum[4].Text]
                        + BintoHex[LabHnum[3].Text + LabHnum[2].Text + LabHnum[1].Text + LabHnum[0].Text];
        }


        void setSegment(int num)//配置段
        {
            if (LED_Segment[num] != 0)//当前为使能状态 执行失能操作
            {
                LED_Segment[num] = 0;
            }
            else
            {
                LED_Segment[num] = 1;
            }
            setSegment_State(LED_Place, LED_Segment);
        }

        private void LabSAnum_Click(object sender, EventArgs e)//段A
        {
            setSegment(0);
        }

        private void LabSBnum_Click(object sender, EventArgs e)//段B
        {
            setSegment(1);
        }

        private void LabSCnum_Click(object sender, EventArgs e)//段C
        {
            setSegment(2);
        }

        private void LabSDnumL_Click(object sender, EventArgs e)//段D
        {
            setSegment(3);
        }

        private void LabSEnum_Click(object sender, EventArgs e)//段E
        {
            setSegment(4);
        }

        private void LabSFnum_Click(object sender, EventArgs e)//段F
        {
            setSegment(5);
        }

        private void LabSGnum_Click(object sender, EventArgs e)//段G
        {
            setSegment(6);
        }

        private void LabSHnum_Click(object sender, EventArgs e)//段H
        {
            setSegment(7);
        }

        private void label3_Click(object sender, EventArgs e)//复位
        {
            LED_Place = 1;
            for (int i = 0; i < 8; i++)
            {
                LED_Segment[i] = 0;
            }
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }

        private void LabPNum_Click(object sender, EventArgs e)
        {
            LED_Place++;
            if (LED_Place > 4)
            {
                LED_Place = 1;
            }
            setSegment_State(LED_Place);
            setSegment_State(LED_Place, LED_Segment);
        }

        /// <summary>
        /// 配色方案修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upodateModifyColor()
        {
            //位更新
            setSegment_State(LED_Place);
            //段更新
            setSegment_State(LED_Place, LED_Segment);

            //窗口容器相关配色更新
            this.BackColor = StdDarkColorD;//窗口底色
            label1.BackColor = StdDarkColorD;//标题标签颜色
            tabPage1.BackColor = StdDarkColorD;//选项卡底色1
            label2.BackColor = StdDarkColorB;//数码管底色
            //tabPage2.BackColor = StdDarkColorD;//选项卡底色2
            groupBox1.BackColor = StdDarkColorD;//容器1底色
            groupBox2.BackColor = StdDarkColorD;//容器2底色
            groupBox3.BackColor = StdDarkColorD;//容器3底色
            groupBox1.ForeColor = StdDarkColorW;//容器1前景
            groupBox2.ForeColor = StdDarkColorW;//容器2前景

            LabBin.ForeColor = StdLightColorB;//二进制前景
            label5.ForeColor = StdLightColorB;//二进制前景
            LabHex.ForeColor = StdLightColorB;//二进制前景

            LabPNum.ForeColor = StdDarkColorW;//位号
            LabPNum.BackColor = StdDarkColorD;
            label3.ForeColor = StdDarkColorD;//复位
            label3.BackColor = StdDarkColorW;
        }

        private void button2_Click(object sender, EventArgs e)//light配色方案
        {
            Label[] LabDisR = { LabDisRA, LabDisRB, LabDisRC, LabDisRD, LabDisRE, LabDisRF, LabDisRG, LabDisRH, };//右侧段显示
            for (int i = 0; i < 8; i++)
            {
                LabDisR[i].ForeColor = Color.FromArgb(255,255,255);
            }

            StdLightColorB = LabLB.BackColor;
            StdLightColorG = LabLG.BackColor;
            StdLightColorR = LabLR.BackColor;
            StdLightColorY = LabLY.BackColor;

            StdDarkColorB = LabDB.BackColor;
            StdDarkColorD = LabDD.BackColor;
            StdDarkColorW = LabDW.BackColor;

            upodateModifyColor();
        }

        private void button1_Click(object sender, EventArgs e)//dark配色方案
        {
            Label[] LabDisR = { LabDisRA, LabDisRB, LabDisRC, LabDisRD, LabDisRE, LabDisRF, LabDisRG, LabDisRH, };//右侧段显示
            for (int i = 0; i < 8; i++)
            {
                LabDisR[i].ForeColor = Color.FromArgb(0,0,0);
            }

            //labDarkB
            StdLightColorB = labDarkB.BackColor;
            StdLightColorG = labDarkG.BackColor;
            StdLightColorR = labDarkR.BackColor;
            StdLightColorY = labDarkY.BackColor;

            StdDarkColorB = labDDB.BackColor;
            StdDarkColorD = labDarkD.BackColor;
            StdDarkColorW = labDarkW.BackColor;

            upodateModifyColor();
        }
        /// <summary>
        /// 自定义配色方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)//应用色
        {
            Label[] LabDisR = { LabDisRA, LabDisRB, LabDisRC, LabDisRD, LabDisRE, LabDisRF, LabDisRG, LabDisRH, };//右侧段显示
            for (int i = 0; i < 8; i++)
            {
                LabDisR[i].ForeColor = labCC.BackColor;
            }
            
            StdLightColorB = labCLB.BackColor;
            StdLightColorG = labCLG.BackColor;
            StdLightColorR = labCLR.BackColor;
            StdLightColorY = labCLY.BackColor;

            StdDarkColorB = labCDB.BackColor;
            StdDarkColorD = labCDD.BackColor;
            StdDarkColorW = labCDW.BackColor;

            upodateModifyColor();
        }

        private void labCLB_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCLB.BackColor = this.colorDialog1.Color;
        }

        private void labCLG_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCLG.BackColor = this.colorDialog1.Color;
        }

        private void labCLY_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCLY.BackColor = this.colorDialog1.Color;
        }

        private void labCLR_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCLR.BackColor = this.colorDialog1.Color;
        }

        private void labCDD_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCDD.BackColor = this.colorDialog1.Color;
        }

        private void labCDB_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCDB.BackColor = this.colorDialog1.Color;
        }

        private void labCDW_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCDW.BackColor = this.colorDialog1.Color;
        }

        private void labCC_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            labCC.BackColor = this.colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)//复位自定义颜色
        {

            labCLB.BackColor = Color.FromArgb(0, 200, 234);
            labCLG.BackColor = Color.FromArgb(100, 255, 0);
            labCLY.BackColor = Color.FromArgb(255, 200, 0);
            labCLR.BackColor = Color.FromArgb(255, 20, 20);

            labCDD.BackColor = Color.FromArgb(32, 42, 52);
            labCDB.BackColor = Color.FromArgb(62, 72, 82);
            labCDW.BackColor = Color.FromArgb(220, 220, 220);
        }

        private void LabBin_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
