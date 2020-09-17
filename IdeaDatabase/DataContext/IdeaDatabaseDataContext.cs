using IdeaDatabase.Enums;
using IdeaDatabase.Interchange;
using MySql.Data.MySqlClient;
using NLog;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using IdeaDatabase.Utils;
using System.Linq;
using System.Data.Entity.Validation;
using System.Text;
using System.Data.Entity;

namespace IdeaDatabase.DataContext
{

    public partial class IdeaDatabaseDataContext : IIdeaDatabaseDataContext
    {
        private static Logger logger = LogManager.GetLogger("DatabaseSubmitChanges");

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void SubmitChanges()
        {
            int retry = 5;
            while (retry > 0)
            {
                try
                {
                    base.SaveChanges();
                    retry = 0;
                }

                // DbEntityValidationException - Exception thrown from SaveChanges 
                // when the validation of entities fails.
                // Exception class hierarchy
                // System.Exception
                //   ->System.SystemException
                //     ->System.Data.DataException
                //       ->System.Data.Entity.Validation.DbEntityValidationException
                catch (DbEntityValidationException e)
                {
                    string method = new StackTrace().GetFrame(1).GetMethod().Name;
                    StringBuilder errorList = new StringBuilder();
                    foreach (DbEntityValidationResult validationResult in e.EntityValidationErrors)
                    {
                        string entityName = validationResult.Entry.Entity.GetType().Name;
                        foreach (DbValidationError error in validationResult.ValidationErrors)
                        {
                            errorList.Append($"{entityName}.{error.PropertyName}=> {error.ErrorMessage}{Environment.NewLine}");
                        }
                    }
                    // No need to retry anymore as the data to be saved is not valid for saving in database
                    retry = 0;
                    logger.Info($"SubmitChanges        | DbEntityValidationException: method='{method}' errorList='{errorList.ToString()}'");
                    throw;
                }

                // DbUpdateException - Exception thrown by DbContext when the 
                // saving of changes to the database fails.
                // Exception class hierarchy
                // System.Exception
                //   ->System.SystemException
                //     ->System.Data.DataException
                //       ->System.Data.Entity.Infrastructure.DbUpdateException
                catch (DbUpdateException e)
                {
                    string entityAcction = string.Empty;
                    string method = string.Empty;
                    try
                    {
                        foreach (DbEntityEntry entry in e.Entries)
                        {
                            if (entry.Entity != null)
                            {
                                entityAcction += $"{entry.Entity.GetType().BaseType.Name} {entry.State}, ";
                            }
                        }
                        method = new StackTrace().GetFrame(1).GetMethod().Name;
                    }
                    catch (Exception)
                    {
                    }

                    logger.Info($"SubmitChanges        | DbUpdateConcurrencyException: method='{method}' entities='{entityAcction}'");
                    throw;
                }

                // SqlException - Exception thrown when SQL Server returns 
                // a warning or error.
                // Exception class hierarchy
                // System.Exception
                //   ->System.SystemException
                //     ->System.Runtime.InteropServices.ExternalException
                //       ->System.Data.Common.DbException
                //         ->System.Data.SqlClient.SqlException
                catch (SqlException e)
                {
                    logger.Info($"SubmitChanges        | SqlException: retry='{retry}' exNumber='{e.Number}'");
                    if (e.Number != 1205 || retry == 0)
                    {
                        throw;
                    }
                    else
                    {
                        retry--;
                    }
                }

                // Generic exception handler to handle all other exceptions
                catch (Exception e)
                {
                    string method = new StackTrace().GetFrame(1).GetMethod().Name;
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.InnerException != null)
                        {
                            logger.Info($"SubmitChanges        | Exception: method='{method}' retry='{retry}' exIInner='{e.InnerException.InnerException.Message}'");
                        }
                        else
                        {
                            logger.Info($"SubmitChanges        | Exception: method='{method}' retry='{retry}' exInner='{e.InnerException.Message}'");
                        }
                    }
                    else
                    {
                        logger.Info($"SubmitChanges        | Exception: method='{method}' retry='{retry}' exMsg='{e.Message}'");
                    }
                    throw;
                }
            }
        }

        public virtual int ValidateHPPToken(Nullable<int> UserID, string token, string callerId)
        {
            MySqlParameter customerIdParam = new MySqlParameter();
            customerIdParam.ParameterName = "@customerId";
            customerIdParam.DbType = DbType.Int32;
            customerIdParam.Value = UserID;
            customerIdParam.Direction = ParameterDirection.Input;

            MySqlParameter tokenParam = new MySqlParameter();
            tokenParam.ParameterName = "@token";
            tokenParam.DbType = DbType.String;
            tokenParam.Size = 8000;
            tokenParam.Value = token;
            tokenParam.Direction = ParameterDirection.Input;

            MySqlParameter callerIdParam = new MySqlParameter();
            callerIdParam.ParameterName = "@callerId";
            callerIdParam.DbType = DbType.String;
            callerIdParam.Size = 15;
            callerIdParam.Value = callerId;
            callerIdParam.Direction = ParameterDirection.Input;

            var result = this.Database.SqlQuery<int>("call ValidateHPPToken (@customerId, @token, @callerId) ", customerIdParam, tokenParam, callerIdParam).FirstOrDefaultAsync();
            return result.Result;
        }

        public virtual int SetHPPToken(Nullable<int> customerId, string token, string callerId)
        {
            MySqlParameter customerIdParam = new MySqlParameter();
            customerIdParam.ParameterName = "@customerId";
            customerIdParam.DbType = DbType.Int32;
            customerIdParam.Value = customerId;
            customerIdParam.Direction = ParameterDirection.Input;

            MySqlParameter tokenParam = new MySqlParameter();
            tokenParam.ParameterName = "@token";
            tokenParam.DbType = DbType.String;
            tokenParam.Size = 8000;
            tokenParam.Value = token;
            tokenParam.Direction = ParameterDirection.Input;

            MySqlParameter callerIdParam = new MySqlParameter();
            callerIdParam.ParameterName = "@callerId";
            callerIdParam.DbType = DbType.String;
            callerIdParam.Size = 15;
            callerIdParam.Value = callerId;
            callerIdParam.Direction = ParameterDirection.Input;

            this.Database.ExecuteSqlCommand("call SetHPPToken (@customerId, @token, @callerId)", customerIdParam, tokenParam, callerIdParam);

            return 0;
        }
 
    }
}