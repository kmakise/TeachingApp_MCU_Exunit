/**
  ******************************************************************************
  * @file    UartCom.c
  * @author  张东
  * @version V1.0.0
  * @date    2019-7-16
  * @brief   串口驱动相关
  ******************************************************************************
  */
	
/*Include start --------------------------------------------------------------*/
#include "keyApp.h"
#include "reg52.h"


#include "UartCom.h"
#include "keyboard.h"
#include "stdio.h"

/*Include end ----------------------------------------------------------------*/

/*Global Data Space start ----------------------------------------------------*/

SysSetTypedef g_SysConfig; //系统设置

StatusTypedef g_State = WITTING;

/*Global Data Space end ------------------------------------------------------*/

//软件延时毫秒
void delay_ms(uint16_t ms)
{
	uint16_t i,j;
	for(i = 0;i < ms;i++)
	{
		for(j = 0;j < 115;j++);
	}
}
/*Communication And System Config---------------------------------------------*/

/*MCU to PC------------------------------------------------------------*/

uint8_t HexToChar(uint8_t hex)
{
	uint8_t ch[16] = { '0','1','2','3','4','5','6','7',
										 '8','9','A','B','C','D','E','F', };
	return ch[hex];
}

//PC消息发送定长5字节 <IXX>
void SendMsg(uint8_t ID,uint8_t dat)
{
	uint8_t str[6];
	
	str[0] = '<';														//开始
	str[1] = ID;														//标识
	str[2] = HexToChar((dat & 0xf0) >> 4);	//数据1
	str[3] = HexToChar(dat & 0x0f);					//数据2
	str[4] = '>';														//结束
	str[5] = 0;
	
	UART1_SendString(str);
}

//上传数据到PC
void UpdataToPC(void)
{
	static uint16_t div = 0;
	uint8_t temp;
	
	div++;
	if(div > 50)//	软件50分频
	{
		div = 0;
		
		//上传矩阵键盘数据
		temp = MatrixKeyboardScan(g_SysConfig.MK_Port);
		SendMsg('M',temp);
		
		delay_ms(10);
		//上传独立键盘数据
		temp = IndependentKeyboardScan(g_SysConfig.SK_Port);
		SendMsg('S',temp);
	}
}

/*PC to MCU------------------------------------------------------------*/
//矩阵键盘port设置
void MatrixKeyboardPortSet(uint8_t * cmd)
{
	switch(cmd[0])
	{
		case '0':
		{
			g_SysConfig.MK_Port = 0;
			SendMsg('P',0x00);
			break;
		}		
		case '1':
		{
			g_SysConfig.MK_Port = 1;
			SendMsg('P',0x01);
			break;
		}		
		case '2':
		{
			g_SysConfig.MK_Port = 2;
			SendMsg('P',0x02);
			break;
		}		
		default:
		{
			SendMsg('P',0x01);
			break;
		}
	}
}
//独立键盘port设置
void SKeyboardPortSet(uint8_t * cmd)
{
	switch(cmd[0])
	{
		case '0':
		{
			g_SysConfig.SK_Port = 0;
			SendMsg('D',0x00);
			break;
		}		
		case '1':
		{
			g_SysConfig.SK_Port = 1;
			SendMsg('D',0x01);
			break;
		}		
		case '2':
		{
			g_SysConfig.SK_Port = 2;
			SendMsg('D',0x02);
			break;
		}		
		default:
		{
			SendMsg('D',0x01);
			break;
		}
	}
}


//串口通信解析程序
void Packet_Analysis(void)																			
{
	USARTMsgTypedef usartMsg; //串口数据
	
	//获得串口消息
	usartMsg = getUsart1Msg();
	
	
	//开始消息内容解析
	switch(usartMsg.ID)
	{
		case 'P'://设定矩阵键盘port
		{
			MatrixKeyboardPortSet(usartMsg.msg);
			break;
		}
		case 'D'://设定独立键盘端口
		{
			 SKeyboardPortSet(usartMsg.msg);
			break;
		}
		
		// null  msg
		case 255:break;
		
		default:
		{
			//func error feedback
			//UART1_SendString("Error\n");
			break;
		}
	}
}




/*setup and loop function ----------------------------------------------------*/
void setup(void)
{
	//hard ware init
	UART1_Init();
	
	g_SysConfig.MK_Port = 0;
	g_SysConfig.SK_Port = 1;
	
	SendMsg('P',0x00);
	SendMsg('D',0x01);
}


void loop(void)
{	
	Packet_Analysis();	
	
	UpdataToPC();
	
}
