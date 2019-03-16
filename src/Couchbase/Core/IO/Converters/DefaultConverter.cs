using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace Couchbase.Core.IO.Converters
{
    /// <summary>
    /// The default <see cref="IByteConverter" /> for for converting types and arrays before
    /// being sent or after being received across the network. Unless an overload is called
    /// with useNbo = false, Network Byte Order will be used in the conversion.
    /// </summary>
    public sealed class DefaultConverter : IByteConverter
    {
        private static T Read<T>(ReadOnlySpan<byte> src, bool useNbo)
            where T: struct
        {
            if (!useNbo)
            {
                return MemoryMarshal.Read<T>(src);
            }
            else
            {
                Span<byte> dst = stackalloc byte[Unsafe.SizeOf<T>()];

                var offset = 0;
                for (var i = dst.Length - 1; i >= 0; i--)
                {
                    dst[i] = src[offset++];
                }

                return MemoryMarshal.Read<T>(dst);
            }
        }

        private static void Write<T>(T value, Span<byte> dst, bool useNbo)
            where T: struct
        {
            if (dst.Length == 0)
            {
                dst = new Span<byte>(new byte[Unsafe.SizeOf<T>()]);
            }

            if (useNbo)
            {
                WriteAndReverse(value, dst);
            }
            else
            {
                MemoryMarshal.Write(dst, ref value);
            }
        }

        private static void WriteAndReverse<T>(T value, Span<byte> dst)
            where T: struct
        {
            Span<byte> buffer = stackalloc byte[Unsafe.SizeOf<T>()];
            MemoryMarshal.Write(dst, ref value);
            
            var offset = 0;
            for (var i = buffer.Length - 1; i >= 0; i--)
            {
                dst[offset++] = buffer[i];
            }
        }

        /// <inheritdoc />
        public bool ToBoolean(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<bool>(buffer, useNbo);
        }

        /// <inheritdoc />
        public float ToSingle(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<float>(buffer, useNbo);
        }

        /// <inheritdoc />
        public DateTime ToDateTime(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return DateTime.FromBinary(Read<long>(buffer, useNbo));
        }

        /// <inheritdoc />
        public double ToDouble(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<double>(buffer, useNbo);
        }

        /// <inheritdoc />
        public byte ToByte(ReadOnlySpan<byte> buffer)
        {
            return buffer[0];
        }

        /// <inheritdoc />
        public short ToInt16(ReadOnlySpan<byte> buffer)
        {
            return ToInt16(buffer, true);
        }

        /// <inheritdoc />
        public short ToInt16(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<short>(buffer, useNbo);
        }

        /// <inheritdoc />
        public ushort ToUInt16(ReadOnlySpan<byte> buffer)
        {
            return ToUInt16(buffer, true);
        }

        /// <inheritdoc />
        public ushort ToUInt16(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<ushort>(buffer, useNbo);
        }

        /// <inheritdoc />
        public int ToInt32(ReadOnlySpan<byte> buffer)
        {
            return ToInt32(buffer, true);
        }

        /// <inheritdoc />
        public int ToInt32(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<int>(buffer, useNbo);
        }

        /// <inheritdoc />
        public uint ToUInt32(ReadOnlySpan<byte> buffer)
        {
            return ToUInt32(buffer, true);
        }

        /// <inheritdoc />
        public uint ToUInt32(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<uint>(buffer, useNbo);
        }

        /// <inheritdoc />
        public long ToInt64(ReadOnlySpan<byte> buffer)
        {
            return ToInt64(buffer, true);
        }

        /// <inheritdoc />
        public long ToInt64(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<long>(buffer, useNbo);
        }

        /// <inheritdoc />
        public ulong ToUInt64(ReadOnlySpan<byte> buffer)
        {
            return ToUInt64(buffer, true);
        }

        /// <inheritdoc />
        public ulong ToUInt64(ReadOnlySpan<byte> buffer, bool useNbo)
        {
            return Read<ulong>(buffer, useNbo);
        }

        /// <inheritdoc />
        public unsafe string ToString(ReadOnlySpan<byte> buffer)
        {
            fixed (byte* ptr = &MemoryMarshal.GetReference(buffer))
            {
                return Encoding.UTF8.GetString(ptr, buffer.Length);
            }
        }

        /// <inheritdoc />
        public void FromInt16(short value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromInt16(short value, ref Span<byte> buffer)
        {
            FromInt16(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromInt16(short value, Span<byte> buffer)
        {
            FromInt16(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromUInt16(ushort value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromUInt16(ushort value, ref Span<byte> buffer)
        {
            FromUInt16(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromUInt16(ushort value, Span<byte> buffer)
        {
            FromUInt16(value, ref buffer);
        }

        /// <inheritdoc />
        public void FromInt32(int value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromInt32(int value, ref Span<byte> buffer)
        {
            FromInt32(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromInt32(int value, Span<byte> buffer)
        {
            FromInt32(value, ref buffer);
        }

        /// <inheritdoc />
        public void FromUInt32(uint value, Span<byte> buffer)
        {
            FromUInt32(value, ref buffer);
        }

        /// <inheritdoc />
        public void FromUInt32(uint value, ref Span<byte> buffer)
        {
            FromUInt32(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromUInt32(uint value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromInt64(long value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromInt64(long value, ref Span<byte> buffer)
        {
            FromInt64(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromInt64(long value, Span<byte> buffer)
        {
            FromInt64(value, ref buffer);
        }

        /// <inheritdoc />
        public void FromUInt64(ulong value, ref Span<byte> buffer, bool useNbo)
        {
            Write(value, buffer, useNbo);
        }

        /// <inheritdoc />
        public void FromUInt64(ulong value, ref Span<byte> buffer)
        {
            FromUInt64(value, ref buffer, true);
        }

        /// <inheritdoc />
        public void FromUInt64(ulong value, Span<byte> buffer)
        {
            FromUInt64(value, ref buffer);
        }

        /// <inheritdoc />
        public unsafe void FromString(string value, ref Span<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                buffer = new byte[Encoding.UTF8.GetByteCount(value)];
            }

            var charSpan = value.AsSpan();

            fixed (byte* bytes = &MemoryMarshal.GetReference(buffer))
            {
                fixed (char* chars = &MemoryMarshal.GetReference(charSpan))
                {
                    Encoding.UTF8.GetBytes(chars, charSpan.Length, bytes, buffer.Length);
                }
            }
        }

        /// <inheritdoc />
        public void FromString(string value, Span<byte> buffer)
        {
            FromString(value, ref buffer);
        }

        /// <inheritdoc />
        public void FromByte(byte value, ref Span<byte> buffer)
        {
            buffer[0] = value;
        }

        /// <inheritdoc />
        public void FromByte(byte value, Span<byte> buffer)
        {
            FromByte(value, ref buffer);
        }

        /// <summary>
        /// Sets the bit from a <see cref="byte" /> at a given position.
        /// </summary>
        /// <param name="theByte">The byte.</param>
        /// <param name="position">The position.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetBit(ref byte theByte, int position, bool value)
        {
            if (value)
            {
                theByte = (byte)(theByte | (1 << position));
            }
            else
            {
                theByte = (byte)(theByte & ~(1 << position));
            }
        }

        /// <summary>
        /// Gets the bit as a <see cref="bool" /> from a <see cref="byte" /> at a given position.
        /// </summary>
        /// <param name="theByte">The byte.</param>
        /// <param name="position">The position.</param>
        /// <returns>
        /// True if the bit is set; otherwise false.
        /// </returns>
        public bool GetBit(byte theByte, int position)
        {
            return ((theByte & (1 << position)) != 0);
        }
    }
}
