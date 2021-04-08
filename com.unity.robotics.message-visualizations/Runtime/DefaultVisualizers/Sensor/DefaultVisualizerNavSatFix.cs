using RosMessageTypes.Sensor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.MessageVisualizers;
using UnityEngine;

public class DefaultVisualizerNavSatFix : BasicVisualizer<MNavSatFix>
{
    public override Action CreateGUI(MNavSatFix message, MessageMetadata meta, BasicDrawing drawing) => () =>
    {
        message.header.GUI();
        message.GUI();
    };
}
