using Microsoft.VisualBasic;
using System;

namespace Passero.Framework
{

    /// <summary>
    /// 
    /// </summary>
    public enum ExecutionResultCodes
    {
        /// <summary>
        /// The success
        /// </summary>
        Success = 0,
        /// <summary>
        /// The warning
        /// </summary>
        Warning = 1,
        /// <summary>
        /// The failed
        /// </summary>
        Failed = 2
    }
    /// <summary>
    /// 
    /// </summary>
    public class ExecutionResult
    {
        /// <summary>
        /// The value
        /// </summary>
        public object Value = null;
        /// <summary>
        /// The result code
        /// </summary>
        public ExecutionResultCodes ResultCode = 0;
        /// <summary>
        /// The last DLL error
        /// </summary>
        public int LastDllError = 0;
        /// <summary>
        /// The error code
        /// </summary>
        public int ErrorCode = 0;
        /// <summary>
        /// The debug information
        /// </summary>
        public string DebugInfo = "";
        /// <summary>
        /// The result message
        /// </summary>
        public string ResultMessage = "";
        /// <summary>
        /// The exception
        /// </summary>
        public Exception Exception = null;
        /// <summary>
        /// The context
        /// </summary>
        public string Context = "";
        /// <summary>
        /// The inner execution result
        /// </summary>
        public InnerExecutionResult InnerExecutionResult;
#pragma warning disable CS0169 // Il campo 'ExecutionResult.mFailed' non viene mai usato
        /// <summary>
        /// The m failed
        /// </summary>
        private bool mFailed;
#pragma warning restore CS0169 // Il campo 'ExecutionResult.mFailed' non viene mai usato

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExecutionResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success
        {
            get
            {

                if (ResultCode == ExecutionResultCodes.Failed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExecutionResult"/> is failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed
        {
            get
            {

                if (ResultCode == ExecutionResultCodes.Failed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Value = null;
            ResultCode = ExecutionResultCodes.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
            Information.Err().Clear();
        }

        /// <summary>
        /// Converts to innerexecutionresult.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the specified execution result.
        /// </summary>
        /// <param name="ExecutionResult">The execution result.</param>
        /// <returns></returns>
        public ExecutionResult Set(ExecutionResult ExecutionResult)
        {
            ResultCode = ExecutionResult.ResultCode;
            Exception = ExecutionResult.Exception;
            InnerExecutionResult = ExecutionResult.ToInnerExecutionResult();
            return this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult"/> class.
        /// </summary>
        /// <param name="Context">The context.</param>
        public ExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class InnerExecutionResult
    {
        /// <summary>
        /// The value
        /// </summary>
        public object Value = null;
        /// <summary>
        /// The result code
        /// </summary>
        public eResultCode ResultCode = 0;
        /// <summary>
        /// The error code
        /// </summary>
        public int ErrorCode = 0;
        /// <summary>
        /// The result message
        /// </summary>
        public string ResultMessage = "";
        /// <summary>
        /// The debug information
        /// </summary>
        public string DebugInfo = "";
        /// <summary>
        /// The exception
        /// </summary>
        public Exception Exception = null;
        /// <summary>
        /// The context
        /// </summary>
        public string Context = "";
#pragma warning disable CS0169 // Il campo 'InnerExecutionResult.mFailed' non viene mai usato
        /// <summary>
        /// The m failed
        /// </summary>
        private bool mFailed;
#pragma warning restore CS0169 // Il campo 'InnerExecutionResult.mFailed' non viene mai usato

        /// <summary>
        /// Gets a value indicating whether this <see cref="InnerExecutionResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="InnerExecutionResult"/> is failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Value = null;
            ResultCode = eResultCode.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerExecutionResult"/> class.
        /// </summary>
        /// <param name="Context">The context.</param>
        public InnerExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum eResultCode
        {
            /// <summary>
            /// The success
            /// </summary>
            Success = 0,
            /// <summary>
            /// The warning
            /// </summary>
            Warning = 1,
            /// <summary>
            /// The failed
            /// </summary>
            Failed = 2
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExecutionResult<T>
    {
        /// <summary>
        /// The value
        /// </summary>
        public T Value = default(T); //CType(Activator.CreateInstance(GetType(T)), T)
        /// <summary>
        /// The result code
        /// </summary>
        public ExecutionResultCodes ResultCode = 0;
        /// <summary>
        /// The last DLL error
        /// </summary>
        public int LastDllError = 0;
        /// <summary>
        /// The error code
        /// </summary>
        public int ErrorCode = 0;
        /// <summary>
        /// The debug information
        /// </summary>
        public string DebugInfo = "";
        /// <summary>
        /// The result message
        /// </summary>
        public string ResultMessage = "";
        /// <summary>
        /// The exception
        /// </summary>
        public Exception Exception = null;
        /// <summary>
        /// The context
        /// </summary>
        public string Context = "";
        /// <summary>
        /// The inner execution result
        /// </summary>
        public InnerExecutionResult InnerExecutionResult;
        /// <summary>
        /// The m failed
        /// </summary>
        private bool mFailed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExecutionResult{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success
        {
            get
            {

                if (ResultCode == ExecutionResultCodes.Failed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExecutionResult{T}"/> is failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed
        {
            get
            {

                if (ResultCode == ExecutionResultCodes.Failed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ExecutionResult{T}"/> is warning.
        /// </summary>
        /// <value>
        ///   <c>true</c> if warning; otherwise, <c>false</c>.
        /// </value>
        public bool Warning
        {
            get
            {

                if (ResultCode == ExecutionResultCodes.Warning)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Value = default(T);
            ResultCode = ExecutionResultCodes.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
            Microsoft.VisualBasic.Information.Err().Clear();
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionResult{T}"/> class.
        /// </summary>
        /// <param name="Context">The context.</param>
        public ExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }
        /// <summary>
        /// Converts to executionresult.
        /// </summary>
        /// <returns></returns>
        public ExecutionResult ToExecutionResult()
        {
            ExecutionResult ER = new ExecutionResult();
            ER.ResultCode = ResultCode;
            ER.Value = Value;
            ER.Context = Context;
            ER.DebugInfo = DebugInfo;
            ER.ErrorCode = ErrorCode;
            ER.Exception = Exception;
            ER.LastDllError = LastDllError;
            ER.ResultMessage = ResultMessage;
            ER.InnerExecutionResult = InnerExecutionResult;
            return ER;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InnerExecutionResult<T>
    {
        /// <summary>
        /// The value
        /// </summary>
        public T Value = default(T);
        /// <summary>
        /// The result code
        /// </summary>
        public eResultCode ResultCode = 0;
        /// <summary>
        /// The error code
        /// </summary>
        public int ErrorCode = 0;
        /// <summary>
        /// The result message
        /// </summary>
        public string ResultMessage = "";
        /// <summary>
        /// The debug information
        /// </summary>
        public string DebugInfo = "";
        /// <summary>
        /// The exception
        /// </summary>
        public Exception Exception = null;
        /// <summary>
        /// The context
        /// </summary>
        public string Context = "";
        /// <summary>
        /// The m failed
        /// </summary>
        private bool mFailed;

        /// <summary>
        /// Gets a value indicating whether this <see cref="InnerExecutionResult{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets a value indicating whether this <see cref="InnerExecutionResult{T}"/> is failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Value = default(T);
            ResultCode = eResultCode.Success;
            ErrorCode = 0;
            ResultMessage = "";
            Exception = null;
            DebugInfo = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerExecutionResult{T}"/> class.
        /// </summary>
        /// <param name="Context">The context.</param>
        public InnerExecutionResult(string Context = "")
        {
            Reset();
            this.Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum eResultCode
        {
            /// <summary>
            /// The success
            /// </summary>
            Success = 0,
            /// <summary>
            /// The warning
            /// </summary>
            Warning = 1,
            /// <summary>
            /// The failed
            /// </summary>
            Failed = 2
        }
    }

}