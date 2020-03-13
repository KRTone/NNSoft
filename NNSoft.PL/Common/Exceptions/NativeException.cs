using System;

namespace NNSoft.PL.Common.Exceptions
{
    public class NativeException : Exception
    {
        const string ErrorCodeName = "ErrorCode";

        public NativeException() { }

        public NativeException(int errorCode)
        {
            Data.Add(ErrorCodeName, errorCode);
        }

        public NativeException(string message, int errorCode)
            : base(message)
        {
            Data.Add(ErrorCodeName, errorCode);
        }

        public override string Message
        {
            get
            {
                return Data.Contains(ErrorCodeName) ? base.Message + $" Код ошибки: {Data[ErrorCodeName].ToString()}." : base.Message;
            }
        }
    }
}
