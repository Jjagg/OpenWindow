// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow.Backends.Windows
{
    internal static class Extensions
    {
        public static Message ToMessage(this Msg nativeMessage)
        {
            var m = new Message();
            
            switch (nativeMessage.message)
            {
                case WindowMessage.Quit:
                    m.Type = MessageType.Closing;
                    break;
                default:
                    m.Type = (MessageType) nativeMessage.message;
                    break;
            }

            return m;
        }

    }
}
