using Windows.System;
using System.Globalization;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Dota2Handbook.Infrastructure
{
    public static class Information
    {
        public static string ApplicationName => SystemInformation.ApplicationName;

        public static string ApplicationVersion => $"{SystemInformation.ApplicationVersion.Major}.{SystemInformation.ApplicationVersion.Minor}.{SystemInformation.ApplicationVersion.Build}.{SystemInformation.ApplicationVersion.Revision}";

        public static CultureInfo Culture => SystemInformation.Culture;

        public static string OperatingSystem => SystemInformation.OperatingSystem;

        public static ProcessorArchitecture OperatingSystemArchitecture => SystemInformation.OperatingSystemArchitecture;

        public static OSVersion OperatingSystemVersion => SystemInformation.OperatingSystemVersion;

        public static string DeviceFamily => SystemInformation.DeviceFamily;

        public static string DeviceModel => SystemInformation.DeviceModel;

        public static string DeviceManufacturer => SystemInformation.DeviceManufacturer;
    }
}