using Confluent.Kafka;
using Elevator.Interfaces;
using Newtonsoft.Json;

namespace EventHandler;
public class EventProcessor
{
	private readonly string _topic;

	private readonly string _bootstrapServers;

	public EventProcessor(string bootstrapServers, string topic)
	{
		_topic = topic;

		_bootstrapServers = bootstrapServers;
	}

	public void PublishMessage<TResult>(ICommand<TResult> message) where TResult : ICommandResult
	{
		var config = new ProducerConfig
		{
			BootstrapServers = _bootstrapServers
		};

		using (var producer = new ProducerBuilder<Null, string>(config).Build())
		{
			var deliveryReport = producer.ProduceAsync(_topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(message) }).Result;

			Console.WriteLine($"Message delivered: {deliveryReport.Value}");
		}
	}

	public void StartConsuming(Action<string> messageHandler)
	{
		var config = new ConsumerConfig
		{
			BootstrapServers = _bootstrapServers,
			GroupId = "elevator",
			AutoOffsetReset = AutoOffsetReset.Earliest
		};

		using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
		{
			consumer.Subscribe(_topic);

			while (true)
			{
				try
				{
					var consumeResult = consumer.Consume();

					messageHandler(consumeResult.Message.Value);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}

			}
		}
	}
}
