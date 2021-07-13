using RosMessageTypes.Sensor;
using System;
using Unity.Robotics.MessageVisualizers;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector.TransformManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserScanVisualizerSettings", menuName = "Robotics/Sensor Messages/LaserScan", order = 1)]
public class LaserScanVisualizerSettings : VisualizerSettings<LaserScanMsg>
{
    public TFTrackingType m_TFTrackingType = TFTrackingType.Exact;
    public bool m_UseIntensitySize;
    public float m_PointRadius = 0.05f;
    public float m_MaxIntensity = 100.0f;

    public enum ColorMode
    {
        Distance,
        Intensity,
        Angle,
    }
    public ColorMode m_ColorMode;

    public override void Draw(BasicDrawing drawing, LaserScanMsg message, MessageMetadata meta)
    {
        drawing.SetTFTrackingType(m_TFTrackingType, message.header);

        PointCloudDrawing pointCloud = drawing.AddPointCloud(message.ranges.Length);
        TransformFrame frame = TransformGraph.instance.GetTransform(message.header);
        // negate the angle because ROS coordinates are right-handed, unity coordinates are left-handed
        float angle = -message.angle_min;
        ColorMode mode = m_ColorMode;
        if (mode == ColorMode.Intensity && message.intensities.Length != message.ranges.Length)
            mode = ColorMode.Distance;
        for (int i = 0; i < message.ranges.Length; i++)
        {
            Vector3 localPoint = Quaternion.Euler(0, Mathf.Rad2Deg * angle, 0) * Vector3.forward * message.ranges[i];
            Vector3 worldPoint = frame.TransformPoint(localPoint);

            Color32 c = Color.white;
            switch (mode)
            {
                case ColorMode.Distance:
                    c = Color.HSVToRGB(Mathf.InverseLerp(message.range_min, message.range_max, message.ranges[i]), 1, 1);
                    break;
                case ColorMode.Intensity:
                    c = new Color(1, message.intensities[i] / m_MaxIntensity, 0, 1);
                    break;
                case ColorMode.Angle:
                    c = Color.HSVToRGB((1 + angle / (Mathf.PI * 2)) % 1, 1, 1);
                    break;
            }

            float radius = m_PointRadius;
            if (m_UseIntensitySize && message.intensities.Length > 0)
            {
                radius = Mathf.InverseLerp(0, m_MaxIntensity, message.intensities[i]);
            }
            pointCloud.AddPoint(worldPoint, c, radius);

            angle -= message.angle_increment;
        }
        pointCloud.Bake();
    }

    public override Action CreateGUI(LaserScanMsg message, MessageMetadata meta)
    {
        return () =>
        {
            message.header.GUI();
            GUILayout.Label($"Angle min {message.angle_min}, max {message.angle_max}, increment {message.angle_increment}");
            GUILayout.Label($"Time between measurements {message.time_increment}; time between scans {message.scan_time}");
            GUILayout.Label($"Range min {message.range_min}, max {message.range_max}");
            GUILayout.Label(message.intensities.Length == 0 ? $"{message.ranges.Length} range entries (no intensity data)" : $"{message.ranges.Length} range and intensity entries");
        };
    }
}
