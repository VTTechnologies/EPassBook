using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace EPassBook.Helper
{
    public class ElmahErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {          
            if (actionExecutedContext.Exception != null)
            {
                var exception = GetExtendedException(actionExecutedContext.Exception);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
            }

            base.OnException(actionExecutedContext);
        }
        private Exception GetExtendedException(Exception ex)
        {
            var baseException = ex.GetBaseException();

            switch (baseException.GetType().ToString())
            {
                case "System.Data.Entity.Validation.DbEntityValidationException":
                    var dbException = baseException as
                           System.Data.Entity.Validation.DbEntityValidationException;
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine(baseException.Message);

                    foreach (var entity in dbException.EntityValidationErrors)
                    {
                        foreach (var error in entity.ValidationErrors)
                        {
                            sb.AppendLine(string.Format(" [{0}: {1}]",
                                      error.PropertyName, error.ErrorMessage));
                        }
                    }

                    return new ElmahExtendedException(sb.ToString(), ex);

                default:
                    return ex;
            }
        }
    }

    public class ElmahExtendedException    : Exception
    {
        public ElmahExtendedException(string message, Exception e) : base(message, e) { }

        
    }
}