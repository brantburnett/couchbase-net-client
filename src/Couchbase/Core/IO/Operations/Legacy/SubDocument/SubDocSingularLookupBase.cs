using System;

namespace Couchbase.Core.IO.Operations.Legacy.SubDocument
{
    internal abstract class SubDocSingularLookupBase<T> : SubDocSingularBase<T>
    {
        public override byte[] Write()
        {
            var totalLength = OperationHeader.Length + KeyLength + ExtrasLength + PathLength + BodyLength;
            var buffer = new byte[totalLength];

            WriteHeader(buffer);
            WriteExtras(buffer, OperationHeader.Length);
            WriteKey(buffer, OperationHeader.Length + ExtrasLength);
            WritePath(buffer, OperationHeader.Length + ExtrasLength + KeyLength);
            WriteBody(buffer, OperationHeader.Length + ExtrasLength + KeyLength + BodyLength);

            return buffer;
        }


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
                Converter.FromInt16(VBucketId.Value, span.Slice(HeaderOffsets.VBucket));//6-7
            }

            Converter.FromInt32(ExtrasLength + PathLength + KeyLength, span.Slice(HeaderOffsets.BodyLength));//8-11
            Converter.FromUInt32(Opaque, span.Slice(HeaderOffsets.Opaque));//12-15
            Converter.FromUInt64(Cas, span.Slice(HeaderOffsets.Cas));
        }

        public override void WriteExtras(byte[] buffer, int offset)
        {
            var span = buffer.AsSpan(offset);

            Converter.FromInt16(PathLength, span); //1-2
            Converter.FromByte((byte) CurrentSpec.PathFlags, span.Slice(2)); //3

            if (CurrentSpec.DocFlags != SubdocDocFlags.None)
            {
                Converter.FromByte((byte) CurrentSpec.DocFlags, span.Slice(3));
            }
        }

        public override void ReadExtras(byte[] buffer)
        {
            // Do nothing, lookups don't return extras
        }

        public override bool CanRetry()
        {
            return ErrorCode == null || ErrorMapRequestsRetry();
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
