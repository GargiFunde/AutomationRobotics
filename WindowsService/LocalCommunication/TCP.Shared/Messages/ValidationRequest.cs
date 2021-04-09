using System;

namespace TCP.Shared.Messages
{
    [Serializable]
    public class ValidationRequest : RequestMessageBase
    {
        public String Email { get; set; }
        public String Password { get; set; } //Piyush
    }
}
