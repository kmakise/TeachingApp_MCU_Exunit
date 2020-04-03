using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySerialPort
{
    public partial class Form1 : Form
    {
        private static bool IsDrag = false;
        private int enterX;
        private int enterY;

        private float X;//窗体宽度
        private float Y;//窗体高度

        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//设置该属性 为false
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                cmbPort.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    cmbPort.Items.Add(sValue);
                }
                if (cmbPort.Items.Count > 0)
                    cmbPort.SelectedIndex = 0;
            }
          
            cmbBaud.Text = "115200";

            this.Resize += new EventHandler(Form1_ResizeEnd);//窗体调整大小时引发事件
            X = this.Width;//获取窗体的宽度
            Y = this.Height;//获取窗体的高度
            setTag(this);//调用方法

        }
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

        bool isOpened = false;//串口状态标志

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (!isOpened)
            {
                serialPort.PortName = cmbPort.Text;
                serialPort.BaudRate = Convert.ToInt32(cmbBaud.Text, 10);
                try
                {
                    serialPort.Open();     //打开串口
                    button1.Text = "终  止";
                    cmbPort.Enabled = false;//关闭使能
                    cmbBaud.Enabled = false;
                    isOpened = true;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(post_DataReceived);//串口接收处理函数
                }
                catch
                {
                    MessageBox.Show("串口打开失败！");
                }
            }
            else
            {
                try
                {
                    serialPort.Close();     //关闭串口
                    button1.Text = "启  动";
                    cmbPort.Enabled = true;//打开使能
                    cmbBaud.Enabled = true;
                    isOpened = false;
                }
                catch
                {
                    MessageBox.Show("串口关闭失败！");
                }
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //发送数据
            if (serialPort.IsOpen)
            {//如果串口开启
                if (SendTbox.Text.Trim() != "")//如果框内不为空则
                {
                    serialPort.Write(SendTbox.Text.Trim());//写数据
                }
                else
                {
                    MessageBox.Show("发送框没有数据");
                }
            }
            else
            {
                MessageBox.Show("串口未打开");
            }
        }
        private void Diaplay_MatrixKeyboard(char row, char col) //矩阵键盘操作显示
        {
            Dictionary<char,char> BtoD = new Dictionary<char,char>();

            Label[] colLabArrayH = { colLabH1, colLabH2 , colLabH3, colLabH4};
            Label[] colLabArrayL = { colLabL1, colLabL2 , colLabL3, colLabL4 };
            Label[] rowLabArray = { rowLab1, rowLab2, rowLab3, rowLab4 };
            Label[] rowLabArrayH = { rowLabH1, rowLabH2, rowLabH3, rowLabH4 };
            Label[] rowLabArrayL = { rowLabL1, rowLabL2, rowLabL3, rowLabL4 };

            Label[] PortLabArray = { PortLab1, PortLab2, PortLab3, PortLab4,
                                     PortLab5, PortLab6, PortLab7, PortLab8, };

            Button[] MKey = {
                MK11,MK12,MK13,MK14,
                MK21,MK22,MK23,MK24,
                MK31,MK32,MK33,MK34,
                MK41,MK42,MK43,MK44,
            };

            BtoD.Add('8', '4');
            BtoD.Add('4', '3');
            BtoD.Add('2', '2');
            BtoD.Add('1', '1');
            BtoD.Add('0', '0');

            //标签显示
            if (row != '0' || col != '0')
            {
                MKDataLab.Text = "<M" + col + row + ">";
                MKDataLab.ForeColor = Color.FromArgb(255, 100, 0);
            }
            else
            {
                MKDataLab.Text = "<M" + col + row + ">";
                MKDataLab.ForeColor = Color.FromArgb(255, 255,255);
            }

            //视图操作
            if ((col == '8' || col == '4' || col == '2' || col == '1' || col == '0' )&&
                (row == '8' || row == '4' || row == '2' || row == '1' || row == '0' ))
            {

                int colnum = BtoD[col] - 0x30;
                int rownum = BtoD[row] - 0x30;


                rowLab.Text = "ROW = " + BtoD[row].ToString();
                colLab.Text = "COL = " + BtoD[col].ToString();

                //序号前景色
                if (rownum == 0) rowLab.ForeColor = Color.FromArgb(72, 82, 92);
                else rowLab.ForeColor = Color.FromArgb(100, 255, 0);

                if (colnum == 0) colLab.ForeColor = Color.FromArgb(72, 82, 92);
                else colLab.ForeColor = Color.FromArgb(255, 200, 0);

                //行点亮

                for (int i = 0; i < 4; i++)
                {
                    if (colnum == (i + 1))
                    {
                        colLabArrayH[i].BackColor = Color.FromArgb(255, 200, 0);
                        colLabArrayL[i].BackColor = Color.FromArgb(255, 200, 0);

                        PortLabArray[i].ForeColor = Color.FromArgb(255, 200, 0);
                    }
                    else
                    {
                        colLabArrayH[i].BackColor = Color.FromArgb(72, 82, 92);
                        colLabArrayL[i].BackColor = Color.FromArgb(72, 82, 92);

                        PortLabArray[i].ForeColor = Color.FromArgb(254,254,254);
                    }
                }

                //列点亮
                for (int i = 0; i < 4; i++)
                {
                    if (rownum == (i + 1))
                    {
                        rowLabArray[i].BackColor = Color.FromArgb(100, 255, 0);
                        rowLabArrayH[i].BackColor = Color.FromArgb(100, 255, 0);
                        rowLabArrayL[i].BackColor = Color.FromArgb(100, 255, 0);

                        PortLabArray[4 + i].ForeColor = Color.FromArgb(100, 255, 0);
                    }
                    else
                    {
                        rowLabArray[i].BackColor = Color.FromArgb(72, 82, 92);
                        rowLabArrayH[i].BackColor = Color.FromArgb(72, 82, 92);
                        rowLabArrayL[i].BackColor = Color.FromArgb(72, 82, 92);

                        PortLabArray[4 + i].ForeColor = Color.FromArgb(254,254,254);
                    }
                }

                //按键点亮
                for (int i = 0; i < 16; i++)
                {
                    if (i == ((rownum - 1) * 4 + (colnum - 1)))
                    {
                        MKey[i].BackColor = Color.FromArgb(0, 200, 255);
                    }
                    else
                    {
                        MKey[i].BackColor = Color.FromArgb(52, 62, 72);
                    }
                }
            }

        }
        private void Diaplay_IndependentKeyboard(char key) //独立键盘操作显示
        {
            Label[] PortLabArray = { SSLab1, SSLab2, SSLab3, SSLab4 };
            Label[] NetLabArray = { keyLab1, keyLab2, keyLab3, keyLab4 };
            Button[] keyButArray = { keyB1, keyB2, keyB3, keyB4 };

            Dictionary<char, int> HtoD = new Dictionary<char, int>();

            HtoD.Add('0', 0); HtoD.Add('1', 1); HtoD.Add('2', 2); HtoD.Add('3', 3);
            HtoD.Add('4', 4); HtoD.Add('5', 5); HtoD.Add('6', 6); HtoD.Add('7', 7);
            HtoD.Add('8', 8); HtoD.Add('9', 9); HtoD.Add('A', 10); HtoD.Add('B', 11);
            HtoD.Add('C', 12); HtoD.Add('D', 13); HtoD.Add('E', 14); HtoD.Add('F', 15);

            if (key == '0' || key == '4' || key == '8' || key == 'C' ||
                key == '1' || key == '5' || key == '9' || key == 'D' ||
                key == '2' || key == '6' || key == 'A' || key == 'E' ||
                key == '3' || key == '7' || key == 'B' || key == 'F' )
            {
                //测试标签显示
                if (key != '0')
                {
                    SKDataLab.Text = "<S0" + key + ">";
                    SKDataLab.ForeColor = Color.FromArgb(255, 100, 0);
                }
                else
                {
                    SKDataLab.Text = "<S0" + key + ">";
                    SKDataLab.ForeColor = Color.FromArgb(255, 255, 255);
                }

                //按钮和网络的点亮
                for (int i = 0; i < 4; i++)
                {
                    int temp = HtoD[key];
                    if ((temp & (0x01 << i)) != 0)
                    {
                        PortLabArray[i].ForeColor = Color.FromArgb(100, 255, 0);
                        NetLabArray[i].BackColor = Color.FromArgb(100, 255, 0);
                        keyButArray[i].BackColor = Color.FromArgb(0, 200, 255);
                    }
                    else
                    {
                        PortLabArray[i].ForeColor = Color.FromArgb(255, 200, 0);
                        NetLabArray[i].BackColor = Color.FromArgb(255, 200, 0);
                        keyButArray[i].BackColor = Color.FromArgb(72, 82, 92);
                    }
                }
            }

        }


        private void MKey_PortDisplay(char num)//矩阵键盘 端口标号 更新
        {
            Label[] PortLabArray = { PortLab1, PortLab2, PortLab3, PortLab4,
                                     PortLab5, PortLab6, PortLab7, PortLab8, };

            //解析标签
            if (MSDataLab.ForeColor == Color.FromArgb(0, 200, 255))
            {
                MSDataLab.ForeColor = Color.FromArgb(100, 255, 000);
            }
            else if (MSDataLab.ForeColor == Color.FromArgb(100, 255, 0))
            {
                MSDataLab.ForeColor = Color.FromArgb(255, 200,0);
            }
            else
            {
                MSDataLab.ForeColor = Color.FromArgb(0, 200, 255);
            }

            MSDataLab.Text = "<P0" + num + ">";

            //pin标签
            for (int i = 0; i < 8; i++)
            {
                PortLabArray[i].Text = "P" + num + i.ToString();
            }

            //连接标签 矩阵键盘：列 P00-P03 行 P04-P07
            MKPortLab.Text = "矩阵键盘：列 P" + num + "0-P" + num + "3 行 P" + num + "4-P" + num + "7";
        }

        private void SKey_PortDisplay(char num)//独立键盘键盘 端口标号 更新
        {
            Label[] PortLabArray = { SSLab1, SSLab2, SSLab3, SSLab4 };

            //解析标签
            if (SSDatalab.ForeColor == Color.FromArgb(0, 200, 255))
            {
                SSDatalab.ForeColor = Color.FromArgb(100, 255, 000);
            }
            else if (MSDataLab.ForeColor == Color.FromArgb(100, 255, 0))
            {
                SSDatalab.ForeColor = Color.FromArgb(255, 200, 0);
            }
            else
            {
                SSDatalab.ForeColor = Color.FromArgb(0, 200, 255);
            }

            SSDatalab.Text = "<D0" + num + ">";

            //连接标签
            SKPortLab.Text = "独立键盘：P" + num + "0 - P" + num + "3";


            //pin 标签
            for (int i = 0; i < 4; i++)
            {
                PortLabArray[i].Text = "P" + num + i.ToString();
            }
        }

        private void Packet_Analysis(string str)//通信协议解析函数
        {
            char[] msg = str.ToCharArray();

            if (str.Length > 3)
            {
                switch (msg[1])
                {
                    case 'M':   //矩阵键盘数据回传
                        {
                            if(MKRad1.Checked)
                                Diaplay_MatrixKeyboard(msg[3], msg[2]);
                            break;
                        }
                    case 'S':   //独立键盘数据回传
                        {
                            if(SKRad1.Checked)
                                Diaplay_IndependentKeyboard(msg[3]);
                            break;
                        }
                    case 'P':   //矩阵键盘配置
                        {
                            MKey_PortDisplay(msg[3]);
                            break;
                        }
                    case 'D':   //独立键盘配置
                        {
                            SKey_PortDisplay(msg[3]);
                            break;
                        }
                    default: break;
                }
            }
        }

        private void post_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //发送数据
            if (serialPort.IsOpen)
            {//如果串口开启
                string str = serialPort.ReadExisting();//字符串方式读

                if (ReceiveTbox.Text.Length > 600)
                {
                    ReceiveTbox.Text = ReceiveTbox.Text.Remove(0, 300);
                }

                ReceiveTbox.AppendText(str);
                ReceiveTbox.Focus();

                Packet_Analysis(str);
            }
            else
            {
                MessageBox.Show("通讯端口由于其他异常被迫关闭！");
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

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            IsDrag = true;
            enterX = e.Location.X;
            enterY = e.Location.Y;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            IsDrag = false;
            enterX = 0;
            enterY = 0;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDrag)
            {
                Left += e.Location.X - enterX;
                Top += e.Location.Y - enterY;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //发送数据
            if (serialPort.IsOpen)
            {//如果串口开启
                if (SPbox.Text == null || MPbox.Text == null)
                {
                    MessageBox.Show("请选择正确的端口");
                }
                else if (SPbox.Text == MPbox.Text)
                {
                    MessageBox.Show("独立键盘和矩阵键盘的端口不能相同");
                }
                else
                {
                    if (SPbox.Text == "Port 0")
                    {
                        serialPort.Write("<D0><D1>");//写数据
                    }
                    else if (SPbox.Text == "Port 1")
                    {
                        serialPort.Write("<D1><D1>");//写数据
                    }
                    else if (SPbox.Text == "Port 2")
                    {
                        serialPort.Write("<D2><D2>");//写数据 
                    }

                    System.Threading.Thread.Sleep(100);

                    if (MPbox.Text == "Port 0")
                    {
                        serialPort.Write("<P0><P0>");//写数据
                    }
                    else if (MPbox.Text == "Port 1")
                    {
                        serialPort.Write("<P1><P1>");//写数据
                    }
                    else if (MPbox.Text == "Port 2")
                    {
                        serialPort.Write("<P2><P2>");//写数据 
                    }

                }
            }
            else
            {
                MessageBox.Show("串口未打开");
            }
        }

        private void ReceiveTbox_TextChanged(object sender, EventArgs e)//保持侧测试窗口
        {
            ReceiveTbox.ScrollToCaret();
        }

        private void SK_MouseDown(object sender, MouseEventArgs e)//独立键盘离线演示程序 激活按键
        {
            Button senderButton = (Button)sender;
            if (SKRad2.Checked)
            {
                if (senderButton.Text == "S1")
                {
                    Diaplay_IndependentKeyboard('1');
                }
                else if (senderButton.Text == "S2")
                {
                    Diaplay_IndependentKeyboard('2');
                }
                else if (senderButton.Text == "S3")
                {
                    Diaplay_IndependentKeyboard('4');
                }
                else if (senderButton.Text == "S4")
                {
                    Diaplay_IndependentKeyboard('8');
                }
            }

        }

        private void SK_MouseUp(object sender, MouseEventArgs e)//独立键盘离线演示程序 激活按键
        {
            if (SKRad2.Checked)
            {
                Diaplay_IndependentKeyboard('0');
            }
        }

        private void ColRawLight(int col, Color colr, int row, Color rowr)//用于高亮网络
        {
            Label[] colLabArrayH = { colLabH1, colLabH2, colLabH3, colLabH4 };
            Label[] colLabArrayL = { colLabL1, colLabL2, colLabL3, colLabL4 };
            Label[] rowLabArray = { rowLab1, rowLab2, rowLab3, rowLab4 };
            Label[] rowLabArrayH = { rowLabH1, rowLabH2, rowLabH3, rowLabH4 };
            Label[] rowLabArrayL = { rowLabL1, rowLabL2, rowLabL3, rowLabL4 };

            colLabArrayH[col].BackColor = colr;
            colLabArrayL[col].BackColor = colr;

            rowLabArray[row].BackColor = rowr;
            rowLabArrayH[row].BackColor = rowr;
            rowLabArrayL[row].BackColor = rowr;

        }

        private void MK11_MouseDown(object sender, MouseEventArgs e)//矩阵键盘离线演示程序 激活按键
        {
            Button senderButton = (Button)sender;

            if (MKRad2.Checked) //离线模式
            {
                switch (senderButton.Text.ToCharArray()[0])
                {
                    case '1': Diaplay_MatrixKeyboard('1', '1'); break;
                    case '2': Diaplay_MatrixKeyboard('1', '2'); break;
                    case '3': Diaplay_MatrixKeyboard('1', '4'); break;
                    case 'A': Diaplay_MatrixKeyboard('1', '8'); break;

                    case '4': Diaplay_MatrixKeyboard('2', '1'); break;
                    case '5': Diaplay_MatrixKeyboard('2', '2'); break;
                    case '6': Diaplay_MatrixKeyboard('2', '4'); break;
                    case 'B': Diaplay_MatrixKeyboard('2', '8'); break;

                    case '7': Diaplay_MatrixKeyboard('4', '1'); break;
                    case '8': Diaplay_MatrixKeyboard('4', '2'); break;
                    case '9': Diaplay_MatrixKeyboard('4', '4'); break;
                    case 'C': Diaplay_MatrixKeyboard('4', '8'); break;

                    case '*': Diaplay_MatrixKeyboard('8', '1'); break;
                    case '0': Diaplay_MatrixKeyboard('8', '2'); break;
                    case '#': Diaplay_MatrixKeyboard('8', '4'); break;
                    case 'D': Diaplay_MatrixKeyboard('8', '8'); break;
                }
            }
            else if (MKRad3.Checked)//过程模式
            {
                char pos = senderButton.Text.ToCharArray()[0];

                if (rowRad.Checked)//行扫描模式
                {
                    if (pos == '1' || pos == '2' || pos == '3' || pos == 'A')
                    {
                        ColRawLight(1, Color.FromArgb(255, 200, 0), 0, Color.FromArgb(255, 200, 0));
                        rowLab.Text = "ROW = 1";
                        rowLab.ForeColor = Color.FromArgb(255, 200, 0);
                        
                    }
                    else if (pos == '4' || pos == '5' || pos == '6' || pos == 'B')
                    {
                        ColRawLight(1, Color.FromArgb(255, 200, 0), 1, Color.FromArgb(255, 200, 0));
                        rowLab.Text = "ROW = 2";
                        rowLab.ForeColor = Color.FromArgb(255, 200, 0);
                    }
                    else if (pos == '7' || pos == '8' || pos == '9' || pos == 'C')
                    {
                        ColRawLight(1, Color.FromArgb(255, 200, 0), 2, Color.FromArgb(255, 200, 0));
                        rowLab.Text = "ROW = 3";
                        rowLab.ForeColor = Color.FromArgb(255, 200, 0);
                    }
                    else if (pos == '*' || pos == '0' || pos == '#' || pos == 'D')
                    {
                        ColRawLight(1, Color.FromArgb(255, 200, 0), 3, Color.FromArgb(255, 200, 0));
                        rowLab.Text = "ROW = 4";
                        rowLab.ForeColor = Color.FromArgb(255, 200, 0);
                    }
                }
                else if (colRad.Checked)//列扫描模式
                {
                    if (pos == '1' || pos == '4' || pos == '7' || pos == '*')
                    {
                        ColRawLight(0, Color.FromArgb(100, 255, 0), 0, Color.FromArgb(100, 255, 0));
                        colLab.Text = "COL = 1";
                        colLab.ForeColor = Color.FromArgb(100, 255, 0);
                    }
                    else if (pos == '2' || pos == '5' || pos == '8' || pos == '0')
                    {
                        ColRawLight(1, Color.FromArgb(100, 255, 0), 0, Color.FromArgb(100, 255, 0));
                        colLab.Text = "COL = 2";
                        colLab.ForeColor = Color.FromArgb(100, 255, 0);
                    }
                    else if (pos == '3' || pos == '6' || pos == '9' || pos == '#')
                    {
                        ColRawLight(2, Color.FromArgb(100, 255, 0), 0, Color.FromArgb(100, 255, 0));
                        colLab.Text = "COL = 3";
                        colLab.ForeColor = Color.FromArgb(100, 255, 0);
                    }
                    else if (pos == 'A' || pos == 'B' || pos == 'C' || pos == 'D')
                    {
                        ColRawLight(3, Color.FromArgb(100, 255, 0), 0,Color.FromArgb(100, 255, 0));
                        colLab.Text = "COL = 4";
                        colLab.ForeColor = Color.FromArgb(100, 255, 0);
                    }
                }
            }
        }

        private void MK11_MouseUp(object sender, MouseEventArgs e)//矩阵键盘离线演示程序 失效按键
        {
            if (MKRad2.Checked)//离线模式
            {
               // Diaplay_MatrixKeyboard('0', '0');
            }
            else if(MKRad3.Checked)//过程模式
            {
                if (rowRad.Checked)//行扫描模式
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ColRawLight(i, Color.FromArgb(255, 200, 0), i, Color.FromArgb(72, 82, 92));
                        rowLab.Text = "ROW = 0";
                        rowLab.ForeColor = Color.FromArgb(72, 82, 92);
                    }
                }
                else if (colRad.Checked)//列扫描模式
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ColRawLight(i, Color.FromArgb(72, 82, 92), i, Color.FromArgb(100, 255, 0));
                        colLab.Text = "COL = 0";
                        colLab.ForeColor = Color.FromArgb(72, 82, 92);
                    }
                }
            }
        }

        private void MKRad3_CheckedChanged(object sender, EventArgs e)//激活行扫描 或 列扫描 的NET
        {
            
            if (rowRad.Checked)
            {
                for(int i = 0;i < 4;i++)
                {
                    ColRawLight(i, Color.FromArgb(255, 200, 0), i, Color.FromArgb(72, 82, 92));
                }
            }
            if (colRad.Checked)
            {
                for (int i = 0; i < 4; i++)
                {
                    ColRawLight(i, Color.FromArgb(72, 82, 92), i, Color.FromArgb(100, 255, 0));
                }
            }
        }

        private void rowRad_Click(object sender, EventArgs e)//行列扫描切换高亮
        {
            if (MKRad3.Checked)
            {
                if (rowRad.Checked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ColRawLight(i, Color.FromArgb(255, 200, 0), i, Color.FromArgb(72, 82, 92));
                    }
                }
                if (colRad.Checked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ColRawLight(i, Color.FromArgb(72, 82, 92), i, Color.FromArgb(100, 255, 0));
                    }
                }
            }
        }
        private void MKRad2_CheckedChanged(object sender, EventArgs e)//清除过程视图
        {
            Diaplay_MatrixKeyboard('0', '0');
        }

        private void MKRad1_CheckedChanged(object sender, EventArgs e)//清除过程视图
        {
            Diaplay_MatrixKeyboard('0', '0');
        }

        private void rowLab_TextChanged(object sender, EventArgs e)//ROW数据发生改变
        {
            Dictionary<string, string> CtoS = new Dictionary<string, string>();

            CtoS.Add("ROW = 0","1111"); CtoS.Add("ROW = 1", "1110"); CtoS.Add("ROW = 2", "1101");
            CtoS.Add("ROW = 3", "1011"); CtoS.Add("ROW = 4", "0111");

            //数据不为0
            if (rowLab.Text != "ROW = 0")
            {
                rowLabHEX.ForeColor = Color.FromArgb(100, 255, 0);
                rowLabHEX.Text = CtoS[rowLab.Text];
            }
            else
            {
                rowLabHEX.ForeColor = Color.FromArgb(72, 82, 92);
                rowLabHEX.Text = "0000";
            }

        }

        private void colLab_TextChanged(object sender, EventArgs e)//col数据发生改变
        {
            Dictionary<string, string> CtoS = new Dictionary<string, string>();

            CtoS.Add("COL = 0", "1111"); CtoS.Add("COL = 1", "1110"); CtoS.Add("COL = 2", "1101");
            CtoS.Add("COL = 3", "1011"); CtoS.Add("COL = 4", "0111");

            //数据不为0
            if (colLab.Text != "COL = 0")
            {
                colLabHEX.ForeColor = Color.FromArgb(255, 200, 0);
                colLabHEX.Text = CtoS[colLab.Text];
            }
            else
            {
                colLabHEX.ForeColor = Color.FromArgb(72, 82, 92);
                colLabHEX.Text = "0000";
            }
        }

        private void groupBox7_TextChanged(object sender, EventArgs e)
        {
            string hexText = "FF";

            Dictionary<string, string> RtoS = new Dictionary<string, string>();
            RtoS.Add("ROW = 0", "F"); RtoS.Add("ROW = 1", "E"); RtoS.Add("ROW = 2", "D");
            RtoS.Add("ROW = 3", "B"); RtoS.Add("ROW = 4", "7");

            Dictionary<string, string> CtoS = new Dictionary<string, string>();
            CtoS.Add("COL = 0", "F"); CtoS.Add("COL = 1", "E"); CtoS.Add("COL = 2", "D");
            CtoS.Add("COL = 3", "B"); CtoS.Add("COL = 4", "7");

            HexLab.Text = RtoS[rowLab.Text] + CtoS[colLab.Text];

            if (HexLab.Text == "FF")
            {
                HexLab.ForeColor = Color.FromArgb(72, 82, 92);
            }
            else
            {
                HexLab.ForeColor = Color.FromArgb(0, 200, 234);
            }
        }
    }
}
