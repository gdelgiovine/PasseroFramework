using System;
using Microsoft.VisualBasic;

namespace Passero.Framework
{
    public class ExecutionResult
    {
        public object Value = null;
        public eResultCode ResultCode = 0;
        public int LastDllError = 0;
        public int ErrorCode = 0;
        public string DebugInfo = "";
        public string ResultMessage = "";
        public Exception Exception = null;
        public string Context = "";
        public InnerExecutionResult InnerExecutionResult;
        private bool mFailed;

        public bool Success
        {
            get
            {

                if (ResultCode == eResultCode.Failed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool Failed
        {
            get
            {

                if (ResultCode == eResultCode.Failed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            Value = null;
            ResultCode = eResultCode.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
            Information.Err().Clear();
        }

        public InnerExecutionResult ToInnerExecutionResult()
        {
            var x = new InnerExecutionResult();
            x.Context = Context;
            x.ErrorCode = ErrorCode;
            x.Exception = Exception;
            x.ResultCode = (InnerExecutionResult.eResultCode)ResultCode;
            x.DebugInfo = DebugInfo;    
            x.ResultMessage = ResultMessage;
            x.Value = Value;
            return x;
        }

        public ExecutionResult Set(ExecutionResult ExecutionResult)
        {
            ResultCode = ExecutionResult.ResultCode;
            Exception = ExecutionResult.Exception;
            InnerExecutionResult = ExecutionResult.ToInnerExecutionResult();
            return this;
        }

        public ExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }

        public enum eResultCode
        {
            Success = 0,
            Warning = 1,
            Failed = 2
        }
    }

    public class InnerExecutionResult
    {
        public object Value = null;
        public eResultCode ResultCode = 0;
        public int ErrorCode = 0;
        public string ResultMessage = "";
        public string DebugInfo = "";
        public Exception Exception = null;
        public string Context = "";
        private bool mFailed;

        public bool Success
        {
            get
            {

                if (ResultCode == eResultCode.Failed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool Failed
        {
            get
            {

                if (ResultCode == eResultCode.Failed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Reset()
        {
            Value = null;
            ResultCode = eResultCode.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
        }

        public InnerExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }

        public enum eResultCode
        {
            Success = 0,
            Warning = 1,
            Failed = 2
        }
    }
}