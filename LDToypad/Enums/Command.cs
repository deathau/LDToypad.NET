using System;
using System.Collections.Generic;
using System.Text;

namespace LDToypad
{
    public enum Command
    {
        LedChange = 0x01,
        TagNotFound = 0x02,
        LstModel = 0x0a,
        TagScan = 0x0b,
        TagRead = 0x12,
        Connected = 0x19
    }
}
