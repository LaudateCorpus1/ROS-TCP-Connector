//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Std
{
    public class MString : Message
    {
        public const string k_RosMessageName = "std_msgs/String";
        public override string RosMessageName => k_RosMessageName;

        public string data;

        public MString()
        {
            this.data = "";
        }

        public MString(string data)
        {
            this.data = data;
        }
        public override List<byte[]> SerializationStatements()
        {
            var listOfSerializations = new List<byte[]>();
            listOfSerializations.Add(SerializeString(this.data));

            return listOfSerializations;
        }

        public override int Deserialize(byte[] data, int offset)
        {
            var dataStringBytesLength = DeserializeLength(data, offset);
            offset += 4;
            this.data = DeserializeString(data, offset, dataStringBytesLength);
            offset += dataStringBytesLength;

            return offset;
        }

        public override string ToString()
        {
            return "MString: " +
            "\ndata: " + data.ToString();
        }

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void OnLoad()
        {
            MessageRegistry.Register<MString>(k_RosMessageName);
        }
    }
}