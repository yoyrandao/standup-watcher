namespace StandupWatcher.Common
{
	public interface IJsonSerializer
	{
		public byte[] SerializeBytes(object @object);

		public string Serialize(object @object);

		public T DeserealizeBytes<T>(byte[] byteArray);

		public T Deserialize<T>(string serialized);
	}
}