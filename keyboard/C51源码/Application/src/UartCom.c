/**
  ******************************************************************************
  * @file    UartCom.c
  * @author  张东
  * @version V1.0.0
  * @date    2019-7-16
  * @brief   串口驱动相关
  ******************************************************************************
  */
#include "UartCom.h"
#include "reg52.h"

#define STARTBT 					0x3c
#define ENDBT							0x3e

bit	B_TX1_Busy;																//发送忙标志

USARTBufTypedef g_UsartRx;

/**********************************************************************/
/*							  串口初始化
*			波特率=11059200 / { 32×[65536-(RCAP2H,RCAP2L)] }    
*			其中的RCAP2H,RCAP2L为自动重装值，由上式得：
*			RCAP2H,RCAP2L=65536 11059200 / (32×波特率) 
*			得波特率为115200时，RCAP2H,RCAP2L=0xff
**********************************************************************/
void UART1_Init(void)   												//串口初始化115200波特率
{
	SCON=0x50; 															//串口工作方式1，8位UART，波特率可变  
	TH2=0xFF;           
	TL2=0xFD;    														//波特率:115200 晶振=11.0592MHz 
	RCAP2H=0xFF;   
	RCAP2L=0xFD; 														//16位自动再装入值
	TCLK=1;   
	RCLK=1;   
	C_T2=0;   
	EXEN2=0; 															//波特率发生器工作方式
	TR2=1; 																//定时器2开始
	ES=1;
	EA=1;
	
	
	g_UsartRx.status = 0;
	g_UsartRx.len = 0;
}
/**********************************************************************/
/*							  串口发送于接收
/**********************************************************************/
void UART1_SendData(uint8_t dat)
{
    while (B_TX1_Busy);               									//等待前面的数据发送完成
    B_TX1_Busy = 1;
    SBUF = dat;                											//写数据到Uart1数据寄存器
}
void UART1_SendString(uint8_t *s)
{
    while (*s)                  										//检测字符串结束标志
    {
        UART1_SendData(*s++);         									//发送当前字符
    }
}


USARTMsgTypedef getUsart1Msg(void)
{
	USARTMsgTypedef msg;
	int i;
	
	if(g_UsartRx.status == 1)		//如果缓冲区锁定
	{
		msg.ID = g_UsartRx.buf[1];								//获取消息标识符
		for(i = 0;i < g_UsartRx.len - 2;i++)	//提取数据
		{
			msg.msg[i] = g_UsartRx.buf[i + 2];
		}
		msg.len = g_UsartRx.len;									//获得数据长度
		g_UsartRx.status = 0;											//缓冲区解锁
	}
	else
	{
		msg.len = 0;
		msg.ID = 255;
	}
	return msg;																//返回消息数据
}

//Usart 1 Receive Function
void Usart1_Receive(void)
{
	uint8_t bt; 
	
	bt = SBUF;//读取接收的字节
	
	if(g_UsartRx.status == 0)//当前消息未被锁定
	{
		if(bt == ENDBT)				//当前字节为结束标志
		{
			g_UsartRx.status = 1;//缓冲区锁定
		}
		else
		{
			if(bt == STARTBT)//当前字节为开始标志
			{
				g_UsartRx.len = 0;
			}
			//将当前字节写入缓冲区
			g_UsartRx.buf[g_UsartRx.len] = bt;
			//移动写入下标并限制
			g_UsartRx.len = (g_UsartRx.len + 1) % 64;
		}
	}
}


/**********************************************************************/
/*							  串口中断
/**********************************************************************/
void UART1_Int() interrupt 4 											//串口中断
{
	if(RI)		 														//接收中断标志位
	{
		RI = 0;
		//SBUF
		Usart1_Receive();
	}
	if(TI)		 														//发送中断标志位
	{
		TI = 0;
		B_TX1_Busy = 0;													//清除发送忙标志
	}
}
	
	
	
	