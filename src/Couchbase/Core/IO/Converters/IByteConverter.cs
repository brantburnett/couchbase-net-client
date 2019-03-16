using System;

namespace Couchbase.Core.IO.Converters
{
    /// <summary>
    /// Provides an interface for converting types and arrays before being sent or after being received across the network.
    /// </summary>
    public interface IByteConverter
    {
        /// <summary>
        /// Reads a <see cref="bool"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">if set to <c>true</c> [use nbo].</param>
        /// <returns></returns>
        bool ToBoolean(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="float"/> from a buffer starting from a given offset..
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">if set to <c>true</c> [use nbo].</param>
        /// <returns></returns>
        float ToSingle(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="DateTime"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">if set to <c>true</c> [use nbo].</param>
        /// <returns></returns>
        DateTime ToDateTime(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// To the double.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">if set to <c>true</c> [use nbo].</param>
        /// <returns></returns>
        double ToDouble(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="Byte"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        byte ToByte(ReadOnlySpan<byte> buffer);

        /// <summary>
        ///  Reads a <see cref="Int16"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        short ToInt16(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads a <see cref="UInt16"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        ushort ToUInt16(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads a <see cref="UInt32"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        uint ToUInt32(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads a <see cref="Int64"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        long ToInt64(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads a <see cref="UInt64"/> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        ulong ToUInt64(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Returns a <see cref="System.String" /> from the buffer starting at a given offset and length.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string ToString(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt16"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt16(ushort value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt16"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt16(ushort value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="Int32"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt32(int value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt32"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt32(uint value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="Int32"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt32(int value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt32"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt32(uint value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="Int64"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt64(long value,  ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt64"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt64(ulong value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="Int64"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt64(long value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt64"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromUInt64(ulong value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="System.String"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromString(string value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="System.String"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromString(string value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="byte"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromByte(byte value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="byte"/> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromByte(byte value, Span<byte> buffer);

        /// <summary>
        /// Sets the bit from a <see cref="byte"/> at a given position.
        /// </summary>
        /// <param name="theByte">The byte.</param>
        /// <param name="position">The position.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void SetBit(ref byte theByte, int position, bool value);

        /// <summary>
        /// Gets the bit as a <see cref="bool"/> from a <see cref="byte"/> at a given position.
        /// </summary>
        /// <param name="theByte">The byte.</param>
        /// <param name="position">The position.</param>
        /// <returns>True if the bit is set; otherwise false.</returns>
        bool GetBit(byte theByte, int position);

        /// <summary>
        /// Reads a <see cref="Int16" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        short ToInt16(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="UInt16" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        ushort ToUInt16(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="Int32" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        int ToInt32(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Reads a <see cref="Int32" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        int ToInt32(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="UInt32" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        uint ToUInt32(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="Int64" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        long ToInt64(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Reads a <see cref="UInt64" /> from a buffer starting from a given offset.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        /// <returns></returns>
        ulong ToUInt64(ReadOnlySpan<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="Int16" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromInt16(short value, ref Span<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="Int16" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt16(short value, ref Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="Int16" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        void FromInt16(short value, Span<byte> buffer);

        /// <summary>
        /// Writes a <see cref="UInt16" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromUInt16(ushort value, ref Span<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="Int32" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromInt32(int value, ref Span<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="UInt32" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromUInt32(uint value, ref Span<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="Int64" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromInt64(long value, ref Span<byte> buffer, bool useNbo);

        /// <summary>
        /// Writes a <see cref="UInt64" /> to a buffer starting at a given offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="useNbo">If <c>true</c> will make most significant byte first.</param>
        void FromUInt64(ulong value, ref Span<byte> buffer, bool useNbo);
    }
}
