using System;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.IntegrationTests.Fixtures;
using Xunit;

namespace Couchbase.IntegrationTests
{
    public class BootstrapFailedTests
    {
        [Fact]
        public async Task Test_BootStrap_Error_Propagates_To_Collection_Operations()
        {
            const string id = "key;";
            var value = new {x = "y"};

            var settings = ClusterFixture.GetSettings();
            var cluster = await Cluster.ConnectAsync(settings.ConnectionString, "Administrator", "password").ConfigureAwait(false);
            var bucket = await cluster.BucketAsync("doesnotexist").ConfigureAwait(false);
            var defaultCollection = bucket.DefaultCollection();

           await Assert.ThrowsAsync<AuthenticationFailureException>(async ()=>
           {
               await ThrowAuthenticationException(()=> defaultCollection.GetAsync(id)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.ExistsAsync(id)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.GetAndLockAsync(id, TimeSpan.MaxValue)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.GetAndTouchAsync(id, TimeSpan.MaxValue)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.GetAnyReplicaAsync(id)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.LookupInAsync(id, null)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.MutateInAsync(id, null)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.RemoveAsync(id)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.TouchAsync(id, TimeSpan.Zero)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.InsertAsync(id,value)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.ReplaceAsync(id,value)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.UnlockAsync<dynamic>(id, 0)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(() => defaultCollection.UpsertAsync(id, value)).ConfigureAwait(false);
           }).ConfigureAwait(false);
           await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
           {
               await ThrowAuthenticationException(()=> defaultCollection.GetAllReplicasAsync(id).First()).ConfigureAwait(false);
           }).ConfigureAwait(false);
        }

        [Fact]
        public async Task Test_BootStrap_Error_Propagates_To_View_Operations()
        {
            var cluster = await Cluster.ConnectAsync("couchbase://10.143.194.101", "Administrator", "password").ConfigureAwait(false);
            var bucket = await cluster.BucketAsync("doesnotexist").ConfigureAwait(false);

            await Assert.ThrowsAsync<AuthenticationFailureException>(async () =>
            {
                await ThrowAuthenticationException(() => bucket.ViewQueryAsync<object,object>("designdoc", "viewname")).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        async Task ThrowAuthenticationException(Func<Task> func)
        {
            try
            {
                await func().ConfigureAwait(false);
            }
            catch (AggregateException e)
            {
                e.Flatten().Handle((x) =>
                {
                    if (x is AuthenticationFailureException)
                    {
                        throw x;
                    }

                    return false;
                });
            }
        }
    }
}
