using Dapper;
using Dapper.Contrib.Extensions;
#if NET
#else
using Microsoft.Reporting.WebForms;
#endif

using Passero.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Passero.Framework.Reports
{
    public enum SSRSRenderFormat
    {
        XML,
        NULL,
        CSV,
        IMAGE,
        PDF,
        HTML40,
        HTML32,
        MHTML,
        EXCEL,
        WORD
    }
    public class DataSet
    {
        public string Name { get; set; } = string.Empty;
        public System.Data.IDbConnection DbConnection { get; set; }
        public Dapper.DynamicParameters Parameters { get; set; }= new DynamicParameters (); 
        public string SQLQuery { get; set; }    
        //public object Repository { get; set; }
        public Type ModelType { get; set; }
        public Dictionary<string, System.Reflection.PropertyInfo> ModelProperties= new Dictionary<string, System.Reflection.PropertyInfo>();
        public object Data { get; set; }    
        public object Model { get; set; }
        public DataSet() 
        {
        }   
        public DataSet(string Name, Type ModelType, IDbConnection DbConnection, Dapper .DynamicParameters Parameters=null)
        {
            this.Name = Name;   
            this.ModelType= ModelType;  
            this.DbConnection = DbConnection;   
            this.Parameters = Parameters;
            
            EnsureReportDataSet ();
        }   

        public bool EnsureReportDataSet()
        {
            if (this.ModelType != null && this.DbConnection != null)
            {
                object obj = Activator.CreateInstance(ModelType);
                this.Model = obj;   
                Passero.Framework.ReflectionHelper.SetPropertyValue(ref obj, "DbConnection", DbConnection);
                Passero.Framework.ReflectionHelper.SetPropertyValue(ref obj, "Parameters", Parameters);
                //this.Repository = obj;

                
                this.ModelProperties.Clear();
                foreach (var item in Passero.Framework.DapperHelper.Utilities.GetPropertiesInfo(ModelType))
                {
                    this.ModelProperties.Add(item.Name, item);
                }
                

                return true;
            }
            return false;   
        }

        public void LoadData()
        {
            this.Data = this.DbConnection.Query(this.SQLQuery, this.Parameters);
            
        }
    }

    public class ReportRenderRequestEventArgs : EventArgs
    {
        public bool Cancel {  get; set; }   
        public string ReportName { get; set; }  
        public Dictionary <string,DataSet> DataSets = new Dictionary<string, DataSet> ();
    }

    public class ReportAfterRenderEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public bool Success { get; set; }   
        public string ReportName { get; set; }
    }
    
    public class SSRSReport
    {
        public string ReportPath { get; set; }
        public SSRSRenderFormat ReportFormat { get; set; } = SSRSRenderFormat.PDF;
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult("Passero.Framework.Reports.SSRSReport.");
        public Dictionary<string, DataSet> DataSets { get; set; } = new Dictionary<string, DataSet>();


#if NET
        public Microsoft.Reporting.NETCore.LocalReport Report { get; set; } 
      
#else
        public Microsoft.Reporting.WebForms.LocalReport Report { get; set; }
      
#endif
     
        public SSRSReport()
        {
        
#if NET
            Report = new Microsoft.Reporting.NETCore.LocalReport();
#else
            Report = new Microsoft.Reporting.WebForms.LocalReport();
#endif

        }


        public event EventHandler ReportRenderRequest;

        protected virtual void OnReportRenderRequest(ReportRenderRequestEventArgs e)
        {
            ReportRenderRequest?.Invoke(this, e);
        }

        public event EventHandler ReportAfterRender;

        protected virtual void OnReportAfterRender(ReportAfterRenderEventArgs  e)
        {
            ReportAfterRender?.Invoke(this, e);
        }

        public byte[] Render(SSRSRenderFormat RenderFormat = SSRSRenderFormat.PDF)
        {
            LastExecutionResult.Reset();
            LastExecutionResult.Context = $"Passero.Framework.Reports.SSRSReports.Render({RenderFormat})";
            byte[] result = null;
            string f = RenderFormat.ToString();

            try
            {
                Report.ReportPath = this.ReportPath;
                IList<string> DataSetNames = Report.GetDataSourceNames();

                // Invoke OnReportRenderRequest
                ReportRenderRequestEventArgs  requestargs = new ReportRenderRequestEventArgs();
                requestargs.DataSets = new Dictionary<string, DataSet>();
                foreach (string DataSetName in this.DataSetNames() )
                {
                    DataSet ds = new DataSet();
                    ds.Name= DataSetName;
                    requestargs.DataSets.Add(DataSetName, ds);
                }
                this.OnReportRenderRequest (requestargs);   
                if (requestargs .Cancel ) 
                {
                    LastExecutionResult.ResultMessage = "Cancelled by User";
                    return null;
                }

                Report.DataSources.Clear();

                if (requestargs .DataSets != null && requestargs .DataSets.Count > 0) 
                {
                    bool ok = true;
                    foreach (var item in requestargs .DataSets .Values )
                    {
                        if (item.Data ==null) 
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok) this.DataSets = requestargs.DataSets;                
                }

                foreach (var item in this.DataSets.Values)
                {
                   item.EnsureReportDataSet();
#if NET
                   Microsoft.Reporting.NETCore.ReportDataSource  ds = new Microsoft.Reporting.NETCore .ReportDataSource(item.Name ,item.Data);
                   Report.DataSources.Add(ds);
#else
                   Microsoft.Reporting.WebForms.ReportDataSource ds = new Microsoft.Reporting.WebForms.ReportDataSource(item.Name ,item.Data);
                   Report.DataSources.Add(ds);
#endif
                }
                
                result = Report.Render(f);
            }
            catch (Exception ex)
            {
                LastExecutionResult.Exception = ex;
                LastExecutionResult.ResultMessage = ex.Message;
                LastExecutionResult.ErrorCode = 1;
            }
            return result;
        }

        public List<string> DataSetNames()
        {
            List<string> result = new List<string>();   
            try
            {
                result= Report.GetDataSourceNames().ToList();
            }
            catch (Exception)
            {

            }
            return result;
        }

    public ExecutionResult RenderAndSaveReport(string FileName, SSRSRenderFormat RenderFormat = SSRSRenderFormat.PDF)
        {
            var ER = new ExecutionResult();
            ER.Context = $"Passero.Framework.Reports.SSRSReports.RenderAndSaveReport({FileName},{RenderFormat})";
            byte[] result = null;

            result = Render(RenderFormat);
            if (LastExecutionResult.Failed)
            {
                ER = LastExecutionResult;
                return ER;
            }

            if (result is null)
            {
                ER.ErrorCode = 2;
                ER.ResultMessage = "Empty Rendering.";
                LastExecutionResult = ER;
                return ER;
            }

            try
            {
                Utilities.SaveByteArrayToFile(result, FileName);
            }

            catch (Exception ex)
            {
                ER.ErrorCode = 3;
                ER.ResultMessage = ex.Message;
                LastExecutionResult = ER;
                return ER;
            }

            return ER;

        }

    }
}