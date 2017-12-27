using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BcfTreeView;

namespace Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var a1 = new Author() { Name = "Author 1" };
            var a2 = new Author() { Name = "Author 2" };
            var a3 = new Author() { Name = "Author 3" };

            var b1 = new Book() { Name = "Book 1" };
            var b2 = new Book() { Name = "Book 2" };
            var b3 = new Book() { Name = "Book 3" };
            var b4 = new Book() { Name = "Book 4\nwith linebreak in title" };
            var b5 = new Book() { Name = "Book 5 with incredible long title - some people say the title has more characters than the rest of the book - it should illustrate the node autosize feature. " + string.Join(" ", Enumerable.Repeat("More Text.", 100)) };

            void Link(Author author, params Book[] books)
            {
                author.Books = author.Books.Concat(books).Distinct().ToArray();
                foreach (var book in books)
                {
                    book.Authors = book.Authors.Concat(Enumerable.Repeat(author, 1)).Distinct().ToArray();
                }
            }

            Link(a1, b1, b2, b3);
            Link(a2, b1, b2);
            Link(a3, b4, b5, b2);
            _initialNodes = new[] { new AuthorNodeObject(a1), new AuthorNodeObject(a2), new AuthorNodeObject(a3) };

            treeView1.WithTransactionScope(tv =>
            {
                treeView1.NodeObjectToString = UnchangedCase;
                tv.AddRootNodes(_initialNodes);
            });

            pgControl.SelectedObject = treeView1;
        }

        private readonly AuthorNodeObject[] _initialNodes;

        #region data classes
        class Author
        {
            public string Name { get; set; }
            public Book[] Books { get; set; } = new Book[] { };
        }

        class Book
        {
            public string Name { get; set; }
            public Author[] Authors { get; set; } = new Author[] { };
        }
        #endregion

        #region data node adapters
        private interface IMyNodeObject
        {
            IEnumerable<IMyNodeObject> GetChildNodeObjects();
            string GetText();
        }

        class AuthorNodeObject : IMyNodeObject
        {
            private readonly Author _author;

            public AuthorNodeObject(Author author)
            {
                _author = author;
            }

            public IEnumerable<IMyNodeObject> GetChildNodeObjects()
            {
                return _author.Books.Select(b => new BookNodeObject(b));
            }

            public string GetText() => _author.Name;
        }

        class BookNodeObject : IMyNodeObject
        {
            private readonly Book _book;

            public BookNodeObject(Book book)
            {
                _book = book;
            }

            public IEnumerable<IMyNodeObject> GetChildNodeObjects()
            {
                return _book.Authors.Select(a => new AuthorNodeObject(a));
            }

            public string GetText()
            {
                return _book.Name;
            }
        }
        #endregion

        private void treeView1_GetChildNodeObjects(object sender, GetChildNodeObjectsEventArgs e)
        {
            e.ChildObjects = ((IMyNodeObject)e.Node.NodeObject).GetChildNodeObjects();
        }

        private void chbCase_CheckStateChanged(object sender, EventArgs e)
        {
            switch (chbCase.CheckState)
            {
                case CheckState.Unchecked:
                    treeView1.NodeObjectToString = LowerCase;
                    break;
                case CheckState.Checked:
                    treeView1.NodeObjectToString = UpperCase;
                    break;
                case CheckState.Indeterminate:
                    treeView1.NodeObjectToString = UnchangedCase;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static readonly Func<object, string> UnchangedCase = obj => ((IMyNodeObject)obj).GetText();
        private static readonly Func<object, string> LowerCase = obj => ((IMyNodeObject)obj).GetText().ToLower();
        private static readonly Func<object, string> UpperCase = obj => ((IMyNodeObject)obj).GetText().ToUpper();


        private void treeView1_SelectedNodesChanged(object sender, EventArgs e)
        {
            pgSelectedNodes.SelectedObjects = treeView1.SelectedNodes.ToArray();
            pgSelectedNodes.Refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            treeView1.WithTransactionScope(tv =>
            {
                var previousNodes = tv.SelectedNodes.Select(n => n.PreviousSibling ?? n.ParentNode).ToArray();
                tv.SelectedNodes.ToList().ForEach(n => n.Delete());
                tv.ActiveNode = previousNodes.FirstOrDefault(n => n?.IsValid == true);
            });
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pgControl.Refresh();
            pgSelectedNodes.Refresh();
        }

        private void btnExpandLevels_Click(object sender, EventArgs e)
        {
            var levels = (int)numLevels.Value;
            treeView1.WithTransactionScope(tv =>
            {
                foreach (var node in tv.SelectedNodes) node.ExpandNode(levels);
            });
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            treeView1.WithTransactionScope(tv =>
            {
                foreach (var node in tv.RootNodes.ToArray()) node.Delete();
                tv.AddRootNodes(_initialNodes);
            });
        }
    }
}
