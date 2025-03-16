// <copyright file="DataArrayBuilder.cs" company="Huang, Zhaoquan">
// Copyright (c) Huang, Zhaoquan. All rights reserved.
// This file may be licensed to you as part of the project (see license file if exists),
// but the copyright info in this file should not be removed.
// </copyright>

// The following code is generated from template DataArray.FillData.tt
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using OpenWECD.VTKL.Core.Xml;

namespace OpenWECD.VTKL.Core
{
    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is byte.
    /// </summary>
    public class DataArrayBuilderUInt8 : IDataArrayBuilder
    {
        private readonly List<byte> data = new List<byte>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderUInt8"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderUInt8(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(byte value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<byte> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is sbyte.
    /// </summary>
    public class DataArrayBuilderInt8 : IDataArrayBuilder
    {
        private readonly List<sbyte> data = new List<sbyte>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderInt8"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderInt8(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(sbyte value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<sbyte> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is short.
    /// </summary>
    public class DataArrayBuilderInt16 : IDataArrayBuilder
    {
        private readonly List<short> data = new List<short>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderInt16"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderInt16(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(short value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<short> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is ushort.
    /// </summary>
    public class DataArrayBuilderUInt16 : IDataArrayBuilder
    {
        private readonly List<ushort> data = new List<ushort>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderUInt16"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderUInt16(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(ushort value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<ushort> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is int.
    /// </summary>
    public class DataArrayBuilderInt32 : IDataArrayBuilder
    {
        private readonly List<int> data = new List<int>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderInt32"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderInt32(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(int value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<int> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is uint.
    /// </summary>
    public class DataArrayBuilderUInt32 : IDataArrayBuilder
    {
        private readonly List<uint> data = new List<uint>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderUInt32"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderUInt32(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(uint value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<uint> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is long.
    /// </summary>
    public class DataArrayBuilderInt64 : IDataArrayBuilder
    {
        private readonly List<long> data = new List<long>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderInt64"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderInt64(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(long value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<long> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is ulong.
    /// </summary>
    public class DataArrayBuilderUInt64 : IDataArrayBuilder
    {
        private readonly List<ulong> data = new List<ulong>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderUInt64"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderUInt64(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(ulong value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<ulong> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is float.
    /// </summary>
    public class DataArrayBuilderFloat32 : IDataArrayBuilder
    {
        private readonly List<float> data = new List<float>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderFloat32"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderFloat32(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(float value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<float> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }

    /// <summary>
    /// The builder to store data for building a <see cref="DataArray"/> XML element.
    /// The element type is double.
    /// </summary>
    public class DataArrayBuilderFloat64 : IDataArrayBuilder
    {
        private readonly List<double> data = new List<double>();

        /// <summary>
        /// Creates an instance of <see cref="DataArrayBuilderFloat64"/>.
        /// </summary>
        /// <param name="name">
        /// The name of the data array.
        /// </param>
        /// <param name="numberOfComponents">
        /// The number of components per value (e.g., per point / per cell).
        /// </param>
        public DataArrayBuilderFloat64(string name, uint numberOfComponents = 1)
        {
            this.Name = name;
            this.NumberOfComponents = numberOfComponents;
        }

        /// <summary>
        /// Gets the name of the data array.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the number of components per data point.
        /// </summary>
        public uint NumberOfComponents { get; }

        /// <summary>
        /// Appends a scalar value to the data array.
        /// </summary>
        /// <param name="value">The value to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// <see cref="NumberOfComponents"/> was set to not 1. A vector is expected but a scalar is appended.
        /// </exception>
        public void AddScalarDatum(double value)
        {
            if (NumberOfComponents != 1)
            {
                throw new InvalidOperationException("To add scalar datum, numberOfComponents must be 1.");
            }

            data.Add(value);
        }

        /// <summary>
        /// Appends a vector value to the data array.
        /// </summary>
        /// <param name="value">The vector components to be appended.</param>
        /// <exception cref="InvalidOperationException">
        /// Length of <paramref name="value"/> does not equal to <see cref="NumberOfComponents"/>.
        /// </exception>
        public void AddVectorDatum(IEnumerable<double> value)
        {
            if (value.Count() != NumberOfComponents)
            {
                throw new InvalidOperationException("The number of components in each vector datum must match numberOfComponents.");
            }

            data.AddRange(value);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="DataArray"/> instance that represents the XML element.</returns>
        public DataArray ToXml()
        {
            DataArray array = new DataArray();
            array.NumberOfComponents = NumberOfComponents;
            array.Name = Name;
            array.FillData(data);
            return array;
        }
    }
}
