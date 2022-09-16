namespace SimpleApp.Exceptions
{
    public class MyException : Exception
    {
        public int Code { get; private set; }

        public MyException(int code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}
