using System;
using System.Linq;
using JetBrains.Annotations;

namespace BcfTreeView.Bcf
{

    partial class TreeModelDataSet
    {

        partial void Initialized()
        {
            ControlRow = ControlsTable.AddNewRow();
        }

        [NotNull]
        public ControlRow ControlRow { get; private set; }

    }
}