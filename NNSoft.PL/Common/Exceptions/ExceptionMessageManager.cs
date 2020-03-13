namespace NNSoft.PL.Common.Exceptions
{
    class ExceptionMessageManager
    {
        public static string BuildException(int errorCode)
        {
            switch(errorCode)
            {
                case 0: return "Что-то пошло не так. Нативная библиотека вернула незавершенный результат.";
                default: return "Операция невозможна";
            }
        }
    }
}
