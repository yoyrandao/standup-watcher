using System.Threading.Tasks;

using StandupWatcher.Workers.Payload;


namespace StandupWatcher.Workers
{
	public class BaseWorker<T> where T : WorkerPayload
	{
		protected BaseWorker(T payload)
		{
			_payload = payload;
		}

		public async Task Work()
		{
			while (true)
			{
				Process();

				await Task.Delay(_payload.Interval);
			}
		}

		protected virtual void Process() { }

		private readonly WorkerPayload _payload;
	}
}