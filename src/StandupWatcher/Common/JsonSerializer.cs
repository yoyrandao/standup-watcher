using System.Text;

using Newtonsoft.Json;


namespace StandupWatcher.Common
{
	public class JsonSerializer : IJsonSerializer
	{
		#region Implementation of IJsonSerializer

		public byte[] SerializeBytes(object @object)
		{
			var serializedString = JsonConvert.SerializeObject(@object);

			return Encoding.UTF8.GetBytes(serializedString);
		}

		string IJsonSerializer.Serialize(object @object)
		{
			return JsonConvert.SerializeObject(@object);
		}

		public T DeserealizeBytes<T>(byte[] byteArray)
		{
			var serializedString = Encoding.UTF8.GetString(byteArray);

			return JsonConvert.DeserializeObject<T>(serializedString);
		}

		public T Deserialize<T>(string serialized)
		{
			return JsonConvert.DeserializeObject<T>(serialized);
		}

		#endregion
	}
}