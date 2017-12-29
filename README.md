# BCF Examples
## Intro
My intension creating BCF was to have a reusable infrastructure for calculation optimized objectmodels supporting
transaction processing, undo & redo, validation and databinding.
## Treeview
A lazy loading treeview with individual node font, node autosize feature, horizontal + vertical scrolling and in tree drag & drop.
This example illustrates caching of computed values and recalculating only when neccessary. Expanding a node - or changing 
font size - will cause recalculation of below (only) nodes location.

![Treeview Snapshot](TreeView/doc/TreeviewSnapshot.png)

<a href="TreeView/doc/README.md">Read mode about Treeview Example</a>

## Sudoku
The example is a fast SUDOKU solution enumerator. Its a "smart"-force approach using subtransactions for backtracking and a
very simple BCF-model (5 columns and 2 rules over 3 tables with 2 relations) to compute cell value candidates.

This test counts all possible solutions of a given puzzle and runs in less than 450 ms on my pc (4 years old dev machine):
```c#
        [TestMethod]
        public void Solve3000Solutions()
        {
            var sudoku = new Sudoku();

            sudoku.Load(
                "123456789" +
                "456789123" +
                "789123456" +
                "912345678" +
                "345678912" +
                "000000000" +
                "000000000" +
                "000000000" +
                "000000000");

            var resultCount = sudoku.Solve(m => 0).Count();
            Assert.AreEqual(3000, resultCount);
        }
```
