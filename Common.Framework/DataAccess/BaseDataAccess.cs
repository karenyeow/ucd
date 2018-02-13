
using Comlib.Common.Framework.DataAccess.Query;
using Comlib.Common.Framework.DataAccess.QueryHandler;
using Comlib.Common.Helpers.Error;
using Comlib.Common.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Comlib.Common.Framework.DataAccess
{
    abstract public class BaseDataAccessService
    {
        public BaseDataAccessService(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public BaseDataAccessService()
        {
        }

        protected string ConnectionString { get; set; }

        protected T RunQueryScalarResult<T>(IStoredProcQuery query)
        {
            return this.RunQueryScalarResult<T>(query, this.ConnectionString);
        }
        protected void RunQueryNonResult(IStoredProcQuery query)
        {
            this.RunQueryNonResult(query, this.ConnectionString);
        }
        protected void RunQueryNonResult(IStoredProcQuery query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                   
                
                        QueryHandlerFactory.Create(query).Assign(command, query);

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.PopulateExceptionWithCommandParameterData(ex, command);
                        throw;
                    }
                }
            }
        }

        protected T RunQueryScalarResult<T>(IStoredProcQuery query, string connectionString)
        {
            object result = null;
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        QueryHandlerFactory.Create(query).Assign(command, query);

                        connection.Open();

                        result = command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.PopulateExceptionWithCommandParameterData(ex, command);
                        throw;
                    }
                }
            }
            return (T)result;
        }

        protected List<T> RunQuerySingleResult<T>(IStoredProcQuery query)
        {
            return this.RunQuerySingleResult<T>(query, this.ConnectionString);
        }


        protected List<T> RunQuerySingleResult<T>(IStoredProcQuery query, string connectionString)
        {
            var results = new List<T>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        QueryHandlerFactory.Create(query).Assign(command, query);

                        connection.Open();

                        using (var dataReader = command.ExecuteReader())
                        {
                            results = dataReader.FillCollection<T>();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.PopulateExceptionWithCommandParameterData(ex, command);
                        throw;
                    }
                }
            }
            return results;
        }

        protected Tuple<List<T1>, List<T2>> RunQueryDoubleResult<T1, T2>(IStoredProcQuery query)
        {
            return this.RunQueryDoubleResult<T1, T2>(query, this.ConnectionString);
        }

        protected Tuple<List<T1>, List<T2>> RunQueryDoubleResult<T1, T2>(IStoredProcQuery query, string connectionString)
        {
            var result1 = new List<T1>();
            var result2 = new List<T2>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        QueryHandlerFactory.Create(query).Assign(command, query);

                        connection.Open();

                        using (var dataReader = command.ExecuteReader())
                        {
                            result1 = dataReader.FillCollection<T1>(false);

                            dataReader.NextResult();

                            result2 = dataReader.FillCollection<T2>();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.PopulateExceptionWithCommandParameterData(ex, command);
                        throw;
                    }
                }
            }

            return new Tuple<List<T1>, List<T2>>(result1, result2);
        }
        protected Tuple<List<T1>, List<T2>, List<T3>> RunQueryTripleResult<T1, T2, T3>(IStoredProcQuery query)
        {
            return this.RunQueryTripleResult<T1, T2, T3>(query, this.ConnectionString);
        }

        protected Tuple<List<T1>, List<T2>, List<T3>> RunQueryTripleResult<T1, T2, T3>(IStoredProcQuery query, string connectionString)
        {
            var result1 = new List<T1>();
            var result2 = new List<T2>();
            var result3 = new List<T3>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        QueryHandlerFactory.Create(query).Assign(command, query);

                        connection.Open();

                        using (var dataReader = command.ExecuteReader())
                        {
                            result1 = dataReader.FillCollection<T1>(false);

                            dataReader.NextResult();

                            result2 = dataReader.FillCollection<T2>(false);

                            dataReader.NextResult();

                            result3 = dataReader.FillCollection<T3>();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.PopulateExceptionWithCommandParameterData(ex, command);
                        throw;
                    }
                }
            }

            return new Tuple<List<T1>, List<T2>, List<T3>>(result1, result2, result3);
        }
    }
}
