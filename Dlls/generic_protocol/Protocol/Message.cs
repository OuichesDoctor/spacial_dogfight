using System;

namespace GameProtocol
{
    public interface Message
    {
        bool WriteFields(byte[] fields);
        byte[] ReadFields();
    }
}
