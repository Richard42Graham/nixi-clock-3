#include "api.h"

int open(int fd, char[] bus){
     int fd;
    if ((fd = open(bus, O_RDWR)) < 0)
    {
        //Failed to open
        return fd;
    }
    init(fd);
}

int init(int fd){
    int i=0;
        for (i = 0; i < sizeof(chips)/sizeof(char); i++)
        {
            if(write_reg(fd,chips[i],0x00,0x00)<0){
                return 1;
            }
        }

}

int close(int fd){
    return close(fd);
}

int read_reg(int fd, u_int8_t device_address, u_int8_t reg, char *output)
{
    ioctl(fd, I2C_SLAVE, device_address);
    struct i2c_msg msgs[2] = {
        {
            .addr = device_address,
            .flags = 0,
            .len = 1,
            .buf = &reg,
        },
        {.addr = device_address,
         .flags = I2C_M_RD | I2C_M_NOSTART,
         .len = 1,
         .buf = output},
    };
    struct i2c_rdwr_ioctl_data msgset[1] = {
        {.msgs = msgs,
         .nmsgs = 2}};
    return ioctl(fd, I2C_RDWR, &msgset);
}

int write_reg(int fd, u_int8_t device_address, u_int8_t reg, char input)
{
    ioctl(fd, I2C_SLAVE, device_address);
    struct i2c_msg msgs[1] = {
        {
            .addr = device_address,
            .flags = 0,
            .len = 2,
            .buf = (char[2]){reg, input},
        }};
    struct i2c_rdwr_ioctl_data msgset = {
        .msgs = msgs,
        .nmsgs = 1};
    return ioctl(fd, I2C_RDWR, &msgset);
}


int set_digit(int fd,digit digi,char value){
    return write_reg(fd,digi.chip,digi.adress, value);
}