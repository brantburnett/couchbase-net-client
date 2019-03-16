using System;
using Couchbase.Utils;

namespace Couchbase.Core.IO.Operations.Legacy.SubDocument
{
    internal abstract class SubDocSingularMutationBase<T> : SubDocSingularBase<T>
    {
        public override void WriteHeader(byte[] buffer)
        {
            var span = buffer.AsSpan();

            Converter.FromByte((byte)Magic.Request, span.Slice(HeaderOffsets.Magic));//0
            Converter.FromByte((byte)OpCode, span.Slice(HeaderOffsets.Opcode));//1
            Converter.FromInt16(KeyLength, span.Slice(HeaderOffsets.KeyLength));//2-3
            Converter.FromByte((byte)ExtrasLength, span.Slice(HeaderOffsets.ExtrasLength));  //4
            //5 datatype?
            if (VBucketId.HasValue)
            {
                Converter.FromInt16((short)VBucketId, span.Slice(HeaderOffsets.VBucket));//6-7
            }

            Converter.FromInt32(ExtrasLength + KeyLength + BodyLength + PathLength, span.Slice(HeaderOffsets.BodyLength));//8-11
            Converter.FromUInt32(Opaque, span.Slice(HeaderOffsets.Opaque));//12-15
            Converter.FromUInt64(Cas, span.Slice(HeaderOffsets.Cas));
        }

        public override byte[] Write()
        {
            var totalLength = OperationHeader.Length + KeyLength + ExtrasLength + PathLength + BodyLength;
            var buffer = new byte[totalLength];

            WriteHeader(buffer);
            WriteExtras(buffer, OperationHeader.Length);
            WriteKey(buffer, OperationHeader.Length + ExtrasLength);
            WritePath(buffer, OperationHeader.Length + ExtrasLength + KeyLength);
            WriteBody(buffer, OperationHeader.Length + ExtrasLength + KeyLength + PathLength);

            return buffer;
        }

        public override void WriteExtras(byte[] buffer, int offset)
        {
            var span = buffer.AsSpan(offset);

            Converter.FromInt16(PathLength, span); //2@24 Path length
            Converter.FromByte((byte) CurrentSpec.PathFlags, span.Slice(2)); //1@26 PathFlags

            var hasExpiry = Expires > 0;
            if (hasExpiry)
            {
                Converter.FromUInt32(Expires, span.Slice(3)); //4@27 Expiration time (if present, extras is 7)
            }
            if (CurrentSpec.DocFlags != SubdocDocFlags.None)
            {
                // write doc flags, offset depends on if there is an expiry
                Converter.FromByte((byte) CurrentSpec.DocFlags, span.Slice(hasExpiry ? 7 : 3));
            }
        }

        public override byte[] CreateBody()
        {
            var bytes = Transcoder.Serializer.Serialize(CurrentSpec.Value);
            if (CurrentSpec.RemoveBrackets)
            {
                return bytes.StripBrackets();
            }
            return bytes;
        }

        public override void ReadExtras(byte[] buffer)
        {
            TryReadMutationToken(buffer);
        }
    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2017 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion
