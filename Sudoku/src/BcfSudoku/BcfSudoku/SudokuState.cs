using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcfSudoku
{
    public enum SudokuState
    {
        /// <summary>
        /// Indicates the sudoku is not solved and has no errors (yet)
        /// </summary>
        Valid,

        /// <summary>
        /// model has error-messages in its ValidationResults list; sudoku is not solvable
        /// </summary>
        Error,

        /// <summary>
        /// sudoku is solved without error-messages
        /// </summary>
        Solved,
    }
}
