#ifndef __UARTCOM_H
#define __UARTCOM_H

#define uint16_t	unsigned int 
#define uint8_t 	unsigned char
	
typedef struct
{
	uint8_t buf[16];
	uint8_t len;
	uint8_t status;
	
}USARTBufTypedef;//串口接收缓冲区数据类型

typedef struct
{
	uint8_t ID;
	uint8_t msg[16];
	uint8_t len;
	
}USARTMsgTypedef;//串口消息存储数据类型



void UART1_Init(void);   												//串口初始化115200波特率

void UART1_SendString(uint8_t *s);							//串口字符串发送

USARTMsgTypedef getUsart1Msg(void);							//串口指定协议消息接收 开始“<“ 标识”A“ 数据”...“ 结束”>“

#endif /*__UARTCOM_H*/


