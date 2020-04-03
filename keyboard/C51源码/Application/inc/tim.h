#ifndef __TIM_H
#define __TIM_H

#define uint16_t	unsigned int 
#define uint8_t 	unsigned char
	
typedef struct
{
	long int 	tick;
	uint8_t 	ms10;
	uint8_t 	s;
	uint8_t 	m;
	uint8_t 	h;
	
}TimDataTypedef;



void TIM_Init(void);
TimDataTypedef getTimeData(void);
#endif /*__TIM_H*/

