
//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,ÕÔ×Óìõ
//
//    This file is part of OpenWECD.VTKL.Core
//
// Licensed under the Boost Software License - Version 1.0 - August 17th, 2003
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.HawtC.cn/licenses.txt
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
// SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
// FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
//**********************************************************************************************************************************

#pragma warning disable SA1412 // Store files as UTF-8 with byte order mark

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// The type-specific definitions of method "FillData".
    /// </summary>
    public partial class DataArray
    {
        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<byte> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.UInt8;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<sbyte> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Int8;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<short> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Int16;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<ushort> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.UInt16;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<int> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Int32;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<uint> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.UInt32;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<long> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Int64;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<ulong> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.UInt64;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<float> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Float32;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Clears and fills the data array with given data.
        /// </summary>
        /// <param name="data">The collection of all components to be filled with.</param>
        public void FillData(IEnumerable<double> data)
        {
            if (Format == DataArrayFormat.Ascii)
            {
                StringBuilder contentString = new StringBuilder();
                foreach (var datum in data)
                {
                    contentString.Append(datum.ToString(CultureInfo.InvariantCulture));
                    contentString.Append(' ');
                }

                if (contentString.Length > 0)
                {
                    contentString.Remove(contentString.Length - 1, 1);
                }

                Content = contentString.ToString();
                Type = DataArrayType.Float64;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}