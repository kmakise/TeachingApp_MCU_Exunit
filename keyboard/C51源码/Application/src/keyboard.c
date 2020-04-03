/**
  ******************************************************************************
  * @file    keyboard.c
  * @author  张东
  * @version V1.0.0
  * @date    2019-7-16
  * @brief   矩阵键盘与独立键盘驱动程序
  ******************************************************************************
  */
#include "keyboard.h"
#include "reg52.h"


void outPort(uint8_t num,uint8_t dat)
{
	switch(num)
	{
		case 0:P0 = dat;break;
		case 1:P1 = dat;break;
		case 2:P2 = dat;break;
		default:P0 = dat;break;
	}
}
uint8_t getPoart(uint8_t num)
{
	switch(num)
	{
		case 0:return P0;break;
		case 1:return P1;break;
		case 2:return P2;break;
		default:return P0;break;
	}
}


/**
  * @breif  矩阵键盘扫描程序.
	* @note  	支持更换port；
	* @param  port序号0 1 2 .
	* @retval none.
	*/
uint8_t MatrixKeyboardScan(uint8_t port)
{
	uint8_t col,row;
	
	outPort(port,0x0f);						//拉高行
	col =  getPoart(port) & 0x0f;	//读取列
	outPort(port,0xf0);						//拉高列
	row =  getPoart(port) & 0xf0;	//读取行
	outPort(port,0xff);						//复位
	
	return ~((row|col)&0xff);
}

/**
  * @breif  矩阵键盘扫描程序.
	* @note  	支持更换port；
	* @param  port序号0 1 2 .
	* @retval none.
	*/
uint8_t IndependentKeyboardScan(uint8_t port)
{
	outPort(port,0xff);	
	return ~(getPoart(port) & 0x0f) & 0x0f;
}









