using System.Threading.Tasks;

namespace Api.Helpers
{
    public static class FaultToleranceWrapper
    {
        private static int RetryCounter => 5;
        private static int RetryTimeout => 2000;

        public static async Task<T> Do<T>(Task<T> task) where T : class
        {
            var result = default(T);
            var counter = RetryCounter;

            while (result == null && counter > 0)
            {
                try
                {
                    result = await task;
                    await Task.Delay(RetryTimeout);
                }
                catch
                {
                    // ignored
                }

                counter -= 1;
            }

            return result;
        }
    }
}
