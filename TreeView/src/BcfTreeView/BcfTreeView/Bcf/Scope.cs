using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculationWorks.BusinessModel;
using JetBrains.Annotations;

namespace BcfTreeView.Bcf
{
    /// <summary>
    /// A disposable wrapper for very simple transaction management.
    /// </summary>
    internal sealed class Scope : IDisposable
    {
        [NotNull]
        private readonly BcfTransaction _transaction;

        [CanBeNull]
        private readonly Action<Exception> _onError;

        internal Scope([NotNull]BcfTransaction transaction, Action<Exception> onError = null)
        {
            _transaction = transaction;
            _onError = onError;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleNullReferenceException
                _transaction.Rollback();
                _onError?.Invoke(ex);
            }
        }
    }
}
