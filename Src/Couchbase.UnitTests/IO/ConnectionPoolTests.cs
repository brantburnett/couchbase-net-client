using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Couchbase.Configuration.Client;
using Couchbase.IO;
using Couchbase.IO.Converters;
using Couchbase.UnitTests.Fakes;
using Couchbase.Utils;
using NUnit.Framework;

namespace Couchbase.UnitTests.IO
{
    [TestFixture]
    public class ConnectionPoolTests
    {
        [Test]
        public void Acquire_AvailConnections_Success()
        {
            // Arrange

            var configuration = new PoolConfiguration(2, 2, 100, 100, 100, 100, 3, 100);

            Func<ConnectionPool<IConnection>, IByteConverter, BufferAllocator, IConnection> factory =
                (pool, converter, allocator) => new FakeConnection();

            var fakeConverter = new FakeConverter();

            var connectionPool = new ConnectionPool<IConnection>(configuration, UriExtensions.GetEndPoint("127.0.0.1:11207"),
                factory, fakeConverter);

            connectionPool.Initialize();

            // Act

            var connection = connectionPool.Acquire();

            // Assert

            Assert.NotNull(connection);
        }

        [Test]
        public void Acquire_NoAvailConnections_ConnectionUnavailableException()
        {
            // Arrange

            var configuration = new PoolConfiguration(2, 2, 100, 100, 100, 100, 3, 100);

            Func<ConnectionPool<IConnection>, IByteConverter, BufferAllocator, IConnection> factory =
                (pool, converter, allocator) => new FakeConnection();

            var fakeConverter = new FakeConverter();

            var connectionPool = new ConnectionPool<IConnection>(configuration, UriExtensions.GetEndPoint("127.0.0.1:11207"),
                factory, fakeConverter);

            connectionPool.Initialize();
            connectionPool.Acquire();
            connectionPool.Acquire();

            // Act/Assert

            Assert.Throws<ConnectionUnavailableException>(() => connectionPool.Acquire());
        }

        [Test]
        public void Acquire_AvailConnectionsAfterRelease_Success()
        {
            // Arrange

            var configuration = new PoolConfiguration(2, 2, 100, 100, 100, 100, 3, 100);

            Func<ConnectionPool<IConnection>, IByteConverter, BufferAllocator, IConnection> factory =
                (pool, converter, allocator) => new FakeConnection();

            var fakeConverter = new FakeConverter();

            var connectionPool = new ConnectionPool<IConnection>(configuration, UriExtensions.GetEndPoint("127.0.0.1:11207"),
                factory, fakeConverter);

            connectionPool.Initialize();
            var connection = connectionPool.Acquire();
            connectionPool.Release(connection);
            connectionPool.Acquire();

            // Act

            connection = connectionPool.Acquire();

            // Assert

            Assert.NotNull(connection);
        }

        [Test]
        public void Acquire_ReleaseAfterAcquireStarts_Success()
        {
            // Arrange

            var configuration = new PoolConfiguration(2, 2, 100, 100, 100, 100, 3, 100);

            Func<ConnectionPool<IConnection>, IByteConverter, BufferAllocator, IConnection> factory =
                (pool, converter, allocator) => new FakeConnection();

            var fakeConverter = new FakeConverter();

            var connectionPool = new ConnectionPool<IConnection>(configuration, UriExtensions.GetEndPoint("127.0.0.1:11207"),
                factory, fakeConverter);

            connectionPool.Initialize();
            connectionPool.Acquire();
            var heldConnection = connectionPool.Acquire();

            // Act

            Task.Delay(50).ContinueWith(task =>
            {
                connectionPool.Release(heldConnection);
            });

            var connection = connectionPool.Acquire();

            // Assert

            Assert.NotNull(connection);
        }

        [Test]
        public void Acquire_ReleaseAfterAcquireStartsGreaterThanWaitTimeout_Success()
        {
            // Arrange

            var configuration = new PoolConfiguration(2, 2, 100, 100, 100, 100, 3, 100);

            Func<ConnectionPool<IConnection>, IByteConverter, BufferAllocator, IConnection> factory =
                (pool, converter, allocator) => new FakeConnection();

            var fakeConverter = new FakeConverter();

            var connectionPool = new ConnectionPool<IConnection>(configuration, UriExtensions.GetEndPoint("127.0.0.1:11207"),
                factory, fakeConverter);

            connectionPool.Initialize();
            connectionPool.Acquire();
            var heldConnection = connectionPool.Acquire();

            // Act

            Task.Delay(200).ContinueWith(task =>
            {
                connectionPool.Release(heldConnection);
            });

            var connection = connectionPool.Acquire();

            // Assert

            Assert.NotNull(connection);
        }
    }
}
