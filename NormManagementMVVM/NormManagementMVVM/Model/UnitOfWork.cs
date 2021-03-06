﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;

namespace NormManagementMVVM.Model
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ObjectContext _objectContext;
        private DbTransaction _transaction;

        public UnitOfWork(ObjectContext context)
        {
            _objectContext = context;
        }

        #region IUnitOfWork Members

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null)
            {
                throw new ApplicationException(
                    "Cannot begin a new transaction while an existing transaction is still running. " +
                    "Please commit or rollback the existing transaction before starting a new one.");
            }
            OpenConnection();
            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public void RollBackTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _transaction.Rollback();
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _objectContext.SaveChanges();
                if (_objectContext.GetType() == typeof (NormEntities))
                {
                    var entities = _objectContext as NormEntities;
                    if (entities.LastChangedNormative != 0 || entities.LastChangedRows != "")
                        entities.UpdateRowItemLoc();
                    if (entities.IsChangeEquipStore)
                    {
                        entities.UpdateEquipStoreDependencies();
                    }
                }
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                ReleaseCurrentTransaction();
            }
        }

        public void SaveChanges()
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call BeginTransaction instead.");
            }
            _objectContext.SaveChanges();
        }

        public void SaveChanges(SaveOptions saveOptions)
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call BeginTransaction instead.");
            }
            _objectContext.SaveChanges(saveOptions);
        }

        #endregion

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private void OpenConnection()
        {
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }
        }

        #region Implementation of IDisposable

        private bool _disposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            ReleaseCurrentTransaction();

            _disposed = true;
        }

        #endregion
    }
}