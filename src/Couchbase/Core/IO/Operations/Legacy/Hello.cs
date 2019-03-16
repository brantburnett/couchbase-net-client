using System;
using Couchbase.Utils;
using Newtonsoft.Json;

namespace Couchbase.Core.IO.Operations.Legacy
{
    internal class Hello : OperationBase<short[]>
    {
        public override OpCode OpCode => OpCode.Helo;

        public override byte[] CreateBody()
        {
            var body = new byte[Content.Length * 2];
            var span = body.AsSpan();

            for (var i = 0; i < Content.Length; i++)
            {
                var offset = i * 2;
                Converter.FromInt16(Content[i], span.Slice(offset));
            }

            return body;
        }

        public override byte[] CreateExtras()
        {
            return new byte[0];
        }

        public override short[] GetValue()
        {
            var result = default(short[]);
            if (Success && Data != null && Data.Length > 0)
            {
                try
                {
                    var buffer = Data.ToArray().AsSpan();
                    var offset = Header.BodyOffset;
                    result = new short[Header.BodyLength/2];

                    for (int i = 0; i < result.Length; i++)
                    {
                        var temp = offset + i * 2;
                        if (temp < buffer.Length)
                        {
                            result[i] = Converter.ToInt16(buffer.Slice(temp));
                        }
                    }
                }
                catch (Exception e)
                {
                    Exception = e;
                    HandleClientError(e.Message, ResponseStatus.ClientFailure);
                }
            }
            return result;
        }

        public override bool RequiresKey => true;

        internal static string BuildHelloKey(ulong connectionId)
        {
            var agent = ClientIdentifier.GetClientDescription();
            if (agent.Length > 200)
            {
                agent = agent.Substring(0, 200);
            }

            return JsonConvert.SerializeObject(new
            {
                i = ClientIdentifier.FormatConnectionString(connectionId),
                a = agent
            }, Formatting.None);
        }
    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2015 Couchbase, Inc.
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
