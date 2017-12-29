using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcfSudoku.Bcf;
using CalculationWorks.BusinessModel;

namespace BcfSudoku
{
    public class Sudoku
    {
        private SudokuDataSet _model;

        public Sudoku()
        {
            _model = new SudokuDataSet();

            // We do not expect exceptions and we don't use triggers - compensation of failed subtransactions is not required.
            _model.TransactionFactory.MicroTransactionMode = BcfMicroTransactionMode.Disabled;

            // Transaction is explicit used to suppress calculation each time model is updated (add rows & set values).
            // Without transaction the model will be computed 27 times when adding clusters, 81 times when adding cells and 243 (81x3) when adding link rows.
            // We will add rows so constraint-checks should be deferred until commit.
            var transaction = _model.BeginTransaction(enforceConstraints: false);

            // BcfDataSet manages model changes. When a value is updated or a row is added or removed it adds depending computed items (computed cells and rules) to a list.
            // When calling Transaction.Compute(true) - implicit called when transaction commits or a subtransaction is created - the items will be recomputed.
            // 
            // This method is called on an empty model - so the list of computed items is empty when transaction begins and contains all items when transaction is finished.
            // When entering dumb mode all computable items will be added to the list and the change management is modified to collect added items only.
            // On a transaction in dumb mode updates will be processed minimal faster and all items will be recomputed when calling Transaction.Compute(true).
            // In short: stop change tracking - recompute all on commit
            transaction.EnterDumbMode();

            var horizontalClusters = new List<ClusterRow>();
            var verticalClusters = new List<ClusterRow>();
            var squareClusters = new List<ClusterRow>();

            // create clusters
            for (var i = 0; i < 9; i++)
            {
                horizontalClusters.Add(_model.ClusterTable.AddNewRow());
                verticalClusters.Add(_model.ClusterTable.AddNewRow());
                squareClusters.Add(_model.ClusterTable.AddNewRow());
            }

            // create cells
            for (var y = 0; y < 9; y++)
            {
                for (var x = 0; x < 9; x++)
                {

                    // create cell
                    var newCell = _model.CellTable.AddNewRow();

                    // link cell with clusters
                    var linkToXCluster = _model.LinkTable.AddNewRow();
                    linkToXCluster.ClusterRow = horizontalClusters[y];
                    linkToXCluster.CellRow = newCell;

                    var linkToYCluster = _model.LinkTable.AddNewRow();
                    linkToYCluster.ClusterRow = verticalClusters[x];
                    linkToYCluster.CellRow = newCell;

                    var linkToQCluster = _model.LinkTable.AddNewRow();
                    var q = (y / 3) * 3 + x / 3;
                    linkToQCluster.ClusterRow = squareClusters[q];
                    linkToQCluster.CellRow = newCell;
                }
            }

            transaction.Commit();

            // HelperClusters are used to detect whether model is completely filled or not.
            HelperClusters = horizontalClusters;
        }

        /// <summary>
        /// Used to detect whether model is completely filled or not.
        /// </summary>
        private List<ClusterRow> HelperClusters { get; set; }

        /// <summary>
        /// Returns the state.
        /// </summary>
        /// <returns>The state</returns>
        public SudokuState GetState()
        {
            // Return value depends on ValidationResults - the list of computed error-messages.
            // So if there are outstanding calculation tasks - process them now to make sure the list is up to date.
            if (!ReferenceEquals(null, _model.CurrentTransaction) && _model.CurrentTransaction.HasDirtyItems) _model.CurrentTransaction.Compute(true);

            // Any rule returned not null - sudoku is wrong.
            if (_model.Faults.Count != 0) return SudokuState.Error;

            // It is not necessary to iterate through all 27 clusters.
            // All cells are covered by the 9 horizontal clusters.
            // No errors & no missing values - sudoku is solved
            if (HelperClusters.All(c => c.MissingValues == 0)) return SudokuState.Solved;

            // No errors but missing values - sudoku is not solved (yet)
            return SudokuState.Valid;
        }


        private void AutoComplete()
        {
            bool updated;
            do
            {
                // update computed values
                // omit compute error-messages - not relevant here
                _model.CurrentTransaction.Compute(false);
                updated = false;

                // solve cells with only one possible value
                foreach (var cell in _model.CellTable)
                {
                    if (cell.Value == 0 && Util.BitCountRegister[cell.AllowedValues] == 1)
                    {
                        cell.Value = cell.AllowedValues;
                        updated = true;
                    }
                }

                if (!updated)
                {
                    // solve cells which are the only possible target for a value in a cluster
                    foreach (var cluster in _model.ClusterTable)
                    {
                        var index = 0;
                        foreach (var targetCount in cluster.MissingValuesTargetCount)
                        {
                            if (targetCount == 1)
                            {
                                var value = cluster.MissingValuesArray[index];
                                var cell = cluster.LinkRows.Select(link => link.CellRow).First(c => (c.AllowedValues & value) != 0);
                                cell.Value = value;
                                updated = true;
                            }
                            index++;
                        }
                    }
                }
            } while (updated);
        }

        /// <summary>
        /// Modifies the <see cref="Sudoku"/> and iterates over all valid solutions.
        /// </summary>
        /// <typeparam name="T">Return type of resultSelector function</typeparam>
        /// <param name="resultSelector">Function called when a valid solution is found.</param>
        /// <param name="rnd">Optional. Used to create solutions in a random order.</param>
        /// <returns>All valid solutions</returns>
        /// <remark>
        /// Original state will be restored when enumeration completed.
        /// </remark>
        public IEnumerable<T> Solve<T>(Func<Sudoku, T> resultSelector, Random rnd = null)
        {
            return new Solver<T>(this, resultSelector, rnd);
        }

        public override string ToString()
        {
            return new string(_model.CellTable.Select(cell => Util.CellValueToChar(cell.Value)).ToArray());
        }

        public void Load(string charValues)
        {
            if (charValues == null) throw new ArgumentNullException(nameof(charValues));
            if (charValues.Length != 81) throw new ArgumentException("expected string length: 81", nameof(charValues));

            var transaction = _model.BeginTransaction();
            for (var i = 0; i < 81; i++) _model.CellTable[i].Value = Util.CharToCellValue(charValues[i]);
            transaction.Commit();
        }

        #region helper classes

        /// <summary>
        /// IEnumerable type for SolutionEnumerator
        /// </summary>
        /// <typeparam name="T">return type of resultSelector function</typeparam>
        private class Solver<T> : IEnumerable<T>
        {
            private readonly Sudoku _sudoku;
            private readonly Func<Sudoku, T> _resultSelector;
            private readonly Random _rnd;

            public Solver(Sudoku sudoku, Func<Sudoku, T> resultSelector, Random rnd = null)
            {
                _sudoku = sudoku;
                _resultSelector = resultSelector;
                _rnd = rnd;
            }

            public IEnumerator<T> GetEnumerator()
            {
                var state = _sudoku.GetState();
                if (state == SudokuState.Error) return Enumerable.Empty<T>().GetEnumerator();
                if (state == SudokuState.Solved) return Enumerable.Repeat(_resultSelector(_sudoku), 1).GetEnumerator();
                return new SolutionEnumerator<T>(_sudoku, _resultSelector, _rnd);
            }
            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        }

        /// <summary>
        /// IEnumerator to iterate through all valid solutions
        /// </summary>
        /// <typeparam name="T">return type of resultSelector function</typeparam>
        private class SolutionEnumerator<T> : IEnumerator<T>
        {
            private readonly Sudoku _sudoku;
            private readonly Func<Sudoku, T> _resultSelector;
            private readonly Random _rnd;
            private readonly BcfTransaction _transaction;
            private T _current; // value for IEnumerator.Current
            private readonly Stack<AllowedValueEnumerator> _stack; // store for regression

            public SolutionEnumerator(Sudoku sudoku, Func<Sudoku, T> resultSelector, Random rnd)
            {
                _sudoku = sudoku;
                _resultSelector = resultSelector;
                _rnd = rnd;
                _stack = new Stack<AllowedValueEnumerator>();
                _transaction = sudoku._model.BeginTransaction();
                _stack.Push(new AllowedValueEnumerator(sudoku, rnd));
            }

            public void Dispose()
            {
                while (_stack.Count != 0)
                {
                    var valueEnumerator = _stack.Pop();
                    valueEnumerator.Dispose();
                }
                _transaction.Rollback();
            }

            public void Reset() { throw new NotSupportedException(); }

            public T Current
            {
                get { return _current; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                while (_stack.Count != 0)
                {
                    var valueEnumerator = _stack.Peek();
                    // remark: valueEnumerator.MoveNext() returns only true if there are no error-messages
                    while (valueEnumerator.MoveNext())
                    {
                        if (_sudoku.GetState() == SudokuState.Solved)
                        {
                            // new a valid solution; set IEnumerator.Current and return true
                            _current = _resultSelector(_sudoku);
                            return true;
                        }
                        // not (yet) solved; fill next cell
                        valueEnumerator = new AllowedValueEnumerator(_sudoku, _rnd);
                        _stack.Push(valueEnumerator);
                    }
                    // valueEnumerator processed - dispose it (rollback its subtransaction) remove from it from stack
                    valueEnumerator.Dispose();
                    _stack.Pop();
                }

                // stackField.Count == 0; solution enumeration complete
                return false;
            }

            /// <summary>
            /// IEnumerator to iterate through a cells possible values
            /// </summary>
            private class AllowedValueEnumerator : IEnumerator<int>
            {
                private readonly Sudoku _sudoku;
                private readonly CellRow _cell;
                private readonly IEnumerator<int> _valueEnumerator;
                private BcfTransaction _transaction;

                public AllowedValueEnumerator(Sudoku sudoku, Random rnd)
                {
                    _sudoku = sudoku;
                    // determine best cell to process
                    _cell = GetNextCell(_sudoku);
                    // create enumerator for possible value
                    var values = Util.BitRegister[_cell.AllowedValues];
                    _valueEnumerator = rnd == null ? values.Cast<int>().GetEnumerator() : values.Randomize(rnd).GetEnumerator();
                }

                private static CellRow GetNextCell(Sudoku sudoku)
                {
                    CellRow best = null;
                    var minCount = 10;
                    foreach (var cell in sudoku._model.CellTable)
                    {
                        if (cell.Value == 0)
                        {
                            var bitCount = Util.BitCountRegister[cell.AllowedValues];
                            // 2 is the best possible result - 0 is prevented by CellValidator and 1 by AutoComplete()
                            if (bitCount == 2) return cell;
                            if (bitCount < minCount)
                            {
                                best = cell;
                                minCount = bitCount;
                            }
                        }
                    }
                    return best;
                }

                public void Dispose()
                {
                    if (!ReferenceEquals(null, _transaction)) { _transaction.Rollback(); }
                    _valueEnumerator.Dispose();
                }

                public bool MoveNext()
                {
                    while (_valueEnumerator.MoveNext())
                    {
                        // rollback previous enumeration step result
                        if (!ReferenceEquals(null, _transaction)) _transaction.Rollback();
                        // create a subtransaction
                        _transaction = _sudoku._model.BeginTransaction();
                        // assign value
                        _cell.Value = _valueEnumerator.Current;
                        // autocomplete other cells
                        _sudoku.AutoComplete();
                        // in case of error try next value (if there is one)
                        if (_sudoku.GetState() != SudokuState.Error) return true;
                    }
                    return false;
                }

                public void Reset() { throw new NotSupportedException(); }

                public int Current
                {
                    get { return _valueEnumerator.Current; }
                }

                object IEnumerator.Current
                {
                    get { return Current; }
                }
            }


        }
        #endregion
    }
}
