using System.Data;
using System.Diagnostics;
using System.Text;
using Library;

long InitializeResult(int typeId1)
{
	var result = 0L;
	if (typeId1 == 1)
		result = 1L;
	else if (typeId1 == 2 || typeId1 == 5 || typeId1 == 6 || typeId1 == 7)
		result = long.MaxValue;
	return result;
}

void ComputeResult(int typeId1, ref long result, long newValue)
{
	switch (typeId1)
	{
		case 0:
			result += newValue;
			break;
		case 1:
			result *= newValue;
			break;
		case 2:
			if (result > newValue)
				result = newValue;
			break;
		case 3:
			if (result < newValue)
				result = newValue;
			break;
		case 5:
			if (result != long.MaxValue)
			{
				if (result > newValue)
					result = 1;
				else
					result = 0;
			}
			else
				result = newValue;
			break;
		case 6:
			if (result != long.MaxValue)
			{
				if (result < newValue)
					result = 1;
				else
					result = 0;
			}
			else
				result = newValue;
			break;
		case 7:
			if (result != long.MaxValue)
			{
				if (result == newValue)
					result = 1;
				else
					result = 0;
			}
			else
				result = newValue;
			break;
		default:
			throw new NotImplementedException("TODO");
	}
}

var packetStream = new string(IO.ReadInputAsStringArray().First().Select(x => Convert.ToString(Convert.ToInt32(x.ToString(), 16), 2).PadLeft(4, '0')).SelectMany(x => x).ToArray());
int versionCountSum = 0;

PartAB();

void PartAB()
{
	var packetResultValue = ProcessPacketStream(packetStream, out int index);
	Console.WriteLine($"Sum of all version numbers {versionCountSum}");
	Console.WriteLine($"Result value {packetResultValue}");
}

long ProcessPacketStream(string stream, out int nextIndex)
{
	int version = Convert.ToInt32(stream.Substring(0, 3), 2);
	versionCountSum += version;
	int typeId = Convert.ToInt32(stream.Substring(3, 3), 2);
	switch (typeId)
	{
		case 4:
			var literalValue = ProcessLiteralValue(stream.Substring(6), out nextIndex);
			Console.WriteLine($"Processed literal value: {literalValue}");
			nextIndex += 6;
			return literalValue;
		default:
			var lengthTypeId = stream[6];
			if (lengthTypeId == '0')
			{
				var totalLengthInBits = Convert.ToInt32(stream.Substring(7, 15), 2);
				var subPackets = stream.Substring(22, totalLengthInBits);
				var processedPacketBits = 0;
				long result = InitializeResult(typeId);
				while (processedPacketBits < totalLengthInBits)
				{
					long value = ProcessPacketStream(subPackets.Substring(processedPacketBits), out int nextSubIndex);
					processedPacketBits += nextSubIndex;
					ComputeResult(typeId, ref result, value);
				}

				nextIndex = 22 + totalLengthInBits;
				return result;

			}
			else
			{
				var numberOfSubPackets = Convert.ToInt32(stream.Substring(7, 11), 2);
				var subPackets = stream.Substring(18);
				var processedPacketBits = 0;
				long result = InitializeResult(typeId);
				for (int i = 0; i < numberOfSubPackets; ++i)
				{
					long value = ProcessPacketStream(subPackets.Substring(processedPacketBits), out int nextSubIndex);
					processedPacketBits += nextSubIndex;
					ComputeResult(typeId, ref result, value);
				}

				nextIndex = 18 + processedPacketBits;
				return result;
			}
	}
}

long ProcessLiteralValue(string stream, out int nextIndex)
{
	int packetsProcessed = 0;
	StringBuilder stringBuilder = new StringBuilder();
	while (true)
	{
		var subPacket = stream.Substring(packetsProcessed * 5, 5);
		stringBuilder.Append(subPacket[1..]);
		if (subPacket.StartsWith('0'))
		{
			break;
		}

		packetsProcessed++;
	}

	nextIndex = packetsProcessed * 5 + 5;

	return Convert.ToInt64(stringBuilder.ToString(), 2);
}