using System;
using System.Collections.Generic;
using System.Text;

namespace TestAppPWA.Utils
{
    public class ReturnResult
    {
        public bool IsOk { get; private set; }
        public string Error { get; private set; }
        public object Data { get; private set; }

        public ReturnResult(bool isOk, string error, object data)
        {
            IsOk = isOk;
            Error = error;
            Data = data;
        }

        public ReturnResult(bool isOk, string error) : this(isOk, error, null)
        {

        }

        public static ReturnResult OK()
        {
            return new ReturnResult(true, null);
        }

        public static ReturnResult OK(object data)
        {
            return new ReturnResult(true, null, data);
        }

        public static ReturnResult KO(string error, object data = null)
        {
            return new ReturnResult(false, error, data);
        }
    }
}
