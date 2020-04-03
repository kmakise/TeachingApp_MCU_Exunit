#ifndef __KEYBOARD_H
#define __KEYBOARD_H

#define uint16_t	unsigned int 
#define uint8_t 	unsigned char


uint8_t MatrixKeyboardScan(uint8_t port);
uint8_t IndependentKeyboardScan(uint8_t port);


#endif /*__KEYBOARD_H*/
