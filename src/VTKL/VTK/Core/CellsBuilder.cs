// <copyright file="CellsBuilder.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// This file may be licensed to you as part of the project (see license file if exists),
// but the copyright info in this file should not be removed.
// </copyright>

using OpenWECD.VTKL.Core.Xml;
using System.Collections.Generic;

namespace OpenWECD.VTKL.Core
{
    /// <summary>
    /// The builder to store data for building a <see cref="Cells"/> XML element.
    /// </summary>
    public class CellsBuilder
    {
        // For definitions of each arrays see VTKUsersGuide.pdf p.487
        private readonly List<int> connectivity = new List<int>();

        private readonly List<int> offsets = new List<int>();

        private readonly List<byte> types = new List<byte>();

        /// <summary>
        /// The count of already-added cells.
        /// </summary>
        public int Count => types.Count;

        /// <summary>
        /// Adds a cell into the internal list.
        /// </summary>
        /// <param name="points">The 0-based numbers of points in a cell.</param>
        /// <param name="type">The type of the cell.</param>
        public void AddCell(IEnumerable<int> points, CellType type)
        {
            connectivity.AddRange(points);
            offsets.Add(connectivity.Count);
            types.Add((byte)type);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="Cells"/> instance that represents the XML element.</returns>
        public Cells ToXml()
        {
            var cells = new Cells();
            cells.FillCells(connectivity, offsets, types);
            return cells;
        }
    }
}
